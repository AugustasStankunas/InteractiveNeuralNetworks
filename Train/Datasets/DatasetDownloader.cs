using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections.Generic;

namespace Train.Datasets
{
    public static class DatasetDownloader
    {
        private static readonly Dictionary<string, (string url, string filename, int imageSize)> DatasetUrls = new Dictionary<string, (string url, string filename, int imageSize)>
        {
            { "MNIST", ("https://ossci-datasets.s3.amazonaws.com/mnist/train-images-idx3-ubyte.gz", "train-images-idx3-ubyte.gz", 28) },
            { "CIFAR-10", ("https://github.com/YoongiKim/CIFAR-10-images/archive/refs/heads/master.zip", "cifar10-images.zip", 32) },
            { "Fashion MNIST", ("https://storage.googleapis.com/tensorflow/tf-keras-datasets/train-images-idx3-ubyte.gz", "train-images-idx3-ubyte.gz", 28) }
        };

        private static readonly string[] Cifar10Labels = new[]
        {
            "airplane", "automobile", "bird", "cat", "deer",
            "dog", "frog", "horse", "ship", "truck"
        };

        private static readonly string[] FashionMnistLabels = new[]
        {
            "t-shirt", "trouser", "pullover", "dress", "coat",
            "sandal", "shirt", "sneaker", "bag", "ankle-boot"
        };

        private static readonly string[] MnistLabels = new[]
        {
            "zero", "one", "two", "three", "four",
            "five", "six", "seven", "eight", "nine"
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

                    string zipPath = Path.Combine(outputDir, filename);

                    using (var client = new WebClient())
                    {
                        MessageBox.Show($"Downloading {datasetName} dataset...", "Progress", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        await client.DownloadFileTaskAsync(url, zipPath);
                    }

                    MessageBox.Show("Processing dataset...", "Progress", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    if (datasetName == "CIFAR-10")
                    {
                        await ProcessCifar10(zipPath, outputDir);
                    }
                    else if (datasetName == "Fashion MNIST")
                    {
                        string trainDir = Path.Combine(outputDir, "train");
                        string testDir = Path.Combine(outputDir, "test");
                        Directory.CreateDirectory(trainDir);
                        Directory.CreateDirectory(testDir);
                        await ProcessFashionMnist(zipPath, trainDir, testDir, imageSize);
                    }
                    else // Regular MNIST
                    {
                        string trainDir = Path.Combine(outputDir, "train");
                        string testDir = Path.Combine(outputDir, "test");
                        Directory.CreateDirectory(trainDir);
                        Directory.CreateDirectory(testDir);
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

        private static async Task ProcessFashionMnist(string zipPath, string trainDir, string testDir, int imageSize)
        {
            // Download labels file
            string labelsUrl = "https://storage.googleapis.com/tensorflow/tf-keras-datasets/train-labels-idx1-ubyte.gz";
            string labelsPath = Path.Combine(Path.GetDirectoryName(zipPath), "labels.gz");
            using (var client = new WebClient())
            {
                await client.DownloadFileTaskAsync(labelsUrl, labelsPath);
            }

            // Read labels
            List<byte> labels = new List<byte>();
            using (FileStream compressedFileStream = File.Open(labelsPath, FileMode.Open))
            using (GZipStream decompressor = new GZipStream(compressedFileStream, CompressionMode.Decompress))
            using (MemoryStream decompressedStream = new MemoryStream())
            {
                await decompressor.CopyToAsync(decompressedStream);
                decompressedStream.Position = 8; // Skip header
                using (BinaryReader reader = new BinaryReader(decompressedStream))
                {
                    while (decompressedStream.Position < decompressedStream.Length)
                    {
                        labels.Add(reader.ReadByte());
                    }
                }
            }

            // Create class directories
            foreach (string className in FashionMnistLabels)
            {
                Directory.CreateDirectory(Path.Combine(trainDir, className));
                Directory.CreateDirectory(Path.Combine(testDir, className));
            }

            using (FileStream compressedFileStream = File.Open(zipPath, FileMode.Open))
            using (GZipStream decompressor = new GZipStream(compressedFileStream, CompressionMode.Decompress))
            using (MemoryStream decompressedStream = new MemoryStream())
            {
                await decompressor.CopyToAsync(decompressedStream);
                decompressedStream.Position = 16; // Skip header

                using (BinaryReader reader = new BinaryReader(decompressedStream))
                {
                    int numImages = (int)((decompressedStream.Length - 16) / (imageSize * imageSize));
                    int trainCount = (int)(numImages * 0.8);

                    for (int i = 0; i < numImages && i < labels.Count; i++)
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
                            int labelIndex = labels[i];
                            string className = FashionMnistLabels[labelIndex];

                            string classDir = Path.Combine(targetDir, className);
                            string imagePath = Path.Combine(classDir, $"image_{i:D5}.png");
                            bitmap.Save(imagePath, ImageFormat.Png);
                        }
                    }
                }
            }

            // Clean up labels file
            File.Delete(labelsPath);
        }

        private static async Task ProcessMnist(string zipPath, string trainDir, string testDir, int imageSize)
        {
            // Download labels file
            string labelsUrl = "https://ossci-datasets.s3.amazonaws.com/mnist/train-labels-idx1-ubyte.gz";
            string labelsPath = Path.Combine(Path.GetDirectoryName(zipPath), "labels.gz");
            using (var client = new WebClient())
            {
                await client.DownloadFileTaskAsync(labelsUrl, labelsPath);
            }

            // Read labels
            List<byte> labels = new List<byte>();
            using (FileStream compressedFileStream = File.Open(labelsPath, FileMode.Open))
            using (GZipStream decompressor = new GZipStream(compressedFileStream, CompressionMode.Decompress))
            using (MemoryStream decompressedStream = new MemoryStream())
            {
                await decompressor.CopyToAsync(decompressedStream);
                decompressedStream.Position = 8; // Skip header
                using (BinaryReader reader = new BinaryReader(decompressedStream))
                {
                    while (decompressedStream.Position < decompressedStream.Length)
                    {
                        labels.Add(reader.ReadByte());
                    }
                }
            }

            // Create class directories
            foreach (string className in MnistLabels)
            {
                Directory.CreateDirectory(Path.Combine(trainDir, className));
                Directory.CreateDirectory(Path.Combine(testDir, className));
            }

            using (FileStream compressedFileStream = File.Open(zipPath, FileMode.Open))
            using (GZipStream decompressor = new GZipStream(compressedFileStream, CompressionMode.Decompress))
            using (MemoryStream decompressedStream = new MemoryStream())
            {
                await decompressor.CopyToAsync(decompressedStream);
                decompressedStream.Position = 16; // Skip header

                using (BinaryReader reader = new BinaryReader(decompressedStream))
                {
                    int numImages = (int)((decompressedStream.Length - 16) / (imageSize * imageSize));
                    int trainCount = (int)(numImages * 0.8);

                    for (int i = 0; i < numImages && i < labels.Count; i++)
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
                            int labelIndex = labels[i];
                            string className = MnistLabels[labelIndex];

                            string classDir = Path.Combine(targetDir, className);
                            string imagePath = Path.Combine(classDir, $"image_{i:D5}.png");
                            bitmap.Save(imagePath, ImageFormat.Png);
                        }
                    }
                }
            }

            // Clean up labels file
            File.Delete(labelsPath);
        }

        private static async Task ProcessCifar10(string zipPath, string outputDir)
        {
            try
            {
                string extractPath = Path.Combine(Path.GetDirectoryName(zipPath), "cifar-extract");
                if (Directory.Exists(extractPath))
                {
                    Directory.Delete(extractPath, true);
                }
                Directory.CreateDirectory(extractPath);

                // Extract the zip file
                ZipFile.ExtractToDirectory(zipPath, extractPath);

                // Find the extracted folder - should be "CIFAR-10-images-master"
                string imageSourceDir = Path.Combine(extractPath, "CIFAR-10-images-master");
                if (!Directory.Exists(imageSourceDir))
                {
                    throw new DirectoryNotFoundException($"Could not find extracted CIFAR-10 images directory at: {imageSourceDir}");
                }

                // Move the entire CIFAR-10-images-master directory to the output directory
                string finalDir = Path.Combine(outputDir, "CIFAR-10-images-master");
                if (Directory.Exists(finalDir))
                {
                    Directory.Delete(finalDir, true);
                }
                Directory.Move(imageSourceDir, finalDir);

                // Clean up the extract directory
                if (Directory.Exists(extractPath))
                {
                    Directory.Delete(extractPath, true);
                }

                MessageBox.Show($"Dataset extracted to: {finalDir}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error processing CIFAR-10 images: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }

    }
}
