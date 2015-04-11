using System;
using System.Collections;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Moq;

namespace Mvc.Bootstrap.Test
{
	public static class MvcHelper
	{
		public const string AppPathModifier = "/$(SESSION)";

		public static HtmlHelper<object> GetHtmlHelper()
		{
			var httpcontext = GetHttpContext("/app/", null, null);
			var rt = new RouteCollection
			         	{
			         		new Route("{controller}/{action}/{id}", null)
			         			{Defaults = new RouteValueDictionary(new {id = "defaultid"})},
			         		{
			         			"namedroute",
			         			new Route("named/{controller}/{action}/{id}", null)
			         				{Defaults = new RouteValueDictionary(new {id = "defaultid"})}
			         			}
			         	};
			var rd = new RouteData();
			rd.Values.Add("controller", "home");
			rd.Values.Add("action", "oldaction");

			var vdd = new ViewDataDictionary();

			var viewContext = new ViewContext 
				{
					HttpContext = httpcontext,
					RouteData = rd,
					ViewData = vdd
				};
			var mockVdc = new Mock<IViewDataContainer>();
			mockVdc.Setup(vdc => vdc.ViewData).Returns(vdd);

			var htmlHelper = new HtmlHelper<object>(viewContext, mockVdc.Object, rt);
			return htmlHelper;
		}

		public static HtmlHelper<TModel> GetHtmlHelper<TModel>(ViewDataDictionary<TModel> viewData)
		{
			var mockViewContext = new Mock<ViewContext> { CallBase = true };
			mockViewContext.Setup(c => c.ViewData).Returns(viewData);
			mockViewContext.Setup(c => c.HttpContext.Items).Returns(new Hashtable());
			var container = GetViewDataContainer(viewData);
			return new HtmlHelper<TModel>(mockViewContext.Object, container);
		}

		public static IViewDataContainer GetViewDataContainer(ViewDataDictionary viewData)
		{
			var mockContainer = new Mock<IViewDataContainer>();
			mockContainer.Setup(c => c.ViewData).Returns(viewData);
			return mockContainer.Object;
		}

		public static HttpContextBase GetHttpContext(string appPath, string requestPath, string httpMethod)
		{
			return GetHttpContext(appPath, requestPath, httpMethod, Uri.UriSchemeHttp, -1);
		}

		public static HttpContextBase GetHttpContext(string appPath, string requestPath, string httpMethod, string protocol, int port)
		{
			var mockHttpContext = new Mock<HttpContextBase>();

			if (!String.IsNullOrEmpty(appPath))
			{
				mockHttpContext.Setup(o => o.Request.ApplicationPath).Returns(appPath);
			}
			if (!String.IsNullOrEmpty(requestPath))
			{
				mockHttpContext.Setup(o => o.Request.AppRelativeCurrentExecutionFilePath).Returns(requestPath);
			}

			var uri = port >= 0 ? new Uri(protocol + "://localhost" + ":" + Convert.ToString(port)) : new Uri(protocol + "://localhost");
			mockHttpContext.Setup(o => o.Request.Url).Returns(uri);

			mockHttpContext.Setup(o => o.Request.PathInfo).Returns(String.Empty);
			if (!String.IsNullOrEmpty(httpMethod))
			{
				mockHttpContext.Setup(o => o.Request.HttpMethod).Returns(httpMethod);
			}

			mockHttpContext.Setup(o => o.Session).Returns((HttpSessionStateBase)null);
			mockHttpContext.Setup(o => o.Response.ApplyAppPathModifier(It.IsAny<string>())).Returns<string>(r => AppPathModifier + r);
			mockHttpContext.Setup(o => o.Items).Returns(new Hashtable());
			return mockHttpContext.Object;
		}
	}
}
