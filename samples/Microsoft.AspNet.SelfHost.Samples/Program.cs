// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.AspNet.SignalR.Infrastructure;
using Microsoft.AspNet.SignalR.Json;
using Microsoft.AspNet.SignalR.Messaging;
using Microsoft.AspNet.SignalR.Samples.Hubs.DemoHub;
using Microsoft.AspNet.SignalR.Transports;
using Microsoft.Owin.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using TB.Common.DataModel.Quote.Struct;

namespace Microsoft.AspNet.SelfHost.Samples
{
    public class Program
    {
        static IMemoryPool _pool;
        static void Main(string[] args)
        {
            using (WebApp.Start<Startup>("http://localhost:8080"))
            {
                Console.WriteLine("Server running at http://localhost:8080/");

                var hubConext = GlobalHost.ConnectionManager.GetHubContext<DemoHub>();
                var topicManager = GlobalHost.ConnectionManager.GetTopicManager();
                var messageBus = GlobalHost.ConnectionManager.GetMessageBus();
                _pool = GlobalHost.ConnectionManager.GetMemoryPool();
                var options = new CmdLineOption();
                var userInput = Console.ReadLine().Split(' ');
                var tick = new TBTickDeepLevel();
                tick.codeId = new TBCodeID { Code = "MSFT" };
                tick.bidAsks = new List<TBBidAsk> { new TBBidAsk { ask = 100.01, askVol = 100 } };
                tick.open = 100.01;
                tick.openInt = 100.01;

                var demoHub = GlobalHost.ConnectionManager.GetHubContext<DemoHub>();
                //demoHub.Clients.Group("a").UpdatePrice(100);
                while (true)
                {

                    if (CommandLine.Parser.Default.ParseArguments(userInput, options))
                    {
                        hubConext.Groups.Add("fakeid", "MSFT");
                        var msftTopic = topicManager.GetOrAdd("hg-demo.MSFT", (name) => { return new Topic(32, TimeSpan.FromMinutes(10)); });
                        var stockGroup = hubConext.Clients.Group(options.stockSymbol);
                        var hubOutgoingContext = (stockGroup as GroupProxy).GetHubOutgoingInvokerContext("UpdatePrice", tick);
                        var connMessage = new ConnectionMessage(hubOutgoingContext.Signal, hubOutgoingContext.Invocation, hubOutgoingContext.ExcludedSignals);
                        ArraySegment<byte> messageBuffer = GetMessageBuffer(connMessage.Value);
                        var message = new Message("fakeconnection", connMessage.Signal, options.price.ToString(), messageBuffer);
                        //messageBus.Publish(message).Wait();
                        msftTopic.Store.Add(message);

                        Message[] messageArray = { message };
                        ArraySegment<Message> segment = new ArraySegment<Message>(messageArray, 0, 1);

                        var persistentResp = new PersistentResponse();
                        persistentResp.Messages = new List<ArraySegment<Message>>();
                        persistentResp.Messages.Add(segment);

                        foreach (var sub in msftTopic.Subscriptions)
                        {

                            try
                            {
                                sub.PassThroughTopicStoreSending(persistentResp).Wait();
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Failed to process work - " + ex.GetBaseException());
                                break;
                            }

                        }


                        //stockGroup.UpdatePrice(options.stockSymbol, options.price);
                    }
                    userInput = Console.ReadLine().Split(' ');
                }
            }
        }
        private static ArraySegment<byte> GetMessageBuffer(object value)
        {
            ArraySegment<byte> messageBuffer;
            // We can't use "as" like we do for Command since ArraySegment is a struct
            if (value is ArraySegment<byte>)
            {
                // We assume that any ArraySegment<byte> is already JSON serialized
                messageBuffer = (ArraySegment<byte>)value;
            }
            else
            {
                messageBuffer = SerializeMessageValue(value);
            }
            return messageBuffer;
        }
        private static ArraySegment<byte> SerializeMessageValue(object value)
        {
            using (var writer = new MemoryPoolTextWriter(_pool))
            {
                var _serializer = new JsonSerializer();
                _serializer.Serialize(value, writer);
                writer.Flush();

                var data = writer.Buffer;

                var buffer = new byte[data.Count];

                Buffer.BlockCopy(data.Array, data.Offset, buffer, 0, data.Count);

                return new ArraySegment<byte>(buffer);
            }
        }
    }
}
