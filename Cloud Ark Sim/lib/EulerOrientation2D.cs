using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloud_Ark_Sim.lib
{
    class EulerOrientation2D
    {
        private double pitch;
        private double yaw;

        public EulerOrientation2D()
        {
            pitch = 0;
            yaw = 0;
        }

        public EulerOrientation2D(double _pitch, double _yaw)
        {
            pitch = _pitch;
            yaw = _yaw;
        }

        public EulerOrientation2D(EulerOrientation2D other)
        {
            pitch = other.pitch;
            yaw = other.yaw;
        }

        public void Apply(double _pitch, double _yaw)
        {
            pitch += _pitch;
            yaw += _yaw;
        }

        public double GetPitch()
        {
            return pitch;
        }

        public double GetYaw()
        {
            return yaw;
        }
    }
}
