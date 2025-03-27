using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Math_Test
{
    public  class MathUtils
    {


        public double GetAverage(double a, double b)
        {
            if (a == b)
                throw new ArgumentException("dump average");

            return (a+b)/2;
        }

        public bool IsEven(int number)
        {
            return number % 2 == 0;
        }
    }
}
