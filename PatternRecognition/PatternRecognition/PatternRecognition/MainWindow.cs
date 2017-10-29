using Accord;
using Accord.Controls;
using Accord.Imaging;
using Accord.MachineLearning;
using Accord.MachineLearning.VectorMachines;
using Accord.MachineLearning.VectorMachines.Learning;
using Accord.Math;
using Accord.Math.Distances;
using Accord.Math.Optimization.Losses;
using Accord.Statistics.Kernels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Test
{
    public partial class MainWindow : Form
    {
        //Create dictionary
        Dictionary<string, Bitmap> originalTrainImages;
        Dictionary<string, Bitmap> originalTestImages;

        Dictionary<string, Bitmap> originalImages;
        Dictionary<string, Bitmap> displayImages;




        //Create SVM
        MulticlassSupportVectorMachine<IKernel> ksvm;
        SupportVectorMachine<IKernel> supportVectorMachine;

        /// <summary>
        /// Initialize component in main window
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Method for upload data.
        /// Find images in folder. Choose with images insert in train and test folder
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            Accord.Math.Random.Generator.Seed = 1;

            var path = new DirectoryInfo(Path.Combine(Application.StartupPath, "Resources"));
            
            foreach(DirectoryInfo classFolder in path.EnumerateDirectories())
            {
                comboBox1.Items.Add(classFolder.Name);
            }
        }

        private void UploadPicture(DirectoryInfo directoryInfo, EventArgs e)
        {
            listView1.Clear();

            originalImages = new Dictionary<string, Bitmap>();
            displayImages = new Dictionary<string, Bitmap>();

            originalTestImages = new Dictionary<string, Bitmap>();
            originalTrainImages = new Dictionary<string, Bitmap>();

            //Cteate image list with images, put depth, and size
            ImageList imageList = new ImageList();
            imageList.ImageSize = new Size(64, 64);
            imageList.ColorDepth = ColorDepth.Depth8Bit;
            listView1.LargeImageList = imageList;

            string name = directoryInfo.Name;

            ListViewGroup trainingGroup = listView1.Groups.Add(name + ".train", name + ".train");
            ListViewGroup testingGroup = listView1.Groups.Add(name + ".test", name + ".test");

            // Load the images from the directory that contains images for each class
            FileInfo[] files = GetFilesByExtensions(directoryInfo, ".jpg", ".tif").ToArray();

            Vector.Shuffle(files);

            for (int i = 0; i < files.Length; i++)
            {
                FileInfo file = files[i];

                Bitmap image = (Bitmap)Bitmap.FromFile(file.FullName);

                string shortName = file.Name;
                string imageKey = file.FullName;

                imageList.Images.Add(imageKey, image);
                originalImages.Add(imageKey, image);
                displayImages.Add(imageKey, image);

                ListViewItem item;
                if ((i / (double)files.Length) < 0.7)
                {
                    // Put the first 70% in training set
                    item = new ListViewItem(trainingGroup);
                    originalTrainImages.Add(imageKey, image);
                }
                else
                {
                    // Put the restant 30% in test set
                    item = new ListViewItem(testingGroup);
                    originalTestImages.Add(imageKey, image);
                }

                item.ImageKey = imageKey;
                item.Name = shortName;
                item.Text = shortName;

                item.Tag = new Tuple<double[], int>(null, 0);

                listView1.Items.Add(item);

            }
        }

        private void buttonComputeBVW_Click(object sender, EventArgs e)
        {
            int numberOfContourFragments = 64;

            Stopwatch sw1 = Stopwatch.StartNew();

            IBagOfWords<Bitmap> BVW;
            
            BinarySplit binarySplit = new BinarySplit(numberOfContourFragments);
            
            BagOfVisualWords surfBow = new BagOfVisualWords(binarySplit);
            
            BVW = surfBow.Learn(originalTrainImages.Values.ToArray());

            sw1.Stop();

            Stopwatch sw2 = Stopwatch.StartNew();

            foreach (ListViewItem item in listView1.Items)
            {
                Bitmap image = originalImages[item.ImageKey] as Bitmap;

                double[] featureVector = (BVW as ITransform<Bitmap, double[]>).Transform(image);

                string featureString = featureVector.ToString(DefaultArrayFormatProvider.InvariantCulture);

                if (item.SubItems.Count == 2)
                    item.SubItems[1].Text = featureString;
                else item.SubItems.Add(featureString);

                int classLabel = (item.Tag as Tuple<double[], int>).Item2;

                item.Tag = Tuple.Create(featureVector, classLabel);
            }

            sw2.Stop();
            
        }
        
        private void buttonTrainning_Click(object sender, EventArgs e)
        {
            IKernel kernel = new Gaussian(0.9);

            var teacher = new OneclassSupportVectorLearning<IKernel>()
            {
                Kernel = new Gaussian(0.9),
            };

            double[][] inputs;
            int[] outputs;
            getData(out inputs, out outputs);
            
            Application.DoEvents();

            Stopwatch sw = Stopwatch.StartNew();

            //this.supportVectorMachine = teacher.Learn(inputs, outputs);

            this.supportVectorMachine = teacher.Learn(inputs);

            sw.Stop();

            double error = new ZeroOneLoss(outputs).Loss(supportVectorMachine.Decide(inputs));
            
        }
        
        private void buttonClassify_Click(object sender, EventArgs e)
        {
            int trainingHits = 0;
            int trainingMiss = 0;

            int testingHits = 0;
            int testingMiss = 0;

            foreach (ListViewGroup group in listView1.Groups)
            {
                foreach (ListViewItem item in group.Items)
                {
                    var info = item.Tag as Tuple<double[], int>;
                    double[] input = info.Item1;
                    int expected = info.Item2;

                    int actual = Convert.ToInt32(supportVectorMachine.Decide(input));

                    if (expected == actual)
                    {
                        item.BackColor = Color.LightGreen;
                        if (item.Group.Name.EndsWith(" train"))
                            trainingHits++;
                        else testingHits++;
                    }
                    else
                    {
                        item.BackColor = Color.Firebrick;
                        if (item.Group.Name.EndsWith(" train"))
                            trainingMiss++;
                        else testingMiss++;
                    }
                }
            }

            int trainingTotal = trainingHits + trainingMiss;
            int testingTotal = testingHits + testingMiss;
           
        }
        
        private void getData(out double[][] inputs, out int[] outputs)
        {
            List<double[]> inputList = new List<double[]>();
            List<int> outputList = new List<int>();

            foreach (ListViewGroup group in listView1.Groups)
            {
                if (group.Name.EndsWith(".train"))
                {
                    foreach (ListViewItem item in group.Items)
                    {
                        var info = item.Tag as Tuple<double[], int>;
                        inputList.Add(info.Item1);
                        outputList.Add(info.Item2);
                    }
                }
            }

            inputs = inputList.ToArray();
            outputs = outputList.ToArray();
        }
        
        public static IEnumerable<FileInfo> GetFilesByExtensions(DirectoryInfo dir, params string[] extensions)
        {
            if (extensions == null)
                throw new ArgumentNullException("extensions");
            IEnumerable<FileInfo> files = dir.EnumerateFiles();
            return files.Where(f => extensions.Contains(f.Extension));
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            string curdir = (string)comboBox1.SelectedItem;
            DirectoryInfo directory = new DirectoryInfo(Path.Combine(Application.StartupPath,
                "Resources/" + curdir.ToString()));

            UploadPicture(directory, e);
        }
        
        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
