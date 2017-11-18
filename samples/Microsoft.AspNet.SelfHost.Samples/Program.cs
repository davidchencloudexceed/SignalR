// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Samples.Hubs.DemoHub;
using Microsoft.Owin.Hosting;
using System;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.AspNet.SignalR.Transports;
using System.Collections.Generic;
using Microsoft.AspNet.SignalR.Messaging;

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
                var topicManager = GlobalHost.ConnectionManager.GetTopicManager();
                var options = new CmdLineOption();
                var userInput = Console.ReadLine().Split(' ');
                
                var demoHub = GlobalHost.ConnectionManager.GetHubContext<DemoHub>();
                //demoHub.Clients.Group("a").UpdatePrice(100);
                while (true)
                {

                    if (CommandLine.Parser.Default.ParseArguments(userInput, options))
                    {
                        //hubConext.Groups.Add("fakeid", "MSFT");
                        var stockGroup = hubConext.Clients.Group(options.stockSymbol);
                        var hubOutgoingContext = (stockGroup as GroupProxy).GetHubOutgoingInvokerContext("UpdatePrice", options.stockSymbol, options.price);
                        var connMessage =  new ConnectionMessage(hubOutgoingContext.Signal, hubOutgoingContext.Invocation, hubOutgoingContext.ExcludedSignals);
                        var message = new Message("fakeconnection", connMessage.Signal, connMessage.Value.ToString());
                        Message[] messageArray = { message };
                        ArraySegment<Message> segment = new ArraySegment<Message>(messageArray,0,1);

                        var persistentResp = new PersistentResponse();
                        persistentResp.Messages = new List<ArraySegment<Message>>();
                        persistentResp.Messages.Add(segment);
                        
                        stockGroup.UpdatePrice(options.stockSymbol, options.price);
                    }
                    userInput = Console.ReadLine().Split(' ');
                }
            }
        }
    }
}
