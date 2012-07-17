﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Mvc.Bootstrap.Core
{
	public static class InputExtensions
	{
		private const string InverseButtonClass = "btn-inverse";
		private const string DangerButtonClass = "btn-danger";
		private const string WarningButtonClass = "btn-warning";
		private const string SuccessButtonClass = "btn-success";
		private const string InfoButtonClass = "btn-info";
		private const string PrimaryButtonClass = "btn-primary";
		private const string ButtonClass = "btn";
		private const string ControlsClass = "controls";
		private const string LabelClass = "control-label";
		private const string HelperClass = "help-inline";
		private const string ControlGroupClass = "control-group";
		private const string ControlGroupErrorClass = "error";

		public static MvcHtmlString BootstrapButton(this HtmlHelper htmlHelper, string label, int type)
		{
			return MvcHtmlString.Create(CreateButton(label, type).ToString());
		}

		public static MvcHtmlString BootstrapButton(this HtmlHelper htmlHelper, string label, int type, object htmlAttributes)
		{
			return CreateButton(label, type, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
		}

		private static MvcHtmlString CreateButton(string label, int type, IDictionary<string, object> htmlAttributes)
		{
			var button = CreateButton(label, type);
			button.MergeAttributes(htmlAttributes);
			return MvcHtmlString.Create(button.ToString());
		}

		public static MvcHtmlString TextBoxControlGroupFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
		{
			return htmlHelper.TextBoxControlGroupFor(expression, format: null);
		}

		public static MvcHtmlString TextBoxControlGroupFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string format)
		{
			return htmlHelper.TextBoxControlGroupFor(expression, format, null);
		}

		public static MvcHtmlString TextBoxControlGroupFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes)
		{
			return htmlHelper.TextBoxControlGroupFor(expression, null, htmlAttributes);
		}

		public static MvcHtmlString TextBoxControlGroupFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string format, object htmlAttributes)
		{
			return htmlHelper.TextBoxControlGroupFor(expression, format, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
		}

		public static MvcHtmlString TextBoxControlGroupFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IDictionary<string, object> htmlAttributes)
		{
			return htmlHelper.TextBoxControlGroupFor(expression, null, htmlAttributes);
		}

		public static MvcHtmlString TextBoxControlGroupFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string format, IDictionary<string, object> htmlAttributes)
		{
			var coreControl = htmlHelper.TextBoxFor(expression, htmlAttributes);

			return Bootstrapify(coreControl);
		}

		public static MvcHtmlString PasswordControlGroupFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
		{
			return PasswordControlGroupFor(htmlHelper, expression, htmlAttributes: null);
		}

		public static MvcHtmlString PasswordControlGroupFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes)
		{
			return PasswordControlGroupFor(htmlHelper, expression, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
		}

		public static MvcHtmlString PasswordControlGroupFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IDictionary<string, object> htmlAttributes)
		{
			var coreControl = htmlHelper.PasswordFor(expression, htmlAttributes);

			return Bootstrapify(coreControl);
		}

		private static MvcHtmlString Bootstrapify(IHtmlString coreControl)
		{
			var controlGroupDiv = ControlGroupDiv();
			var coreHtml = coreControl.ToHtmlString();
			var textBox = Bootstrapify(coreHtml);

			var controlsDiv = ControlsDiv();

			var label = ControlLabel(textBox);

			var errorMessage = HandleErrors(textBox, controlGroupDiv);

			controlsDiv.InnerHtml = coreControl.ToHtmlString() + errorMessage;

			controlGroupDiv.InnerHtml = label + controlsDiv.ToString();

			return MvcHtmlString.Create(controlGroupDiv.ToString());
		}

		private static BootstrapControl Bootstrapify(string html)
		{
			var cssClass = GetClass(html);
			var errorMessage = GetErrorMessage(html);
			var id = GetId(html);

			return new BootstrapControl { Id = id, ErrorMessage = errorMessage, Class = cssClass };
		}

		private static string HandleErrors(BootstrapControl textBox, TagBuilder controlGroupDiv)
		{
			var errorMessage = "";

			if (textBox.IsValid)
			{
				controlGroupDiv.AddCssClass(ControlGroupErrorClass);
				var errorBox = new TagBuilder("span");
				errorBox.AddCssClass(HelperClass);
				errorBox.InnerHtml = textBox.ErrorMessage;
				errorMessage = errorBox.ToString();
			}

			return errorMessage;
		}

		private static TagBuilder ControlLabel(BootstrapControl textBox)
		{
			var label = new TagBuilder("label");
			label.AddCssClass(LabelClass);
			label.MergeAttribute("for", textBox.Id);
			label.InnerHtml = textBox.Id;

			return label;
		}

		private static TagBuilder ControlsDiv()
		{
			var controlsDiv = new TagBuilder("div");
			controlsDiv.AddCssClass(ControlsClass);

			return controlsDiv;
		}

		private static TagBuilder ControlGroupDiv()
		{
			var controlGroupDiv = new TagBuilder("div");
			controlGroupDiv.AddCssClass(ControlGroupClass);

			return controlGroupDiv;
		}

		private static string GetId(string html)
		{
			return GetAttribute("id", html);
		}

		private static string GetErrorMessage(string html)
		{
			return GetAttribute("data-val-required", html);
		}

		private static string GetClass(string html)
		{
			return GetAttribute("class", html);
		}

		private static string GetAttribute(string attribute, string html)
		{
			var value = "";
			var match = Regex.Match(html, attribute + @"=""([^""]*)""", RegexOptions.IgnoreCase);
			if (match.Success)
			{
				value = match.Groups[1].Value;
			}

			return value;
		}

		private static TagBuilder CreateButton(string label, int type)
		{
			var button = new TagBuilder("button");
			button.AddCssClass(GetButtonClass(type));
			button.InnerHtml = label;
			return button;
		}

		private static string GetButtonClass(int type)
		{
			var cssClass = ButtonClass + " ";
			switch (type)
			{
				case Buttons.Primary:
					cssClass += PrimaryButtonClass;
					break;

				case Buttons.Info:
					cssClass += InfoButtonClass;
					break;

				case Buttons.Success:
					cssClass += SuccessButtonClass;
					break;

				case Buttons.Warning:
					cssClass += WarningButtonClass;
					break;

				case Buttons.Danger:
					cssClass += DangerButtonClass;
					break;

				case Buttons.Inverse:
					cssClass += InverseButtonClass;
					break;
			}

			return cssClass;
		}
	}

	internal class BootstrapControl
	{
		public string Id { get; set; }

		public string ErrorMessage { get; set; }

		public string Class { get; set; }

		public bool IsValid
		{
			get
			{
				return Class.Contains("input-validation-error");
			}
		}
	}
}