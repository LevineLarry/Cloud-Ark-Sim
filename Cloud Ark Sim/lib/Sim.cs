using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloud_Ark_Sim.lib
{
    class Sim
    {
        private static double timestep; //The simulation timestep in seconds

        public static void Init(double _timestep)
        {
            timestep = _timestep;
        }

        public static double GetTimestep()
        {
            return timestep;
        }
    }
}
