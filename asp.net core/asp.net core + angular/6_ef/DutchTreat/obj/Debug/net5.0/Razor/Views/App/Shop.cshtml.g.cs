#pragma checksum "C:\other\SelfEducation\asp.net core\asp.net core + angular\6_ef\DutchTreat\Views\App\Shop.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "fb21964028ea82c57d2be60f57ff8fd448ba7fd4"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_App_Shop), @"mvc.1.0.view", @"/Views/App/Shop.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "C:\other\SelfEducation\asp.net core\asp.net core + angular\6_ef\DutchTreat\Views\_ViewImports.cshtml"
using DutchTreat.Controllers;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\other\SelfEducation\asp.net core\asp.net core + angular\6_ef\DutchTreat\Views\_ViewImports.cshtml"
using DutchTreat.ViewModels;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "C:\other\SelfEducation\asp.net core\asp.net core + angular\6_ef\DutchTreat\Views\_ViewImports.cshtml"
using DutchTreat.Data.Entities;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"fb21964028ea82c57d2be60f57ff8fd448ba7fd4", @"/Views/App/Shop.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"402ff29ed5304f90d7247425045b59e26db665d3", @"/Views/_ViewImports.cshtml")]
    #nullable restore
    public class Views_App_Shop : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<IEnumerable<Product>>
    #nullable disable
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("img-fluid"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        #pragma warning restore 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("<h1>Shop</h1>\r\n<p>Count: ");
#nullable restore
#line 3 "C:\other\SelfEducation\asp.net core\asp.net core + angular\6_ef\DutchTreat\Views\App\Shop.cshtml"
     Write(Model.Count());

#line default
#line hidden
#nullable disable
            WriteLiteral("</p>\r\n<div class=\"row\">\r\n");
#nullable restore
#line 5 "C:\other\SelfEducation\asp.net core\asp.net core + angular\6_ef\DutchTreat\Views\App\Shop.cshtml"
 foreach(var p in Model)
{

#line default
#line hidden
#nullable disable
            WriteLiteral("    <div class=\"col-md-3\">\r\n        <div class=\"border bg-light rounded p-1\"");
            BeginWriteAttribute("alt", " alt=\"", 198, "\"", 213, 1);
#nullable restore
#line 8 "C:\other\SelfEducation\asp.net core\asp.net core + angular\6_ef\DutchTreat\Views\App\Shop.cshtml"
WriteAttributeValue("", 204, p.Artist, 204, 9, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">\r\n");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("img", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "fb21964028ea82c57d2be60f57ff8fd448ba7fd44761", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
            BeginAddHtmlAttributeValues(__tagHelperExecutionContext, "src", 3, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            AddHtmlAttributeValue("", 227, "~/img/", 227, 6, true);
#nullable restore
#line 9 "C:\other\SelfEducation\asp.net core\asp.net core + angular\6_ef\DutchTreat\Views\App\Shop.cshtml"
AddHtmlAttributeValue("", 233, p.ArtId, 233, 10, false);

#line default
#line hidden
#nullable disable
            AddHtmlAttributeValue("", 243, ".jpg", 243, 4, true);
            EndAddHtmlAttributeValues(__tagHelperExecutionContext);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n      <h3>");
#nullable restore
#line 10 "C:\other\SelfEducation\asp.net core\asp.net core + angular\6_ef\DutchTreat\Views\App\Shop.cshtml"
     Write(p.Category);

#line default
#line hidden
#nullable disable
            WriteLiteral(" - ");
#nullable restore
#line 10 "C:\other\SelfEducation\asp.net core\asp.net core + angular\6_ef\DutchTreat\Views\App\Shop.cshtml"
                   Write(p.Size);

#line default
#line hidden
#nullable disable
            WriteLiteral("</h3>\r\n      <ul>\r\n        <li>Price: $");
#nullable restore
#line 12 "C:\other\SelfEducation\asp.net core\asp.net core + angular\6_ef\DutchTreat\Views\App\Shop.cshtml"
               Write(p.Price);

#line default
#line hidden
#nullable disable
            WriteLiteral("</li>\r\n        <li>Artist: ");
#nullable restore
#line 13 "C:\other\SelfEducation\asp.net core\asp.net core + angular\6_ef\DutchTreat\Views\App\Shop.cshtml"
               Write(p.Artist);

#line default
#line hidden
#nullable disable
            WriteLiteral("</li>\r\n        <li>Title: ");
#nullable restore
#line 14 "C:\other\SelfEducation\asp.net core\asp.net core + angular\6_ef\DutchTreat\Views\App\Shop.cshtml"
              Write(p.Title);

#line default
#line hidden
#nullable disable
            WriteLiteral("</li>\r\n        <li>Description: ");
#nullable restore
#line 15 "C:\other\SelfEducation\asp.net core\asp.net core + angular\6_ef\DutchTreat\Views\App\Shop.cshtml"
                    Write(p.ArtDescription);

#line default
#line hidden
#nullable disable
            WriteLiteral("</li>\r\n      </ul>\r\n      <button id=\"buyButton\" class=\"btn btn-success\">Buy</button>\r\n      </div>\r\n        </div>\r\n");
#nullable restore
#line 20 "C:\other\SelfEducation\asp.net core\asp.net core + angular\6_ef\DutchTreat\Views\App\Shop.cshtml"
}

#line default
#line hidden
#nullable disable
            WriteLiteral("</div>\r\n");
        }
        #pragma warning restore 1998
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<IEnumerable<Product>> Html { get; private set; } = default!;
        #nullable disable
    }
}
#pragma warning restore 1591