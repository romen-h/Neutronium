using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Neutronium.Api.Meta;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Loader;
using System.Security.Cryptography;
using System.Text;
#pragma warning disable RS1035

namespace NeutroniumApi.CodeGen
{
	[Generator]
	public class SourceGenerator : IIncrementalGenerator
	{
		public void Initialize(IncrementalGeneratorInitializationContext context)
		{
			IncrementalValueProvider<string?> coreApiDllProvider =
				context.AnalyzerConfigOptionsProvider
				.Select(static (options, _) =>
				{
					if (options.GlobalOptions.TryGetValue("build_property.CoreApiLibrary", out var solutionDir))
					{
						return solutionDir;
					}
					return null;
				});
			
			context.RegisterSourceOutput(coreApiDllProvider, static (spc, coreApiDllFile) =>
			{
				if (coreApiDllFile == null)
				{
					spc.Error(1, "The CoreApiLibrary project property must be set and exposed with <CompilerVisibleProperty>.");
					return;
				}

				try
				{
					List<string> assemblyPaths = new List<string>();
					foreach (string dllFile in Directory.GetFiles(RuntimeEnvironment.GetRuntimeDirectory(), "*.dll", SearchOption.TopDirectoryOnly))
					{
						assemblyPaths.Add(dllFile);
					}
					// TODO: Add Unity lib paths if Api exposes anything like that.
					var resolver = new PathAssemblyResolver(assemblyPaths);
					var asmContext = new MetadataLoadContext(resolver, "mscorlib");
					Assembly coreApiAssembly = asmContext.LoadFromAssemblyPath(coreApiDllFile);
					GenerateApi(spc, coreApiAssembly);
				}
				catch (Exception ex)
				{
					spc.Error(2, $"Exception thrown during generation: {ex.ToString()}");
				}
			});
		}

		private static void GenerateApi(SourceProductionContext spc, Assembly apiAssembly)
		{
			GeneratorUtils.ProcessApiTypes(apiAssembly);
			
			var apiTypes = apiAssembly.GetTypes().Where(GeneratorUtils.IsWrappableType).ToArray();

			foreach (var type in apiTypes)
			{
				//string oldSourceFile = $"old_{type.FullName}.g.cs";
				//spc.AddSource(oldSourceFile, GenerateWrapper(type));

				string interfaceSourceFile = $"{type.FullName}.g.cs";
				string? interfaceSourceCode = GeneratorUtils.GenerateInterfaceSource(type);
				if (interfaceSourceCode != null)
				{
					spc.AddSource(interfaceSourceFile, interfaceSourceCode);
				}
				
				string wrapperSourceFile = $"{type.FullName}_Wrapper.g.cs";
				string? wrapperSourceCode = GeneratorUtils.GenerateInterfaceWrapperSource(type);
				if (wrapperSourceCode != null)
				{
					spc.AddSource(wrapperSourceFile, wrapperSourceCode);
				}
			}
		}

		private static string GenerateWrapper(Type type)
		{
			string originalNestedNamespace = type.Namespace?.Replace("Neutronium.Api", "") ?? "";
			string localApiNamespace = $"Neutronium.MergeLib.Api{originalNestedNamespace}".TrimEnd('.');
			string wrapperClassName = type.Name + "_Wrapper";
			string interfaceName = type.Name;
			var constructorBody = new StringBuilder();
			var membersBody = new StringBuilder();
			
			foreach (var propertyInfo in type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
			{
				string propType = GeneratorUtils.GetLocalTypeName(propertyInfo.PropertyType);
				
				bool isSharedType = GeneratorUtils.IsCommonRuntimeType(propertyInfo.PropertyType.FullName, out string sharedTypeName);

				if (GeneratorUtils.IsGetOnceProperty(propertyInfo))
				{
					membersBody.AppendLine($"\t\tpublic {propType} {propertyInfo.Name}");
					membersBody.AppendLine("\t\t{ get; private set; }");
					
					constructorBody.AppendLine($"\t\t\tvar get_{propertyInfo.Name} = RemoteTypes.FindPropertyGetter(s_remoteType, \"{propertyInfo.Name}\");");
					constructorBody.AppendLine($"\t\t\t{propertyInfo.Name} = get_{propertyInfo.Name}?.Invoke(WrappedInstance, []) as {propType};");
				}
				else
				{
					string? getterName = null;
					if (propertyInfo.CanRead)
					{
						getterName = $"_get_{propertyInfo.Name}";
						if (isSharedType)
						{
							membersBody.AppendLine($"\t\tprivate readonly GenericRemoteGetterDelegate<{sharedTypeName}>? {getterName};");
							constructorBody.AppendLine($"\t\t\t{getterName} = RemoteTypes.BuildGenericGetterDelegate<{sharedTypeName}>(s_remoteType, \"{propertyInfo.Name}\");");
						}
						else
						{
							membersBody.AppendLine($"\t\tprivate readonly RemoteGetterDelegate? {getterName};");
							constructorBody.AppendLine($"\t\t\t{getterName} = RemoteTypes.BuildGetterDelegate(s_remoteType, \"{propertyInfo.Name}\");");
						}
					}

					string? setterName = null;
					if (propertyInfo.CanWrite)
					{
						setterName = $"_set_{propertyInfo.Name}";
						if (isSharedType)
						{
							membersBody.AppendLine($"\t\tprivate readonly GenericRemoteSetterDelegate<{sharedTypeName}>? {setterName};");
							constructorBody.AppendLine($"\t\t\t{setterName} = RemoteTypes.BuildGenericSetterDelegate<{sharedTypeName}>(s_remoteType, \"{propertyInfo.Name}\");");
						}
						else
						{
							membersBody.AppendLine($"\t\tprivate readonly RemoteSetterDelegate? {setterName};");
							constructorBody.AppendLine($"\t\t\t{setterName} = RemoteTypes.BuildSetterDelegate(s_remoteType, \"{propertyInfo.Name}\");");
						}
					}

					membersBody.AppendLine($"\t\tpublic {propType} {propertyInfo.Name}");
					membersBody.AppendLine("\t\t{");
					if (GeneratorUtils.RemoteToLocalInterfaceNames.ContainsKey(propertyInfo.PropertyType.FullName ?? ""))
					{
						if (propertyInfo.CanRead)
						{
							string methodCall = $"{getterName}?.DynamicInvoke(WrappedInstance)";
							membersBody.AppendLine($"\t\t\tget => {propType}_Wrapper.Wrap({methodCall});");
						}
						if (propertyInfo.CanWrite)
						{
							string methodCall = $"{setterName}?.DynamicInvoke(WrappedInstance, Unwrap(value))";
							membersBody.AppendLine($"\t\t\tset => {methodCall};");
						}
					}
					else
					{
						if (propertyInfo.CanRead)
						{
							if (isSharedType)
							{
								membersBody.AppendLine($"\t\t\tget => {getterName}(WrappedInstance);");
							}
							else
							{
								string methodCall = $"{getterName}?.DynamicInvoke(WrappedInstance)";
								membersBody.AppendLine($"\t\t\tget => ({propType}){methodCall};");
							}
						}
						if (propertyInfo.CanWrite)
						{
							if (isSharedType)
							{
								membersBody.AppendLine($"\t\t\tset => {setterName}(WrappedInstance, value);");
							}
							else
							{
								string methodCall = $"{setterName}?.DynamicInvoke(WrappedInstance, value)";
								membersBody.AppendLine($"\t\t\tset => {methodCall};");
							}
						}
					}
					membersBody.AppendLine("\t\t}");
				}
				membersBody.AppendLine();
			}

			foreach (var methodInfo in type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
			{
				// Skip special property or event accessors.
				if (methodInfo.IsSpecialName) continue;                       
				
				var arguments = methodInfo.GetParameters();
				string? returnType = GeneratorUtils.GetLocalTypeName(methodInfo.ReturnType);
				string argumentsSignature = string.Join(", ", arguments.Select(p => $"{GeneratorUtils.GetLocalTypeName(p.ParameterType)} {p.Name}"));

				var remoteMethodName = "_call_" + GeneratorUtils.GetMethodOverloadName(methodInfo);
				
				membersBody.AppendLine($"\t\tprivate readonly RemoteMethodDelegate? {remoteMethodName};");
				constructorBody.AppendLine($"\t\t\t{remoteMethodName} = RemoteTypes.BuildMethodDelegate(s_remoteType, \"{methodInfo.Name}\", {arguments.Length});");

				// unwrap mirror args -> real objects
				var argsList = string.Join(", ", arguments.Select(p => GeneratorUtils.RemoteToLocalInterfaceNames.ContainsKey(p.ParameterType.FullName ?? "") ? $"Unwrap({p.Name})" : p.Name));
				

				string methodCall = $"{remoteMethodName}?.DynamicInvoke(WrappedInstance, new object[] {{ {argsList} }})";
				
				string body;
				if (methodInfo.ReturnType.FullName == "System.Void")
				{
					// Just call the method because nothing is returned
					body = $"{methodCall};";
				}
				else if (GeneratorUtils.RemoteToLocalInterfaceNames.ContainsKey(methodInfo.ReturnType.FullName ?? ""))
				{
					// Wrap the returned remote value in a wrapper for its remote type.
					string returnTypeName = GeneratorUtils.GetLocalTypeName(methodInfo.ReturnType);
					body = $"return {returnTypeName}_Wrapper.Wrap({methodCall});";
				}
				else
				{
					//body = $"var r = {methodCall}; return r is {returnType} __c ? __c : default;";
					body = $"return ({returnType}){methodCall};";
				}

				membersBody.AppendLine($"\t\tpublic {returnType} {methodInfo.Name}({argumentsSignature}) {{ {body} }}");
				membersBody.AppendLine();
			}

			// properties: getter wraps if remoteApiTypes, setter unwraps if remoteApiTypes (same rules) — omitted for length

			return
$$"""
using System;
using System.Collections.Generic;
using System.Reflection;
using Neutronium.MergeLib.Internal;

namespace {{localApiNamespace}}
{
	internal sealed partial class {{wrapperClassName}}_Old : InterfaceWrapper, {{interfaceName}}
	{
		private static readonly Type s_remoteType = RemoteTypes.FindType("{{type.FullName}}");

		internal static {{interfaceName}} Wrap(object instance)
		{
			if (instance == null) return null;
			{{wrapperClassName}}_Old wrapper = null;
			if (!s_wrapperCache.TryGetValue(instance, out wrapper))
			{
				wrapper = new {{wrapperClassName}}_Old(instance);
				s_wrapperCache[instance] = wrapper;
			}
			return wrapper;
		}

		private {{wrapperClassName}}_Old(object instance) : base(instance)
		{
{{constructorBody}}
		}
		
{{membersBody}}
	}
}
""";
		}
	}
}
