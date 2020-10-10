using System;
using System.Collections.Generic;
using System.Text;

namespace FaceLibrary
{
    public sealed class FaceServiceCredentials
    {
        public FaceServiceCredentials(string subsciptionKey, string endpoint)
        {
            SubscriptionKey = subsciptionKey;
            Endpoint = endpoint;
        }

        internal string SubscriptionKey { get; }

        internal string Endpoint { get; }
    }
}
