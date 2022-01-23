// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;

namespace VirtualAssistantSample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
             .WriteTo.File(
 @"D:\home\LogFiles\Application\myapp.txt",
             fileSizeLimitBytes: 1_000_000,
             rollOnFileSizeLimit: true,
             shared: true,
             flushToDiskInterval: TimeSpan.FromSeconds(1))
             .WriteTo.Console()
             .CreateLogger();

            try
            {
                CreateHostBuilder(args).Build().Run();
            }
            catch(Exception ex)
            {
                Log.Error(ex.Message + ex.StackTrace);
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .UseDefaultServiceProvider((context, options) =>
                {
                    options.ValidateScopes = context.HostingEnvironment.IsDevelopment();
                    options.ValidateOnBuild = true;
                });
    }
}