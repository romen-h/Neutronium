using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
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
			IncrementalValueProvider<string?> solutionDirProvider =
				context.AnalyzerConfigOptionsProvider
				.Select(static (options, _) =>
				{
					if (options.GlobalOptions.TryGetValue("build_property.SolutionDir", out var solutionDir))
					{
						return solutionDir;
					}
					return null;
				});
			
			context.RegisterSourceOutput(solutionDirProvider, static (spc, solutionDir) =>
			{
#if DEBUG
				string coreApiDllFile = Path.Combine(solutionDir, "NeutroniumApi.Interfaces/bin/Debug/netstandard2.1/NeutroniumApi.Interfaces.dll");
#else
				string coreApiDllFile = Path.Combine(solutionDir, "NeutroniumApi.Interfaces/bin/Release/netstandard2.1/NeutroniumApi.Interfaces.dll");
#endif

				string unityLibsFolder = Path.Combine(solutionDir, "gamelibs/libs/Unity-6000.3.5f2");
				
				try
				{
					List<string> assemblyPaths = new List<string>();
					HashSet<string> loadedAssemblyNames = new HashSet<string>();
					foreach (string dllFile in Directory.GetFiles(RuntimeEnvironment.GetRuntimeDirectory(), "*.dll", SearchOption.TopDirectoryOnly))
					{
						string libName = Path.GetFileName(dllFile).ToLowerInvariant();
						assemblyPaths.Add(dllFile);
						loadedAssemblyNames.Add(libName);
					}
					foreach (string dllFile in Directory.GetFiles(unityLibsFolder, "*.dll", SearchOption.TopDirectoryOnly))
					{
						string libName = Path.GetFileName(dllFile).ToLowerInvariant();
						if (loadedAssemblyNames.Contains(libName)) continue;

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
				if (type.IsEnum)
				{
					string enumSourceFile = $"{type.FullName}.g.cs";
					string? enumSourceCode = GeneratorUtils.GenerateEnumSource(type);
					if (enumSourceCode != null)
					{
						spc.AddSource(enumSourceFile, enumSourceCode);
					}
				}
				else if (type.IsInterface)
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
}
