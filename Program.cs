using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PlantHealthDetector
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
            // This finds the folder where your .exe is running (bin\Debug)
            string exePath = AppDomain.CurrentDomain.BaseDirectory;
            string projectRoot = Path.GetFullPath(Path.Combine(exePath, @"..\..\..\"));
            string datasetPath = Path.Combine(projectRoot, "dataset");
            Console.WriteLine("Looking for dataset at: " + datasetPath);
            DataLoader.LoadData(datasetPath, out var trainSet, out var testSet);
        }
    }
}
