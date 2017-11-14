// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Microsoft.AspNet.SignalR.Client.Samples
{
    class Program
    {
        static void Main(string[] args)
        {
            var writer = Console.Out;
            var client = new CommonClient(writer);

            var options = new CmdLineOption();
            var userInput = Console.ReadLine().Split(' ');
            while (true)
            {

                if (CommandLine.Parser.Default.ParseArguments(userInput, options))
                {
                    try
                    {
                        switch (options.action)
                        {
                            case "connect":
                                client.Connect("http://localhost:8080/").Wait();
                                break;
                            case "addGroup":
                                client.AddGroup().Wait();
                                break;
                            default:
                                break;

                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                else
                {
                    Console.WriteLine("wrong cmd line, please input again");
                }
                userInput = Console.ReadLine().Split(' ');
            }
        }
    }
}
