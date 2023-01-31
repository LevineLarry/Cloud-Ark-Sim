using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloud_Ark_Sim.lib
{
    class StateVector
    {
        public Vect3D velocity;
        public Vect3D position;
        public Vect3D acceleration;

        public StateVector()
        {
            velocity = new();
            position = new();
            acceleration = new();
        }

        public StateVector(StateVector other)
        {
            velocity = new(other.velocity);
            position = new(other.position);
            acceleration = new(other.acceleration);
        }
    }
}
