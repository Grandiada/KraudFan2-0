﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Globalization;
using System.Threading.Tasks;

namespace KraudFan2_0.Middleware
{
    public class CultureMiddleware
    {
        private readonly RequestDelegate _next;

        public CultureMiddleware(RequestDelegate next)
        {
            this._next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var lang = context.Request.Cookies["lang"].ToString();
            if (!string.IsNullOrEmpty(lang))
            {
                try
                {
                    CultureInfo.CurrentCulture = new CultureInfo(lang);
                    CultureInfo.CurrentUICulture = new CultureInfo(lang);
                }
                catch (CultureNotFoundException) { }
            }
            else
            {
                context.Response.Cookies.Append("lang", "ru");
                try
                {
                    CultureInfo.CurrentCulture = new CultureInfo("ru");
                    CultureInfo.CurrentUICulture = new CultureInfo("ru");
                }
                catch (CultureNotFoundException) { }
            }
            await _next.Invoke(context);
        }
    }

    public static class CultureExtensions
    {
        public static IApplicationBuilder UseCulture(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CultureMiddleware>();
        }
    }
}