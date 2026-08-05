using System;
using System.Collections.Generic;
using System.Text;
using Neutronium.Api.Logging;
using Neutronium.Api.Meta;

namespace Neutronium.Api.Plugins
{
	/// <summary>
	/// A Neutronium plugin DLL must implement a single class that has this interface on it.
	/// </summary>
	[StableApi(ApiVersions.Alpha_Milestone1)]
	public interface IPlugin
	{
		/// <summary>
		/// Provides a string to uniquely identify this plugin.
		/// </summary>
		[StableApi(ApiVersions.Alpha_Milestone1)]
		string UniqueID
		{ get; }

		/// <summary>
		/// Declares the minimum game version required to load this plugin.
		/// If null is provided then this condition will not be checked.
		/// </summary>
		[StableApi(ApiVersions.Alpha_Milestone1)]
		uint? MinGameVersion
		{ get; }

		/// <summary>
		/// Declares the maximum game version required to load this plugin.
		/// If null is provided then this condition will not be checked.
		/// </summary>
		[StableApi(ApiVersions.Alpha_Milestone1)]
		uint? MaxGameVersion
		{ get; }

		/// <summary>
		/// Called by Neutronium as soon as the Unity API is ready to use, but before Oxygen Not Included has run any code.
		/// Use this hook to do lightweight initialization of variables in your mod that depend on only Unity APIs.
		/// </summary>
		/// <remarks>
		///	Do not apply patches or do any significant processing work from this method.
		/// </remarks>
		[StableApi(ApiVersions.Alpha_Milestone1)]
		void OnUnityInitialized();
		
		/// <summary>
		/// Called by Neutronium to give an ILogger instance to a plugin.
		/// Logging through this object will push messages to the dedicated Neutronium log file.
		/// </summary>
		[StableApi(ApiVersions.Alpha_Milestone1)]
		void ProvideLoggerFactory(ILoggerFactory logger);
	}
}
