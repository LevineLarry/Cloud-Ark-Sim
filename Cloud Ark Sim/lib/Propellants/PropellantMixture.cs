using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloud_Ark_Sim.lib.Propellants
{
    class PropellantMixture
    {
        public readonly double oxidizerFuelRatio;
        public readonly Propellant fuel;
        public readonly Propellant oxidizer;

        public PropellantMixture(double _oxidizerFuelRatio, Propellant _fuel, Propellant _oxidizer)
        {
            oxidizerFuelRatio = _oxidizerFuelRatio;
            fuel = _fuel;
            oxidizer = _oxidizer;
        }

        //KG. Converts oxidizer amount to moles, uses the fuel-oxidizer ratio, and then converts to kg of fuel
        public double GetFuelRequired(double oxidizerAmount)
        {
            return fuel.ToMass(oxidizer.ToMol(oxidizerAmount) * (1 / oxidizerFuelRatio));
        }

        //KG. Converts fuel amount to moles, uses the fuel-oxidizer ratio, and then converts to kg of oxidizer
        public double GetOxidizerRequired(double fuelAmount)
        {
            return oxidizer.ToMass(fuel.ToMol(fuelAmount) * oxidizerFuelRatio);
        }

        //KG. Returns the kg of fuel within 1 kg of propellant mixture
        public double GetKgFuelPerKGProp()
        {
            return fuel.ToMass(oxidizerFuelRatio) / (oxidizerFuelRatio + 1);
        }

        //KG. Returns the kg of oxidizer within 1 kg of propellant mixture
        public double GetKgOxidizerPerKGProp()
        {
            return oxidizer.ToMass(1) / (oxidizerFuelRatio + 1);
        }
    }
}
