using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloud_Ark_Sim.lib.Propellants
{
    class Propellant
    {
        private double molarMass;

        //KG/mol
        public Propellant(double _molarMass)
        {
            molarMass = _molarMass;
        }

        public void SetMolarMass(double _molarMass)
        {
            molarMass = _molarMass;
        }

        public double GetMolarMass()
        {
            return molarMass;
        }

        //Converts mol of propellant to kg
        public double ToMass(double mol)
        {
            return mol * molarMass;
        }

        //Converts kg of propellant to mol
        public double ToMol(double mass)
        {
            return mass * (1 / molarMass);
        }
    }
}
