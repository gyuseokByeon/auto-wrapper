﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoWrapper.CodeGen.Contracts;

namespace AutoWrapper.CodeGen
{
	public sealed class WrapperTypeContainer : IWrapperTypeContainer
	{
		public WrapperTypeContainer()
		{
			_typesToWrap = new List<Type>();
		}

		public IEnumerable<Type> RegisteredTypes => _typesToWrap;

		public void RegisterAssembly(Assembly assembly)
		{
			_typesToWrap.AddRange(
				 assembly.GetTypes()
				 .Where(t => t.IsClass && t.IsPublic && t.IsAbstract == false && t.GetInterfaces().Length == 0)
				 .ToArray()
			);
		}

		public void RegisterAssemblyWithType(Type type)
		{
			RegisterAssembly(Assembly.GetAssembly(type));
		}

		public void Register(Type type)
		{
			_typesToWrap.Add(type);
		}

		public void Register<TType>()
			where TType : class
		{
			Register(typeof(TType));
		}

		public void Unregister<TType>()
			where TType : class
		{
			_typesToWrap.Remove(typeof(TType));
		}

		public void Unregister(Type type)
		{
			_typesToWrap.Remove(type);
		}

		public IEnumerator<Type> GetEnumerator()
		{
			return _typesToWrap.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return _typesToWrap.GetEnumerator();
		}

		private readonly List<Type> _typesToWrap;
	}
}