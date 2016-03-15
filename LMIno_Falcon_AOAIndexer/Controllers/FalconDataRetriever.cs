using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using F4SharedMem;
using F4SharedMem.Headers;
using LMIno.Falcon.AOAIndexer.Resources;

namespace LMIno.Falcon.AOAIndexer.Controllers
{
    class FalconDataRetriever {

        private Reader _sharedMemReader = new Reader();  //Memory reader from lightning's library
        private FlightData _lastFlightData; //Most recent falcon memory data 

        /// <summary>
        /// Retrieve the AoAState of the angle of attack indexer in the game.
        /// </summary>
        /// <returns>an AoAState object representation of the current state of the aoa indexer in falcon</returns>
        public int GetAoa() {

            try {
                _lastFlightData = _sharedMemReader.GetCurrentData(); //Retrieve the data from falcon, store in variable
                var lightBits = (LightBits) _lastFlightData.lightBits; // Get the LightBits from the retrieved falcon data

                bool high = (lightBits & LightBits.AOAAbove) != 0; //If specific LightBit is not = 0 - means the LED is on in game.
                bool on = (lightBits & LightBits.AOAOn) != 0;
                bool low = (lightBits & LightBits.AOABelow) != 0;

                var totalValue = 0; //The total value of all of the LEDs that are on.

                if (high) //If AoA.Above is on in the game add it to the total value of the AoAState to send.
                {
                    totalValue += AoaState.H;
                }
                if (on) //same as above but for the "on" bit.
                {
                    totalValue += AoaState.O;
                }
                if (low) // low bit.
                {
                    totalValue += AoaState.L;
                }

                //Determine which AoAState the totalValue variable corresponds to.
                //Not exactly necessary, but why the hell not.

                switch (totalValue) 
                {
                    case AoaState.H:
                        return AoaState.H;
                    case AoaState.L:
                        return AoaState.L;
                    case AoaState.O:
                        return AoaState.O;
                    case AoaState.HL:
                        return AoaState.HL;
                    case AoaState.HO:
                        return AoaState.HO;
                    case AoaState.OL:
                        return AoaState.OL;
                    case AoaState.HOL:
                        return AoaState.HOL;
                    case AoaState.OFF:
                        return AoaState.OFF;
                    default:
                        return AoaState.OFF;
                }
            }
            catch (NullReferenceException) { //LightBits will be null if the game isnt running.
                Console.WriteLine("Falcon is not running!");
                return 0;
            }
        }
    }
}
