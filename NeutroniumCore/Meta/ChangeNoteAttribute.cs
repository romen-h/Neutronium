using System;
using System.Collections.Generic;
using System.Text;

namespace Neutronium.Core.Meta
{
	[AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = true)]
	public class ChangeNoteAttribute : Attribute
	{
		public readonly string Note;

		internal ChangeNoteAttribute(string note)
		{
			Note = note;
		}
	}
}
