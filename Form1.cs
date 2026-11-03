using Backprop;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static PlantHealthDetector.DatasetLoader;


namespace PlantHealthDetector
{
    public partial class Form1 : Form
    {
        NeuralNet nn;
        Bitmap image;
        List<ImageData> trainSet;
        List<ImageData> testSet;
        int hiddenNeurons;
        double learningRate;
        int epochs;
        bool isTrained;

        public Form1()
        {
            InitializeComponent();
            // This finds the folder where your .exe is running (bin\Debug)
            string exePath = AppDomain.CurrentDomain.BaseDirectory;
            string projectRoot = Path.GetFullPath(Path.Combine(exePath, @"..\..\"));
            string datasetPath = Path.Combine(projectRoot, "dataset");
            Console.WriteLine("Looking for dataset at: " + datasetPath);
            DatasetLoader.LoadData(datasetPath, out trainSet, out testSet);
            hiddenNeurons = 16;
            learningRate = 0.15;
            epochs = 100;
            isTrained = false;
        }

        private void healthDetectorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Initialize your custom network
            nn = new NeuralNet(DatasetLoader.size, hiddenNeurons, 1);

            // Apply the tuned learning rates
            nn.LRPIN = learningRate;
            nn.LRPOUT = learningRate;
            Console.WriteLine("Preparation complete. Ready to train the model.");
        }

        private void trainModelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (nn == null) { Console.WriteLine("Please run 'Preparation' first."); return; }

            string csvPath = "training_log.csv";
            bool fileExists = File.Exists(csvPath);

            using (StreamWriter sw = new StreamWriter(csvPath, true))
            {
                if (!fileExists)
                {
                    sw.WriteLine("SessionStartTime,Epoch,TrainLoss,TestAccuracy,HiddenNeurons,LearningRate");
                }
                sw.WriteLine($"\nStarting training (Hidden={hiddenNeurons}, LR={learningRate}, Epochs={epochs})...\n");
                string sessionTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                sw.WriteLine();
                sw.WriteLine($"--- New Training Session: {sessionTime} ---");

                // Variables for Early Stopping
                double bestAccuracy = 0.0;
                int epochsWithoutImprovement = 0;
                int patience = 15; // Stop if no improvement for 15 epochs

                // Create a copy of the training set that we can shuffle
                var currentTrainSet = new List<ImageData>(trainSet);
                var rng = new Random(42);

                for (int epoch = 1; epoch <= epochs; epoch++)
                {
                    // Shuffle training data every epoch
                    currentTrainSet = currentTrainSet.OrderBy(_ => rng.Next()).ToList();

                    double totalLoss = 0.0;

                    foreach (var sample in currentTrainSet)
                    {
                        for (int i = 0; i < DatasetLoader.size; i++)
                            nn.setInputs(i, sample.pixels[i]);

                        nn.setDesiredOutput(0, sample.label);
                        nn.learn();

                        double prediction = nn.getOuputData(0);
                        totalLoss += Math.Abs(sample.label - prediction);
                    }

                    double avgLoss = totalLoss / currentTrainSet.Count;

                    // Evaluate on the test set every 10 epochs to save time
                    if (epoch % 10 == 0 || epoch == 1 || epoch == epochs)
                    {
                        double accuracy = EvaluateAccuracy(nn, testSet);

                        Console.WriteLine($"Epoch {epoch}/{epochs} | Loss: {avgLoss:F4} | Test Accuracy: {accuracy:P2}");
                        sw.WriteLine($"{sessionTime},{epoch},{avgLoss:F4},{accuracy:F4},{hiddenNeurons},{learningRate}");

                        // Early Stopping Logic
                        if (accuracy > bestAccuracy)
                        {
                            bestAccuracy = accuracy;
                            epochsWithoutImprovement = 0; // Reset counter
                        }
                        else
                        {
                            epochsWithoutImprovement += 5;
                        }

                        if (epochsWithoutImprovement >= patience)
                        {
                            Console.WriteLine($"\nEarly stopping triggered! No improvement for {patience} epochs.");
                            sw.WriteLine($"Early stopping triggered at epoch {epoch}.");
                            break; // Exit the loop early
                        }
                    }
                }
            }
            Console.WriteLine("\nTraining completed. Results appended to 'training_log.csv'");
            isTrained = true;
        }
        static double EvaluateAccuracy(NeuralNet net, List<ImageData> testSet)
        {
            int correct = 0;
            foreach (var sample in testSet)
            {
                for (int i = 0; i < DatasetLoader.size; i++) net.setInputs(i, sample.pixels[i]);
                net.run(); // Forward pass only

                double output = net.getOuputData(0);
                double prediction = output >= 0.5 ? 1.0 : 0.0;

                if (prediction == sample.label) correct++;
            }
            return (double)correct / testSet.Count;
        }
        static void PrintConfusionMatrix(NeuralNet net, List<ImageData> testSet)
        {
            int trueHealthy = 0;   // True Negatives
            int trueUnhealthy = 0; // True Positives
            int falseHealthy = 0;  // False Negatives (Predicted healthy, actually unhealthy)
            int falseUnhealthy = 0;// False Positives (Predicted unhealthy, actually healthy)

            foreach (var sample in testSet)
            {
                for (int i = 0; i < DatasetLoader.size; i++) net.setInputs(i, sample.pixels[i]);
                net.run();

                double output = net.getOuputData(0);
                int prediction = output >= 0.5 ? 1 : 0;
                int actual = (int)sample.label;

                if (prediction == 0 && actual == 0) trueHealthy++;
                else if (prediction == 1 && actual == 1) trueUnhealthy++;
                else if (prediction == 0 && actual == 1) falseHealthy++;
                else if (prediction == 1 && actual == 0) falseUnhealthy++;
            }

            Console.WriteLine("                  Predicted Healthy | Predicted Unhealthy");
            Console.WriteLine("---------------------------------------------------------");
            Console.WriteLine($"Actual Healthy  | {trueHealthy,17} | {falseUnhealthy,19}");
            Console.WriteLine($"Actual Unhealthy| {falseHealthy,17} | {trueUnhealthy,19}");
            Console.WriteLine("---------------------------------------------------------\n");

            int totalCorrect = trueHealthy + trueUnhealthy;
            int totalWrong = falseHealthy + falseUnhealthy;

            Console.WriteLine("=== PREDICTION SUMMARY ===");
            Console.WriteLine($"Predicted Healthy:   {trueHealthy + falseHealthy}");
            Console.WriteLine($"Predicted Unhealthy: {trueUnhealthy + falseUnhealthy}");
            Console.WriteLine($"Correct Predictions: {totalCorrect}");
            Console.WriteLine($"Wrong Predictions:   {totalWrong}");
        }

        private void healthDetectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (nn == null) { Console.WriteLine("Please train the model first."); return; }

            Console.WriteLine("\n=== Final Evaluation ===");
            PrintConfusionMatrix(nn, testSet);

            nn.saveWeights("trained_plant_model.txt");
            Console.WriteLine("\nWeights saved to 'trained_plant_model.txt'");
        }

        private void openImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (nn == null)
            {
                MessageBox.Show("Please initialize the network (Preparation) first.");
                return;
            }
            openFileDialog1.ShowDialog();
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            if(!File.Exists(openFileDialog1.FileName))
            {
                MessageBox.Show("File not found. Please select a valid image.");
                return;
            }
            image = new Bitmap(openFileDialog1.FileName);
            pictureBox1.Image = image;
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
        }

        private void manualTestingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.FileName == "")
            {
                MessageBox.Show("No file selected. Please choose an image.");
                return;
            }
            double[] pixels = DatasetLoader.ProcessSingleImage(openFileDialog1.FileName);

            for (int i = 0; i < DatasetLoader.size; i++)
                nn.setInputs(i, pixels[i]);

            nn.run(); // Forward pass only (do not learn yet)

            double output = nn.getOuputData(0);
            string predictedLabel = output >= 0.5 ? "UNHEALTHY" : "HEALTHY";
            double confidence = output >= 0.5 ? output : (1.0 - output);

            //Update the UI text box
            textBox1.Text = predictedLabel;

            //Ask user for manual feedback
            DialogResult feedback = MessageBox.Show(
                $"Prediction: {predictedLabel}\nConfidence: {confidence:P1}\n\nIs this prediction correct?",
                "Manual Validation",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            double actualLabel;
            bool wasPredictionCorrect;

            if (feedback == DialogResult.Yes)
            {
                actualLabel = output >= 0.5 ? 1.0 : 0.0;
                wasPredictionCorrect = true;
            }
            else
            {
                // User said it was wrong. Flip the label.
                actualLabel = output >= 0.5 ? 0.0 : 1.0;
                wasPredictionCorrect = false;

                // TEACH THE NETWORK (Online Learning)
                nn.setDesiredOutput(0, actualLabel);
                nn.learn(); // This updates the weights immediately!
                nn.saveWeights("trained_plant_model.txt"); // Save the new brain
                Console.WriteLine("Network weights updated based on manual feedback.");
                MessageBox.Show("Prediction corrected. Network weights have been updated and saved!");
            }

            // 5. Log the result to a CSV
            LogManualResult(openFileDialog1.FileName, predictedLabel, actualLabel, wasPredictionCorrect, confidence);
        }
        private void LogManualResult(string fileName, string predicted, double actual, bool isCorrect, double confidence)
        {
            string csvPath = "manual_test_log.csv";
            bool fileExists = File.Exists(csvPath);

            using (StreamWriter sw = new StreamWriter(csvPath, true)) // 'true' means append
            {
                if (!fileExists)
                {
                    sw.WriteLine("ImageName,Predicted,ActualLabel,Correct?,Confidence");
                }

                string actualText = actual == 1.0 ? "UNHEALTHY" : "HEALTHY";
                sw.WriteLine($"{Path.GetFileName(fileName)},{predicted},{actualText},{isCorrect},{confidence:P2}");
            }
        }

        private void setWeightsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (nn == null)
            {
                MessageBox.Show("Please run Preparation first.");
                return;
            }
            openFileDialog2.ShowDialog();
        }

        private void exportWeightsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (nn == null || !isTrained)
            {
                MessageBox.Show("Please train the network before exporting weights.");
                return;
            }
            if (nn == null)
            {
                MessageBox.Show("Please train or prepare the network first.");
                return;
            }
            using (SaveFileDialog sfd = saveFileDialog1)
            {
                sfd.Filter = "Text Files (*.txt)|*.txt";
                sfd.FileName = "trained_plant_model.txt";
                sfd.Title = "Save Trained Weights";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        nn.saveWeights(sfd.FileName);
                        MessageBox.Show($"Weights exported successfully to:\n{sfd.FileName}");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error saving weights: {ex.Message}");
                    }
                }
            }
        }

        private void openFileDialog2_FileOk(object sender, CancelEventArgs e)
        {
            if(File.Exists(openFileDialog2.FileName))
            {
                nn.loadWeights(openFileDialog2.FileName);
                MessageBox.Show("Weights loaded successfully!");
            }
            else
            {
                MessageBox.Show("File not found. Searching for saved weights from this program.");
                if (File.Exists("trained_plant_model.txt"))
                {
                    nn.loadWeights("trained_plant_model.txt");
                    MessageBox.Show("Weights loaded successfully from trained_plant_model.txt");
                }
                else
                {
                    MessageBox.Show("No saved weights found. Please train the model first.");
                }
            }
        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
        }

        private void exportCSVFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string trainingPath = "training_log.csv";
            string manualPath = "manual_test_log.csv";

            bool hasTraining = File.Exists(trainingPath);
            bool hasManual = File.Exists(manualPath);

            if (!hasTraining && !hasManual)
            {
                MessageBox.Show("No logs found. Please train the model or test an image first.", "File Not Found");
                return;
            }

            using (SaveFileDialog sfd = saveFileDialog2)
            {
                sfd.Filter = "CSV Files|*.csv";
                sfd.FileName = "Combined_Training_Logs.csv";
                sfd.Title = "Export Combined Logs";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        using (StreamWriter sw = new StreamWriter(sfd.FileName))
                        {
                            // Write Training Log Section
                            if (hasTraining)
                            {
                                sw.WriteLine("=== TRAINING LOG ===");
                                // Read all lines from the training file and write them
                                string[] trainLines = File.ReadAllLines(trainingPath);
                                foreach (string line in trainLines)
                                {
                                    sw.WriteLine(line);
                                }
                            }
                            else
                            {
                                sw.WriteLine("=== TRAINING LOG ===");
                                sw.WriteLine("No training data recorded.");
                            }

                            // Add empty rows for spacing
                            sw.WriteLine();
                            sw.WriteLine();

                            // Write Manual Test Section
                            if (hasManual)
                            {
                                sw.WriteLine("=== MANUAL TEST LOG ===");
                                string[] manualLines = File.ReadAllLines(manualPath);
                                foreach (string line in manualLines)
                                {
                                    sw.WriteLine(line);
                                }
                            }
                            else
                            {
                                sw.WriteLine("=== MANUAL TEST LOG ===");
                                sw.WriteLine("No manual test data recorded.");
                            }
                        }

                        MessageBox.Show($"Combined report successfully exported to:\n{sfd.FileName}", "Export Success");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error exporting file: {ex.Message}", "Error");
                    }
                }
            }

        }
    }
}
