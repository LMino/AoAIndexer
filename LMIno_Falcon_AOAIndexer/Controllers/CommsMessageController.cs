using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LMIno.Falcon.AOAIndexer.Resources;

namespace LMIno.Falcon.AOAIndexer.Controllers
{
    class CommsMessageController
    {
        private SerialPort p;
        private CommsController _cc;

        private int _lastAoa = AoaState.HOL; //The last AOA state which was sent to the arduino

        public CommsMessageController(CommsController cc, FalconDataRetriever fdr) {
            _cc = cc; //An instance of CommsController
            p = _cc.CurrentPort; // The port that the arduino is conected to.

            

            while (true) { // runs forever
                if (_cc.PortFound) {
                    //RunTestLoop(); //Uncomment to test LED Wiring

                    var newAoa = fdr.GetAoa();

                    if (_lastAoa != newAoa) {    //Check wether the AoA state has changed
                        _lastAoa = newAoa;       //If it has - set the last state as the new one
                        SendAoaMessage(newAoa);  //Send the new AoA state
                    }

                    
                }
            }
        }

        /// <summary>
        /// Send a AoAState message to the arduino
        /// </summary>
        /// <param name="aoaState">The integer representation of the AoAState - defined in the AoAState class</param>
        public void SendAoaMessage(int aoaState)
        {
            byte[] buffer = new byte[3];
            buffer[0] = Convert.ToByte(255); //Message begin
            buffer[1] = Convert.ToByte(170); //New AoA State
            buffer[2] = Convert.ToByte(aoaState);  //VALUE TO SEND
            try {
                Console.WriteLine("State: " + buffer.GetValue(2)); //See what were sending in the console - for debug purposes
                p.Write(buffer, 0, 3); //Send the message over the serial port
                Thread.Sleep(100); //Wait
            }
            catch (InvalidOperationException) {  //Unfinished attempt at handling loss of connection to the arduino
                Console.WriteLine("Conection to AOA Indexer lost");
                _cc.PortFound = false;
                _cc.SearchForDevice();
            }

        }

        /// <summary>
        /// A test sending all of the AoAStates in sequence, to check the LED wiring
        /// </summary>
        public void RunTestLoop()
        {
                SendAoaMessage(AoaState.H);
                SendAoaMessage(AoaState.O);
                SendAoaMessage(AoaState.L);
                SendAoaMessage(AoaState.HO);
                SendAoaMessage(AoaState.OL);
                SendAoaMessage(AoaState.HL);
                SendAoaMessage(AoaState.HOL);
        }

    }
}
