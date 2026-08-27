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
		private class BuildProperties
		{
			public string? SolutionDir
			{ get; set; }
			
			public string? UnityLibsDir
			{ get; set; }
		}
		
		public void Initialize(IncrementalGeneratorInitializationContext context)
		{
			IncrementalValueProvider<BuildProperties> solutionDirProvider =
				context.AnalyzerConfigOptionsProvider
				.Select(static (options, _) =>
				{
					options.GlobalOptions.TryGetValue("build_property.SolutionDir", out var solutionDir);
					options.GlobalOptions.TryGetValue("build_property.UnityLibsFolder", out var unityLibsDir);
					
					return new BuildProperties()
					{
						SolutionDir = solutionDir,
						UnityLibsDir = unityLibsDir
					};
				});
			
			context.RegisterSourceOutput(solutionDirProvider, static (spc, properties) =>
			{
#if DEBUG
				string coreApiDllFile = Path.Combine(properties.SolutionDir, "NeutroniumApi.Interfaces/bin/Debug/netstandard2.1/NeutroniumApi.Interfaces.dll");
#else
				string coreApiDllFile = Path.Combine(properties.SolutionDir, "NeutroniumApi.Interfaces/bin/Release/netstandard2.1/NeutroniumApi.Interfaces.dll");
#endif

				string unityLibsFolder = properties.UnityLibsDir;
				
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
#if false
				if (type.IsEnum)
				{
					string enumSourceFile = $"{type.FullName}.g.cs";
					string? enumSourceCode = GeneratorUtils.GenerateEnumSource(type);
					if (enumSourceCode != null)
					{
						spc.AddSource(enumSourceFile, enumSourceCode);
					}
				}
#endif
				if (type.IsInterface)
				{
#if false
					string interfaceSourceFile = $"{type.FullName}.g.cs";
					string? interfaceSourceCode = GeneratorUtils.GenerateInterfaceSource(type);
					if (interfaceSourceCode != null)
					{
						spc.AddSource(interfaceSourceFile, interfaceSourceCode);
					}
#endif
					
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
