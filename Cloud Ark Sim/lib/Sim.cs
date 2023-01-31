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
        private static double earthMass = 5_970_000_000_000_000_000_000_000d;
        private static double earthRadius = 6_378_100d;

        public static void Init(double _timestep)
        {
            timestep = _timestep;
        }

        public static double GetTimestep()
        {
            return timestep;
        }

        public static double GetEarthMass()
        {
            return earthMass;
        }

        public static double GetEarthRadius()
        {
            return earthRadius;
        }
    }
}
