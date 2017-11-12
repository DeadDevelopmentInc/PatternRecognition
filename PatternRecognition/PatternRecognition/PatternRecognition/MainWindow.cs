using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Accord.Controls;
using Accord.Imaging;
using Accord.MachineLearning;
using Accord.MachineLearning.VectorMachines;
using Accord.MachineLearning.VectorMachines.Learning;
using Accord.Math;
using Accord.Statistics.Kernels;
using AForge;
using Accord;
using Accord.Math.Distances;
using Accord.Math.Optimization.Losses;
using System.Threading;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Text;
using System.Collections;

namespace Test
{
    
    public partial class MainWindow : Form
    {
        double[][] inputsInfo = new double[19000][];
        int[] outputResult;

        bool adminIn = false;
                
        MulticlassSupportVectorMachine<IKernel> multiSVM;

        OpenFileDialog openFileDialog = new OpenFileDialog();

        IBagOfWords<Bitmap> bow;
        private Dictionary<string, Bitmap> originalTestImages;
        private Dictionary<string, Bitmap> originalTrainImages;

        public MainWindow()
        {
            InitializeComponent();

            openFileDialog.DefaultExt = ".tif";
            openFileDialog.Title = "Open Image";
            openFileDialog.Filter = "JPEG files (*.jpg)|*.jpg";

            for(int i =0; i < inputsInfo.Length; i++)
            {
                inputsInfo[i] = new double[240];
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            var path = new DirectoryInfo(Path.Combine(Application.StartupPath, "Resources/Txt"));

            

            ArrayList list = new ArrayList();

            foreach(FileInfo file in path.GetFiles())
            {
                list.Add(File.ReadAllLines(file.FullName.ToString()).Take(100).ToArray());
            }

            int k = 0;
            int m = 0;
            for(int p = 0; p < list.Count; p++)
            {
                string[] lines = (string[])list[p];
                for (int i = 0; i < lines.Length; i++)
                {
                    m = 0;
                    double[] row = lines[i].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(double.Parse).ToArray();
                    for(int j=0; j < row.Length; j++)
                    {
                        inputsInfo[k][m] = row[j];
                        m++;
                    }
                }
                k++;
            }

            var teacher = new MulticlassSupportVectorLearning<IKernel>()
            {
                
                Learner = (param) =>
                {
                    return new SequentialMinimalOptimization<IKernel>()
                    {
                        Kernel = new HistogramIntersection(0.25, 1),
                        UseComplexityHeuristic = true,
                        Tolerance = 0.001,
                        CacheSize = 2048,
                    };
                }
            };

            // Get the input and output data

            
            

            // Train the machines. It should take a while.
            //this.multiSVM = teacher.Learn(inputsInfo, outputResult);
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {

            Properties.Settings.Default.Save();
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if (openFileDialog.OpenFile() != null)
                    {
                        pictureBox1.Image = new Bitmap(openFileDialog.OpenFile());

                        pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }

            buttonClassify.Visible = true;
        }

        private void buttonClassify_Click(object sender, EventArgs e)
        {
            Bitmap image = new Bitmap(pictureBox1.Image);
            
            double[] featureVector = ( Properties.Settings.Default.bagOfVisualWords
                as ITransform<Bitmap, double[]>).Transform(image);

            int a = this.multiSVM.Decide(featureVector);
        }

        private void computeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Compute form = new Compute();
            form.ShowDialog();
            if(form.GetPass())
            {
                buttonCompute.Visible = true;
            }
            
        }

        private void cop()
        {

        }

        private void buttonCompute_Click(object sender, EventArgs e)
        {
            DirectoryInfo path = new DirectoryInfo(Path.Combine(Application.StartupPath, "Resources/Res"));


            originalTestImages = new Dictionary<string, Bitmap>();
            originalTrainImages = new Dictionary<string, Bitmap>();

            foreach (DirectoryInfo classFolder in path.EnumerateDirectories())
            {
                string name = classFolder.Name;

                FileInfo[] files = GetFilesByExtensions(classFolder, ".jpg", ".tif").ToArray();

                Vector.Shuffle(files);

                for (int i = 0; i < files.Length; i++)
                {
                    FileInfo file = files[i];

                    Bitmap image = (Bitmap)Bitmap.FromFile(file.FullName);

                    string shortName = file.Name;
                    string imageKey = file.FullName;

                    if ((i / (double)files.Length) < 0.7)
                    {
                        originalTrainImages.Add(imageKey, image);
                    }
                    else
                    {
                        originalTestImages.Add(imageKey, image);
                    }
                }
            }

            BinarySplit binarySplit = new BinarySplit(240);

            var hog = new HistogramsOfOrientedGradientsCorrect();

            var hogBow = BagOfVisualWords.Create(hog, binarySplit);

            //var hogBow = NewBagOfWords.Create(hog, binarySplit);
            
            hogBow.Learn(originalTrainImages.Values.ToArray());

            BinarySave.WriteBinary(bagOfVisualWords: hogBow);

            var HOGBOW = BinarySave.ReadBinary();
        }

        public static IEnumerable<FileInfo> GetFilesByExtensions(DirectoryInfo dir, params string[] extensions)
        {
            if (extensions == null)
                throw new ArgumentNullException("extensions");
            IEnumerable<FileInfo> files = dir.EnumerateFiles();
            return files.Where(f => extensions.Contains(f.Extension));
        }
    }
}