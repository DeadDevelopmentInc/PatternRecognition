//#define MulticlassSVM
#define OneclassSVM
//#define BackgroundWorker

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

namespace Test
{
    
    public partial class MainWindow : Form
    {

        Dictionary<string, Bitmap> originalTrainImages;
        Dictionary<string, Bitmap> originalTestImages;

        Dictionary<string, Bitmap> originalImages;
        Dictionary<string, Bitmap> displayImages;


#if (BackgroundWorker)
        System.ComponentModel.BackgroundWorker backgroundWorker;
#endif


#if (MulticlassSVM)
        MulticlassSupportVectorMachine<IKernel> multiSVM;
#endif

#if (OneclassSVM)
        SupportVectorMachine<IKernel> oneSVM;
#endif 
        

        public MainWindow()
        {
            InitializeComponent();

#if (BackgroundWorker)

            backgroundWorker.DoWork +=
                backgroundWorker_DoWork(EventArgs e);

#endif

        }



#if (BackgroundWorker)

        private backgroundWorker_DoWork(EventArgs e)
        {

        }

#endif

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            var path = new DirectoryInfo(Path.Combine(Application.StartupPath, "Resources"));

            foreach (DirectoryInfo classFolder in path.EnumerateDirectories())
            {
                comboBox1.Items.Add(classFolder.Name);
            }
        }

        private void UploadImageFromFolder(DirectoryInfo directory ,EventArgs e)
        {
            listView1.Clear();

            Accord.Math.Random.Generator.Seed = 1;
            
            originalImages = new Dictionary<string, Bitmap>();
            displayImages = new Dictionary<string, Bitmap>();

            originalTestImages = new Dictionary<string, Bitmap>();
            originalTrainImages = new Dictionary<string, Bitmap>();

            ImageList imageList = new ImageList();
            imageList.ImageSize = new Size(64, 64);
            imageList.ColorDepth = ColorDepth.Depth8Bit;
            listView1.LargeImageList = imageList;

            int currentClassLabel = 0;


            string name = directory.Name;


            ListViewGroup trainingGroup = listView1.Groups.Add(name + ".train", name + ".train");
            ListViewGroup testingGroup = listView1.Groups.Add(name + ".test", name + ".test");


            FileInfo[] files = GetFilesByExtensions(directory, ".jpg", ".tif").ToArray();


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

                    item = new ListViewItem(trainingGroup);
                    originalTrainImages.Add(imageKey, image);
                    item.Text = directory.Name + "." + i.ToString() + ".train";

                }
                else
                {

                    item = new ListViewItem(testingGroup);
                    originalTestImages.Add(imageKey, image);
                    item.Text = directory.Name + "." + i.ToString() + ".test";

                }

                item.ImageKey = imageKey;
                item.Name = shortName;

                item.Tag = new Tuple<double[], int>(null, currentClassLabel);

                listView1.Items.Add(item);
            }
        }

        
        private void buttonComputeBVW_Click(object sender, EventArgs e)
        {
            int numberOfWords = 64;

            /*progressBar.Minimum = 0;

            progressBar.Maximum = listView1.Items.Count;

            progressBar.Step = 1;*/
            
            Stopwatch sw1 = Stopwatch.StartNew();

            IBagOfWords<Bitmap> bow;


            BinarySplit binarySplit = new BinarySplit(numberOfWords);

            BagOfVisualWords surfBow = new BagOfVisualWords(binarySplit);

            bow = surfBow.Learn(originalTrainImages.Values.ToArray());

            sw1.Stop();

            Stopwatch sw2 = Stopwatch.StartNew();

            foreach (ListViewItem item in listView1.Items)
            {
                Bitmap image = originalImages[item.ImageKey] as Bitmap;

                double[] featureVector = (bow as ITransform<Bitmap, double[]>).Transform(image);

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

#if (MulticlassSVM)

            IKernel kernel = getKernel();

          
            double complexity = 1;
            double tolerance = 0.01;
            int cacheSize = 1024;

            
            var teacher = new MulticlassSupportVectorLearning<IKernel>()
            {
                Kernel = kernel,
                Learner = (param) =>
                {
                    return new SequentialMinimalOptimization<IKernel>()
                    {
                        Kernel = kernel,
                        Complexity = complexity,
                        Tolerance = tolerance,
                        CacheSize = cacheSize,
                    };
                }
            };

            double[][] inputs;
            int[] outputs;
            getData(out inputs, out outputs);
            
            lbStatus.Text = "Training the classifiers. This may take a (very) significant amount of time...";
            Application.DoEvents();

            Stopwatch sw = Stopwatch.StartNew();
            
            this.multiSVM = teacher.Learn(inputs, outputs);

            sw.Stop();
#endif

#if (OneclassSVM)
            IKernel kernel = getKernel();

            var teacher = new OneclassSupportVectorLearning<IKernel>()
            {
                Kernel = kernel,
                Nu = 0.9
            };

            double[][] inputs;
            int[] outputs;
            getData(out inputs, out outputs);
            lbStatus.Text = "Training the classifiers. This may take a (very) significant amount of time...";
            Application.DoEvents();

            Stopwatch sw = Stopwatch.StartNew();

            this.oneSVM = teacher.Learn(inputs);

            sw.Stop();
#endif           
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

#if (MulticlassSVM)
                    int actual = multiSVM.Decide(input);
#endif

#if (OneclassSVM)
                    int actual = Convert.ToInt32(oneSVM.Decide(input));
#endif

                    if (expected == actual)
                    {
                        
                        item.BackColor = Color.LightGreen;
                        if (item.Group.Name.EndsWith(".train"))
                            trainingHits++;
                        else testingHits++;
                    }
                    else
                    {
                        item.BackColor = Color.Firebrick;
                        if (item.Group.Name.EndsWith(".train"))
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

        private IKernel getKernel()
        {
           return new HistogramIntersection(1, 1);

            throw new Exception();
        }
        

        public static IEnumerable<FileInfo> GetFilesByExtensions(DirectoryInfo dir, params string[] extensions)
        {
            if (extensions == null)
                throw new ArgumentNullException("extensions");
            IEnumerable<FileInfo> files = dir.EnumerateFiles();
            return files.Where(f => extensions.Contains(f.Extension));
        }
        
        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            DirectoryInfo directory = new DirectoryInfo(Path.Combine(Application.StartupPath,
                "Resources", comboBox1.SelectedItem.ToString()));
            UploadImageFromFolder(directory, e);
        }
    }
}