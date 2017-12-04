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

namespace PatternRecognition
{
    
    public partial class MainWindow : Form
    {
        //const number of words
        private const int numberOfContour = 256;

        //const int number of trainning images
        private int V;

        //List with contain keys and names of classes 
        SortedList<int, string> Animals = new SortedList<int, string>();

        //matrix of vectors input information for trsinning
        double[][] inputsInfo;

        //array of results number of class for trainning
        int[] outputsResult;
                
        //Create empty multiclass support vector machine
        MulticlassSupportVectorMachine<HistogramIntersection> multiSVM;

        //Create file dialog, for users, which upload image
        OpenFileDialog openFileDialog = new OpenFileDialog();

        //Empty bag of visual words for calculate class of image
        BagOfVisualWords bagOfContourFragments;

        //Dictionary of trainning image, which be used for trainning SVM
        private Dictionary<int, Bitmap> originalTrainImages;

        /// <summary>
        /// Main constructor for windows form
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            //Add properties for openFileDialog
            openFileDialog.DefaultExt = ".tif";

            openFileDialog.Title = "Open Image";
            openFileDialog.Filter = "JPEG files (*.jpg)| *.jpg|TIFF files (*.tif)| *.tif|" +
                " All files | *.*";

            //Close button Yes and No, which user click for assert recognition
            labelCorrect.Visible = false;
            buttonCorrectYes.Visible = false;
            buttonCorrectYes.Visible = false;
            this.BackColor = Color.White;

            //Read ready SVM and bagOfContourFragments
            multiSVM = BinarySave.ReadBinary(true);

            bagOfContourFragments = BinarySave.ReadBinary();
        }

        /// <summary>
        /// Override method? which called with start application
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            //Find directory for scan folders and images
            DirectoryInfo path = new DirectoryInfo(Path.Combine(Application.StartupPath, "Resources/Res"));

            //Number of folders
            int i = 0;

            foreach(DirectoryInfo classFolder in path.EnumerateDirectories())
            {
                //Create new class of images
                Animals.Add(i, classFolder.ToString());
                i++;
            }

            V = 70 * i;

            inputsInfo = new double[V][];

            outputsResult = new int[V];
        }

        /// <summary>
        /// Call dialog for upload images, with user take
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            //If classification call two ore more times we need close some buttons,
            //such as buttons Yes and No
            labelCorrect.Visible = false;
            buttonCorrectYes.Visible = false;
            buttonCorrectNo.Visible = false;
            this.BackColor = Color.White;

            //Upload image
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                //Try to upload image
                try
                {
                    //If user take image, we display this image
                    if (openFileDialog.OpenFile() != null)
                    {
                        pictureBox1.Image = new Bitmap(openFileDialog.OpenFile());

                        pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                    }
                }
                //Display exception
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }

            //Show button for classify
            buttonClassify.Visible = true;
        }

        /// <summary>
        /// Classify user's image
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonClassify_Click(object sender, EventArgs e)
        {
            //Create bitmap for recognition users's image
            Bitmap image = (Bitmap)(pictureBox1.Image);

            //Create Canny detector for contours
            CannyEdgeDetector filterCanny = new CannyEdgeDetector();

            //Detect contour on image
            filterCanny.ApplyInPlace(image);

            //Transform image in feature vector
            double[] featureVector = (bagOfContourFragments as ITransform<Bitmap, double[]>).Transform(image);
            
            //SVM decide from which class this image
            string animal = GetAnimalClass(this.multiSVM.Decide(featureVector));

            //display this information for user
            label2.Text = "This is: " + animal + "?";

            //Show buttons to analyse correct detect
            labelCorrect.Visible = true;
            buttonCorrectYes.Visible = true;
            buttonCorrectNo.Visible = true;
        }

        /// <summary>
        /// Take key of class and return they value
        /// </summary>
        /// <param name="i">This is number of class, which contain this iamge </param>
        /// <returns>Name of class</returns>
        private string GetAnimalClass(int i)
        {
            foreach(KeyValuePair<int, string> kvp in Animals)
            {
                if (i == kvp.Key)
                    return kvp.Value;

            }
            return "Coincidence not found";
        }

        /// <summary>
        /// Static method to upload images from folder
        /// </summary>
        /// <param name="dir">Where find objects</param>
        /// <param name="extensions">With this extension</param>
        /// <returns></returns>
        public static IEnumerable<FileInfo> GetFilesByExtensions(DirectoryInfo dir, 
            params string[] extensions)
        {
            if (extensions == null)
                throw new ArgumentNullException("extensions");
            IEnumerable<FileInfo> files = dir.EnumerateFiles();
            return files.Where(f => extensions.Contains(f.Extension));
        }

        /// <summary>
        /// This methods only for admin, and this recompute bagOfContourFragments and svm
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonCompute_Click(object sender, EventArgs e)
        {

            //Accord.Math.Random.Generator.Seed = 1;

            DirectoryInfo path = new DirectoryInfo(Path.Combine(Application.StartupPath, "Resources/Res"));
            
            ///Create dictionary for train images
            originalTrainImages = new Dictionary<int, Bitmap>();

            int j = 0;

            int k = 0;

            foreach (DirectoryInfo classFolder in path.EnumerateDirectories())
            {
                ///Add name of folder
                string name = classFolder.Name;

                ///Upload all files in aarray
                FileInfo[] files = GetFilesByExtensions(classFolder, ".jpg", ".tif").ToArray();

                //Shuffle objects in array
                Vector.Shuffle(files);

                //For each image complite some easy operations
                for (int i = 0; i < files.Length; i++)
                {
                    //Uploat only train images
                    //70%
                    if ((i / (double)files.Length) < 0.7)
                    {
                        //Add file
                        FileInfo file = files[i];

                        //Create image from file
                        Bitmap image = (Bitmap)Bitmap.FromFile(file.FullName);

                        //Use detector
                        CannyEdgeDetector filterCanny = new CannyEdgeDetector();

                        //Apply changes
                        filterCanny.ApplyInPlace(image);

                        //Add some information of image
                        string shortName = file.Name;
                        int imageKey = j;

                        //Add image to dictionary
                        originalTrainImages.Add(j, image);

                        //Save correct key of class for image
                        outputsResult[j] = k;
                        j++;
                    }
                }
                //Change key of folder
                k++;
            }

            //Create teacher for svm, using Histogram Intersection
            var teacher = new MulticlassSupportVectorLearning<HistogramIntersection>()
            {
                //Add leaner params
                Learner = (param) => new SequentialMinimalOptimization<HistogramIntersection>()
                {
                    //Create kernel with optimal params
                    Kernel = new HistogramIntersection(0.25, 1),
                }
            };

            //Create KMeans algr
            var kmodes = new KModes<byte>(numberOfContour, new Hamming());

            //Create detector
            var detector = new FastRetinaKeypointDetector();
            
            //Create bagOfContourFragments
            bagOfContourFragments = new BagOfVisualWords(numberOfContour);

            //Learned bagOfContourFragments
            bagOfContourFragments.Learn(originalTrainImages.Values.ToArray());            

            //For each iamge add inputs info
            for (int i = 0; i < originalTrainImages.Count; i++)
            {
                Bitmap image = originalTrainImages[i] as Bitmap;

                inputsInfo[i] = (bagOfContourFragments as ITransform<Bitmap, double[]>).Transform(image);
            }

            //Save condition of bagOfContourFragments
            BinarySave.WriteBinary(bagOfContourFragments);

            //Teach svm
            multiSVM = teacher.Learn(inputsInfo, outputsResult);

            //Save condition of svm
            BinarySave.WriteBinary(multiSVM);


        }

        /// <summary>
        /// Save params and close program
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Save();
            Close();
        }

        /// <summary>
        /// Change statistic with natural num
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Change statictic with negative num
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Use if user forgot or unknown password, but need recompute svm
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void changePassToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangePass form = new ChangePass();
            form.ShowDialog();
            
        }

        /// <summary>
        /// For use it need write password
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void computeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Compute form = new Compute();
            form.ShowDialog();
            //If password correct
            if (form.GetPass())
            {
                buttonCompute.Visible = true;
            }

        }

        /// <summary>
        /// Display average num of true classify
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void averageRecordResToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(((double)Properties.Settings.Default.recognTrue
                / (double)Properties.Settings.Default.recordNum * 100).ToString());
        }

        /// <summary>
        /// Display information about programm and other information
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}