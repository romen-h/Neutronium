using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;

namespace Neutronium.MergeLib.Internal
{
	internal abstract class InterfaceWrapper
	{
		internal object WrappedInstance
		{ get; private set; }

		internal static object? Unwrap(object? o)
		{
			if (o is not InterfaceWrapper wrapper) return o;
			return wrapper.WrappedInstance;
		}
		
		protected InterfaceWrapper(object instance)
		{
			WrappedInstance = instance;
		}
	}

	internal abstract class InterfaceWrapper<TDerived> : InterfaceWrapper
		where TDerived : InterfaceWrapper<TDerived>
	{
		private static readonly object s_wrapperCacheLock = new object();
		protected static readonly Dictionary<object, TDerived> s_wrapperCache = new Dictionary<object, TDerived>();

		internal static TDerived? Wrap(object? instance)
		{
			if (instance == null) return null;
			
			TDerived wrapper = null;
			lock (s_wrapperCacheLock)
			{
				if (!s_wrapperCache.TryGetValue(instance, out wrapper))
				{
					var ctorInfo = typeof(TDerived).GetConstructor(BindingFlags.DeclaredOnly | BindingFlags.NonPublic | BindingFlags.Instance, null, [ typeof(object) ], null);
					wrapper = (TDerived)ctorInfo.Invoke([instance]);
					s_wrapperCache[instance] = wrapper;
				}
			}
			return wrapper;
		}
		
		protected InterfaceWrapper(object instance) : base(instance)
		{ }
	}
}
