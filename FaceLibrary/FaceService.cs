using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Azure.CognitiveServices.Vision.Face;

namespace FaceLibrary
{
    public class FaceService
    {
        private readonly FaceServiceCredentials _creds;
        private FaceClient _client;

        public FaceService(FaceServiceCredentials creds)
        {
            _creds = creds;
        }

        public void Authenticate()
        {
            _client = new FaceClient(new ApiKeyServiceClientCredentials(_creds.SubscriptionKey))
            {
                Endpoint = _creds.Endpoint
            };
        }

        public FaceDetector GetFaceDetector()
        {
            if (_client == null)
            {
                throw new UnauthorizedAccessException($"{nameof(FaceService)} is not authenticated");
            }

            return new FaceDetector(_client);
        }
    }
}
