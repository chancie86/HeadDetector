using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;

namespace FaceLibrary
{
    public class FaceDetector
    {
        private readonly IFaceClient _faceClient;
        private readonly string _recognitionModel = RecognitionModel.Recognition03;

        public FaceDetector(IFaceClient faceClient)
        {
            _faceClient = faceClient;
        }

        public async Task<IList<DetectedFace>> Detect(Stream image, IList<FaceAttributeType?> faceAttributeTypes)
        {
            return await _faceClient.Face.DetectWithStreamAsync(
                image,
                returnFaceAttributes: faceAttributeTypes,
                detectionModel: DetectionModel.Detection01,
                recognitionModel: _recognitionModel);
        }
    }
}
