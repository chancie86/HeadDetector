using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;

namespace FaceLibrary.Face
{
    public class Filter
    {
        private readonly Config _config;
        private readonly string _filePath;

        public Filter(Config config, string filePath)
        {
            _config = config;
            _filePath = filePath;
        }

        public async Task<IList<Head>> Run()
        {
            var credentials = new FaceServiceCredentials(
                _config.SubscriptionKey,
                _config.EndpointUrl);
            var faceService = new FaceService(credentials);

            faceService.Authenticate();
            var detector = faceService.GetFaceDetector();

            var attributeTypes = new List<FaceAttributeType?>
            {
                FaceAttributeType.Accessories, FaceAttributeType.Age,
                FaceAttributeType.Blur, FaceAttributeType.Emotion, FaceAttributeType.Exposure,
                FaceAttributeType.FacialHair,
                FaceAttributeType.Gender, FaceAttributeType.Glasses, FaceAttributeType.Hair, FaceAttributeType.HeadPose,
                FaceAttributeType.Makeup, FaceAttributeType.Noise, FaceAttributeType.Occlusion, FaceAttributeType.Smile
            };

            using var fileStream = File.OpenRead(_filePath);
            var faces = await detector.Detect(fileStream, attributeTypes);
            var results = new List<Head>();

            for (var i = 0; i < faces.Count; i++)
            {
                var face = faces[i];
                results.Add(new Head(new Rectangle(
                    face.FaceRectangle.Left,
                    face.FaceRectangle.Top,
                    face.FaceRectangle.Width,
                    face.FaceRectangle.Height),
                    face.GetAttributeText($"Face id: {i}", faces.Count == 1)));
            }

            return results;
        }
    }
}
