using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Test.OverrideAccord;

namespace Test.OverrideAccord
{
    public interface IKernel<in T>
    {
        /// <summary>
        ///   The kernel function.
        /// </summary>
        /// 
        /// <param name="x">Vector <c>x</c> in input space.</param>
        /// <param name="y">Vector <c>y</c> in input space.</param>
        /// <returns>Dot product in feature (kernel) space.</returns>
        /// 
        double Function(T x, T y);
    }
}
