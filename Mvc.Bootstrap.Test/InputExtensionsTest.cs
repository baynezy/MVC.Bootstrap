using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
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
		private RouteValueDictionary _attributesDictionary;
		private static readonly object AttributesObjectDictionary = new { baz = "BazObjValue" };
		private static readonly object AttributesObjectUnderscoresDictionary = new { foo_baz = "BazObjValue" };
		private static readonly object TextAreaAttributesObjectDictionary = new { rows = "15", cols = "12" };
		private static readonly object TextAreaAttributesObjectUnderscoresDictionary = new { rows = "15", cols = "12", foo_bar = "baz" };
		private static readonly RouteValueDictionary TextAreaAttributesDictionary = new RouteValueDictionary(new { rows = "15", cols = "12" });
		private static readonly ViewDataDictionary<FooModel> DropDownListViewData = new ViewDataDictionary<FooModel> { { "foo", "Bravo" } };
		
		[SetUp]
		public void SetUp()
		{
			_attributesDictionary = new RouteValueDictionary(new {baz = "BazValue"});
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
			Assert.AreEqual(@"<div class=""form-group""><label class=""control-label"" for=""Foo"">Foo</label><input class=""form-control"" id=""Foo"" name=""Foo"" type=""text"" value=""ViewItemFoo"" /></div>", html.ToHtmlString());
		}

		[Test]
		public void TextBoxControlGroupForValid()
		{
			var helper = MvcHelper.GetHtmlHelper(GetTextBoxViewData());
			var html = helper.TextBoxControlGroupFor(m => m.Foo);

			Assert.AreEqual(@"<div class=""form-group""><label class=""control-label"" for=""Foo"">Foo</label><input class=""form-control"" id=""Foo"" name=""Foo"" type=""text"" value=""ViewItemFoo"" /></div>", html.ToHtmlString());
		}

		[Test]
		public void TextBoxControlGroupForValid2()
		{
			var helper = MvcHelper.GetHtmlHelper(GetTextBoxViewData());
			var html = helper.TextBoxControlGroupFor(m => m.Bar);

			Assert.AreEqual(@"<div class=""form-group""><label class=""control-label"" for=""Bar"">Bar</label><input class=""form-control"" id=""Bar"" name=""Bar"" type=""text"" value=""ViewItemBar"" /></div>", html.ToHtmlString());
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
			var html = helper.TextBoxControlGroupFor(m => m.Foo, _attributesDictionary);

			// Assert
			Assert.AreEqual(@"<div class=""form-group""><label class=""control-label"" for=""Foo"">Foo</label><input baz=""BazValue"" class=""form-control"" id=""Foo"" name=""Foo"" type=""text"" value=""ViewItemFoo"" /></div>", html.ToHtmlString());
		}

		[Test]
		public void TextBoxControlGroupForWithAttributesObject()
		{
			// Arrange
			var helper = MvcHelper.GetHtmlHelper(GetTextBoxViewData());

			// Act
			var html = helper.TextBoxControlGroupFor(m => m.Foo, AttributesObjectDictionary);

			// Assert
			Assert.AreEqual(@"<div class=""form-group""><label class=""control-label"" for=""Foo"">Foo</label><input baz=""BazObjValue"" class=""form-control"" id=""Foo"" name=""Foo"" type=""text"" value=""ViewItemFoo"" /></div>", html.ToHtmlString());
		}

		[Test]
		public void TextBoxControlGroupForWithAttributesObjectWithUnderscores()
		{
			// Arrange
			var helper = MvcHelper.GetHtmlHelper(GetTextBoxViewData());

			// Act
			var html = helper.TextBoxControlGroupFor(m => m.Foo, AttributesObjectUnderscoresDictionary);

			// Assert
			Assert.AreEqual(@"<div class=""form-group""><label class=""control-label"" for=""Foo"">Foo</label><input class=""form-control"" foo-baz=""BazObjValue"" id=""Foo"" name=""Foo"" type=""text"" value=""ViewItemFoo"" /></div>", html.ToHtmlString());
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
			Assert.AreEqual(@"<div class=""form-group""><label class=""control-label"" for=""MyPrefix_Foo"">MyPrefix_Foo</label><input class=""form-control"" id=""MyPrefix_Foo"" name=""MyPrefix.Foo"" type=""text"" value=""ViewItemFoo"" /></div>", html.ToHtmlString());
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
			Assert.AreEqual(@"<div class=""form-group""><label class=""control-label"" for=""MyPrefix"">MyPrefix</label><input class=""form-control"" id=""MyPrefix"" name=""MyPrefix"" type=""text"" value=""{ Foo = ViewItemFoo, Bar = ViewItemBar }"" /></div>", html.ToHtmlString());
		}

		[Test]
		public void TextBoxControlGroupForWithErrors()
		{
			// Arrange
			var helper = MvcHelper.GetHtmlHelper(GetTextBoxViewDataWithErrors());

			// Act
			var html = helper.TextBoxControlGroupFor(m => m.Foo, AttributesObjectDictionary);

			// Assert
			Assert.AreEqual(@"<div class=""has-error form-group""><label class=""control-label"" for=""Foo"">Foo</label><input baz=""BazObjValue"" class=""input-validation-error form-control"" id=""Foo"" name=""Foo"" type=""text"" value=""AttemptedValueFoo"" /></div>", html.ToHtmlString());
		}

		[Test]
		public void TextBoxControlGroupForWithErrorsAndCustomClass()
		{
			// Arrange
			var helper = MvcHelper.GetHtmlHelper(GetTextBoxViewDataWithErrors());

			// Act
			var html = helper.TextBoxControlGroupFor(m => m.Foo, new { @class = "foo-class" });

			// Assert
			Assert.AreEqual(@"<div class=""has-error form-group""><label class=""control-label"" for=""Foo"">Foo</label><input class=""input-validation-error foo-class form-control"" id=""Foo"" name=""Foo"" type=""text"" value=""AttemptedValueFoo"" /></div>", html.ToHtmlString());
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
			Assert.AreEqual(@"<div class=""form-group""><label class=""control-label"" for=""Foo"">Foo</label><input class=""form-control"" id=""Foo"" name=""Foo"" type=""fooType"" /></div>", html.ToHtmlString());
		}

		[Test]
		public void PasswordControlGroupForExpressionNameOverridesDictionary()
		{
			// Arrange
			var helper = MvcHelper.GetHtmlHelper(GetPasswordViewData());

			// Act
			var html = helper.PasswordControlGroupFor(m => m.Foo, new { name = "bar" });

			// Assert
			Assert.AreEqual(@"<div class=""form-group""><label class=""control-label"" for=""Foo"">Foo</label><input class=""form-control"" id=""Foo"" name=""Foo"" type=""password"" /></div>", html.ToHtmlString());
		}

		[Test]
		public void PasswordControlGroupForWithImplicitValue()
		{
			// Arrange
			var helper = MvcHelper.GetHtmlHelper(GetPasswordViewData());

			// Act
			var html = helper.PasswordControlGroupFor(m => m.Foo);

			// Assert
			Assert.AreEqual(@"<div class=""form-group""><label class=""control-label"" for=""Foo"">Foo</label><input class=""form-control"" id=""Foo"" name=""Foo"" type=""password"" /></div>", html.ToHtmlString());
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
			Assert.AreEqual(@"<input class=""form-control"" data-val=""true"" data-val-type=""error"" id=""foo"" name=""foo"" type=""password"" />", html.ToHtmlString());
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
				Assert.AreEqual(@"<div class=""form-group""><label class=""control-label"" for=""Contained_Foo"">Contained_Foo</label><input class=""form-control"" data-val=""true"" data-val-required=""The Foo field is required."" id=""Contained_Foo"" name=""Contained.Foo"" type=""password"" /></div>", html.ToHtmlString());
			}
		}

		[Test]
		public void PasswordControlGroupForWithAttributesDictionary()
		{
			// Arrange
			var helper = MvcHelper.GetHtmlHelper(GetPasswordViewData());

			// Act
			var html = helper.PasswordControlGroupFor(m => m.Foo, _attributesDictionary);

			// Assert
			Assert.AreEqual(@"<div class=""form-group""><label class=""control-label"" for=""Foo"">Foo</label><input baz=""BazValue"" class=""form-control"" id=""Foo"" name=""Foo"" type=""password"" /></div>", html.ToHtmlString());
		}

		[Test]
		public void PasswordControlGroupForWithAttributesObject()
		{
			// Arrange
			var helper = MvcHelper.GetHtmlHelper(GetPasswordViewData());

			// Act
			var html = helper.PasswordControlGroupFor(m => m.Foo, AttributesObjectDictionary);

			// Assert
			Assert.AreEqual(@"<div class=""form-group""><label class=""control-label"" for=""Foo"">Foo</label><input baz=""BazObjValue"" class=""form-control"" id=""Foo"" name=""Foo"" type=""password"" /></div>", html.ToHtmlString());
		}

		[Test]
		public void PasswordControlGroupForWithAttributesObjectWithUnderscores()
		{
			// Arrange
			var helper = MvcHelper.GetHtmlHelper(GetPasswordViewData());

			// Act
			var html = helper.PasswordControlGroupFor(m => m.Foo, AttributesObjectUnderscoresDictionary);

			// Assert
			Assert.AreEqual(@"<div class=""form-group""><label class=""control-label"" for=""Foo"">Foo</label><input class=""form-control"" foo-baz=""BazObjValue"" id=""Foo"" name=""Foo"" type=""password"" /></div>", html.ToHtmlString());
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
			Assert.AreEqual(@"<div class=""form-group""><label class=""control-label"" for=""MyPrefix_Foo"">MyPrefix_Foo</label><input class=""form-control"" id=""MyPrefix_Foo"" name=""MyPrefix.Foo"" type=""password"" /></div>", html.ToHtmlString());
		}

		[Test]
		public void PasswordControlGroupForWithViewDataErrors()
		{
			// Arrange
			var helper = MvcHelper.GetHtmlHelper(GetPasswordViewDataWithErrors());

			// Act
			var html = helper.PasswordControlGroupFor(m => m.Foo, AttributesObjectDictionary);

			// Assert
			Assert.AreEqual(@"<div class=""has-error form-group""><label class=""control-label"" for=""Foo"">Foo</label><input baz=""BazObjValue"" class=""input-validation-error form-control"" id=""Foo"" name=""Foo"" type=""password"" /></div>", html.ToHtmlString());
		}

		[Test, ExpectedException(typeof(ArgumentNullException))]
		public void TextAreaControlGroupForWithNullExpression()
		{
			// Arrange
			var helper = MvcHelper.GetHtmlHelper(GetTextAreaViewData());

			// Act & Assert
			helper.TextAreaControlGroupFor<TextAreaModel, object>(null /* expression */);
		}

		[Test, ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void TextAreaControlGroupForWithOutOfRangeColsThrows()
		{
			// Arrange
			var helper = MvcHelper.GetHtmlHelper(GetTextAreaViewData());

			// Act & Assert
			helper.TextAreaControlGroupFor(m => m.Foo, 0, -1, null /* htmlAttributes */);
		}

		[Test, ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void TextAreaControlGroupForWithOutOfRangeRowsThrows()
		{
			// Arrange
			var helper = MvcHelper.GetHtmlHelper(GetTextAreaViewData());

			// Act & Assert
			helper.TextAreaControlGroupFor(m => m.Foo, -1, 0, null /* htmlAttributes */);
		}

		[Test]
		public void TextAreaControlGroupForParameterDictionaryMerging()
		{
			// Arrange
			var helper = MvcHelper.GetHtmlHelper(GetTextAreaViewData());

			// Act
			var html = helper.TextAreaControlGroupFor(m => m.Foo, new { rows = "30" });

			// Assert
			var sb = new StringBuilder();
			sb.AppendLine(@"<div class=""form-group""><label class=""control-label"" for=""Foo"">Foo</label><textarea class=""form-control"" cols=""20"" id=""Foo"" name=""Foo"" rows=""30"">");
			sb.Append(@"ViewItemFoo</textarea></div>");

			Assert.AreEqual(sb.ToString(), html.ToHtmlString());
		}

		[Test, Ignore("Cannot get ClientValidationRuleFactory to work")]
		public void TextAreaControlGroupForParameterDictionaryMerging_Unobtrusive()
		{
			// Arrange
			var helper = MvcHelper.GetHtmlHelper(GetTextAreaViewData());
			helper.ViewContext.ClientValidationEnabled = true;
			helper.ViewContext.UnobtrusiveJavaScriptEnabled = true;
			helper.ViewContext.FormContext = new FormContext();
			ModelMetadata modelMetadata;
			//helper.ClientValidationRuleFactory = (name, metadata) =>
			{
				//modelMetadata = metadata;
				//return new[] { new ModelClientValidationRule { ValidationType = "type", ErrorMessage = "error" } };
			};

			// Act
			var html = helper.TextAreaControlGroupFor(m => m.Foo, new { rows = "30" });

			// Assert
			//Assert.NotNull(modelMetadata);
			//Assert.AreEqual("Foo", modelMetadata.PropertyName);
			Assert.AreEqual(@"<textarea class=""form-control"" cols=""20"" data-val=""true"" data-val-type=""error"" id=""foo"" name=""foo"" rows=""30"">
ViewItemFoo</textarea>", html.ToHtmlString());
		}

		[Test]
		public void TextAreaControlGroupForWithDefaultAttributes()
		{
			// Arrange
			var helper = MvcHelper.GetHtmlHelper(GetTextAreaViewData());

			// Act
			var html = helper.TextAreaControlGroupFor(m => m.Foo);

			// Assert
			var sb = new StringBuilder();
			sb.AppendLine(@"<div class=""form-group""><label class=""control-label"" for=""Foo"">Foo</label><textarea class=""form-control"" cols=""20"" id=""Foo"" name=""Foo"" rows=""2"">");
			sb.Append(@"ViewItemFoo</textarea></div>");

			Assert.AreEqual(sb.ToString(), html.ToHtmlString());
		}

		[Test]
		public void TextAreaControlGroupForWithZeroRowsAndColumns()
		{
			// Arrange
			var helper = MvcHelper.GetHtmlHelper(GetTextAreaViewData());

			// Act
			var html = helper.TextAreaControlGroupFor(m => m.Foo, 0, 0, null);

			// Assert
			var sb = new StringBuilder();
			sb.AppendLine(@"<div class=""form-group""><label class=""control-label"" for=""Foo"">Foo</label><textarea class=""form-control"" id=""Foo"" name=""Foo"">");
			sb.Append(@"ViewItemFoo</textarea></div>");

			Assert.AreEqual(sb.ToString(), html.ToHtmlString());
		}

		[Test]
		public void TextAreaControlGroupForWithObjectAttributes()
		{
			// Arrange
			var helper = MvcHelper.GetHtmlHelper(GetTextAreaViewData());

			// Act
			var html = helper.TextAreaControlGroupFor(m => m.Foo, TextAreaAttributesObjectDictionary);

			// Assert
			var sb = new StringBuilder();
			sb.AppendLine(@"<div class=""form-group""><label class=""control-label"" for=""Foo"">Foo</label><textarea class=""form-control"" cols=""12"" id=""Foo"" name=""Foo"" rows=""15"">");
			sb.Append(@"ViewItemFoo</textarea></div>");


			Assert.AreEqual(sb.ToString(), html.ToHtmlString());
		}

		[Test]
		public void TextAreaControlGroupForWithObjectAttributesWithUnderscores()
		{
			// Arrange
			var helper = MvcHelper.GetHtmlHelper(GetTextAreaViewData());

			// Act
			var html = helper.TextAreaControlGroupFor(m => m.Foo, TextAreaAttributesObjectUnderscoresDictionary);

			// Assert
			var sb = new StringBuilder();
			sb.AppendLine(@"<div class=""form-group""><label class=""control-label"" for=""Foo"">Foo</label><textarea class=""form-control"" cols=""12"" foo-bar=""baz"" id=""Foo"" name=""Foo"" rows=""15"">");
			sb.Append(@"ViewItemFoo</textarea></div>");

			Assert.AreEqual(sb.ToString(), html.ToHtmlString());
		}

		[Test]
		public void TextAreaControlGroupForWithDictionaryAttributes()
		{
			// Arrange
			var helper = MvcHelper.GetHtmlHelper(GetTextAreaViewData());

			// Act
			var html = helper.TextAreaControlGroupFor(m => m.Foo, TextAreaAttributesDictionary);

			// Assert
			var sb = new StringBuilder();
			sb.AppendLine(@"<div class=""form-group""><label class=""control-label"" for=""Foo"">Foo</label><textarea class=""form-control"" cols=""12"" id=""Foo"" name=""Foo"" rows=""15"">");
			sb.Append(@"ViewItemFoo</textarea></div>");

			Assert.AreEqual(sb.ToString(), html.ToHtmlString());
		}

		[Test]
		public void TextAreaControlGroupForWithViewDataErrors()
		{
			// Arrange
			var helper = MvcHelper.GetHtmlHelper(GetTextAreaViewDataWithErrors());

			// Act
			var html = helper.TextAreaControlGroupFor(m => m.Foo, TextAreaAttributesObjectDictionary);

			// Assert
			var sb = new StringBuilder();
			sb.AppendLine(@"<div class=""has-error form-group""><label class=""control-label"" for=""Foo"">Foo</label><textarea class=""input-validation-error form-control"" cols=""12"" id=""Foo"" name=""Foo"" rows=""15"">");
			sb.Append(@"AttemptedValueFoo</textarea></div>");


			Assert.AreEqual(sb.ToString(), html.ToHtmlString());
		}

		[Test]
		public void TextAreaControlGroupForWithViewDataErrorsAndCustomClass()
		{
			// Arrange
			var helper = MvcHelper.GetHtmlHelper(GetTextAreaViewDataWithErrors());

			// Act
			var html = helper.TextAreaControlGroupFor(m => m.Foo, new { @class = "foo-class" });

			// Assert
			var sb = new StringBuilder();
			sb.AppendLine(@"<div class=""has-error form-group""><label class=""control-label"" for=""Foo"">Foo</label><textarea class=""input-validation-error foo-class form-control"" cols=""20"" id=""Foo"" name=""Foo"" rows=""2"">");
			sb.Append(@"AttemptedValueFoo</textarea></div>");


			Assert.AreEqual(sb.ToString(), html.ToHtmlString());
		}

		[Test]
		public void TextAreaControlGroupForWithPrefix()
		{
			// Arrange
			var helper = MvcHelper.GetHtmlHelper(GetTextAreaViewData());
			helper.ViewContext.ViewData.TemplateInfo.HtmlFieldPrefix = "MyPrefix";

			// Act
			var html = helper.TextAreaControlGroupFor(m => m.Foo);

			// Assert
			var sb = new StringBuilder();
			sb.AppendLine(@"<div class=""form-group""><label class=""control-label"" for=""MyPrefix_Foo"">MyPrefix_Foo</label><textarea class=""form-control"" cols=""20"" id=""MyPrefix_Foo"" name=""MyPrefix.Foo"" rows=""2"">");
			sb.Append(@"ViewItemFoo</textarea></div>");

			Assert.AreEqual(sb.ToString(), html.ToHtmlString());
		}

		[Test]
		public void TextAreaControlGroupForWithPrefixAndEmptyName()
		{
			// Arrange
			var helper = MvcHelper.GetHtmlHelper(GetTextAreaViewData());
			helper.ViewContext.ViewData.TemplateInfo.HtmlFieldPrefix = "MyPrefix";

			// Act
			var html = helper.TextAreaControlGroupFor(m => m);

			// Assert
			var sb = new StringBuilder();
			sb.AppendLine(@"<div class=""form-group""><label class=""control-label"" for=""MyPrefix"">MyPrefix</label><textarea class=""form-control"" cols=""20"" id=""MyPrefix"" name=""MyPrefix"" rows=""2"">");
			sb.Append(@"Mvc.Bootstrap.Test.TextAreaModel</textarea></div>");

			Assert.AreEqual(sb.ToString(), html.ToHtmlString());
		}

		[Test]
		public void TextAreaControlGroupForParameterDictionaryMergingWithObjectValues()
		{
			// Arrange
			var helper = MvcHelper.GetHtmlHelper(GetTextAreaViewData());

			// Act
			var html = helper.TextAreaControlGroupFor(m => m.Foo, 10, 25, new { rows = "30" });

			// Assert
			var sb = new StringBuilder();
			sb.AppendLine(@"<div class=""form-group""><label class=""control-label"" for=""Foo"">Foo</label><textarea class=""form-control"" cols=""25"" id=""Foo"" name=""Foo"" rows=""10"">");
			sb.Append(@"ViewItemFoo</textarea></div>");

			Assert.AreEqual(sb.ToString(), html.ToHtmlString());
		}

		[Test]
		public void TextAreaControlGroupForParameterDictionaryMergingWithObjectValuesWithUnderscores()
		{
			// Arrange
			var helper = MvcHelper.GetHtmlHelper(GetTextAreaViewData());

			// Act
			var html = helper.TextAreaControlGroupFor(m => m.Foo, 10, 25, new { rows = "30", foo_bar = "baz" });

			// Assert
			var sb = new StringBuilder();
			sb.AppendLine(@"<div class=""form-group""><label class=""control-label"" for=""Foo"">Foo</label><textarea class=""form-control"" cols=""25"" foo-bar=""baz"" id=""Foo"" name=""Foo"" rows=""10"">");
			sb.Append(@"ViewItemFoo</textarea></div>");

			Assert.AreEqual(sb.ToString(), html.ToHtmlString());
		}

		[Test]
		public void TextAreaControlGroupForParameterDictionaryMergingWithDictionaryValues()
		{
			// Arrange
			var helper = MvcHelper.GetHtmlHelper(GetTextAreaViewData());

			// Act
			var html = helper.TextAreaControlGroupFor(m => m.Foo, 10, 25, new RouteValueDictionary(new { rows = "30" }));

			// Assert
			var sb = new StringBuilder();
			sb.AppendLine(@"<div class=""form-group""><label class=""control-label"" for=""Foo"">Foo</label><textarea class=""form-control"" cols=""25"" id=""Foo"" name=""Foo"" rows=""10"">");
			sb.Append(@"ViewItemFoo</textarea></div>");

			Assert.AreEqual(sb.ToString(), html.ToHtmlString());
		}

		[Test, ExpectedException(typeof(ArgumentNullException))]
		public void DropDownListControlGroupForWithNullExpressionThrows()
		{
			// Arrange
			var helper = MvcHelper.GetHtmlHelper(new ViewDataDictionary<object>());
			var selectList = new SelectList(GetSampleAnonymousObjects(), "Letter", "FullWord", "C");

			// Act & Assert
			helper.DropDownListControlGroupFor<object, object>(null /* expression */, selectList);
		}

		[Test]
		public void DropDownListControlGroupForUsesExplicitValueIfNotProvidedInViewData()
		{
			// Arrange
			var helper = MvcHelper.GetHtmlHelper(new ViewDataDictionary<FooModel>());
			var selectList = new SelectList(GetSampleAnonymousObjects(), "Letter", "FullWord", "C");

			// Act
			var html = helper.DropDownListControlGroupFor(m => m.Foo, selectList, (string)null /* optionLabel */);

			// Assert
			var sb = new StringBuilder();
			sb.AppendLine(@"<div class=""form-group""><label class=""control-label"" for=""Foo"">Foo</label><select class=""form-control"" id=""Foo"" name=""Foo""><option value=""A"">Alpha</option>");
			sb.AppendLine(@"<option value=""B"">Bravo</option>");
			sb.AppendLine(@"<option selected=""selected"" value=""C"">Charlie</option>");
			sb.Append(@"</select></div>");
			Assert.AreEqual(sb.ToString(),html.ToHtmlString());
		}

		[Test, Ignore("Cannot get ClientValidationRuleFactory to work. Really needs finishing")]
		public void DropDownListControlGroupForUsesExplicitValueIfNotProvidedInViewData_Unobtrusive()
		{
			// Arrange
			var helper = MvcHelper.GetHtmlHelper(new ViewDataDictionary<FooModel>());
			helper.ViewContext.ClientValidationEnabled = true;
			helper.ViewContext.UnobtrusiveJavaScriptEnabled = true;
			helper.ViewContext.FormContext = new FormContext();
			//helper.ClientValidationRuleFactory = (name, metadata) => new[] { new ModelClientValidationRule { ValidationType = "type", ErrorMessage = "error" } };
			var selectList = new SelectList(GetSampleAnonymousObjects(), "Letter", "FullWord", "C");

			// Act
			var html = helper.DropDownListControlGroupFor(m => m.Foo, selectList, (string)null /* optionLabel */);

			// Assert
			Assert.AreEqual(
				@"<select class=""form-control"" data-val=""true"" data-val-type=""error"" id=""foo"" name=""foo""><option value=""A"">Alpha</option>
<option value=""B"">Bravo</option>
<option selected=""selected"" value=""C"">Charlie</option>
</select>",
				html.ToHtmlString());
		}

		[Test]
		public void DropDownListControlGroupForWithEnumerableModel_Unobtrusive()
		{
			// Arrange
			var helper = MvcHelper.GetHtmlHelper(new ViewDataDictionary<IEnumerable<RequiredModel>>());
			helper.ViewContext.ClientValidationEnabled = true;
			helper.ViewContext.UnobtrusiveJavaScriptEnabled = true;
			helper.ViewContext.FormContext = new FormContext();
			helper.ViewContext.ViewData.TemplateInfo.HtmlFieldPrefix = "MyPrefix";

			var selectList = new SelectList(GetSampleAnonymousObjects(), "Letter", "FullWord", "C");

			using (HtmlHelperTest.ReplaceCulture("en-US", "en-US"))
			{
				// Act
				var html = helper.DropDownListControlGroupFor(m => m.ElementAt(0).Foo, selectList);

				// Assert
				Assert.AreEqual(
					@"<div class=""form-group""><label class=""control-label"" for=""MyPrefix_Foo"">MyPrefix_Foo</label><select class=""form-control"" data-val=""true"" data-val-required=""The Foo field is required."" id=""MyPrefix_Foo"" name=""MyPrefix.Foo""><option value=""A"">Alpha</option>" + Environment.NewLine + @"<option value=""B"">Bravo</option>" + Environment.NewLine + @"<option selected=""selected"" value=""C"">Charlie</option>" + Environment.NewLine + "</select></div>",
					html.ToHtmlString());
			}
		}

		[Test]
		public void DropDownListControlGroupForUsesViewDataDefaultValue()
		{
			// Arrange
			var helper = MvcHelper.GetHtmlHelper(DropDownListViewData);
			var selectList = new SelectList(GetSampleStrings(), "Charlie");

			// Act
			var html = helper.DropDownListControlGroupFor(m => m.Foo, selectList, (string)null /* optionLabel */);

			// Assert
			var sb = new StringBuilder();
			sb.AppendLine(@"<div class=""form-group""><label class=""control-label"" for=""Foo"">Foo</label><select class=""form-control"" id=""Foo"" name=""Foo""><option>Alpha</option>");
			sb.AppendLine(@"<option selected=""selected"">Bravo</option>");
			sb.AppendLine(@"<option>Charlie</option>");
			sb.Append(@"</select></div>");

			Assert.AreEqual(sb.ToString(), html.ToHtmlString());
		}

		[Test]
		public void DropDownListControlGroupForWithAttributesDictionary()
		{
			// Arrange
			var helper = MvcHelper.GetHtmlHelper(new ViewDataDictionary<FooModel>());
			var selectList = new SelectList(GetSampleStrings());

			// Act
			var html = helper.DropDownListControlGroupFor(m => m.Foo, selectList, null /* optionLabel */, HtmlHelperTest.AttributesDictionary);

			// Assert
			var sb = new StringBuilder();
			sb.AppendLine(@"<div class=""form-group""><label class=""control-label"" for=""Foo"">Foo</label><select baz=""BazValue"" class=""form-control"" id=""Foo"" name=""Foo""><option>Alpha</option>");
			sb.AppendLine(@"<option>Bravo</option>");
			sb.AppendLine(@"<option>Charlie</option>");
			sb.Append(@"</select></div>");

			Assert.AreEqual(sb.ToString(), html.ToHtmlString());
		}

		[Test]
		public void DropDownListControlGroupForWithErrorsAndCustomClass()
		{
			// Arrange
			var helper = MvcHelper.GetHtmlHelper(GetViewDataWithErrors());
			var selectList = new SelectList(GetSampleStrings());

			// Act
			var html = helper.DropDownListControlGroupFor(m => m.Foo, selectList, null /* optionLabel */, new { @class = "foo-class" });

			// Assert
			var sb = new StringBuilder();
			sb.AppendLine(@"<div class=""has-error form-group""><label class=""control-label"" for=""Foo"">Foo</label><select class=""input-validation-error foo-class form-control"" id=""Foo"" name=""Foo""><option>Alpha</option>");
			sb.AppendLine(@"<option selected=""selected"">Bravo</option>");
			sb.AppendLine(@"<option>Charlie</option>");
			sb.Append(@"</select></div>");

			Assert.AreEqual(sb.ToString(),html.ToHtmlString());
		}

		[Test]
		public void DropDownListControlGroupForWithObjectDictionary()
		{
			// Arrange
			var helper = MvcHelper.GetHtmlHelper(new ViewDataDictionary<FooModel>());
			var selectList = new SelectList(GetSampleStrings());

			// Act
			var html = helper.DropDownListControlGroupFor(m => m.Foo, selectList, null /* optionLabel */, HtmlHelperTest.AttributesObjectDictionary);

			// Assert
			var sb = new StringBuilder();
			sb.AppendLine(@"<div class=""form-group""><label class=""control-label"" for=""Foo"">Foo</label><select baz=""BazObjValue"" class=""form-control"" id=""Foo"" name=""Foo""><option>Alpha</option>");
			sb.AppendLine(@"<option>Bravo</option>");
			sb.AppendLine(@"<option>Charlie</option>");
			sb.Append(@"</select></div>");
			
			Assert.AreEqual(sb.ToString(), html.ToHtmlString());
		}

		[Test]
		public void DropDownListControlGroupForWithObjectDictionaryWithUnderscores()
		{
			// Arrange
			var helper = MvcHelper.GetHtmlHelper(new ViewDataDictionary<FooModel>());
			var selectList = new SelectList(GetSampleStrings());

			// Act
			var html = helper.DropDownListControlGroupFor(m => m.Foo, selectList, null /* optionLabel */, HtmlHelperTest.AttributesObjectUnderscoresDictionary);

			// Assert
			var sb = new StringBuilder();
			sb.AppendLine(@"<div class=""form-group""><label class=""control-label"" for=""Foo"">Foo</label><select class=""form-control"" foo-baz=""BazObjValue"" id=""Foo"" name=""Foo""><option>Alpha</option>");
			sb.AppendLine(@"<option>Bravo</option>");
			sb.AppendLine(@"<option>Charlie</option>");
			sb.Append(@"</select></div>");

			Assert.AreEqual(sb.ToString(), html.ToHtmlString());
		}

		[Test]
		public void DropDownListControlGroupForWithObjectDictionaryAndSelectListNoOptionLabel()
		{
			// Arrange
			var helper = MvcHelper.GetHtmlHelper(new ViewDataDictionary<FooModel>());
			var selectList = new SelectList(GetSampleStrings());

			// Act
			var html = helper.DropDownListControlGroupFor(m => m.Foo, selectList, HtmlHelperTest.AttributesObjectDictionary);

			// Assert
			var sb = new StringBuilder();
			sb.AppendLine(@"<div class=""form-group""><label class=""control-label"" for=""Foo"">Foo</label><select baz=""BazObjValue"" class=""form-control"" id=""Foo"" name=""Foo""><option>Alpha</option>");
			sb.AppendLine(@"<option>Bravo</option>");
			sb.AppendLine(@"<option>Charlie</option>");
			sb.Append(@"</select></div>");


			Assert.AreEqual(sb.ToString(), html.ToHtmlString());
		}

		[Test]
		public void DropDownListControlGroupForWithObjectDictionaryWithUnderscoresAndSelectListNoOptionLabel()
		{
			// Arrange
			var helper = MvcHelper.GetHtmlHelper(new ViewDataDictionary<FooModel>());
			var selectList = new SelectList(GetSampleStrings());

			// Act
			var html = helper.DropDownListControlGroupFor(m => m.Foo, selectList, HtmlHelperTest.AttributesObjectUnderscoresDictionary);

			// Assert
			var sb = new StringBuilder();
			sb.AppendLine(@"<div class=""form-group""><label class=""control-label"" for=""Foo"">Foo</label><select class=""form-control"" foo-baz=""BazObjValue"" id=""Foo"" name=""Foo""><option>Alpha</option>");
			sb.AppendLine(@"<option>Bravo</option>");
			sb.AppendLine(@"<option>Charlie</option>");
			sb.Append(@"</select></div>");

			Assert.AreEqual(sb.ToString(), html.ToHtmlString());
		}

		[Test]
		public void DropDownListControlGroupForWithObjectDictionaryAndEmptyOptionLabel()
		{
			// Arrange
			var helper = MvcHelper.GetHtmlHelper(new ViewDataDictionary<FooModel>());
			var selectList = new SelectList(GetSampleStrings());

			// Act
			var html = helper.DropDownListControlGroupFor(m => m.Foo, selectList, String.Empty /* optionLabel */, HtmlHelperTest.AttributesObjectDictionary);

			// Assert
			var sb = new StringBuilder();
			sb.AppendLine(@"<div class=""form-group""><label class=""control-label"" for=""Foo"">Foo</label><select baz=""BazObjValue"" class=""form-control"" id=""Foo"" name=""Foo""><option value=""""></option>");
			sb.AppendLine(@"<option>Alpha</option>");
			sb.AppendLine(@"<option>Bravo</option>");
			sb.AppendLine(@"<option>Charlie</option>");
			sb.Append(@"</select></div>");


			Assert.AreEqual(sb.ToString(), html.ToHtmlString());
		}

		[Test]
		public void DropDownListControlGroupForWithObjectDictionaryAndTitle()
		{
			// Arrange
			var helper = MvcHelper.GetHtmlHelper(new ViewDataDictionary<FooModel>());
			var selectList = new SelectList(GetSampleStrings());

			// Act
			var html = helper.DropDownListControlGroupFor(m => m.Foo, selectList, "[Select Something]", HtmlHelperTest.AttributesObjectDictionary);

			// Assert
			var sb = new StringBuilder();
			sb.AppendLine(@"<div class=""form-group""><label class=""control-label"" for=""Foo"">Foo</label><select baz=""BazObjValue"" class=""form-control"" id=""Foo"" name=""Foo""><option value="""">[Select Something]</option>");
			sb.AppendLine(@"<option>Alpha</option>");
			sb.AppendLine(@"<option>Bravo</option>");
			sb.AppendLine(@"<option>Charlie</option>");
			sb.Append(@"</select></div>");

			Assert.AreEqual(sb.ToString(), html.ToHtmlString());
		}

		[Test]
		public void DropDownListControlGroupForWithIEnumerableSelectListItemSelectsDefaultFromViewData()
		{
			// Arrange
			var vdd = new ViewDataDictionary<FooModel> { { "Foo", "123456789" } };
			var helper = MvcHelper.GetHtmlHelper(vdd);

			// Act
			var html = helper.DropDownListControlGroupFor(m => m.Foo, GetSampleIEnumerableObjects());

			// Assert
			var sb = new StringBuilder();
			sb.AppendLine(@"<div class=""form-group""><label class=""control-label"" for=""Foo"">Foo</label><select class=""form-control"" id=""Foo"" name=""Foo""><option selected=""selected"" value=""123456789"">John</option>");
			sb.AppendLine(@"<option value=""987654321"">Jane</option>");
			sb.AppendLine(@"<option value=""111111111"">Joe</option>");
			sb.Append(@"</select></div>");
			
			Assert.AreEqual(sb.ToString(), html.ToHtmlString());
		}

		[Test]
		public void DropDownListControlGroupForWithListOfSelectListItemSelectsDefaultFromViewData()
		{
			// Arrange
			var vdd = new ViewDataDictionary<FooModel> { { "foo", "123456789" } };
			var helper = MvcHelper.GetHtmlHelper(vdd);

			// Act
			var html = helper.DropDownListControlGroupFor(m => m.Foo, GetSampleListObjects());

			// Assert
			var sb = new StringBuilder();
			sb.AppendLine(@"<div class=""form-group""><label class=""control-label"" for=""Foo"">Foo</label><select class=""form-control"" id=""Foo"" name=""Foo""><option selected=""selected"" value=""123456789"">John</option>");
			sb.AppendLine(@"<option value=""987654321"">Jane</option>");
			sb.AppendLine(@"<option value=""111111111"">Joe</option>");
			sb.Append(@"</select></div>");


			Assert.AreEqual(sb.ToString(), html.ToHtmlString());
		}

		[Test]
		public void DropDownListControlGroupForWithPrefix()
		{
			// Arrange
			var helper = MvcHelper.GetHtmlHelper(new ViewDataDictionary<FooModel>());
			var selectList = new SelectList(GetSampleStrings());
			helper.ViewContext.ViewData.TemplateInfo.HtmlFieldPrefix = "MyPrefix";

			// Act
			var html = helper.DropDownListControlGroupFor(m => m.Foo, selectList, HtmlHelperTest.AttributesObjectDictionary);

			// Assert
			var sb = new StringBuilder();
			sb.AppendLine(@"<div class=""form-group""><label class=""control-label"" for=""MyPrefix_Foo"">MyPrefix_Foo</label><select baz=""BazObjValue"" class=""form-control"" id=""MyPrefix_Foo"" name=""MyPrefix.Foo""><option>Alpha</option>");
			sb.AppendLine(@"<option>Bravo</option>");
			sb.AppendLine(@"<option>Charlie</option>");
			sb.Append(@"</select></div>");


			Assert.AreEqual(sb.ToString(), html.ToHtmlString());
		}

		[Test]
		public void DropDownListControlGroupForWithPrefixAndEmptyName()
		{
			// Arrange
			var helper = MvcHelper.GetHtmlHelper(new ViewDataDictionary<FooModel>());
			var selectList = new SelectList(GetSampleStrings());
			helper.ViewContext.ViewData.TemplateInfo.HtmlFieldPrefix = "MyPrefix";

			// Act
			var html = helper.DropDownListControlGroupFor(m => m, selectList, HtmlHelperTest.AttributesObjectDictionary);

			// Assert
			var sb = new StringBuilder();
			sb.AppendLine(@"<div class=""form-group""><label class=""control-label"" for=""MyPrefix"">MyPrefix</label><select baz=""BazObjValue"" class=""form-control"" id=""MyPrefix"" name=""MyPrefix""><option>Alpha</option>");
			sb.AppendLine(@"<option>Bravo</option>");
			sb.AppendLine(@"<option>Charlie</option>");
			sb.Append(@"</select></div>");


			Assert.AreEqual(sb.ToString(), html.ToHtmlString());
		}

		[Test]
		public void DropDownListControlGroupForWithPrefixAndIEnumerableSelectListItemSelectsDefaultFromViewData()
		{
			// Arrange
			var vdd = new ViewDataDictionary<FooModel> { { "foo", "123456789" } };
			vdd.TemplateInfo.HtmlFieldPrefix = "MyPrefix";
			var helper = MvcHelper.GetHtmlHelper(vdd);

			// Act
			var html = helper.DropDownListControlGroupFor(m => m.Foo, GetSampleIEnumerableObjects());

			// Assert
			var sb = new StringBuilder();
			sb.AppendLine(@"<div class=""form-group""><label class=""control-label"" for=""MyPrefix_Foo"">MyPrefix_Foo</label><select class=""form-control"" id=""MyPrefix_Foo"" name=""MyPrefix.Foo""><option selected=""selected"" value=""123456789"">John</option>");
			sb.AppendLine(@"<option value=""987654321"">Jane</option>");
			sb.AppendLine(@"<option value=""111111111"">Joe</option>");
			sb.Append(@"</select></div>");

			Assert.AreEqual(sb.ToString(), html.ToHtmlString());
		}

		[Test]
		public void ButtonDefaultWithAttributes()
		{
			var helper = MvcHelper.GetHtmlHelper(GetTextBoxViewData());
			var attributes = new { id = "search"};

			var button = helper.BootstrapButton("Create", Buttons.Default, attributes);

			Assert.AreEqual(@"<button class=""btn"" id=""search"">Create</button>", button.ToHtmlString());
		}

		[Test]
		public void ButtonDefault()
		{
			var helper = MvcHelper.GetHtmlHelper(GetTextBoxViewData());

			var button = helper.BootstrapButton("Create", Buttons.Default);

			Assert.AreEqual(@"<button class=""btn"">Create</button>", button.ToHtmlString());
		}

		[Test]
		public void ButtonDanger()
		{
			var helper = MvcHelper.GetHtmlHelper(GetTextBoxViewData());

			var button = helper.BootstrapButton("Create", Buttons.Danger);

			Assert.AreEqual(@"<button class=""btn btn-danger"">Create</button>", button.ToHtmlString());
		}

		[Test]
		public void ButtonInfo()
		{
			var helper = MvcHelper.GetHtmlHelper(GetTextBoxViewData());

			var button = helper.BootstrapButton("Create", Buttons.Info);

			Assert.AreEqual(@"<button class=""btn btn-info"">Create</button>", button.ToHtmlString());
		}

		[Test]
		public void ButtonInverse()
		{
			var helper = MvcHelper.GetHtmlHelper(GetTextBoxViewData());

			var button = helper.BootstrapButton("Create", Buttons.Inverse);

			Assert.AreEqual(@"<button class=""btn btn-inverse"">Create</button>", button.ToHtmlString());
		}

		[Test]
		public void ButtonPrimary()
		{
			var helper = MvcHelper.GetHtmlHelper(GetTextBoxViewData());

			var button = helper.BootstrapButton("Create", Buttons.Primary);

			Assert.AreEqual(@"<button class=""btn btn-primary"">Create</button>", button.ToHtmlString());
		}

		[Test]
		public void ButtonSuccess()
		{
			var helper = MvcHelper.GetHtmlHelper(GetTextBoxViewData());

			var button = helper.BootstrapButton("Create", Buttons.Success);

			Assert.AreEqual(@"<button class=""btn btn-success"">Create</button>", button.ToHtmlString());
		}

		[Test]
		public void ButtonWarning()
		{
			var helper = MvcHelper.GetHtmlHelper(GetTextBoxViewData());

			var button = helper.BootstrapButton("Create", Buttons.Warning);

			Assert.AreEqual(@"<button class=""btn btn-warning"">Create</button>", button.ToHtmlString());
		}

		[Test]
		public void ButtonControlGroup()
		{
			var helper = MvcHelper.GetHtmlHelper(GetTextBoxViewData());

			var button = helper.ButtonControlGroup("Create", Buttons.Warning);

			Assert.AreEqual(@"<div class=""form-group""><button class=""btn btn-warning"">Create</button></div>", button.ToHtmlString());
		}

		[Test]
		public void ButtonControlGroupWithAttributes()
		{
			var helper = MvcHelper.GetHtmlHelper(GetTextBoxViewData());
			var attributes = new { id = "search" };

			var button = helper.ButtonControlGroup("Create", Buttons.Warning, attributes);

			Assert.AreEqual(@"<div class=""form-group""><button class=""btn btn-warning"" id=""search"">Create</button></div>", button.ToHtmlString());
		}

		[Test]
		public void ButtonFormAction()
		{
			var helper = MvcHelper.GetHtmlHelper(GetTextBoxViewData());

			var button = helper.ButtonFormAction("Create", Buttons.Warning);

			Assert.AreEqual(@"<div class=""form-actions""><button class=""btn btn-warning"">Create</button></div>", button.ToHtmlString());
		}

		[Test]
		public void ButtonFormActionWithAttributes()
		{
			var helper = MvcHelper.GetHtmlHelper(GetTextBoxViewData());
			var attributes = new { id = "search" };

			var button = helper.ButtonFormAction("Create", Buttons.Warning, attributes);

			Assert.AreEqual(@"<div class=""form-actions""><button class=""btn btn-warning"" id=""search"">Create</button></div>", button.ToHtmlString());
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

		private static ViewDataDictionary<TextAreaModel> GetTextAreaViewData()
		{
			var viewData = new ViewDataDictionary<TextAreaModel> { { "Foo", "ViewDataFoo" } };
			viewData.Model = new TextAreaModel { Foo = "ViewItemFoo", Bar = "ViewItemBar" };
			return viewData;
		}

		private static ViewDataDictionary<TextAreaModel> GetTextAreaViewDataWithErrors()
		{
			var viewData = new ViewDataDictionary<TextAreaModel> { { "Foo", "ViewDataFoo" } };
			viewData.Model = new TextAreaModel { Foo = "ViewItemFoo", Bar = "ViewItemBar" };

			var modelStateFoo = new ModelState();
			modelStateFoo.Errors.Add(new ModelError("Foo error 1"));
			modelStateFoo.Errors.Add(new ModelError("Foo error 2"));
			viewData.ModelState["Foo"] = modelStateFoo;
			modelStateFoo.Value = HtmlHelperTest.GetValueProviderResult(new[] { "AttemptedValueFoo" }, "AttemptedValueFoo");

			return viewData;
		}

		private static IEnumerable GetSampleAnonymousObjects()
		{
			return new[]
            {
                new { Letter = 'A', FullWord = "Alpha" },
                new { Letter = 'B', FullWord = "Bravo" },
                new { Letter = 'C', FullWord = "Charlie" }
            };
		}

		private static IEnumerable<SelectListItem> GetSampleListObjects()
        {
			const string selectedSsn = "111111111";

			return GetSamplePeople().Select(person => new SelectListItem
			                                          	{
			                                          		Text = person.FirstName, Value = person.Ssn, Selected = String.Equals(person.Ssn, selectedSsn)
			                                          	}).ToList();
        }

		private static IEnumerable GetSampleStrings()
		{
			return new[] { "Alpha", "Bravo", "Charlie" };
		}

		private static IEnumerable<SelectListItem> GetSampleIEnumerableObjects()
		{
			var people = GetSamplePeople();

			const string selectedSsn = "111111111";
			var list = from person in people
											   select new SelectListItem
											   {
												   Text = person.FirstName,
												   Value = person.Ssn,
												   Selected = String.Equals(person.Ssn, selectedSsn)
											   };
			return list;
		}

		private static ViewDataDictionary<FooBarModel> GetViewDataWithErrors()
		{
			var viewData = new ViewDataDictionary<FooBarModel> { { "Foo", "ViewDataFoo" } };
			viewData.Model = new FooBarModel { Foo = "ViewItemFoo", Bar = "ViewItemBar" };

			var modelStateFoo = new ModelState();
			modelStateFoo.Errors.Add(new ModelError("Foo error 1"));
			modelStateFoo.Errors.Add(new ModelError("Foo error 2"));
			viewData.ModelState["Foo"] = modelStateFoo;
			modelStateFoo.Value = new ValueProviderResult(new[] { "Bravo", "Charlie" }, "Bravo", CultureInfo.InvariantCulture);

			return viewData;
		}

		private class RequiredModel
		{
			[Required]
			public string Foo { get; set; }
		}

		internal static Person[] GetSamplePeople()
		{
			return new[]
            {
                new Person
                {
                    FirstName = "John",
                    Ssn = "123456789"
                },
                new Person
                {
                    FirstName = "Jane",
                    Ssn = "987654321"
                },
                new Person
                {
                    FirstName = "Joe",
                    Ssn = "111111111"
                }
            };
		}

		internal class Person
		{
			public string FirstName { get; set; }

			public string Ssn { get; set; }
		}
	}

	internal class TextAreaModel
	{
		public string Foo { get; set; }
		public string Bar { get; set; }
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
