using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloud_Ark_Sim.lib
{
    class Vect3D
    {
        private double x;
        private double y;
        private double z;

        public Vect3D()
        {
            x = 0;
            y = 0;
            z = 0;
        }
        public Vect3D(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Vect3D(Vect3D other)
        {
            x = other.x;
            y = other.y;
            z = other.z;
        }

        //Adds to the state vector
        public void Apply(double _x, double _y, double _z)
        {
            x += _x;
            y += _y;
            z += _z;
        }

        public double GetX()
        {
            return x;
        }

        public double GetY()
        {
            return y;
        }

        public double GetZ()
        {
            return z;
        }

        public double Magnitude()
        {
            return Math.Sqrt(Math.Pow(x,2) + Math.Pow(y,2) + Math.Pow(z,2));
        }

        public Vect3D UnitVector()
        {
            return new Vect3D(x / Magnitude(), y / Magnitude(), z / Magnitude());
        }
    }
}
