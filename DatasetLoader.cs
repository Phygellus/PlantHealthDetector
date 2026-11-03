using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace PlantHealthDetector
{
    public class ImageData
    {
        public double[] pixels;
        public double label;
    }
    public static class DatasetLoader
    {
        public const int W = 32, H = 32;
        public const int size = W * H * 3; // 32x32 RGB images
        public static void LoadData(
                    string datasetFolderPath,
                    out List<ImageData> trainSet,
                    out List<ImageData> testSet,
                    double trainRatio = 0.8)
        {
            var allSamples = new List<ImageData>();

            string healthyPath = Path.Combine(datasetFolderPath, "healthy");
            string unhealthyPath = Path.Combine(datasetFolderPath, "unhealthy");
            var healthyList = new List<ImageData>();
            var unhealthyList = new List<ImageData>();


            // 1. Load Healthy Images (Label 0.0)
            LoadFolder(healthyPath, 0.0, healthyList);

            // 2. Load Unhealthy Images (Label 1.0)
            LoadFolder(unhealthyPath, 1.0, unhealthyList);

            allSamples.AddRange(healthyList.Take(250));
            allSamples.AddRange(unhealthyList.Take(250));
            Console.WriteLine($"Total images loaded: {allSamples.Count}");

            // 3. Shuffle the dataset to ensure a random mix
            var rng = new Random(42); // Fixed seed for reproducible experiments
            var shuffled = allSamples.OrderBy(_ => rng.Next()).ToList();

            // 4. Train / Test Split
            int trainCount = (int)(shuffled.Count * trainRatio);

            trainSet = shuffled.Take(trainCount).ToList();
            testSet = shuffled.Skip(trainCount).ToList();

            Console.WriteLine($"Training set size: {trainSet.Count}");
            Console.WriteLine($"Testing set size:  {testSet.Count}");
        }
        private static void LoadFolder(string folderPath, double label, List<ImageData> targetList)
        {
            if (!Directory.Exists(folderPath))
            {
                Console.WriteLine($"Warning: Folder not found -> {folderPath}");
                return;
            }

            var files = Directory.GetFiles(folderPath, "*.*")
                                 .Where(f => f.EndsWith(".jpg") || f.EndsWith(".png") || f.EndsWith(".jpeg"));

            foreach (var file in files)
            {
                try
                {
                    // Load the image
                    var bmp = new Bitmap(file);
                    var resized = new Bitmap(W, H);

                    // Resize (Safe guard in case a file isn't exactly 64x64)
                    using (var g = Graphics.FromImage(resized))
                    {
                        g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        g.DrawImage(bmp, 0, 0, W, H);
                    }

                    // Flatten and Normalize
                    double[] pixels = new double[size];
                    var rect = new Rectangle(0, 0, W, H);

                    // Lock the bitmap's bits in memory
                    System.Drawing.Imaging.BitmapData bmpData = resized.LockBits(rect, 
                        System.Drawing.Imaging.ImageLockMode.ReadOnly, 
                        System.Drawing.Imaging.PixelFormat.Format24bppRgb);

                    int bytes = Math.Abs(bmpData.Stride) * resized.Height;
                    byte[] rgbValues = new byte[bytes];

                    // Copy the RGB values into the array
                    System.Runtime.InteropServices.Marshal.Copy(bmpData.Scan0, rgbValues, 0, bytes);
                    resized.UnlockBits(bmpData);
                    int i = 0;
                    for (int y = 0; y < H; y++)
                    {
                        for (int x = 0; x < W; x++)
                        {
                            int index = y * bmpData.Stride + x * 3;
                            // CRITICAL: Divide by 255.0 to normalize to 0.0 - 1.0
                            pixels[i++] = rgbValues[index + 2] / 255.0; // R
                            pixels[i++] = rgbValues[index + 1] / 255.0; // G
                            pixels[i++] = rgbValues[index + 0] / 255.0; // B                        }
                        }
                    }
                    targetList.Add(new ImageData { pixels = pixels, label = label });
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error loading {file}: {ex.Message}");
                }
            }
        }
        public static double[] ProcessSingleImage(string filePath)
        {
            var bmp = new Bitmap(filePath);
            var resized = new Bitmap(W, H);
            using (var g = Graphics.FromImage(resized))
            {
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(bmp, 0, 0, W, H);
            }

            double[] pixels = new double[size];
            var rect = new Rectangle(0, 0, W, H);
            BitmapData bmpData = resized.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            int bytes = Math.Abs(bmpData.Stride) * resized.Height;
            byte[] rgbValues = new byte[bytes];
            Marshal.Copy(bmpData.Scan0, rgbValues, 0, bytes);
            resized.UnlockBits(bmpData);

            int i = 0;
            for (int y = 0; y < H; y++)
            {
                for (int x = 0; x < W; x++)
                {
                    int index = y * bmpData.Stride + x * 3;
                    pixels[i++] = rgbValues[index + 2] / 255.0;
                    pixels[i++] = rgbValues[index + 1] / 255.0;
                    pixels[i++] = rgbValues[index + 0] / 255.0;
                }
            }
            return pixels;
        }
    }
}
