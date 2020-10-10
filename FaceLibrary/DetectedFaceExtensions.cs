using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;

namespace FaceLibrary
{
    public static class DetectedFaceExtensions
    {
        public static string GetAttributeText(this DetectedFace face)
        {
            var resultBuilder = new StringBuilder();

            // Get bounding box of the faces
            resultBuilder.AppendLine($"Rectangle(Left/Top/Width/Height) : {face.FaceRectangle.Left} {face.FaceRectangle.Top} {face.FaceRectangle.Width} {face.FaceRectangle.Height}");

            // Get accessories of the faces
            List<Accessory> accessoriesList = (List<Accessory>)face.FaceAttributes.Accessories;
            int count = face.FaceAttributes.Accessories.Count;
            string accessory; string[] accessoryArray = new string[count];
            if (count == 0) { accessory = "NoAccessories"; }
            else
            {
                for (int i = 0; i < count; ++i) { accessoryArray[i] = accessoriesList[i].Type.ToString(); }
                accessory = string.Join(",", accessoryArray);
            }
            resultBuilder.AppendLine($"Accessories : {accessory}");

            // Get face other attributes
            resultBuilder.AppendLine($"Age : {face.FaceAttributes.Age}");
            resultBuilder.AppendLine($"Blur : {face.FaceAttributes.Blur.BlurLevel}");

            // Get emotion on the face
            string emotionType = string.Empty;
            double emotionValue = 0.0;
            Emotion emotion = face.FaceAttributes.Emotion;
            if (emotion.Anger > emotionValue) { emotionValue = emotion.Anger; emotionType = "Anger"; }
            if (emotion.Contempt > emotionValue) { emotionValue = emotion.Contempt; emotionType = "Contempt"; }
            if (emotion.Disgust > emotionValue) { emotionValue = emotion.Disgust; emotionType = "Disgust"; }
            if (emotion.Fear > emotionValue) { emotionValue = emotion.Fear; emotionType = "Fear"; }
            if (emotion.Happiness > emotionValue) { emotionValue = emotion.Happiness; emotionType = "Happiness"; }
            if (emotion.Neutral > emotionValue) { emotionValue = emotion.Neutral; emotionType = "Neutral"; }
            if (emotion.Sadness > emotionValue) { emotionValue = emotion.Sadness; emotionType = "Sadness"; }
            if (emotion.Surprise > emotionValue) { emotionType = "Surprise"; }
            resultBuilder.AppendLine($"Emotion : {emotionType}");

            // Get more face attributes
            resultBuilder.AppendLine($"Exposure : {face.FaceAttributes.Exposure.ExposureLevel}");
            resultBuilder.AppendLine($"FacialHair : {string.Format("{0}", face.FaceAttributes.FacialHair.Moustache + face.FaceAttributes.FacialHair.Beard + face.FaceAttributes.FacialHair.Sideburns > 0 ? "Yes" : "No")}");
            resultBuilder.AppendLine($"Gender : {face.FaceAttributes.Gender}");
            resultBuilder.AppendLine($"Glasses : {face.FaceAttributes.Glasses}");

            // Get hair color
            Hair hair = face.FaceAttributes.Hair;
            string color = null;
            if (hair.HairColor.Count == 0) { if (hair.Invisible) { color = "Invisible"; } else { color = "Bald"; } }
            HairColorType returnColor = HairColorType.Unknown;
            double maxConfidence = 0.0f;
            foreach (HairColor hairColor in hair.HairColor)
            {
                if (hairColor.Confidence <= maxConfidence) { continue; }
                maxConfidence = hairColor.Confidence; returnColor = hairColor.Color; color = returnColor.ToString();
            }
            resultBuilder.AppendLine($"Hair : {color}");

            // Get more attributes
            resultBuilder.AppendLine($"HeadPose : {string.Format("Pitch: {0}, Roll: {1}, Yaw: {2}", Math.Round(face.FaceAttributes.HeadPose.Pitch, 2), Math.Round(face.FaceAttributes.HeadPose.Roll, 2), Math.Round(face.FaceAttributes.HeadPose.Yaw, 2))}");
            resultBuilder.AppendLine($"Makeup : {string.Format("{0}", (face.FaceAttributes.Makeup.EyeMakeup || face.FaceAttributes.Makeup.LipMakeup) ? "Yes" : "No")}");
            resultBuilder.AppendLine($"Noise : {face.FaceAttributes.Noise.NoiseLevel}");
            resultBuilder.AppendLine($"Occlusion : {string.Format("EyeOccluded: {0}", face.FaceAttributes.Occlusion.EyeOccluded ? "Yes" : "No")} " +
                $" {string.Format("ForeheadOccluded: {0}", face.FaceAttributes.Occlusion.ForeheadOccluded ? "Yes" : "No")}   {string.Format("MouthOccluded: {0}", face.FaceAttributes.Occlusion.MouthOccluded ? "Yes" : "No")}");
            resultBuilder.AppendLine($"Smile : {face.FaceAttributes.Smile}");

            return resultBuilder.ToString();
        }
    }
}
