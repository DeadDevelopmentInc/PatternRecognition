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

namespace Test
{
    class BinarySave
    {
        internal static void WriteBinary(object bagOfVisualWords)
        {
            FileStream fs = new FileStream(Path.Combine(Application.StartupPath, "Resources/" +
                "hogBow.dat"), FileMode.OpenOrCreate);


            BinaryFormatter formatter = new BinaryFormatter();
            

            formatter.Serialize(fs, bagOfVisualWords);


            fs.Close();
        }

        internal static BagOfVisualWords ReadBinary()
        {
            BinaryFormatter formatter = new BinaryFormatter();

            FileStream fs = new FileStream(Path.Combine(Application.StartupPath, "Resources/" +
                "hogBow.dat"), FileMode.OpenOrCreate);

            BagOfVisualWords freak =
                (BagOfVisualWords)formatter.Deserialize(fs);

            fs.Close();

            return freak;
        }

        internal static void WriteBinary(MulticlassSupportVectorMachine<HistogramIntersection>
            multiclassSupportVectorLearning)
        {
            FileStream fs = new FileStream(Path.Combine(Application.StartupPath, "Resources/" +
                "teacher.dat"), FileMode.OpenOrCreate);


            BinaryFormatter formatter = new BinaryFormatter();


            formatter.Serialize(fs, multiclassSupportVectorLearning);


            fs.Close();
        }

        


        internal static MulticlassSupportVectorMachine<HistogramIntersection> ReadBinary(bool log)
        {
            BinaryFormatter formatter = new BinaryFormatter();

            FileStream fs = new FileStream(Path.Combine(Application.StartupPath, "Resources/" +
                "teacher.dat"), FileMode.OpenOrCreate);

            MulticlassSupportVectorMachine<HistogramIntersection> multiclassSupportVectorLearning =
                (MulticlassSupportVectorMachine<HistogramIntersection>)formatter.Deserialize(fs);

            fs.Close();

            return multiclassSupportVectorLearning;
        }
    }
}