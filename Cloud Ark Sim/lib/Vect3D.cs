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


    }
}
