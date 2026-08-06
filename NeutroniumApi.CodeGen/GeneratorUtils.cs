using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Neutronium.Api.Meta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace NeutroniumApi.CodeGen
{
	internal class GeneratorUtils
	{
		internal static readonly Dictionary<string, string> CSharpTypeKeywords = new Dictionary<string, string>()
		{
			{ "System.Void", "void" },
			{ "System.Boolean", "bool" },
			{ "System.Byte", "byte" },
			{ "System.Char", "char" },
			{ "System.Int32", "int" },
			{ "System.Int64", "long" },
			{ "System.Single", "float" },
			{ "System.Double", "double" },
			{ "System.String", "string" },
			{ "System.Object", "object" },
		};
		
		internal static string? WrappedAssemblyName
		{ get; private set; }
		
		internal static readonly Dictionary<string,string> RemoteToLocalInterfaceNames = new Dictionary<string, string>();
		internal static readonly Dictionary<string, string> LocalToRemoteInterfaceNames = new Dictionary<string, string>();
		
		internal static void ProcessApiTypes(Assembly assembly)
		{
			WrappedAssemblyName = assembly.GetName().Name;
			
			var wrappableTypes = assembly.GetTypes().Where(IsWrappableType).ToArray();
			foreach (var type in wrappableTypes)
			{
				string remoteName = type.FullName;
				string localName = remoteName.Replace("Neutronium.Api.", "Neutronium.MergeLib.Api.");
				RemoteToLocalInterfaceNames[remoteName] = localName;
				LocalToRemoteInterfaceNames[localName] = remoteName;
			}
		}

		internal static bool IsWrappableType(Type apiType)
		{
			bool isStableApi = false;
			bool isWrappable = false;
			foreach (var attr in apiType.CustomAttributes)
			{
				isStableApi |= attr.AttributeType.Name == nameof(StableApiAttribute);
				isWrappable |= attr.AttributeType.Name == nameof(WrapInterfaceAttribute);
			}
			return isStableApi && isWrappable;
		}

		internal static bool IsCommonRuntimeType(string name, out string? betterName)
		{
			betterName = null;
			if (GeneratorUtils.CSharpTypeKeywords.TryGetValue(name, out betterName)) return true;
			if (name.StartsWith("System."))
			{
				betterName = name;
				return true;
			}
			return false;
		}

		internal static bool IsGetOnceProperty(PropertyInfo property)
		{
			foreach (var attr in property.CustomAttributes)
			{
				if (attr.AttributeType.Name == nameof(GetOnceAttribute)) return true;
			}
			return false;
		}
		
		internal static string GetLocalInterfaceName(Type remoteInterface)
		{
			return remoteInterface.Name;
		}
		
		internal static string GetWrapperClassName(Type remoteInterface)
		{
			return $"{remoteInterface.Name}_Wrapper";
		}

		internal static string GetLocalTypeName(Type? remoteType)
		{
			if (remoteType == null) throw new ArgumentNullException(nameof(remoteType));

			if (remoteType.IsByRef)
			{
				return GetLocalTypeName(remoteType.GetElementType());
			}
			
			if (remoteType.IsArray)
			{
				return GetLocalTypeName(remoteType.GetElementType()) + "[]";
			}
			
			if (RemoteToLocalInterfaceNames.TryGetValue(remoteType.FullName, out string localName)) return localName;

			if (remoteType.Assembly.GetName().Name == WrappedAssemblyName) return "object";           // non-remoteApiTypes dep remoteType
			
			return GetCSharpTypeName(remoteType);
		}

		internal static string GetCSharpTypeName(Type? remoteType)
		{
			if (remoteType == null) throw new ArgumentNullException(nameof(remoteType));

			if (remoteType.IsArray)
			{
				return GetCSharpTypeName(remoteType.GetElementType()) + "[]";
			}

			if (remoteType.IsGenericType)
			{
				var def = remoteType.GetGenericTypeDefinition();
				var name = ((def.Namespace is { } ns ? ns + "." : "") + def.Name);
				var tick = name.IndexOf('`'); if (tick >= 0) name = name.Substring(0, tick);
				var args = remoteType.GetGenericArguments().Select(GetLocalTypeName);
				return name.Replace("+", ".") + "<" + string.Join(",", args) + ">";
			}

			var typeName = remoteType.FullName;
			if (typeName != null && CSharpTypeKeywords.TryGetValue(typeName, out var primitiveName)) return primitiveName;
			return (typeName ?? remoteType.Name).Replace("+", ".");
		}
		
		internal static PropertyDeclarationSyntax GeneratePropertySyntax(PropertyInfo propertyInfo)
		{
			string propertyLocalTypeName = GetLocalTypeName(propertyInfo.PropertyType);

			var accessors = SyntaxFactory.List<AccessorDeclarationSyntax>();
			if (propertyInfo.CanRead)
			{
				accessors = accessors.Add(
					SyntaxFactory.AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
						.WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken)));
			}
			if (propertyInfo.CanWrite)
			{
				accessors = accessors.Add(
					SyntaxFactory.AccessorDeclaration(SyntaxKind.SetAccessorDeclaration)
						.WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken)));
			}

			return SyntaxFactory.PropertyDeclaration(SyntaxFactory.ParseTypeName(propertyLocalTypeName), propertyInfo.Name)
				.WithAccessorList(SyntaxFactory.AccessorList(accessors));
		}
		
		internal static PropertyDeclarationSyntax GeneratePropertySyntax(PropertyInfo propertyInfo, string getterImpl, string? setterImpl = null)
		{
			string propertyLocalTypeName = GetLocalTypeName(propertyInfo.PropertyType);

			var accessors = SyntaxFactory.List<AccessorDeclarationSyntax>();
			if (propertyInfo.CanRead)
			{
				var parsedGetter = (BlockSyntax)SyntaxFactory.ParseStatement(getterImpl);

				accessors = accessors.Add(
					SyntaxFactory.AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
						.WithBody(parsedGetter));
			}
			if (propertyInfo.CanWrite)
			{
				if (setterImpl == null) throw new ArgumentNullException(nameof(setterImpl));
				
				var parsedSetter = (BlockSyntax)SyntaxFactory.ParseStatement(setterImpl);
				
				accessors = accessors.Add(
					SyntaxFactory.AccessorDeclaration(SyntaxKind.SetAccessorDeclaration)
						.WithBody(parsedSetter));
			}

			return SyntaxFactory.PropertyDeclaration(SyntaxFactory.ParseTypeName(propertyLocalTypeName), propertyInfo.Name)
				.WithAccessorList(SyntaxFactory.AccessorList(accessors));
		}
		
		internal static MethodDeclarationSyntax GenerateMethodSyntax(MethodInfo methodInfo)
		{
			string localReturnTypeName = GetLocalTypeName(methodInfo.ReturnType);

			var methodDecl = SyntaxFactory.MethodDeclaration(SyntaxFactory.ParseTypeName(localReturnTypeName), methodInfo.Name);

			var parameters = SyntaxFactory.List<ParameterSyntax>();
			foreach (ParameterInfo param in methodInfo.GetParameters())
			{
				string parameterLocalTypeName = GetLocalTypeName(param.ParameterType);

				var parameter = SyntaxFactory.Parameter(SyntaxFactory.Identifier(param.Name))
					.WithType(SyntaxFactory.ParseTypeName(parameterLocalTypeName));

				bool isOut = param.IsOut || (param.ParameterType.IsByRef && (param.Attributes & ParameterAttributes.Out) != 0);

				if (isOut)
				{
					parameter = parameter.AddModifiers(SyntaxFactory.Token(SyntaxKind.OutKeyword));
				}
				else if (param.ParameterType.IsByRef)
				{
					parameter = parameter.AddModifiers(SyntaxFactory.Token(SyntaxKind.RefKeyword));
				}

				parameters = parameters.Add(parameter);
			}

			if (parameters.Any())
			{
				methodDecl = methodDecl.WithParameterList(SyntaxFactory.ParameterList(SyntaxFactory.SeparatedList(parameters)));
			}
			
			return methodDecl;
		}

		internal static MethodDeclarationSyntax GenerateMethodSyntax(MethodInfo methodInfo, string impl)
		{
			string localReturnTypeName = GetLocalTypeName(methodInfo.ReturnType);

			var methodDecl = SyntaxFactory.MethodDeclaration(SyntaxFactory.ParseTypeName(localReturnTypeName), methodInfo.Name);

			var parameters = SyntaxFactory.List<ParameterSyntax>();
			foreach (ParameterInfo param in methodInfo.GetParameters())
			{
				string parameterLocalTypeName = GetLocalTypeName(param.ParameterType);

				var parameter = SyntaxFactory.Parameter(SyntaxFactory.Identifier(param.Name))
					.WithType(SyntaxFactory.ParseTypeName(parameterLocalTypeName));

				bool isOut = param.IsOut || (param.ParameterType.IsByRef && (param.Attributes & ParameterAttributes.Out) != 0);

				if (isOut)
				{
					parameter = parameter.AddModifiers(SyntaxFactory.Token(SyntaxKind.OutKeyword));
				}
				else if (param.ParameterType.IsByRef)
				{
					parameter = parameter.AddModifiers(SyntaxFactory.Token(SyntaxKind.RefKeyword));
				}

				parameters = parameters.Add(parameter);
			}

			if (parameters.Any())
			{
				methodDecl = methodDecl.WithParameterList(SyntaxFactory.ParameterList(SyntaxFactory.SeparatedList(parameters)));
			}
			
			BlockSyntax body = (BlockSyntax)SyntaxFactory.ParseStatement(impl);
			
			methodDecl = methodDecl.WithBody(body);

			return methodDecl;
		}

		internal static string? GenerateInterfaceSource(Type interfaceType)
		{
			if (!interfaceType.IsInterface) return null;

			string interfaceName = GetLocalInterfaceName(interfaceType);
			string originalNestedNamespace = interfaceType.Namespace?.Replace("Neutronium.Api", "") ?? "";
			string interfaceNamespace = $"Neutronium.MergeLib.Api{originalNestedNamespace}".TrimEnd('.');
			
			var members = SyntaxFactory.List<MemberDeclarationSyntax>();
			foreach (var propertyInfo in interfaceType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
			{
				var propertyDecl = GeneratePropertySyntax(propertyInfo);
				members = members.Add(propertyDecl);
			}
			
			foreach (var methodInfo in interfaceType.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
			{
				if (methodInfo.IsSpecialName) continue; // Skip props/events backing methods
				
				var methodDecl = GenerateMethodSyntax(methodInfo).WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken));
				members = members.Add(methodDecl);
			}
			
			var interfaceDecl = SyntaxFactory.InterfaceDeclaration(interfaceName)
				.AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword), SyntaxFactory.Token(SyntaxKind.PartialKeyword))
				.WithMembers(members);

			var namespaceDecl = SyntaxFactory.NamespaceDeclaration(SyntaxFactory.ParseName(interfaceNamespace))
				.AddMembers(interfaceDecl);

			var compilationUnit = SyntaxFactory.CompilationUnit()
				.AddMembers(namespaceDecl)
				.NormalizeWhitespace();
				
			return compilationUnit.ToFullString();
		}
		
		internal static string? GenerateInterfaceWrapperSource(Type interfaceType)
		{
			if (!interfaceType.IsInterface) return null;
			
			string interfaceName = GetLocalInterfaceName(interfaceType);
			string className = GetWrapperClassName(interfaceType);
			string originalNestedNamespace = interfaceType.Namespace?.Replace("Neutronium.Api", "") ?? "";
			string classNamespace = "Neutronium.MergeLib.Internal";
			string fullInterfaceName = $"Neutronium.MergeLib.Api{originalNestedNamespace}.{interfaceName}";

			var members = SyntaxFactory.List<MemberDeclarationSyntax>();
			
			var remoteTypeStaticField = SyntaxFactory.ParseMemberDeclaration($"private static readonly System.Type s_remoteType = Neutronium.MergeLib.Internal.RemoteTypes.FindType(\"{interfaceType.FullName}\");");
			members = members.Add(remoteTypeStaticField);
			
			StringBuilder constructorBodyLines = new StringBuilder();
			constructorBodyLines.AppendLine("{");
			
			foreach (var propertyInfo in interfaceType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
			{
				string localPropertyType = GetLocalTypeName(propertyInfo.PropertyType);
				
				if (IsGetOnceProperty(propertyInfo))
				{
					var propertyDecl = GeneratePropertySyntax(propertyInfo).AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword));
					members = members.Add(propertyDecl);

					constructorBodyLines.AppendLine($"var get_{propertyInfo.Name} = RemoteTypes.FindPropertyGetter(s_remoteType, \"{propertyInfo.Name}\");");
					if (RemoteToLocalInterfaceNames.ContainsKey(propertyInfo.PropertyType.FullName ?? ""))
					{
						constructorBodyLines.AppendLine($"{propertyInfo.Name} = {propertyInfo.PropertyType.Name}_Wrapper.Wrap(get_{propertyInfo.Name}.Invoke(WrappedInstance, []));");
					}
					else
					{
						constructorBodyLines.AppendLine($"{propertyInfo.Name} = ({localPropertyType})get_{propertyInfo.Name}.Invoke(WrappedInstance, []);");
					}
				}
				else
				{
					bool isSharedType = GeneratorUtils.IsCommonRuntimeType(propertyInfo.PropertyType.FullName, out string sharedTypeName);
					string getterDelegateType = "RemoteGetterDelegate";
					string setterDelegateType = "RemoteSetterDelegate";
					if (isSharedType)
					{
						getterDelegateType = $"GenericRemoteGetterDelegate<{sharedTypeName}>";
						setterDelegateType = $"GenericRemoteSetterDelegate<{sharedTypeName}>";
					}

					string? getterExpression = null;
					if (propertyInfo.CanRead)
					{
						var getterDelegateField = SyntaxFactory.ParseMemberDeclaration($"private readonly {getterDelegateType}? _get_{propertyInfo.Name};");
						members = members.Add(getterDelegateField);
						constructorBodyLines.AppendLine($"_get_{propertyInfo.Name} = RemoteTypes.Build{getterDelegateType}(s_remoteType, \"{propertyInfo.Name}\");");

						getterExpression = $"_get_{propertyInfo.Name}(WrappedInstance)";

						if (RemoteToLocalInterfaceNames.ContainsKey(propertyInfo.PropertyType.FullName ?? ""))
						{
							getterExpression = $"{propertyInfo.PropertyType.Name}_Wrapper.Wrap({getterExpression})";
						}
						else
						{
							getterExpression = $"({localPropertyType}){getterExpression}";
						}
					}
					
					string? setterExpression = null;
					if (propertyInfo.CanWrite)
					{
						var setterDelegateField = SyntaxFactory.ParseMemberDeclaration($"private readonly {setterDelegateType} _set_{propertyInfo.Name};");
						members = members.Add(setterDelegateField);
						constructorBodyLines.AppendLine($"_set_{propertyInfo.Name} = RemoteTypes.Build{setterDelegateType}(s_remoteType, \"{propertyInfo.Name}\");");
						
						string valueExpression = "value";
						if (RemoteToLocalInterfaceNames.ContainsKey(propertyInfo.PropertyType.FullName ?? ""))
						{
							valueExpression = "Unwrap(value)";
						}
						
						setterExpression = $"_set_{propertyInfo.Name}(WrappedInstance, {valueExpression})";
					}
					
					if (getterExpression != null)
					{
						string getterImpl = $"{{ return {getterExpression}; }}";
						string? setterImpl = null;
						if (setterExpression != null)
						{
							setterImpl = $"{{ {setterExpression}; }}";
						}

						var propertyDecl = GeneratePropertySyntax(propertyInfo, getterImpl, setterImpl).AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword));
						members = members.Add(propertyDecl);
					}
				}
			}

			foreach (var methodInfo in interfaceType.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
			{
				if (methodInfo.IsSpecialName) continue; // Skip props/events backing methods

				var arguments = methodInfo.GetParameters();
				string? localReturnType = GeneratorUtils.GetLocalTypeName(methodInfo.ReturnType);
				string overloadName = GetMethodOverloadName(methodInfo);

				constructorBodyLines.AppendLine($"_call_{overloadName} = RemoteTypes.BuildRemoteMethodDelegate(s_remoteType, \"{methodInfo.Name}\", {arguments.Length});");

				var methodDelegateField = SyntaxFactory.ParseMemberDeclaration($"private readonly RemoteMethodDelegate? _call_{overloadName};");
				members = members.Add(methodDelegateField);

				var argsList = string.Join(", ", arguments.Select(p => GeneratorUtils.RemoteToLocalInterfaceNames.ContainsKey(p.ParameterType.FullName ?? "") ? $"Unwrap({p.Name})" : p.Name));
				string methodCall = $"_call_{overloadName}(WrappedInstance, new object[] {{ {argsList} }})";
				
				string methodBody;
				if (methodInfo.ReturnType.FullName == "System.Void")
				{
					// Just call the method because nothing is returned
					methodBody = $"{{ {methodCall}; }}";
				}
				else if (RemoteToLocalInterfaceNames.ContainsKey(methodInfo.ReturnType.FullName ?? ""))
				{
					// Wrap the returned remote value in a wrapper for its remote type.
					string returnTypeName = GeneratorUtils.GetLocalTypeName(methodInfo.ReturnType);
					methodBody = $"{{ return {methodInfo.ReturnType.Name}_Wrapper.Wrap({methodCall}); }}";
				}
				else
				{
					//body = $"var r = {methodCall}; return r is {returnType} __c ? __c : default;";
					methodBody = $"{{ return ({localReturnType}){methodCall}; }}";
				}

				var methodDecl = GenerateMethodSyntax(methodInfo, methodBody).AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword));
				members = members.Add(methodDecl);
			}
			
			constructorBodyLines.AppendLine("}");
			
			BlockSyntax constructorBody = (BlockSyntax)SyntaxFactory.ParseStatement(constructorBodyLines.ToString());
			var constructorDecl = SyntaxFactory.ConstructorDeclaration(className)
				.AddModifiers(SyntaxFactory.Token(SyntaxKind.InternalKeyword))
				.WithParameterList(SyntaxFactory.ParameterList([
					SyntaxFactory.Parameter(SyntaxFactory.Identifier("instance"))
						.WithType(SyntaxFactory.ParseTypeName("object"))
					]))
				.WithInitializer(SyntaxFactory.ConstructorInitializer(
					SyntaxKind.BaseConstructorInitializer,
					SyntaxFactory.ArgumentList(
						SyntaxFactory.SeparatedList([
							SyntaxFactory.Argument(SyntaxFactory.IdentifierName("instance"))
							]))))
				.WithBody(constructorBody);
			
			members = members.Add(constructorDecl);
			
			var baseClass = SyntaxFactory.SimpleBaseType(SyntaxFactory.ParseTypeName($"InterfaceWrapper<{className}>"));
			var localInterfaceName = SyntaxFactory.SimpleBaseType(SyntaxFactory.ParseTypeName(fullInterfaceName));

			var classDecl = SyntaxFactory.ClassDeclaration(className)
				.AddModifiers(SyntaxFactory.Token(SyntaxKind.InternalKeyword),
							  SyntaxFactory.Token(SyntaxKind.SealedKeyword),
							  SyntaxFactory.Token(SyntaxKind.PartialKeyword))
				.WithBaseList(
					SyntaxFactory.BaseList(
						SyntaxFactory.SeparatedList<BaseTypeSyntax>([
							baseClass,
							localInterfaceName
					])))
				.WithMembers(members);

			var namespaceDecl = SyntaxFactory.NamespaceDeclaration(SyntaxFactory.ParseName(classNamespace))
				.AddMembers(classDecl);

			var compilationUnit = SyntaxFactory.CompilationUnit()
				.AddMembers(namespaceDecl)
				.NormalizeWhitespace();

			return compilationUnit.ToFullString();
		}
		
		internal static string GetMethodOverloadName(MethodInfo methodInfo)
		{
			var remoteMethodName = new StringBuilder();
			remoteMethodName.Append(methodInfo.Name);
			foreach (var arg in methodInfo.GetParameters())
			{
				remoteMethodName.Append("_");
				remoteMethodName.Append(arg.ParameterType.Name);
			}
			remoteMethodName.Replace("&", "");
			remoteMethodName.Replace("+", "");
			return remoteMethodName.ToString();
		}
	}
}
