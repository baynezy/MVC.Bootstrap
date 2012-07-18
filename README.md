#MVC.Bootstrap#

This is a library project the extend the HtmlHelper class in ASP.Net MVC to allow you create forms for Twitter's Bootstrap CSS project.

##Usage##

Once you build the project you need add a reference to it in your ASP.Net MVC project. So you can use the extension methods in your views you need to update the web.config in your views directory to add a reference to the new .dll.

    <system.web.webPages.razor>
        <host factoryType="System.Web.Mvc.MvcWebRazorHostFactory, System.Web.Mvc, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31BF3856AD364E35" />
        <pages pageBaseType="System.Web.Mvc.WebViewPage">
            <namespaces>
                <add namespace="System.Web.Mvc" />
                <add namespace="System.Web.Mvc.Ajax" />
                <add namespace="System.Web.Mvc.Html" />
                <add namespace="System.Web.Routing" />
                <add namespace="Mvc.Bootstrap.Core"/>
            </namespaces>
        </pages>
    </system.web.webPages.razor>