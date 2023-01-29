using Cloud_Ark_Sim.lib.Configuration;
using Cloud_Ark_Sim.lib.Propellants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloud_Ark_Sim.lib.Ship
{
    class ThrusterQuad : Steppable
    {
        //Store the direction of the thruster quads normal vector (all thrusters in the quad are rotated about the normal vector)
        private CardinalDirection normalDirection;

        private Dictionary<String, Thruster> thrusters = new();

        private PropellantTank fuelTank;
        private PropellantTank oxidizerTank;

        public ThrusterQuad(ThrusterConfiguration config, CardinalDirection _normalDirection, Point _position, double _minimumThrottlePercent)
        {
            normalDirection = _normalDirection;

            //Copy fuel & oxidizer tank in case shared by other quads
            fuelTank = new PropellantTank(config.fuelTank);
            oxidizerTank = new PropellantTank(config.oxidizerTank);

            //Make a new ThrusterConfiguration object to hold the new fuel & oxidizer tank references
            ThrusterConfiguration thrusterConfig = new(config.specificImpulse, config.maxThrust, fuelTank, oxidizerTank, config.mixture);

            //Create thrusters in quad. Orientations are relative to the quad, looking towards the front of the ship
            thrusters["Fore"] = new Thruster(thrusterConfig, _position, CardinalDirection.FORE, _minimumThrottlePercent);
            thrusters["Aft"] = new Thruster(thrusterConfig, _position, CardinalDirection.AFT, _minimumThrottlePercent);

            //Convert direction of port & starboard thrusters from local-space to ship-space
            switch (normalDirection) 
            {
                case CardinalDirection.ZENITH:
                    thrusters["Port"] = new Thruster(thrusterConfig, _position, CardinalDirection.PORT, _minimumThrottlePercent);
                    thrusters["Starboard"] = new Thruster(thrusterConfig, _position, CardinalDirection.STARBORD, _minimumThrottlePercent);
                    break;
                case CardinalDirection.PORT:
                    thrusters["Port"] = new Thruster(thrusterConfig, _position, CardinalDirection.NADIR, _minimumThrottlePercent);
                    thrusters["Starboard"] = new Thruster(thrusterConfig, _position, CardinalDirection.ZENITH, _minimumThrottlePercent);
                    break;
                case CardinalDirection.NADIR:
                    thrusters["Port"] = new Thruster(thrusterConfig, _position, CardinalDirection.STARBORD, _minimumThrottlePercent);
                    thrusters["Starboard"] = new Thruster(thrusterConfig, _position, CardinalDirection.PORT, _minimumThrottlePercent);
                    break;
                case CardinalDirection.STARBORD:
                    thrusters["Port"] = new Thruster(thrusterConfig, _position, CardinalDirection.ZENITH, _minimumThrottlePercent);
                    thrusters["Starboard"] = new Thruster(thrusterConfig, _position, CardinalDirection.NADIR, _minimumThrottlePercent);
                    break;
            }

            
        }

        public void SetThrusterThrottlePercentage(CardinalDirection thruster, double throttlePercentage)
        {
            switch(thruster) {
            case CardinalDirection.FORE:
                thrusters["Fore"].SetThrottlePercentage(throttlePercentage);
                break;
            case CardinalDirection.AFT:
                thrusters["Aft"].SetThrottlePercentage(throttlePercentage);
                break;
            case CardinalDirection.PORT:
                thrusters["Port"].SetThrottlePercentage(throttlePercentage);
                break;
            case CardinalDirection.STARBORD:
                thrusters["Starboard"].SetThrottlePercentage(throttlePercentage);
                break;
            }
        }

        public Thruster GetThruster(CardinalDirection thruster)
        {
            switch (thruster)
            {
                case CardinalDirection.FORE:
                    return(thrusters["Fore"]);
                case CardinalDirection.AFT:
                    return (thrusters["Aft"]);
                case CardinalDirection.PORT:
                    return (thrusters["Port"]);
                case CardinalDirection.STARBORD:
                    return (thrusters["Starboard"]);
                default:
                    throw new KeyNotFoundException();
            }
        }

        public PropellantTank GetFuelTank()
        {
            return fuelTank;
        }

        public PropellantTank GetOxidizerTank()
        {
            return oxidizerTank;
        }

        public void ResetThrusters()
        {
            GetThruster(CardinalDirection.FORE).SetThrottlePercentage(0);
            GetThruster(CardinalDirection.PORT).SetThrottlePercentage(0);
            GetThruster(CardinalDirection.AFT).SetThrottlePercentage(0);
            GetThruster(CardinalDirection.STARBORD).SetThrottlePercentage(0);
        }

        public void Step()
        {
            thrusters["Fore"].Step();
            thrusters["Aft"].Step();
            thrusters["Port"].Step();
            thrusters["Starboard"].Step();
        }
    }
}
