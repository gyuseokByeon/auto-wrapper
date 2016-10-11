﻿using AutoWrapper.CodeGen;
using FluentAssertions;
using GwtUnit.XUnit;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Reflection;
using AutoWrapper.Tests.TestClasses;
using Xunit;

namespace AutoWrapper.Tests.CodeGen
{
	public class TypeExtensionsTests : XUnitTestBase<TypeExtensionsTests.Thens>
	{
		#region MemberMethod
		[Theory,
		InlineData("Function1"),
		InlineData("Function2"),
		InlineData("Function3")]
		public void ShouldSetName_WhenCreatingMemberMethod_GivenMethodInfo(string name)
		{
			Given.MethodInfo = typeof(SomeType).GetMethod(name);

			When(CreatingMemberMethod);

			Then.MemberMethod.Name.Should().Be(name);
		}

		[Fact]
		public void ShouldSetParameters_WhenCreatingMemberMethod_GivenMethodInfoForFunction1()
		{
			Given.MethodInfo = typeof(SomeType).GetMethod("Function1");
			Given.MethodInfo2 = typeof(SomeType).GetMethod("Function2");

			When(CreatingMemberMethod);

			Then.MemberMethod.Parameters[0].Type.BaseType.Should().Be("System.Int32");
			Then.MemberMethod.Parameters[0].Name.Should().Be("x");
		}

		[Fact]
		public void ShouldSetParameters_WhenCreatingMemberMethod_GivenMethodInfoForFunction2()
		{
			Given.MethodInfo = typeof(SomeType).GetMethod("Function2");
			
			When(CreatingMemberMethod);

			Then.MemberMethod.Parameters[0].Type.BaseType.Should().Be("System.Boolean?");
			Then.MemberMethod.Parameters[0].Name.Should().Be("b");
			Then.MemberMethod.Parameters[1].Type.BaseType.Should().Be("System.Tuple<System.String, System.Int32>");
			Then.MemberMethod.Parameters[1].Name.Should().Be("o");
		}

		[Fact]
		public void ShouldSetParameters_WhenCreatingMemberMethod_GivenMethodInfoForFunction3()
		{
			Given.MethodInfo = typeof(SomeType).GetMethod("Function3");

			When(CreatingMemberMethod);

			Then.MemberMethod.Parameters[0].Type.BaseType.Should().Be("System.Int32");
			Then.MemberMethod.Parameters[0].Name.Should().Be("x");
			Then.MemberMethod.Parameters[1].Type.BaseType.Should().Be("System.String");
			Then.MemberMethod.Parameters[1].Name.Should().Be("s");
		}

		[Theory,
		InlineData("Function1", MemberAttributes.Public),
		InlineData("Function2", MemberAttributes.Public),
		InlineData("Function3", MemberAttributes.Public)]
		public void ShouldBePublic_WhenCreatingMemberMethod_GivenMethodInfo(string name, MemberAttributes attributes)
		{
			Given.MethodInfo = typeof(SomeType).GetMethod(name);
			
			When(CreatingMemberMethod);

			Then.MemberMethod.Attributes.Should().HaveFlag(attributes);
		}

		[Fact]
		public void ShouldThrow_WhenCreatingMemberMethod_GivenNonPublicMethodInfo()
		{
			Given.MethodInfo = typeof(SomeType).GetMethod("NotSupportedFunction", BindingFlags.NonPublic | BindingFlags.Instance);

			WhenLastActionDeferred(CreatingMemberMethod);

			ThenLastAction.ShouldThrow<NotSupportedException>();
		}

		private void CreatingMemberMethod()
		{
			Func<MethodInfo, CodeMemberMethod> func = m => m.ToMemberMethod(GenerateAs.Type);

			Then.MemberMethod = func(Given.MethodInfo);
		}
		#endregion

		#region MemberProperty
		[Fact]
		public void ShouldSetName_WhenCreatingMemberProperty_GivenPropertyInfo()
		{
			Given.PropertyInfo1 = typeof(SomeType).GetProperty("Property1");
			Given.PropertyInfo2 = typeof(SomeType).GetProperty("Property2");

			When(CreatingMemberProperty);

			Then.MemberProperty1.Name.Should().Be("Property1");
			Then.MemberProperty2.Name.Should().Be("Property2");
		}

		[Fact]
		public void ShouldSetType_WhenCreatingMemberProperty_GivenPropertyInfo()
		{
			Given.PropertyInfo1 = typeof(SomeType).GetProperty("Property1");
			Given.PropertyInfo2 = typeof(SomeType).GetProperty("Property2");

			When(CreatingMemberProperty);

			Then.MemberProperty1.Type.BaseType.Should().Be("System.Boolean");
			Then.MemberProperty2.Type.BaseType.Should().Be("System.Object");
		}

		[Fact]
		public void ShouldBePublic_WhenCreatingMemberProperty_GivenPropertyInfo()
		{
			Given.PropertyInfo1 = typeof(SomeType).GetProperty("Property1");
			Given.PropertyInfo2 = typeof(SomeType).GetProperty("Property2");

			When(CreatingMemberProperty);

			Then.MemberProperty1.Attributes.Should().HaveFlag(MemberAttributes.Public);
			Then.MemberProperty2.Attributes.Should().HaveFlag(MemberAttributes.Public);
		}

		[Fact]
		public void ShouldThrow_WhenCreatingMemberProperty_GivenNonPublicPropertyInfo()
		{
			Given.PropertyInfo1 = typeof(SomeType).GetProperty("NotSupportedProperty", BindingFlags.NonPublic | BindingFlags.Instance);

			WhenLastActionDeferred(CreatingMemberProperty);

			ThenLastAction.ShouldThrow<NotSupportedException>();
		}

		[Fact]
		public void ShouldDeclareGettersSetters_WhenCreatingMemberProperty_GivenPropertyInfo()
		{
			Given.PropertyInfo1 = typeof(SomeType).GetProperty("Property1");
			Given.PropertyInfo2 = typeof(SomeType).GetProperty("Property2");

			When(CreatingMemberProperty);

			Then.MemberProperty1.HasGet.Should().BeTrue();
			Then.MemberProperty1.HasSet.Should().BeTrue();

			Then.MemberProperty2.HasGet.Should().BeTrue();
			Then.MemberProperty2.HasSet.Should().BeFalse();
		}

		[Theory,
		InlineData(typeof(object), "System.Object"),
		InlineData(typeof(void), "System.Void"),
		InlineData(typeof(int), "System.Int32"),
		InlineData(typeof(bool), "System.Boolean"),
		InlineData(typeof(string), "System.String"),
		InlineData(typeof(int?), "System.Int32?"),
		InlineData(typeof(IEnumerable<Tuple<int?, Tuple<string, bool?>>>), "System.Collections.Generic.IEnumerable<System.Tuple<System.Int32?, System.Tuple<System.String, System.Boolean?>>>")
		]
		public void ShouldHaveName_WhenGettingName_GivenType(Type givenType, string expectedName)
		{
			Given.Type = givenType;

			When(GettingTypeName);

			Then.ActualName.Should().Be(expectedName);
		}

		private void CreatingMemberProperty()
		{
			Func<PropertyInfo, CodeMemberProperty> func = p => p.ToMemberProperty();

			Then.MemberProperty1 = func(Given.PropertyInfo1);
			Then.MemberProperty2 = func(Given.PropertyInfo2);
		}
		#endregion

		private void GettingTypeName()
		{
			Then.ActualName = ((Type)Given.Type).GetName();
		}

		protected override void Creating() { }

		public class Thens
		{
			public CodeMemberMethod MemberMethod;
			//public CodeMemberMethod MemberMethod2;

			public CodeMemberProperty MemberProperty1;
			public CodeMemberProperty MemberProperty2;
			public string ActualName;
		}
	}
}