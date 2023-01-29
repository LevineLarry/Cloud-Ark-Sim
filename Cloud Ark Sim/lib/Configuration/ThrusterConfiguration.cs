using Cloud_Ark_Sim.lib.Propellants;
using Cloud_Ark_Sim.lib.Ship;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloud_Ark_Sim.lib.Configuration
{
    class ThrusterConfiguration
    {
        public double specificImpulse;
        public double maxThrust;
        public PropellantTank fuelTank;
        public PropellantTank oxidizerTank;
        public PropellantMixture mixture;

        public ThrusterConfiguration(double _specificImpulse, double _maxThrust, PropellantTank _fuelTank, PropellantTank _oxidizerTank, PropellantMixture _mixture)
        {
            specificImpulse = _specificImpulse;
            maxThrust = _maxThrust;
            fuelTank = _fuelTank;
            oxidizerTank = _oxidizerTank;
            mixture = _mixture;
        }
    }
}
