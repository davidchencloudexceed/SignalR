// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading;

namespace Microsoft.AspNet.SignalR.Messaging
{
    // Represents a message store that is backed by a ring buffer.
    public sealed class SimpleMessageStore<T> where T : class
    {

        private T _message;
        
        // Creates a message store with the specified capacity. The actual capacity will be *at least* the
        // specified value. That is, GetMessages may return more data than 'capacity'.
        public SimpleMessageStore(uint capacity, uint offset)
        {
         
        }

        public SimpleMessageStore(uint capacity)
            : this(capacity, offset: 0)
        {
        }

        // only for testing purposes
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Only for testing")]
        public ulong GetMessageCount()
        {
            return 1;
        }

        // Adds a message to the store. Returns the ID of the newly added message.
        public ulong Add(T message)
        {
            _message = message;
            return 0;
        }

     
        public MessageStoreResult<T> GetMessages(ulong firstMessageId, int maxMessages)
        {
            T[] msgArray = { _message };
            return new MessageStoreResult<T>(0, new ArraySegment<T>(msgArray), hasMoreData: false);
        }
        
    }
}
