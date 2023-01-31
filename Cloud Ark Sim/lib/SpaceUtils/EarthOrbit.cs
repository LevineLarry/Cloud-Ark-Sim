using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloud_Ark_Sim.lib.SpaceUtils
{
    class EarthOrbit
    {
        private static readonly double G = 0.000000000066743; //SI units
        public static double GetAltitude(Vect3D position)
        {
            return position.Magnitude() - Sim.GetEarthRadius();
        }

        private static double GetAcceleration1D(double r)
        {
            double a = -1 * G * (Sim.GetEarthMass() / Math.Pow(r, 2));
            return a;
        }

        public static Vect3D GetAccelerationVector(Vect3D position)
        {
            double mag = position.Magnitude();
            double a = GetAcceleration1D(mag);
            return new Vect3D(a * position.UnitVector().GetX(), a * position.UnitVector().GetY(), a * position.UnitVector().GetZ());
        }
    }
}
