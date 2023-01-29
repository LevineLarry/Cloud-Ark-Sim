using Cloud_Ark_Sim.lib.Propellants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloud_Ark_Sim.lib.Ship
{
    class PropellantTank
    {
        private double capacityKG; //Capacity defined in terms of mas rather than volume to simplify math
        private double amountKG; //Amount of fuel currently in propellant tank
        private double tankWeight; //Empty weight of tank
        private Propellant propellant; //The type of propellant in the tank
        private TankTypes tankType; //Fuel or oxidizer

        public PropellantTank(double _capacity, TankTypes _type, double _tankWeight)
        {
            capacityKG = _capacity;
            amountKG = _capacity;
            tankType = _type;
            tankWeight = _tankWeight;
        }

        //Copy constructor
        public PropellantTank(PropellantTank otherTank)
        {
            capacityKG = otherTank.capacityKG;
            amountKG = capacityKG;
            tankType = otherTank.tankType;
            tankWeight = otherTank.tankWeight;
        }

        //Gets amount of remaining propellant, in KG
        public double GetAmountKG()
        {
            return amountKG;
        }

        //Gets amount of remaining propellant, in moles
        public double GetAmountMol()
        {
            return propellant.ToMol(amountKG);
        }

        //Drains _amount KG from tank. Returns true if tank has enough, or false if not
        public bool UseKG(double _amount)
        {
            if(amountKG >= _amount)
            {
                amountKG -= _amount;
                return true;
            } else
            {
                return false;
            }
        }

        //Drains _amount mol from tank. Returns true if tank has enough or false if not
        public bool UseMol(double _amount)
        {
            if(amountKG >= propellant.ToMass(_amount))
            {
                amountKG -= propellant.ToMass(_amount);
                return true;
            } else
            {
                return false;
            }
        }
    }
}
