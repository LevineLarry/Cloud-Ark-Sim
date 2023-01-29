using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloud_Ark_Sim.lib
{
    class EulerOrientation
    {
        private double roll;
        private double pitch;
        private double yaw;

        public EulerOrientation()
        {
            roll = 0;
            pitch = 0;
            yaw = 0;
        }

        public EulerOrientation(double _roll, double _pitch, double _yaw)
        {
            roll = _roll;
            pitch = _pitch;
            yaw = _yaw;
        }
        
        public EulerOrientation(EulerOrientation other)
        {
            roll = other.roll;
            pitch = other.pitch;
            yaw = other.yaw;
        }

        public void Apply(double _roll, double _pitch, double _yaw)
        {
            roll += _roll;
            pitch += _pitch;
            yaw += _yaw;
        }

        public double GetRoll()
        {
            return roll;
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
