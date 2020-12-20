using System;

namespace FaceLibrary.Hair
{
    // https://answers.opencv.org/question/167044/frequential-analysis-in-opencv/
    public class FrequentialMap
    {
        public FrequentialMap(int kernelSize = 100, double f0 = 0.4, double sigma = 0.04)
        {
            KernelSize = kernelSize;
            F0 = f0;
            Sigma = sigma;
        }

        public int KernelSize { get; }

        public double F0 { get; }

        public double Sigma { get; }

        public Kernel Run()
        {
            return GetGaussianFilter();
        }

        private Kernel GetGaussianFilter()
        {
            var kernel = new double[KernelSize, KernelSize];

            double min = -0.5;
            double max = 0.5;
            double increment = (max - min) / KernelSize;

            double MapIndex(int x) => min + x * increment;

            double sum = 0;

            for (var i = 0; i < kernel.GetLength(0); i++)
            {
                for (var j = 0; j < kernel.GetLength(1); j++)
                {
                    var x = MapIndex(i);
                    var y = MapIndex(j);

                    kernel[i, j] = HairFilter(x, y);
                    sum += kernel[i, j];
                }
            }

            // Normalise the values
            for (var i = 0; i < kernel.GetLength(0); i++)
            {
                for (var j = 0; j < kernel.GetLength(1); j++)
                {
                    kernel[i, j] /= sum;
                }
            }

            return new Kernel(kernel);
        }

        private double HairFilter(double x, double y)
        {
            var fPheta = Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));
            return Math.Exp((-Math.Pow(fPheta - F0, 2)) / (2 * Math.Pow(Sigma, 2)));
        }
    }
}
