using Accord;
using Accord.Imaging;
using Accord.MachineLearning;
using Accord.MachineLearning.VectorMachines;
using Accord.Statistics.Kernels;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PatternRecognition
{
    class BinarySave
    {   
        /// <summary>
        /// Method for serialize bcf to file
        /// </summary>
        /// <param name="bagOfVisualWords">bcf</param>
        internal static void WriteBinary(object bagOfVisualWords)
        {
            //Open stream for write
            FileStream fs = new FileStream(Path.Combine(Application.StartupPath, "Resources/" +
                "bcf.dat"), FileMode.OpenOrCreate);

            //Create binary formatter
            BinaryFormatter formatter = new BinaryFormatter();
            
            //Serialize bcf in binary format
            formatter.Serialize(fs, bagOfVisualWords);

            //Close stream, if didn't do this, be error with reopen openned stream
            fs.Close();
        }

        /// <summary>
        /// Method use for deserialize bcf from file
        /// </summary>
        /// <returns>deserialized object</returns>
        internal static BagOfVisualWords ReadBinary()
        {
            //Create binary formatter
            BinaryFormatter formatter = new BinaryFormatter();

            //Open stream for read
            FileStream fs = new FileStream(Path.Combine(Application.StartupPath, "Resources/" +
                "bcf.dat"), FileMode.OpenOrCreate);

            //Deserialize data from strim and convert it in bcf
            BagOfVisualWords freak = (BagOfVisualWords)formatter.Deserialize(fs);

            //Close stream
            fs.Close();
            
            return freak;
        }

        /// <summary>
        /// Method for serialize svm to file
        /// </summary>
        /// <param name="multiclassSupportVectorLearning">svm object</param>
        internal static void WriteBinary(MulticlassSupportVectorMachine<HistogramIntersection>
            multiclassSupportVectorLearning)
        {
            //Open stream for write
            FileStream fs = new FileStream(Path.Combine(Application.StartupPath, "Resources/" +
                "svm.dat"), FileMode.OpenOrCreate);

            //Create binary formatter
            BinaryFormatter formatter = new BinaryFormatter();

            //Serialize svm to file
            formatter.Serialize(fs, multiclassSupportVectorLearning);

            //Close stream
            fs.Close();
        }

        /// <summary>
        /// Method use for deserialize bcf from file
        /// </summary>
        /// <param name="log">formaly parametr need to overload function,
        /// can have value true/false</param>
        /// <returns></returns>
        internal static MulticlassSupportVectorMachine<HistogramIntersection> ReadBinary(bool log)
        {
            //Create binary formatter
            BinaryFormatter formatter = new BinaryFormatter();

            //Open stream for write
            FileStream fs = new FileStream(Path.Combine(Application.StartupPath, "Resources/" +
                "svm.dat"), FileMode.OpenOrCreate);

            //Deserialize data from strim and convert it in svm
            MulticlassSupportVectorMachine<HistogramIntersection> multiclassSupportVectorLearning =
                (MulticlassSupportVectorMachine<HistogramIntersection>)formatter.Deserialize(fs);

            //Close strim
            fs.Close();

            return multiclassSupportVectorLearning;
        }
    }
}