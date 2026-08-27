using System;
using System.Collections.Generic;
using System.Text;
using Neutronium.Api.Meta;

namespace Neutronium.Api.Elements
{
	/// <summary>
	/// A handle that refers to an element in the Neutronium API.
	/// </summary>
	[PreviewApi]
	public interface IElementHandle
	{
		/// <summary>
		/// The element id.
		/// </summary>
		[PreviewApi]
		[GetOnce]
		string Id
		{ get; }

		/// <summary>
		/// The hash of the element id.
		/// </summary>
		/// <remarks>
		/// Can be cast to a SimHashes value.
		/// </remarks>
		[PreviewApi]
		[GetOnce]
		int Hash
		{ get; }

		/// <summary>
		/// The Neutronium element group name.
		/// </summary>
		[PreviewApi]
		[GetOnce]
		string GroupName
		{ get; }

		/// <summary>
		/// The state of the element.
		/// </summary>
		[PreviewApi]
		[GetOnce]
		int State
		{ get; }
	}
}
