using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloud_Ark_Sim.lib
{
    class PidTune
    {
        private double kp;
        private double ki;
        private double kd;

        public PidTune(double _kp, double _ki, double _kd)
        {
            kp = _kp;
            ki = _ki;
            kd = _kd;
        }

        public double GetP()
        {
            return kp;
        }

        public double GetI()
        {
            return ki;
        }

        public double GetD()
        {
            return kd;
        }
    }
}
