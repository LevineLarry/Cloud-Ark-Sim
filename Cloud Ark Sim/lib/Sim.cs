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
        private static decimal earthMass = 5.97e21m;
        private static double earthRadius = 6378100;

        public static void Init(double _timestep)
        {
            timestep = _timestep;
        }

        public static double GetTimestep()
        {
            return timestep;
        }

        public static decimal GetEarthMass()
        {
            return earthMass;
        }

        public static double GetEarthRadius()
        {
            return earthRadius;
        }
    }
}
