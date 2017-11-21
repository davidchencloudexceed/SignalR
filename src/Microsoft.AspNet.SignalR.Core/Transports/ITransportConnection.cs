// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace Microsoft.AspNet.SignalR.Transports
{
    public interface ITransportConnection
    {
        IDisposable Receive(string messageId, Func<PersistentResponse, object, Task<bool>> callback, int maxMessages, object state);

        Task Send(ConnectionMessage message);
        void SetWebSocket(WebSocket socket);
        WebSocket GetWebSocket();
    }
}
