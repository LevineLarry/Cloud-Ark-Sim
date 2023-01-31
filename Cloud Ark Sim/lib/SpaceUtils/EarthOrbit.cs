using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloud_Ark_Sim.lib.SpaceUtils
{
    class EarthOrbit
    {
        private static readonly decimal G = 6.6743e-11m; //SI units
        public static double GetAltitude(Vect3D position)
        {
            return position.Magnitude() - Sim.GetEarthRadius();
        }

        private static double GetAcceleration1D(double r, double mass)
        {
            double a = -1 * (double)G * (((double)Sim.GetEarthMass() * mass) / Math.Pow(r, 2));
            return a;
        }

        public static Vect3D GetAccelerationVector(Vect3D position, double mass)
        {
            double a = GetAcceleration1D(position.Magnitude(), mass);
            return new Vect3D(a * position.UnitVector().GetX(), a * position.UnitVector().GetY(), a * position.UnitVector().GetZ());
        }
    }
}
