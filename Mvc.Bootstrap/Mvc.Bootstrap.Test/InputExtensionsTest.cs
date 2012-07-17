using System;
using System.Web.Mvc;
using System.Web.Routing;
using NUnit.Framework;
using Mvc.Bootstrap.Core;
using System.ComponentModel.DataAnnotations;

namespace Mvc.Bootstrap.Test
{
	[TestFixture]
	class InputExtensionsTest
	{
		private static readonly RouteValueDictionary AttributesDictionary = new RouteValueDictionary(new { baz = "BazValue" });
		private static readonly object AttributesObjectDictionary = new { baz = "BazObjValue" };
		private static readonly object AttributesObjectUnderscoresDictionary = new { foo_baz = "BazObjValue" };
		
		[SetUp]
		public void SetUp()
		{
			
		}

		[Test, ExpectedException(typeof(ArgumentNullException))]
		public void TextBoxControlGroupForWithNullExpressionThrows()
		{
			// Arrange
			var helper = MvcHelper.GetHtmlHelper(GetTextBoxViewData());
			helper.TextBoxControlGroupFor<FooBarModel, object>(null /* expression */);
		}

		[Test]
		public void TextBoxControlGroupForWithSimpleExpression()
		{
			// Arrange
			var helper = MvcHelper.GetHtmlHelper(GetTextBoxViewData());

			// Act
			var html = helper.TextBoxControlGroupFor(m => m.Foo);

			// Assert
			Assert.AreEqual(@"<div class=""control-group""><label class=""control-label"" for=""Foo"">Foo</label><div class=""controls""><input id=""Foo"" name=""Foo"" type=""text"" value=""ViewItemFoo"" /></div></div>", html.ToHtmlString());
		}

		[Test]
		public void TextBoxControlGroupForValid()
		{
			var helper = MvcHelper.GetHtmlHelper(GetTextBoxViewData());
			var html = helper.TextBoxControlGroupFor(m => m.Foo);

			Assert.AreEqual(@"<div class=""control-group""><label class=""control-label"" for=""Foo"">Foo</label><div class=""controls""><input id=""Foo"" name=""Foo"" type=""text"" value=""ViewItemFoo"" /></div></div>", html.ToHtmlString());
		}

		[Test]
		public void TextBoxControlGroupForValid2()
		{
			var helper = MvcHelper.GetHtmlHelper(GetTextBoxViewData());
			var html = helper.TextBoxControlGroupFor(m => m.Bar);

			Assert.AreEqual(@"<div class=""control-group""><label class=""control-label"" for=""Bar"">Bar</label><div class=""controls""><input id=""Bar"" name=""Bar"" type=""text"" value=""ViewItemBar"" /></div></div>", html.ToHtmlString());
		}

		[Test, Ignore("Cannot get ClientValidationRuleFactory to work. Really needs finishing")]
		public void TextBoxControlGroupForWithSimpleExpression_Unobtrusive()
		{
			// Arrange
			var helper = MvcHelper.GetHtmlHelper(GetTextBoxViewData());
			helper.ViewContext.ClientValidationEnabled = true;
			helper.ViewContext.UnobtrusiveJavaScriptEnabled = true;
			helper.ViewContext.FormContext = new FormContext();
			//helper.ClientValidationRuleFactory = (name, metadata) => new[] { new ModelClientValidationRule { ValidationType = "type", ErrorMessage = "error" } };

			// Act
			var html = helper.TextBoxControlGroupFor(m => m.Foo);

			// Assert
			Assert.AreEqual(@"<input data-val=""true"" data-val-type=""error"" id=""foo"" name=""foo"" type=""text"" value=""ViewItemFoo"" />", html.ToHtmlString());
		}

		[Test]
		public void TextBoxControlGroupForWithAttributesDictionary()
		{
			// Arrange
			var helper = MvcHelper.GetHtmlHelper(GetTextBoxViewData());

			// Act
			var html = helper.TextBoxControlGroupFor(m => m.Foo, AttributesDictionary);

			// Assert
			Assert.AreEqual(@"<div class=""control-group""><label class=""control-label"" for=""Foo"">Foo</label><div class=""controls""><input baz=""BazValue"" id=""Foo"" name=""Foo"" type=""text"" value=""ViewItemFoo"" /></div></div>", html.ToHtmlString());
		}

		[Test]
		public void TextBoxControlGroupForWithAttributesObject()
		{
			// Arrange
			var helper = MvcHelper.GetHtmlHelper(GetTextBoxViewData());

			// Act
			var html = helper.TextBoxControlGroupFor(m => m.Foo, AttributesObjectDictionary);

			// Assert
			Assert.AreEqual(@"<div class=""control-group""><label class=""control-label"" for=""Foo"">Foo</label><div class=""controls""><input baz=""BazObjValue"" id=""Foo"" name=""Foo"" type=""text"" value=""ViewItemFoo"" /></div></div>", html.ToHtmlString());
		}

		[Test]
		public void TextBoxControlGroupForWithAttributesObjectWithUnderscores()
		{
			// Arrange
			var helper = MvcHelper.GetHtmlHelper(GetTextBoxViewData());

			// Act
			var html = helper.TextBoxControlGroupFor(m => m.Foo, AttributesObjectUnderscoresDictionary);

			// Assert
			Assert.AreEqual(@"<div class=""control-group""><label class=""control-label"" for=""Foo"">Foo</label><div class=""controls""><input foo-baz=""BazObjValue"" id=""Foo"" name=""Foo"" type=""text"" value=""ViewItemFoo"" /></div></div>", html.ToHtmlString());
		}

		[Test]
		public void TextBoxControlGroupForWithPrefix()
		{
			// Arrange
			var helper = MvcHelper.GetHtmlHelper(GetTextBoxViewData());
			helper.ViewContext.ViewData.TemplateInfo.HtmlFieldPrefix = "MyPrefix";

			// Act
			var html = helper.TextBoxControlGroupFor(m => m.Foo);

			// Assert
			Assert.AreEqual(@"<div class=""control-group""><label class=""control-label"" for=""MyPrefix_Foo"">MyPrefix_Foo</label><div class=""controls""><input id=""MyPrefix_Foo"" name=""MyPrefix.Foo"" type=""text"" value=""ViewItemFoo"" /></div></div>", html.ToHtmlString());
		}

		[Test, Ignore("This should be returning a JSON Packet")]
		public void TextBoxControlGroupForWithPrefixAndEmptyName()
		{
			// Arrange
			var helper = MvcHelper.GetHtmlHelper(GetTextBoxViewData());
			helper.ViewContext.ViewData.TemplateInfo.HtmlFieldPrefix = "MyPrefix";

			// Act
			var html = helper.TextBoxControlGroupFor(m => m);

			// Assert
			Assert.AreEqual(@"<div class=""control-group""><label class=""control-label"" for=""MyPrefix"">MyPrefix</label><div class=""controls""><input id=""MyPrefix"" name=""MyPrefix"" type=""text"" value=""{ Foo = ViewItemFoo, Bar = ViewItemBar }"" /></div></div>", html.ToHtmlString());
		}

		[Test]
		public void TextBoxControlGroupForWithErrors()
		{
			// Arrange
			var helper = MvcHelper.GetHtmlHelper(GetTextBoxViewDataWithErrors());

			// Act
			var html = helper.TextBoxControlGroupFor(m => m.Foo, AttributesObjectDictionary);

			// Assert
			Assert.AreEqual(@"<div class=""error control-group""><label class=""control-label"" for=""Foo"">Foo</label><div class=""controls""><input baz=""BazObjValue"" class=""input-validation-error"" id=""Foo"" name=""Foo"" type=""text"" value=""AttemptedValueFoo"" /><span class=""help-inline""></span></div></div>", html.ToHtmlString());
		}

		[Test]
		public void TextBoxControlGroupForWithErrorsAndCustomClass()
		{
			// Arrange
			var helper = MvcHelper.GetHtmlHelper(GetTextBoxViewDataWithErrors());

			// Act
			var html = helper.TextBoxControlGroupFor(m => m.Foo, new { @class = "foo-class" });

			// Assert
			Assert.AreEqual(@"<div class=""error control-group""><label class=""control-label"" for=""Foo"">Foo</label><div class=""controls""><input class=""input-validation-error foo-class"" id=""Foo"" name=""Foo"" type=""text"" value=""AttemptedValueFoo"" /><span class=""help-inline""></span></div></div>", html.ToHtmlString());
		}

		[Test, ExpectedException(typeof(ArgumentNullException))]
		public void PasswordControlGroupForWithNullExpressionThrows()
		{
			// Arrange
			var helper = MvcHelper.GetHtmlHelper(GetPasswordViewData());

			// Act & Assert
			helper.PasswordControlGroupFor<FooModel, object>(null);
		}

		[Test]
		public void PasswordControlGroupForDictionaryOverridesImplicitParameters()
		{
			// Arrange
			var helper = MvcHelper.GetHtmlHelper(GetPasswordViewData());

			// Act
			var html = helper.PasswordControlGroupFor(m => m.Foo, new { type = "fooType" });

			// Assert
			Assert.AreEqual(@"<div class=""control-group""><label class=""control-label"" for=""Foo"">Foo</label><div class=""controls""><input id=""Foo"" name=""Foo"" type=""fooType"" /></div></div>", html.ToHtmlString());
		}

		[Test]
		public void PasswordControlGroupForExpressionNameOverridesDictionary()
		{
			// Arrange
			var helper = MvcHelper.GetHtmlHelper(GetPasswordViewData());

			// Act
			var html = helper.PasswordControlGroupFor(m => m.Foo, new { name = "bar" });

			// Assert
			Assert.AreEqual(@"<div class=""control-group""><label class=""control-label"" for=""Foo"">Foo</label><div class=""controls""><input id=""Foo"" name=""Foo"" type=""password"" /></div></div>", html.ToHtmlString());
		}

		[Test]
		public void PasswordControlGroupForWithImplicitValue()
		{
			// Arrange
			var helper = MvcHelper.GetHtmlHelper(GetPasswordViewData());

			// Act
			var html = helper.PasswordControlGroupFor(m => m.Foo);

			// Assert
			Assert.AreEqual(@"<div class=""control-group""><label class=""control-label"" for=""Foo"">Foo</label><div class=""controls""><input id=""Foo"" name=""Foo"" type=""password"" /></div></div>", html.ToHtmlString());
		}

		[Test, Ignore("Cannot get ClientValidationRuleFactory to work")]
		public void PasswordControlGroupForWithImplicitValue_Unobtrusive()
		{
			// Arrange
			var helper = MvcHelper.GetHtmlHelper(GetPasswordViewData());
			helper.ViewContext.ClientValidationEnabled = true;
			helper.ViewContext.UnobtrusiveJavaScriptEnabled = true;
			helper.ViewContext.FormContext = new FormContext();
			//helper.ClientValidationRuleFactory = (name, metadata) => new[] { new ModelClientValidationRule { ValidationType = "type", ErrorMessage = "error" } };

			// Act
			var html = helper.PasswordControlGroupFor(m => m.Foo);

			// Assert
			Assert.AreEqual(@"<input data-val=""true"" data-val-type=""error"" id=""foo"" name=""foo"" type=""password"" />", html.ToHtmlString());
		}

		[Test]
		public void PasswordControlGroupForWithDeepValueWithNullModel_Unobtrusive()
		{
			// Arrange
			var helper = MvcHelper.GetHtmlHelper(new ViewDataDictionary<DeepContainerModel>());
			helper.ViewContext.ClientValidationEnabled = true;
			helper.ViewContext.UnobtrusiveJavaScriptEnabled = true;
			helper.ViewContext.FormContext = new FormContext();

			using (HtmlHelperTest.ReplaceCulture("en-US", "en-US"))
			{
				// Act
				var html = helper.PasswordControlGroupFor(m => m.Contained.Foo);

				// Assert
				Assert.AreEqual(@"<div class=""control-group""><label class=""control-label"" for=""Contained_Foo"">Contained_Foo</label><div class=""controls""><input data-val=""true"" data-val-required=""The Foo field is required."" id=""Contained_Foo"" name=""Contained.Foo"" type=""password"" /></div></div>", html.ToHtmlString());
			}
		}

		[Test]
		public void PasswordControlGroupForWithAttributesDictionary()
		{
			// Arrange
			var helper = MvcHelper.GetHtmlHelper(GetPasswordViewData());

			// Act
			var html = helper.PasswordControlGroupFor(m => m.Foo, AttributesDictionary);

			// Assert
			Assert.AreEqual(@"<div class=""control-group""><label class=""control-label"" for=""Foo"">Foo</label><div class=""controls""><input baz=""BazValue"" id=""Foo"" name=""Foo"" type=""password"" /></div></div>", html.ToHtmlString());
		}

		[Test]
		public void PasswordControlGroupForWithAttributesObject()
		{
			// Arrange
			var helper = MvcHelper.GetHtmlHelper(GetPasswordViewData());

			// Act
			var html = helper.PasswordControlGroupFor(m => m.Foo, AttributesObjectDictionary);

			// Assert
			Assert.AreEqual(@"<div class=""control-group""><label class=""control-label"" for=""Foo"">Foo</label><div class=""controls""><input baz=""BazObjValue"" id=""Foo"" name=""Foo"" type=""password"" /></div></div>", html.ToHtmlString());
		}

		[Test]
		public void PasswordControlGroupForWithAttributesObjectWithUnderscores()
		{
			// Arrange
			var helper = MvcHelper.GetHtmlHelper(GetPasswordViewData());

			// Act
			var html = helper.PasswordControlGroupFor(m => m.Foo, AttributesObjectUnderscoresDictionary);

			// Assert
			Assert.AreEqual(@"<div class=""control-group""><label class=""control-label"" for=""Foo"">Foo</label><div class=""controls""><input foo-baz=""BazObjValue"" id=""Foo"" name=""Foo"" type=""password"" /></div></div>", html.ToHtmlString());
		}

		[Test]
		public void PasswordControlGroupForWithPrefix()
		{
			// Arrange
			var helper = MvcHelper.GetHtmlHelper(GetPasswordViewData());
			helper.ViewContext.ViewData.TemplateInfo.HtmlFieldPrefix = "MyPrefix";

			// Act
			var html = helper.PasswordControlGroupFor(m => m.Foo);

			// Assert
			Assert.AreEqual(@"<div class=""control-group""><label class=""control-label"" for=""MyPrefix_Foo"">MyPrefix_Foo</label><div class=""controls""><input id=""MyPrefix_Foo"" name=""MyPrefix.Foo"" type=""password"" /></div></div>", html.ToHtmlString());
		}

		[Test]
		public void PasswordControlGroupForWithViewDataErrors()
		{
			// Arrange
			var helper = MvcHelper.GetHtmlHelper(GetPasswordViewDataWithErrors());

			// Act
			var html = helper.PasswordControlGroupFor(m => m.Foo, AttributesObjectDictionary);

			// Assert
			Assert.AreEqual(@"<div class=""error control-group""><label class=""control-label"" for=""Foo"">Foo</label><div class=""controls""><input baz=""BazObjValue"" class=""input-validation-error"" id=""Foo"" name=""Foo"" type=""password"" /><span class=""help-inline""></span></div></div>", html.ToHtmlString());
		}

		private static ViewDataDictionary<FooModel> GetPasswordViewData()
		{
			return new ViewDataDictionary<FooModel> {{"Foo", "ViewDataFoo"}};
		}

		private static ViewDataDictionary<FooModel> GetPasswordViewDataWithErrors()
		{
			var viewData = new ViewDataDictionary<FooModel> { { "Foo", "ViewDataFoo" } };
			var modelStateFoo = new ModelState();
			modelStateFoo.Errors.Add(new ModelError("foo error 1"));
			modelStateFoo.Errors.Add(new ModelError("foo error 2"));
			viewData.ModelState["Foo"] = modelStateFoo;
			modelStateFoo.Value = HtmlHelperTest.GetValueProviderResult("AttemptedValueFoo", "AttemptedValueFoo");

			return viewData;
		}

		private static ViewDataDictionary<FooBarModel> GetTextBoxViewDataWithErrors()
		{
			var viewData = new ViewDataDictionary<FooBarModel> { { "Foo", "ViewDataFoo" } };
			viewData.Model = new FooBarModel { Foo = "ViewItemFoo", Bar = "ViewItemBar" };
			var modelStateFoo = new ModelState();
			modelStateFoo.Errors.Add(new ModelError("Foo error 1"));
			modelStateFoo.Errors.Add(new ModelError("Foo error 2"));
			viewData.ModelState["Foo"] = modelStateFoo;
			modelStateFoo.Value = HtmlHelperTest.GetValueProviderResult(new[] { "AttemptedValueFoo" }, "AttemptedValueFoo");

			return viewData;
		}

		private static ViewDataDictionary<FooBarModel> GetTextBoxViewData()
		{
			var viewData = new ViewDataDictionary<FooBarModel> { { "Foo", "ViewDataFoo" } };
			viewData.Model = new FooBarModel { Foo = "ViewItemFoo", Bar = "ViewItemBar" };

			return viewData;
		}
	}



	internal class DeepContainerModel
	{
		public ShallowModel Contained { get; set; }
	}

	internal class ShallowModel
	{
		[Required]
		public string Foo { get; set; }
	}

	internal class FooModel
	{
		public string Foo { get; set; }
	}

	internal class FooBarModel
	{
		public string Foo { get; set; }

		public string Bar { get; set; }
	}
}
