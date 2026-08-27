using Neutronium.Api.Elements;
using Neutronium.MergeLib.Internal;

namespace Neutronium.MergeLib.Internal
{
	internal partial class IElementHandle_Wrapper : InterfaceWrapper<IElementHandle_Wrapper>
	{
		/// <inheritdoc/>
		public override string ToString() => Id;

		/// <inheritdoc/>
		public override bool Equals(object? obj)
		{
			if (obj == null) return false;
			if (obj is IElementHandle handle)
			{
				return Id == handle.Id;
			}

			return obj.ToString() == Id;
		}

		/// <inheritdoc/>
		public override int GetHashCode() => Id.GetHashCode();
	}
}
