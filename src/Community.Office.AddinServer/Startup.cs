﻿using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Community.Office.AddinServer.Middleware;
using Community.Office.AddinServer.Resources;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace Community.Office.AddinServer
{
    /// <summary>Server startup logic.</summary>
    internal sealed class Startup
    {
        private static readonly string _errorPageContent = EmbeddedResourceManager.GetString("Assets.ErrorPage.html");
        private static readonly Regex _errorPageRegex = new Regex(@"\{message\}", RegexOptions.Compiled);

        public void Configure(IApplicationBuilder app)
        {
            app.UseMiddleware<RequestHandling>();
            app.UseMiddleware<RequestTracing>();

            var staticFileOptions = new StaticFileOptions
            {
                ServeUnknownFileTypes = true,
                DefaultContentType = "application/octet-stream"
            };

            app.UseStaticFiles(staticFileOptions);
            app.UseStatusCodePages(CreateStatus);
        }

        private static Task CreateStatus(StatusCodeContext context)
        {
            context.HttpContext.Response.ContentType = "text/html";

            var message = string.Format(CultureInfo.InvariantCulture, "{0} \"{1}{2}\"",
                context.HttpContext.Response.StatusCode, context.HttpContext.Request.Path, context.HttpContext.Request.QueryString);

            return context.HttpContext.Response.WriteAsync(_errorPageRegex.Replace(_errorPageContent, message));
        }
    }
}