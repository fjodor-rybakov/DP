#pragma checksum "D:\work\DP\Frontend\Views\Home\TextDetails.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "7c019c96cbebeab92ede3c6ce3b49938d0582783"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Home_TextDetails), @"mvc.1.0.view", @"/Views/Home/TextDetails.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Home/TextDetails.cshtml", typeof(AspNetCore.Views_Home_TextDetails))]
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
#line 1 "D:\work\DP\Frontend\Views\_ViewImports.cshtml"
using Frontend;

#line default
#line hidden
#line 2 "D:\work\DP\Frontend\Views\_ViewImports.cshtml"
using Frontend.Models;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"7c019c96cbebeab92ede3c6ce3b49938d0582783", @"/Views/Home/TextDetails.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"da9a7313b162216ba4fd0de62e6951289e87a78f", @"/Views/_ViewImports.cshtml")]
    public class Views_Home_TextDetails : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#line 1 "D:\work\DP\Frontend\Views\Home\TextDetails.cshtml"
  
    ViewData["Title"] = "Text Details";

#line default
#line hidden
            BeginContext(48, 21, true);
            WriteLiteral("\r\n<div>\r\n    <h3>id: ");
            EndContext();
            BeginContext(71, 34, false);
#line 6 "D:\work\DP\Frontend\Views\Home\TextDetails.cshtml"
        Write(ViewContext.RouteData.Values["id"]);

#line default
#line hidden
            EndContext();
            BeginContext(106, 9, true);
            WriteLiteral("</h3>\r\n\r\n");
            EndContext();
#line 8 "D:\work\DP\Frontend\Views\Home\TextDetails.cshtml"
     if (ViewData["error_message"] != null)
    {

#line default
#line hidden
            BeginContext(167, 19, true);
            WriteLiteral("        <h3>Error: ");
            EndContext();
            BeginContext(188, 25, false);
#line 10 "D:\work\DP\Frontend\Views\Home\TextDetails.cshtml"
               Write(ViewData["error_message"]);

#line default
#line hidden
            EndContext();
            BeginContext(214, 7, true);
            WriteLiteral("</h3>\r\n");
            EndContext();
#line 11 "D:\work\DP\Frontend\Views\Home\TextDetails.cshtml"
    }

#line default
#line hidden
            BeginContext(228, 6, true);
            WriteLiteral("    \r\n");
            EndContext();
#line 13 "D:\work\DP\Frontend\Views\Home\TextDetails.cshtml"
     if (ViewData["relation"] != null && ViewData["region"] != null)
    {

#line default
#line hidden
            BeginContext(311, 22, true);
            WriteLiteral("        <h3>Relation: ");
            EndContext();
            BeginContext(335, 20, false);
#line 15 "D:\work\DP\Frontend\Views\Home\TextDetails.cshtml"
                  Write(ViewData["relation"]);

#line default
#line hidden
            EndContext();
            BeginContext(356, 7, true);
            WriteLiteral("</h3>\r\n");
            EndContext();
            BeginContext(373, 20, true);
            WriteLiteral("        <h3>Region: ");
            EndContext();
            BeginContext(395, 18, false);
#line 17 "D:\work\DP\Frontend\Views\Home\TextDetails.cshtml"
                Write(ViewData["region"]);

#line default
#line hidden
            EndContext();
            BeginContext(414, 7, true);
            WriteLiteral("</h3>\r\n");
            EndContext();
#line 18 "D:\work\DP\Frontend\Views\Home\TextDetails.cshtml"
    }

#line default
#line hidden
            BeginContext(428, 10, true);
            WriteLiteral("</div>\r\n\r\n");
            EndContext();
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591
