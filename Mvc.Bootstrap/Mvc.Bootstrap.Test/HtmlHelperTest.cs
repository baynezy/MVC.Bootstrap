using System.Globalization;
using System.Web.Mvc;

namespace Mvc.Bootstrap.Test
{
	class HtmlHelperTest
	{
		public static ValueProviderResult GetValueProviderResult(object rawValue, string attemptedValue)
		{
			return new ValueProviderResult(rawValue, attemptedValue, CultureInfo.InvariantCulture);
		}
	}
}
