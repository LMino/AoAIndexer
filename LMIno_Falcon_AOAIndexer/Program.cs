using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LMIno.Falcon.AOAIndexer.Controllers;
using F4SharedMem;

namespace LMIno.Falcon.AOAIndexer
{
    class Program {
        static void Main(string[] args) {

            var cc = new CommsController();  // Responsible for opening the serial port.
            var fdr = new FalconDataRetriever(); // Responsible for getting the data from Falcon - uses lightning's dll.
            var cmc = new CommsMessageController(cc, fdr); // Sends the data to the arudino
        }
    }
}
