// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Samples.Hubs.DemoHub;
using Microsoft.Owin.Hosting;
using System;

namespace Microsoft.AspNet.SelfHost.Samples
{
    public class Program
    {
        static void Main(string[] args)
        {
            using (WebApp.Start<Startup>("http://localhost:8080"))
            {
                Console.WriteLine("Server running at http://localhost:8080/");

                var hubConext = GlobalHost.ConnectionManager.GetHubContext<DemoHub>();
                var options = new CmdLineOption();
                var userInput = Console.ReadLine().Split(' ');
                while (true)
                {

                    if (CommandLine.Parser.Default.ParseArguments(userInput, options))
                    {
                        var stockGroup = hubConext.Clients.Group(options.stockSymbol);
                        stockGroup.UpdatePrice(options.stockSymbol, options.price);
                    }
                    userInput = Console.ReadLine().Split(' ');
                }
            }
        }
    }
}
