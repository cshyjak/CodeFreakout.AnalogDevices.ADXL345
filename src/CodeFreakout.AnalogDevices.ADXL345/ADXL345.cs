using System;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;

namespace CodeFreakout.AnalogDevices.ADXL345
{
    public class ADXL345
    {
        private readonly SPI.Configuration _accelerometerConfig;
        private readonly SPI _accelerometer;
        private int _lastReadXValue;
        private int _lastReadYValue;
        private int _lastReadZValue;
        private readonly Thread _dataReceiveThread;

        public ThreeAxisReading GetAxisValues()
        {
            return new ThreeAxisReading(_lastReadXValue, _lastReadYValue, _lastReadZValue);
        }

        public ADXL345(Cpu.Pin pin, SPI.SPI_module module)
        {
            _accelerometerConfig = new SPI.Configuration(
                pin, // SS-pin
                false,             // SS-pin active state
                0,                 // The setup time for the SS port
                0,                 // The hold time for the SS port
                true,              // The idle state of the clock
                true,             // The sampling clock edge
                1600,              // The SPI clock rate in KHz
                module   // The used SPI bus (refers to a MOSI MISO and SCLK pinset)
            );

            _accelerometer = new SPI(_accelerometerConfig);

            //Put device into +/-2g mode
            _accelerometer.Write(new byte[] { 0x31, 0x08 });

            //Set FIFO to stream
            _accelerometer.Write(new byte[] { 0x38, 0x80 });

            //Put device into measurement mode
            _accelerometer.Write(new byte[] { 0x2D, 0x08 });

            _dataReceiveThread = new Thread(ReadValues);
            _dataReceiveThread.Start();
        }

        private void ReadValues()
        {
            //attempt to read values
            var valueLocations = new byte[2] { 0xF2, 0xFF };
            var values = new byte[7];

            while (true)
            {
                _accelerometer.WriteRead(valueLocations, values);

                _lastReadXValue = (short)(values[2] << 8) | values[1];
                _lastReadYValue = (short)(values[4] << 8) | values[3];
                _lastReadZValue = (short)(values[6] << 8) | values[5];
            }
        }
    }
}
