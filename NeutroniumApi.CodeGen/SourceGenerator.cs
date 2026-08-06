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
	}
}
