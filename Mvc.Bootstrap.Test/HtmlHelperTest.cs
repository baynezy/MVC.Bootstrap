﻿using System.Collections.Generic;
using System.Globalization;
﻿using System;
﻿using System.Threading;
using System.Web.Mvc;
using System.Web.Routing;

namespace Mvc.Bootstrap.Test
{
	class HtmlHelperTest
	{

		public static readonly RouteValueDictionary AttributesDictionary = new RouteValueDictionary(new { baz = "BazValue" });
		public static readonly object AttributesObjectDictionary = new { baz = "BazObjValue" };
		public static readonly object AttributesObjectUnderscoresDictionary = new { foo_baz = "BazObjValue" };
		
		public static ValueProviderResult GetValueProviderResult(object rawValue, string attemptedValue)
		{
			return new ValueProviderResult(rawValue, attemptedValue, CultureInfo.InvariantCulture);
		}

		internal static IDisposable ReplaceCulture(string currentCulture, string currentUiCulture)
		{
			var newCulture = CultureInfo.GetCultureInfo(currentCulture);
			var newUiCulture = CultureInfo.GetCultureInfo(currentUiCulture);
			var originalCulture = Thread.CurrentThread.CurrentCulture;
			var originalUiCulture = Thread.CurrentThread.CurrentUICulture;
			Thread.CurrentThread.CurrentCulture = newCulture;
			Thread.CurrentThread.CurrentUICulture = newUiCulture;
			return new CultureReplacement { OriginalCulture = originalCulture, OriginalUiCulture = originalUiCulture };
		}

		private class CultureReplacement : IDisposable
		{
			public CultureInfo OriginalCulture;
			public CultureInfo OriginalUiCulture;

			public void Dispose()
			{
				Thread.CurrentThread.CurrentCulture = OriginalCulture;
				Thread.CurrentThread.CurrentUICulture = OriginalUiCulture;
			}
		}
	}
}
