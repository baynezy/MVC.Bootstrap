#MVC.Bootstrap

This is a library project the extend the HtmlHelper class in ASP.Net MVC to allow you create forms for Twitter's Bootstrap CSS project.

[![Stories in Ready](https://badge.waffle.io/baynezy/MVC.Bootstrap.svg?label=ready&title=Stories%20in%20Ready)](http://waffle.io/baynezy/MVC.Bootstrap)

## Build Status

<table>
    <tr>
        <th>master</th>
		<td><a href="https://ci.appveyor.com/project/baynezy/mvc-bootstrap"><img src="https://ci.appveyor.com/api/projects/status/8x7g55ql67j0ydd3/branch/master?svg=true" alt="master" title="master" /></a></td>
    </tr>
    <tr>
        <th>develop</th>
		<td><a href="https://ci.appveyor.com/project/baynezy/mvc-bootstrap"><img src="https://ci.appveyor.com/api/projects/status/8x7g55ql67j0ydd3/branch/develop?svg=true" alt="develop" title="develop" /></a></td>
    </tr>
</table>

## Documentation
Fully navigable documentation available on [GitHub Pages](http://baynezy.github.io/MVC.Bootstrap/)

##Usage

### Install via NuGet

[![NuGet version](https://badge.fury.io/nu/MVC.Bootstrap.Core.svg)](http://badge.fury.io/nu/MVC.Bootstrap.Core)

    Install-Package Mvc.Bootstrap.Core

### Make Available in Razor

So you can use the extension methods in your views you need to update the web.config in your views directory to add a reference to the new .dll.

    <system.web.webPages.razor>
        <host factoryType="System.Web.Mvc.MvcWebRazorHostFactory, System.Web.Mvc, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31BF3856AD364E35" />
        <pages pageBaseType="System.Web.Mvc.WebViewPage">
            <namespaces>
                <add namespace="System.Web.Mvc" />
                <add namespace="System.Web.Mvc.Ajax" />
                <add namespace="System.Web.Mvc.Html" />
                <add namespace="System.Web.Routing" />
                <add namespace="Mvc.Bootstrap.Core"/> <!--This is the the important bit -->
            </namespaces>
        </pages>
    </system.web.webPages.razor>

### Use in your Views

#### Example Model

    public class Item
    {
        public string Title { get; set; }
        public string Description { get; set; }
    }

#### Usage in a View

    @model Item

    @using (Html.BeginForm("Index", "Home", FormMethod.Post, new {@class = "form-vertical"}))
    {
        @Html.TextBoxControlGroupFor(m => m.Title)
        @Html.TextAreaControlGroupFor(m => m.Description)

        @Html.ButtonFormAction("Submit", Buttons.Primary)
    }

This will create an ASP.Net MVC Input Extension that works with Twitter Bootstrap 3.0. This includes validation and error messages.

##Contributing

###Pull Requests

After forking the repository please create a pull request before creating the fix. This way we can talk about how the fix will be implemented. This will greatly increase your chance of your patch getting merged into the code base.

## License
This project is licensed under [Apache License 2.0](http://www.apache.org/licenses/LICENSE-2.0).
