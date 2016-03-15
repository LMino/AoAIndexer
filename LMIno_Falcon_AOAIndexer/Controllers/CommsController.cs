using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LMIno.Falcon.AOAIndexer.Controllers
{



    class CommsController
    {
        public SerialPort CurrentPort { get; set; }

        private string[] _ports;
        public bool PortFound;

        public CommsController()
        {
            SearchForDevice();
        }

        public void SearchForDevice()
        {
            while (!PortFound)
            {
                _ports = SerialPort.GetPortNames(); //Gets all of the USB devices connected to the pc.
                foreach (var p in _ports) 
                {
                    CurrentPort = new SerialPort(p, 115200);
                    PortFound = isArduino(CurrentPort); //Checks whether this port has an arduino on it.
                }
            }
        }

        private bool isArduino(SerialPort p)
        {
            byte[] buffer = new byte[3];
            buffer[0] = Convert.ToByte(255); //Message begin
            buffer[1] = Convert.ToByte(128); //Handshake
            buffer[2] = Convert.ToByte(7);  //Irrelevant value, just to keep the byte from being all 0s
            try
            {
                p.Open(); //Open the usb port.

                Console.WriteLine("Trying port " + p.PortName + ":" + 115200); //Debug text for the console.
                p.Write(buffer, 0, 3); //Send handhake to usb device
                Thread.Sleep(500); //Wait for response
                var fromPort = p.ReadExisting(); //Read incoming response from arduino
                Console.WriteLine("Response: " + fromPort); //Debug text for console

                //If the response contained 128 (#define HANDSHAKE in the arduino), the device identified itself as the arudino.
                //Could be defined as some other number, to distinguish between multiple arduinos.
                if (fromPort.Contains("128"))
                {
                    Console.WriteLine("Found arduino on " + p.PortName); //Announce arduino found on console.
                    return true;
                }
                p.Close(); //Close comm ports.
            }
            catch (IOException) //Port not open
            {
                return false;
            }
            catch (UnauthorizedAccessException) //Port open, but in use by another process
            {
                return false;
            }
            return false;
        }
    }
}
