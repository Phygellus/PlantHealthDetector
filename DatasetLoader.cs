using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace PlantHealthDetector
{
    public class ImageData
    {
        public double[] pixels;
        public double label;
    }
    public static class DatasetLoader
    {
        public const int W = 64, H = 64;
        public const int size = W * H * 3; // 64x64 RGB images
        public static void LoadData(
                    string datasetFolderPath,
                    out List<ImageData> trainSet,
                    out List<ImageData> testSet,
                    double trainRatio = 0.8)
        {
            var allSamples = new List<ImageData>();

            string healthyPath = Path.Combine(datasetFolderPath, "healthy");
            string unhealthyPath = Path.Combine(datasetFolderPath, "unhealthy");

            // 1. Load Healthy Images (Label 0.0)
            LoadFolder(healthyPath, 0.0, allSamples);

            // 2. Load Unhealthy Images (Label 1.0)
            LoadFolder(unhealthyPath, 1.0, allSamples);

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
                    int i = 0;
                    for (int y = 0; y < H; y++)
                    {
                        for (int x = 0; x < W; x++)
                        {
                            Color c = resized.GetPixel(x, y);
                            // CRITICAL: Divide by 255.0 to normalize to 0.0 - 1.0
                            pixels[i++] = c.R / 255.0;
                            pixels[i++] = c.G / 255.0;
                            pixels[i++] = c.B / 255.0;
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
    }
}
