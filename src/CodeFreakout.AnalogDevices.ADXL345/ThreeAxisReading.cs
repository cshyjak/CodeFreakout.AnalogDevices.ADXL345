using System;
using Microsoft.SPOT;

namespace CodeFreakout.AnalogDevices.ADXL345
{
    public class ThreeAxisReading
    {
        public int XAxis { get; private set; }

        public int YAxis { get; private set; }

        public int ZAxis { get; private set; }

        public ThreeAxisReading(int xAxis, int yAxis, int zAxis)
        {
            XAxis = xAxis;
            YAxis = yAxis;
            ZAxis = zAxis;
        }
    }
}
