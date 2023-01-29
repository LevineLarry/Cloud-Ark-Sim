using Cloud_Ark_Sim.lib.Configuration;
using Cloud_Ark_Sim.lib.Propellants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloud_Ark_Sim.lib.Ship
{
    class Thruster : Steppable
    {
        //Set throttle setting
        //Get throttle setting
        //Get orientation
        //Get position
        //Set orientation
        //Set position
        //Set impulse
        //Set thrust

        //Stores the thrusters direction
        private CardinalDirection direction;

        //Stores the thrusters position relative to the ship
        private Point position;

        private double specificImpulse;
        private double maxThrust;
        private double throttleSetting;
        private double minimumThrottlePercent;

        private PropellantTank fuelTank;
        private PropellantTank oxidizerTank;
        private PropellantMixture mixture;

        public Thruster(ThrusterConfiguration config, Point _position, CardinalDirection _direction, double _minimumThrottlePercent)
        {
            position = _position;
            direction = _direction;
            specificImpulse = config.specificImpulse;
            maxThrust = config.maxThrust;
            minimumThrottlePercent = _minimumThrottlePercent;

            fuelTank = config.fuelTank; 
            oxidizerTank = config.oxidizerTank;

            mixture = config.mixture;
            throttleSetting = 0;
        }

        public void SetLocalPosition(Point _position)
        {
            position = _position;
        }

        public void SetDirection(CardinalDirection newDirection)
        {
            direction = newDirection;
        }

        public double GetMassFlow(double throttlePercentage)
        {
            if (throttlePercentage > 1 || throttlePercentage < 0)
            {
                throw new ArgumentOutOfRangeException("throttlePercentage must be between 0 and 1");
            }

            double thrustN = maxThrust * throttlePercentage;
            double massFlow = thrustN / (specificImpulse * 9.81);
            return massFlow;
        }

        //Percentage of max throttle. 0-1
        private double GetKGOxidizerForBurn(double throttlePercentage)
        {
            double massFlow = this.GetMassFlow(throttlePercentage);
            return (massFlow * Sim.GetTimestep()) * mixture.GetKgOxidizerPerKGProp(); //Multiply the mass flow by the sim timestep to get mass per timestep. Multiply that by the kg of oxidizer per kg of propellant to get the amount of oxidizer
        }

        //Percentage of max throttle. 0-1
        private double GetKGFuelForBurn(double throttlePercentage)
        {
            double massFlow = this.GetMassFlow(throttlePercentage);
            return (massFlow * Sim.GetTimestep()) * mixture.GetKgFuelPerKGProp(); //Multiply the mass flow by the sim timestep to get mass per timestep. Multiply that by the kg of fuel per kg of propellant to get the amount of fuel
        }

        //Burns engine for 1 time step & drains appropriate prop amount TODO: affect ship velocity, rotation, and angular velocity
        private void Burn()
        {
            if(GetKGFuelForBurn(throttleSetting) > fuelTank.GetAmountKG() || GetKGOxidizerForBurn(throttleSetting) > oxidizerTank.GetAmountKG())
            {
                SetThrottlePercentage(0); //Turn off thruster if out of fuel
            }

            fuelTank.UseKG(GetKGFuelForBurn(throttleSetting)); //Drain appropriate amount of fuel
            oxidizerTank.UseKG(GetKGOxidizerForBurn(throttleSetting)); //Drain appropriate amount of oxidizer
        }

        public void SetThrottlePercentage(double throttlePercentage)
        {
            if(throttlePercentage > minimumThrottlePercent)
            {
                throttleSetting = throttlePercentage;
            }
        }

        public double GetThrottlePercentage()
        {
            return throttleSetting;
        }

        public double GetThrust()
        {
            return throttleSetting * maxThrust;
        }

        public void Step()
        {
            Burn();
        }
    }
}
