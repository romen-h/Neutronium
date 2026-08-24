using Neutronium.Api.Meta;

namespace Neutronium.Api.Elements
{
	/// <summary>
	/// Represents an element registered with Neutronium that has a group name and state.
	/// </summary>
	[PreviewApi]
	public struct ElementHandle
	{
		/// <summary>
		/// The group name of the element.
		/// </summary>
		public readonly string GroupName;
		/// <summary>
		/// The state of the element.
		/// </summary>
		public readonly ElementState State;
		/// <summary>
		/// Converts this ElementHandle into a System.Tuple.
		/// </summary>
		public static implicit operator (string,int)(ElementHandle handle) => (handle.GroupName,(int)handle.State);
		/// <summary>
		/// Converts this ElementHandle into an element ID string.
		/// </summary>
		public static implicit operator string(ElementHandle handle) => $"{handle.State}_{handle.GroupName}";
		
		/// <summary>
		/// Creates an ElementHandle.
		/// </summary>
		/// <param name="groupName">The group name of the element.</param>
		/// <param name="state">The state of matter for the element.</param>
		[PreviewApi]
		public ElementHandle(string groupName, ElementState state)
		{
			GroupName = groupName;
			State = state;
		}
	}
}
