using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;

namespace Neutronium.MergeLib.Internal
{
	internal delegate object RemoteMethodDelegate(object instance, object[] args);
	
	internal delegate object RemoteGetterDelegate(object instance);
	internal delegate T GenericRemoteGetterDelegate<T>(object instance);
	
	internal delegate void RemoteSetterDelegate(object instance, object value);
	internal delegate void GenericRemoteSetterDelegate<T>(object instance, T value);

	internal static class RemoteTypes
	{
		private static readonly Dictionary<string, Type> s_cache = new Dictionary<string, Type>();
		
		internal static Type? FindType(string typeName)
		{
			if (s_cache.TryGetValue(typeName, out Type? type)) return type;
			
			type = Type.GetType(typeName, false);
			if (type == null)
			{
				foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
				{
					type = a.GetType(typeName, false);
					if (type != null) break;
				}
			}

			s_cache[typeName] = type;
			return type;
		}
		
		internal static MethodInfo? FindMethod(Type remoteType, string methodName, int argCount)
		{
			MethodInfo? methodInfo = null;
			foreach (var method in remoteType.GetMethods(BindingFlags.Public | BindingFlags.Instance))
			{
				if (method.Name == methodName && method.GetParameters().Length == argCount)
				{
					if (methodInfo != null) return null;
					methodInfo = method;
				}
			}

			return methodInfo;
		}
		
		internal static RemoteMethodDelegate? BuildRemoteMethodDelegate(Type remoteType, string methodName, int argCount)
		{
			var methodInfo = FindMethod(remoteType, methodName, argCount);
			if (methodInfo == null) return null;
			
			var instanceParameter = Expression.Parameter(typeof(object), "instance");
			var argsParameter = Expression.Parameter(typeof(object[]), "args");
			var parameters = methodInfo.GetParameters();
			var argExpressions = new Expression[parameters.Length];
			for (int i=0; i < parameters.Length; i++)
			{
				argExpressions[i] = Expression.Convert(
					Expression.ArrayIndex(argsParameter, Expression.Constant(i)),
					parameters[i].ParameterType);
			}
			Expression call = Expression.Call(Expression.Convert(instanceParameter, methodInfo.DeclaringType), methodInfo, argExpressions);
			Expression body;
			if (methodInfo.ReturnType == typeof(void))
			{
				body = Expression.Block(call, Expression.Constant(null, typeof(object)));
			}
			else
			{
				body = Expression.Convert(call, typeof(object));
			}
			
			return Expression.Lambda<RemoteMethodDelegate>(body, instanceParameter, argsParameter).Compile();
		}

		internal static TDelegate? BuildGenericRemoteMethodDelegate<TDelegate>(Type remoteType, string methodName, int argCount, object remoteInstance)
			where TDelegate : Delegate
		{
			var methodInfo = FindMethod(remoteType, methodName, argCount);
			if (methodInfo == null) return null;
			
			return (TDelegate)methodInfo.CreateDelegate(typeof(TDelegate), remoteInstance);
		}

		internal static MethodInfo? FindPropertyGetter(Type type, string propertyName)
		{
			PropertyInfo? propertyInfo = null;
			foreach (var prop in type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
			{
				if (prop.Name == propertyName && prop.CanRead)
				{
					if (propertyInfo != null) return null;
					propertyInfo = prop;
				}
			}

			return propertyInfo.GetMethod;
		}

		internal static RemoteGetterDelegate? BuildRemoteGetterDelegate(Type remoteType, string propertyName)
		{
			var getterInfo = FindPropertyGetter(remoteType, propertyName);
			if (getterInfo == null) return null;

			var instanceParameter = Expression.Parameter(typeof(object), "instance");

			Expression call = Expression.Call(Expression.Convert(instanceParameter, getterInfo.DeclaringType), getterInfo);
			Expression body = Expression.Convert(call, typeof(object));

			return Expression.Lambda<RemoteGetterDelegate>(body, instanceParameter).Compile();
		}

		internal static GenericRemoteGetterDelegate<T>? BuildGenericRemoteGetterDelegate<T>(Type remoteType, string propertyName)
		{
			var getterInfo = FindPropertyGetter(remoteType, propertyName);
			if (getterInfo == null) return null;

			var instanceParameter = Expression.Parameter(typeof(object), "instance");

			Expression body = Expression.Call(Expression.Convert(instanceParameter, getterInfo.DeclaringType), getterInfo);

			return Expression.Lambda<GenericRemoteGetterDelegate<T>>(body, instanceParameter).Compile();
		}

		internal static MethodInfo? FindPropertySetter(Type type, string propertyName)
		{
			PropertyInfo? propertyInfo = null;
			foreach (var prop in type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
			{
				if (prop.Name == propertyName && prop.CanWrite)
				{
					if (propertyInfo != null) return null;
					propertyInfo = prop;
				}
			}

			return propertyInfo.SetMethod;
		}

		internal static RemoteSetterDelegate? BuildRemoteSetterDelegate(Type remoteType, string propertyName)
		{
			var setterInfo = FindPropertySetter(remoteType, propertyName);
			if (setterInfo == null) return null;

			var instanceParameter = Expression.Parameter(typeof(object), "instance");
			var argParameter = Expression.Parameter(typeof(object), "value");
			var parameters = setterInfo.GetParameters();
			var argExpression = Expression.Convert(argParameter, parameters[0].ParameterType);
			Expression call = Expression.Call(Expression.Convert(instanceParameter, setterInfo.DeclaringType), setterInfo, argExpression);
			Expression body = Expression.Block(call, Expression.Constant(null, typeof(object)));

			return Expression.Lambda<RemoteSetterDelegate>(body, instanceParameter, argParameter).Compile();
		}

		internal static GenericRemoteSetterDelegate<T>? BuildGenericRemoteSetterDelegate<T>(Type remoteType, string propertyName)
		{
			var setterInfo = FindPropertySetter(remoteType, propertyName);
			if (setterInfo == null) return null;

			var instanceParameter = Expression.Parameter(typeof(object), "instance");
			var argParameter = Expression.Parameter(typeof(T), "value");
			var parameters = setterInfo.GetParameters();
			var argExpression = Expression.Convert(argParameter, parameters[0].ParameterType);
			Expression body = Expression.Call(Expression.Convert(instanceParameter, setterInfo.DeclaringType), setterInfo, argExpression);

			return Expression.Lambda<GenericRemoteSetterDelegate<T>>(body, instanceParameter, argParameter).Compile();
		}
	}
}