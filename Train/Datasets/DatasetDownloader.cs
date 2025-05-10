using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net;
using System.Windows.Forms;
using System.IO.Compression;
using System.Drawing;
using System.Drawing.Imaging;

namespace Train.Datasets
{
    public static class DatasetDownloader
    {
        private static readonly Dictionary<string, (string url, string filename, int imageSize)> DatasetUrls = new Dictionary<string, (string url, string filename, int imageSize)>
        {
            { "MNIST", ("https://ossci-datasets.s3.amazonaws.com/mnist/train-images-idx3-ubyte.gz", "train-images-idx3-ubyte.gz", 28) },
            { "CIFAR-10", ("https://www.cs.toronto.edu/~kriz/cifar-10-python.tar.gz", "cifar-10-python.tar.gz", 32) },
            { "Fashion MNIST", ("https://storage.googleapis.com/tensorflow/tf-keras-datasets/train-images-idx3-ubyte.gz", "train-images-idx3-ubyte.gz", 28) }
        };

        private static readonly string[] Cifar10Labels = new[]
        {
            "airplane", "automobile", "bird", "cat", "deer",
            "dog", "frog", "horse", "ship", "truck"
        };

        static DatasetDownloader()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }

        public static async Task<(bool success, string downloadPath)> DownloadDatasetAsync(string datasetName)
        {
            try
            {
                var dialog = new FolderBrowserDialog
                {
                    Description = "Select folder to download dataset",
                    UseDescriptionForTitle = true
                };

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    string downloadPath = dialog.SelectedPath;
                    
                    if (!DatasetUrls.ContainsKey(datasetName))
                    {
                        MessageBox.Show($"Unknown dataset: {datasetName}\nAvailable datasets: {string.Join(", ", DatasetUrls.Keys)}", 
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return (false, null);
                    }

                    var (url, filename, imageSize) = DatasetUrls[datasetName];
                    string outputDir = Path.Combine(downloadPath, $"{datasetName}Dataset");
                    Directory.CreateDirectory(outputDir);

                    string trainDir = Path.Combine(outputDir, "train");
                    string testDir = Path.Combine(outputDir, "test");
                    Directory.CreateDirectory(trainDir);
                    Directory.CreateDirectory(testDir);

                    string zipPath = Path.Combine(outputDir, filename);
                    
                    using (var client = new WebClient())
                    {
                        MessageBox.Show($"Downloading {datasetName} dataset...", "Progress", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        await client.DownloadFileTaskAsync(url, zipPath);
                    }

                    MessageBox.Show("Processing dataset...", "Progress", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    if (datasetName == "CIFAR-10")
                    {
                        await ProcessCifar10(zipPath, trainDir, testDir);
                    }
                    else // MNIST or Fashion MNIST
                    {
                        await ProcessMnist(zipPath, trainDir, testDir, imageSize);
                    }

                    File.Delete(zipPath);

                    MessageBox.Show($"Dataset downloaded and processed to: {outputDir}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return (true, outputDir);
                }
                return (false, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error downloading dataset: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return (false, null);
            }
        }

        private static async Task ProcessMnist(string zipPath, string trainDir, string testDir, int imageSize)
        {
            using (FileStream compressedFileStream = File.Open(zipPath, FileMode.Open))
            using (GZipStream decompressor = new GZipStream(compressedFileStream, CompressionMode.Decompress))
            using (MemoryStream decompressedStream = new MemoryStream())
            {
                await decompressor.CopyToAsync(decompressedStream);
                decompressedStream.Position = 0;

                using (BinaryReader reader = new BinaryReader(decompressedStream))
                {
                    decompressedStream.Position = 16; // Skip header
                    int numImages = (int)((decompressedStream.Length - 16) / (imageSize * imageSize));
                    int trainCount = (int)(numImages * 0.8);

                    for (int i = 0; i < numImages; i++)
                    {
                        byte[] imageData = reader.ReadBytes(imageSize * imageSize);
                        using (Bitmap bitmap = new Bitmap(imageSize, imageSize))
                        {
                            for (int y = 0; y < imageSize; y++)
                            {
                                for (int x = 0; x < imageSize; x++)
                                {
                                    byte pixel = imageData[y * imageSize + x];
                                    bitmap.SetPixel(x, y, Color.FromArgb(pixel, pixel, pixel));
                                }
                            }

                            string targetDir = i < trainCount ? trainDir : testDir;
                            string imagePath = Path.Combine(targetDir, $"image_{i:D5}.png");
                            bitmap.Save(imagePath, ImageFormat.Png);
                        }
                    }
                }
            }
        }

        private static async Task ProcessCifar10(string zipPath, string trainDir, string testDir)
        {
            foreach (string className in Cifar10Labels)
            {
                Directory.CreateDirectory(Path.Combine(trainDir, className));
                Directory.CreateDirectory(Path.Combine(testDir, className));
            }

            using (FileStream compressedFileStream = File.Open(zipPath, FileMode.Open))
            using (GZipStream decompressor = new GZipStream(compressedFileStream, CompressionMode.Decompress))
            using (MemoryStream decompressedStream = new MemoryStream())
            {
                await decompressor.CopyToAsync(decompressedStream);
                decompressedStream.Position = 0;

                using (BinaryReader reader = new BinaryReader(decompressedStream))
                {
                    // Process training data (5 batches)
                    for (int batch = 0; batch < 5; batch++)
                    {
                        for (int i = 0; i < 10000; i++)
                        {
                            byte label = reader.ReadByte();
                            byte[] imageData = reader.ReadBytes(3072);

                            using (Bitmap bitmap = new Bitmap(32, 32))
                            {
                                for (int y = 0; y < 32; y++)
                                {
                                    for (int x = 0; x < 32; x++)
                                    {
                                        int pixelIndex = y * 32 + x;
                                        Color color = Color.FromArgb(
                                            imageData[pixelIndex + 2048],
                                            imageData[pixelIndex + 1024],
                                            imageData[pixelIndex]
                                        );
                                        bitmap.SetPixel(x, y, color);
                                    }
                                }

                                string className = Cifar10Labels[label];
                                string imagePath = Path.Combine(trainDir, className, $"image_{batch}_{i:D5}.png");
                                bitmap.Save(imagePath, ImageFormat.Png);
                            }
                        }
                    }

                    // Process test data
                    for (int i = 0; i < 10000; i++)
                    {
                        byte label = reader.ReadByte();
                        byte[] imageData = reader.ReadBytes(3072);

                        using (Bitmap bitmap = new Bitmap(32, 32))
                        {
                            for (int y = 0; y < 32; y++)
                            {
                                for (int x = 0; x < 32; x++)
                                {
                                    int pixelIndex = y * 32 + x;
                                    Color color = Color.FromArgb(
                                        imageData[pixelIndex + 2048],
                                        imageData[pixelIndex + 1024],
                                        imageData[pixelIndex]
                                    );
                                    bitmap.SetPixel(x, y, color);
                                }
                            }

                            string className = Cifar10Labels[label];
                            string imagePath = Path.Combine(testDir, className, $"image_{i:D5}.png");
                            bitmap.Save(imagePath, ImageFormat.Png);
                        }
                    }
                }
            }
        }
    }
} 