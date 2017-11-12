using Accord;
using Accord.Imaging;
using Accord.MachineLearning;
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
        internal static void WriteBinary(BagOfVisualWords<IFeatureDescriptor<double[]>, double[], BinarySplit, HistogramsOfOrientedGradientsCorrect> bagOfVisualWords)
        {
            FileStream fs = new FileStream(Path.Combine(Application.StartupPath, "Resources/" +
                "hogBow.dat"), FileMode.OpenOrCreate);


            BinaryFormatter formatter = new BinaryFormatter();
            

            formatter.Serialize(fs, bagOfVisualWords);


            fs.Close();
        }

        internal static BagOfVisualWords<IFeatureDescriptor<double[]>, double[], BinarySplit, HistogramsOfOrientedGradientsCorrect> ReadBinary()
        {
            BinaryFormatter formatter = new BinaryFormatter();
            
            FileStream fs = new FileStream(Path.Combine(Application.StartupPath, "Resources/" +
                "hogBow.dat"), FileMode.OpenOrCreate);

            BagOfVisualWords<IFeatureDescriptor<double[]>, double[], BinarySplit, HistogramsOfOrientedGradientsCorrect> hogBow = 
                (BagOfVisualWords<IFeatureDescriptor<double[]>, double[], BinarySplit, HistogramsOfOrientedGradientsCorrect>)formatter.Deserialize(fs);

            return hogBow;
        }
    }
}
//BagOfVisualWords<IFeatureDescriptor<double[]>, double[], BinarySplit, HistogramsOfOrientedGradients>