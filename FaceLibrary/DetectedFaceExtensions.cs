using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;

namespace FaceLibrary
{
    public static class DetectedFaceExtensions
    {
        public static string GetAttributeText(this DetectedFace face, string headerText = null, bool verbose = true)
        {
            var resultBuilder = new StringBuilder();

            if (headerText != null)
            {
                Append(resultBuilder, verbose, headerText);
            }

            // Get bounding box of the faces
            if (verbose)
            {
                Append(resultBuilder, verbose,
                    $"Rectangle(Left/Top/Width/Height) : {face.FaceRectangle.Left} {face.FaceRectangle.Top} {face.FaceRectangle.Width} {face.FaceRectangle.Height}");
            }

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

            if (verbose)
            {
                Append(resultBuilder, verbose, $"Accessories : {accessory}");
            }

            // Get face other attributes
            Append(resultBuilder, verbose, $"Age : {face.FaceAttributes.Age}");
            if (verbose)
            {
                Append(resultBuilder, verbose, $"Blur : {face.FaceAttributes.Blur.BlurLevel}");
            }
            
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
            Append(resultBuilder, verbose, $"Emotion : {emotionType}");

            // Get more face attributes
            if (verbose)
            {
                Append(resultBuilder, verbose, $"Exposure : {face.FaceAttributes.Exposure.ExposureLevel}");
            }
            
            Append(resultBuilder, verbose, $"FacialHair : {string.Format("{0}", face.FaceAttributes.FacialHair.Moustache + face.FaceAttributes.FacialHair.Beard + face.FaceAttributes.FacialHair.Sideburns > 0 ? "Yes" : "No")}");
            Append(resultBuilder, verbose, $"Gender : {face.FaceAttributes.Gender}");
            Append(resultBuilder, verbose, $"Glasses : {face.FaceAttributes.Glasses}");

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
            if (verbose)
            {
                Append(resultBuilder, verbose, $"HeadPose : {string.Format("Pitch: {0}, Roll: {1}, Yaw: {2}", Math.Round(face.FaceAttributes.HeadPose.Pitch, 2), Math.Round(face.FaceAttributes.HeadPose.Roll, 2), Math.Round(face.FaceAttributes.HeadPose.Yaw, 2))}");
                Append(resultBuilder, verbose, $"Makeup : {string.Format("{0}", (face.FaceAttributes.Makeup.EyeMakeup || face.FaceAttributes.Makeup.LipMakeup) ? "Yes" : "No")}");
                Append(resultBuilder, verbose, $"Noise : {face.FaceAttributes.Noise.NoiseLevel}");
                Append(resultBuilder, verbose, $"Occlusion : {string.Format("EyeOccluded: {0}", face.FaceAttributes.Occlusion.EyeOccluded ? "Yes" : "No")} " +
                                               $" {string.Format("ForeheadOccluded: {0}", face.FaceAttributes.Occlusion.ForeheadOccluded ? "Yes" : "No")}   {string.Format("MouthOccluded: {0}", face.FaceAttributes.Occlusion.MouthOccluded ? "Yes" : "No")}");
                Append(resultBuilder, verbose, $"Smile : {face.FaceAttributes.Smile}");
            }
            
            return resultBuilder.ToString();
        }

        private static void Append(StringBuilder sb, bool newLine, string text)
        {
            if (newLine)
            {
                sb.AppendLine(text);
            }
            else
            {
                sb.Append(text);
                sb.Append(", ");
            }
        }
    }
}
