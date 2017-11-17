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
using Accord.Imaging.Filters;

namespace Test
{
    
    public partial class MainWindow : Form
    {
        private int numberofwords = 124;
        private const int V = 280;
        SortedList<int, string> Animals = new SortedList<int, string>();

        double[][] inputsInfo = new double[V][];
        int[] outputResult = new int[V];
                
        MulticlassSupportVectorMachine<IKernel> multiSVM;

        OpenFileDialog openFileDialog = new OpenFileDialog();
        
        IBagOfWords<Bitmap> bow;

        private Dictionary<int, Bitmap> originalTrainImages;

        public MainWindow()
        {
            InitializeComponent();

            openFileDialog.DefaultExt = ".tif";
            openFileDialog.Title = "Open Image";
            openFileDialog.Filter = "JPEG files (*.jpg)|*.jpg |TIFF files (*.tif)| *.tif|" +
                " All files | *.*";


            labelCorrect.Visible = false;
            buttonCorrectYes.Visible = false;
            buttonCorrectYes.Visible = false;
            this.BackColor = Color.White;

            Animals.Add(0, "Butterflys");
            Animals.Add(1, "Cow");
            Animals.Add(2, "Monkey");
            Animals.Add(3, "Spider");

            /*Animals.Add(0, "Bird");
            Animals.Add(1, "Butterflys");
            Animals.Add(2, "Cow");
            Animals.Add(3, "Crocodile");
            Animals.Add(4, "Deer");
            Animals.Add(5, "Dog");
            Animals.Add(6, "Dolphine ");
            Animals.Add(7, "Duck");
            Animals.Add(8, "Elephant");
            Animals.Add(9, "Fish");
            Animals.Add(10, "Flyingbird");
            Animals.Add(11, "Hen");
            Animals.Add(12, "Horse");
            Animals.Add(13, "Leopard");
            Animals.Add(14, "Monkey");
            Animals.Add(15, "Rabbit");
            Animals.Add(16, "Rat");
            Animals.Add(17, "Spider");
            Animals.Add(18, "Tortoise");*/

            multiSVM = BinarySave.ReadBinary(true);

            bow = BinarySave.ReadBinary();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            labelCorrect.Visible = false;
            buttonCorrectYes.Visible = false;
            buttonCorrectNo.Visible = false;
            this.BackColor = Color.White;

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
            Bitmap image = (Bitmap)(pictureBox1.Image);

            CannyEdgeDetector filterCanny = new CannyEdgeDetector();

            filterCanny.ApplyInPlace(image);

            double[] featureVector = (bow as ITransform<Bitmap, double[]>).Transform(image);

            int a = this.multiSVM.Decide(featureVector);

            string animal = GetAnimalClass(a);

            label2.Text = "This is: " + animal + "?";

            labelCorrect.Visible = true;
            buttonCorrectYes.Visible = true;
            buttonCorrectNo.Visible = true;
        }

        private string GetAnimalClass(int i)
        {
            foreach(KeyValuePair<int, string> kvp in Animals)
            {
                if (i == kvp.Key)
                    return kvp.Value;

            }
            return "Coincidence not found";
        }

        public static IEnumerable<FileInfo> GetFilesByExtensions(DirectoryInfo dir, 
            params string[] extensions)
        {
            if (extensions == null)
                throw new ArgumentNullException("extensions");
            IEnumerable<FileInfo> files = dir.EnumerateFiles();
            return files.Where(f => extensions.Contains(f.Extension));
        }

        private void buttonCompute_Click(object sender, EventArgs e)
        {
            DirectoryInfo path = new DirectoryInfo(Path.Combine(Application.StartupPath, "Resources/Res"));
            
            originalTrainImages = new Dictionary<int, Bitmap>();

            int j = 0;

            int k = 0;

            foreach (DirectoryInfo classFolder in path.EnumerateDirectories())
            {
                string name = classFolder.Name;

                FileInfo[] files = GetFilesByExtensions(classFolder, ".jpg", ".tif").ToArray();

                Vector.Shuffle(files);
                for (int i = 0; i < files.Length; i++)
                {
                    FileInfo file = files[i];

                    Bitmap image = (Bitmap)Bitmap.FromFile(file.FullName);

                    CannyEdgeDetector filterCanny = new CannyEdgeDetector();

                    filterCanny.ApplyInPlace(image);

                    string shortName = file.Name;
                    int imageKey = j;

                    if ((i / (double)files.Length) < 0.7)
                    {
                        originalTrainImages.Add(j, image);
                        outputResult[j] = k;
                        j++;
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
                        Kernel = new Gaussian(0.25),
                        //Complexity = 400,
                        UseComplexityHeuristic = true,
                        Tolerance = 0.001,
                        CacheSize = 2048,
                        UseKernelEstimation = true,
                    };
                }
            };

            var kmodes = new KModes<byte>(numberofwords, new Hamming());
            var detector = new FastRetinaKeypointDetector();

            // Create bag-of-words (BoW) with the given algorithm
            var surf = new BagOfVisualWords<FastRetinaKeypoint, byte[]>(detector, kmodes);
            
            //var freakBow = new BagOfVisualWords<FastRetinaKeypoint, byte[]>(detector, kmodes);

            // Compute the BoW codebook using training images only
            surf.Learn(originalTrainImages.Values.ToArray());

            bow = surf;
            

            for (int i = 0; i < originalTrainImages.Count; i++)
            {
                Bitmap image = originalTrainImages[i] as Bitmap;

                inputsInfo[i] = (bow as ITransform<Bitmap, double[]>).Transform(image);
            }

            BinarySave.WriteBinary(surf);

            multiSVM = teacher.Learn(inputsInfo, outputResult);

            BinarySave.WriteBinary(multiSVM);


        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {

            Properties.Settings.Default.Save();
            Close();
        }

        private void buttonCorrectYes_Click(object sender, EventArgs e)
        {
            this.BackColor = Color.Green;
            Properties.Settings.Default.recognTrue += 1;
            Properties.Settings.Default.Save();
            Properties.Settings.Default.recordNum += 1;
            Properties.Settings.Default.Save();
            labelCorrect.Visible = false;
            buttonCorrectYes.Visible = false;
            buttonCorrectNo.Visible = false;
            label2.Text = "Please, upload new image";
        }

        private void buttonCorrectNo_Click(object sender, EventArgs e)
        {
            this.BackColor = Color.Red;
            Properties.Settings.Default.recognFalse += 1;
            Properties.Settings.Default.Save();
            Properties.Settings.Default.recordNum += 1;
            Properties.Settings.Default.Save();
            labelCorrect.Visible = false;
            buttonCorrectYes.Visible = false;
            buttonCorrectNo.Visible = false;
            label2.Text = "Please, upload new image";
        }

        private void changePassToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangePass form = new ChangePass();
            form.ShowDialog();
            
        }

        private void computeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Compute form = new Compute();
            form.ShowDialog();
            if (form.GetPass())
            {
                buttonCompute.Visible = true;
            }

        }

        private void averageRecordResToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(((double)Properties.Settings.Default.recognTrue
                / (double)Properties.Settings.Default.recordNum * 100).ToString());
        }
    }
}