using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMIno.Falcon.AOAIndexer.Resources
{
    class AoaState {
        public const int H = 1;
        public const int O = 2;
        public const int L = 4;

        public const int HO = H + O;
        public const int OL = O + L;
        public const int HL = H + L;

        public const int HOL = H + O + L;
        public const int OFF = 0;

    }
}
