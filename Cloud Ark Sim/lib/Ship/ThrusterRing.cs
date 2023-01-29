using Cloud_Ark_Sim.lib.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloud_Ark_Sim.lib.Ship
{
    class ThrusterRing : Steppable
    {
        private Dictionary<String, ThrusterQuad> thrusterQuads = new();
        private double diameter;
        private double xPos;

        //Thruster rings are centered on the ship, only shifted in the long (x) direction from CoM, and have 4 thruster quads. Diameter (m)
        public ThrusterRing(double _xPos, ThrusterConfiguration config, double _diameter, double _minimumThrottlePercent)
        {
            diameter = _diameter;
            xPos = _xPos;

            thrusterQuads["Zenith"] = new ThrusterQuad(config, CardinalDirection.ZENITH, new Point(xPos, 0, diameter/2), _minimumThrottlePercent);
            thrusterQuads["Nadir"] = new ThrusterQuad(config, CardinalDirection.NADIR, new Point(xPos, 0, -diameter/2), _minimumThrottlePercent);
            thrusterQuads["Port"] = new ThrusterQuad(config, CardinalDirection.PORT, new Point(xPos, -diameter/2, 0), _minimumThrottlePercent);
            thrusterQuads["Starboard"] = new ThrusterQuad(config, CardinalDirection.STARBORD, new Point(xPos, diameter/2, 0), _minimumThrottlePercent);
        }

        public double GetRollTorque()
        {
            double rollTorque = 0;
            rollTorque += thrusterQuads["Zenith"].GetThruster(CardinalDirection.PORT).GetThrust() * (diameter / 2);
            rollTorque -= thrusterQuads["Zenith"].GetThruster(CardinalDirection.STARBORD).GetThrust() * (diameter / 2);

            rollTorque += thrusterQuads["Port"].GetThruster(CardinalDirection.PORT).GetThrust() * (diameter / 2);
            rollTorque -= thrusterQuads["Port"].GetThruster(CardinalDirection.STARBORD).GetThrust() * (diameter / 2);

            rollTorque += thrusterQuads["Nadir"].GetThruster(CardinalDirection.PORT).GetThrust() * (diameter / 2);
            rollTorque -= thrusterQuads["Nadir"].GetThruster(CardinalDirection.STARBORD).GetThrust() * (diameter / 2);

            rollTorque += thrusterQuads["Starboard"].GetThruster(CardinalDirection.PORT).GetThrust() * (diameter / 2);
            rollTorque -= thrusterQuads["Starboard"].GetThruster(CardinalDirection.STARBORD).GetThrust() * (diameter / 2);

            return rollTorque;
        }

        //Returns the cosine losses for the thrusters in the ring when fired along the long axis, accounting for their position. Returns a number between 0 and 1
        public double GetThrusterCosineLosses()
        {
            double r = diameter / 2;
            return r * Math.Pow(Math.Pow(xPos, 2) + Math.Pow(r, 2), -0.5);
        }

        public double GetYawTorque()
        {
            double yawTorque = 0;

            //If forward of CoM
            if(xPos > 0)
            {
                yawTorque -= GetQuad(CardinalDirection.ZENITH).GetThruster(CardinalDirection.PORT).GetThrust();
                yawTorque += GetQuad(CardinalDirection.ZENITH).GetThruster(CardinalDirection.STARBORD).GetThrust();
                yawTorque -= GetQuad(CardinalDirection.NADIR).GetThruster(CardinalDirection.STARBORD).GetThrust();
                yawTorque += GetQuad(CardinalDirection.NADIR).GetThruster(CardinalDirection.PORT).GetThrust();
            }

            //If aft of CoM
            if(xPos < 0)
            {
                yawTorque += GetQuad(CardinalDirection.ZENITH).GetThruster(CardinalDirection.PORT).GetThrust();
                yawTorque -= GetQuad(CardinalDirection.ZENITH).GetThruster(CardinalDirection.STARBORD).GetThrust();
                yawTorque += GetQuad(CardinalDirection.NADIR).GetThruster(CardinalDirection.STARBORD).GetThrust();
                yawTorque -= GetQuad(CardinalDirection.NADIR).GetThruster(CardinalDirection.PORT).GetThrust();
            }
            
            return yawTorque;
        }

        public double GetPitchTorque()
        {
            double pitchTorque = 0;

            //If forward of CoM
            if (xPos > 0)
            {
                pitchTorque += GetQuad(CardinalDirection.PORT).GetThruster(CardinalDirection.PORT).GetThrust();
                pitchTorque -= GetQuad(CardinalDirection.PORT).GetThruster(CardinalDirection.STARBORD).GetThrust();
                pitchTorque += GetQuad(CardinalDirection.STARBORD).GetThruster(CardinalDirection.STARBORD).GetThrust();
                pitchTorque -= GetQuad(CardinalDirection.STARBORD).GetThruster(CardinalDirection.PORT).GetThrust();
            }

            //If aft of CoM
            if (xPos < 0)
            {
                pitchTorque -= GetQuad(CardinalDirection.PORT).GetThruster(CardinalDirection.PORT).GetThrust();
                pitchTorque += GetQuad(CardinalDirection.PORT).GetThruster(CardinalDirection.STARBORD).GetThrust();
                pitchTorque -= GetQuad(CardinalDirection.STARBORD).GetThruster(CardinalDirection.STARBORD).GetThrust();
                pitchTorque += GetQuad(CardinalDirection.STARBORD).GetThruster(CardinalDirection.PORT).GetThrust();
            }

            return pitchTorque;
        }

        public ThrusterQuad GetQuad(CardinalDirection _direction)
        {
            switch(_direction)
            {
                case CardinalDirection.ZENITH:
                    return thrusterQuads["Zenith"];
                case CardinalDirection.PORT:
                    return thrusterQuads["Port"];
                case CardinalDirection.NADIR:
                    return thrusterQuads["Nadir"];
                case CardinalDirection.STARBORD:
                    return thrusterQuads["Starboard"];
                default:
                    throw new KeyNotFoundException();
            }
        }

        public void ResetThrusters()
        {
            GetQuad(CardinalDirection.ZENITH).ResetThrusters();
            GetQuad(CardinalDirection.PORT).ResetThrusters();
            GetQuad(CardinalDirection.NADIR).ResetThrusters();
            GetQuad(CardinalDirection.STARBORD).ResetThrusters();
        }

        public void SetRollPercent(double thrustPercentage)
        {
            if (thrustPercentage < 0)
            {
                GetQuad(CardinalDirection.ZENITH).GetThruster(CardinalDirection.STARBORD).SetThrottlePercentage(Math.Abs(thrustPercentage));
                GetQuad(CardinalDirection.PORT).GetThruster(CardinalDirection.STARBORD).SetThrottlePercentage(Math.Abs(thrustPercentage));
                GetQuad(CardinalDirection.NADIR).GetThruster(CardinalDirection.STARBORD).SetThrottlePercentage(Math.Abs(thrustPercentage));
                GetQuad(CardinalDirection.STARBORD).GetThruster(CardinalDirection.STARBORD).SetThrottlePercentage(Math.Abs(thrustPercentage));
            }
            else
            {
                GetQuad(CardinalDirection.ZENITH).GetThruster(CardinalDirection.PORT).SetThrottlePercentage(Math.Abs(thrustPercentage));
                GetQuad(CardinalDirection.PORT).GetThruster(CardinalDirection.PORT).SetThrottlePercentage(Math.Abs(thrustPercentage));
                GetQuad(CardinalDirection.NADIR).GetThruster(CardinalDirection.PORT).SetThrottlePercentage(Math.Abs(thrustPercentage));
                GetQuad(CardinalDirection.STARBORD).GetThruster(CardinalDirection.PORT).SetThrottlePercentage(Math.Abs(thrustPercentage));
            }
        }

        public void SetYawPercent(double thrustPercentage)
        {
            //Yaw Right
            if (thrustPercentage < 0)
            {
                //If forward of CoM
                if (xPos > 0)
                {
                    GetQuad(CardinalDirection.ZENITH).GetThruster(CardinalDirection.PORT).SetThrottlePercentage(Math.Abs(thrustPercentage));
                    GetQuad(CardinalDirection.NADIR).GetThruster(CardinalDirection.STARBORD).SetThrottlePercentage(Math.Abs(thrustPercentage));
                }

                //If aft of CoM
                if (xPos < 0)
                {
                    GetQuad(CardinalDirection.ZENITH).GetThruster(CardinalDirection.STARBORD).SetThrottlePercentage(Math.Abs(thrustPercentage));
                    GetQuad(CardinalDirection.NADIR).GetThruster(CardinalDirection.PORT).SetThrottlePercentage(Math.Abs(thrustPercentage));
                }
            }

            //Yaw Left
            else
            {
                //If forward of CoM
                if (xPos > 0)
                {
                    GetQuad(CardinalDirection.ZENITH).GetThruster(CardinalDirection.STARBORD).SetThrottlePercentage(Math.Abs(thrustPercentage));
                    GetQuad(CardinalDirection.NADIR).GetThruster(CardinalDirection.PORT).SetThrottlePercentage(Math.Abs(thrustPercentage));
                }

                //If aft of CoM
                if (xPos < 0)
                {
                    GetQuad(CardinalDirection.ZENITH).GetThruster(CardinalDirection.PORT).SetThrottlePercentage(Math.Abs(thrustPercentage));
                    GetQuad(CardinalDirection.NADIR).GetThruster(CardinalDirection.STARBORD).SetThrottlePercentage(Math.Abs(thrustPercentage));
                }
            }
        }

        public void SetPitchPercent(double thrustPercentage)
        {
            //Pitch Down
            if (thrustPercentage < 0)
            {
                //If forward of CoM
                if(xPos > 0)
                {
                    GetQuad(CardinalDirection.PORT).GetThruster(CardinalDirection.STARBORD).SetThrottlePercentage(Math.Abs(thrustPercentage));
                    GetQuad(CardinalDirection.STARBORD).GetThruster(CardinalDirection.PORT).SetThrottlePercentage(Math.Abs(thrustPercentage));
                }
                
                //If aft of CoM
                if(xPos < 0)
                {
                    GetQuad(CardinalDirection.PORT).GetThruster(CardinalDirection.PORT).SetThrottlePercentage(Math.Abs(thrustPercentage));
                    GetQuad(CardinalDirection.STARBORD).GetThruster(CardinalDirection.STARBORD).SetThrottlePercentage(Math.Abs(thrustPercentage));
                }
            }
            
            //Pitch Up
            else
            {
                //If forward of CoM
                if (xPos > 0)
                {
                    GetQuad(CardinalDirection.PORT).GetThruster(CardinalDirection.PORT).SetThrottlePercentage(Math.Abs(thrustPercentage));
                    GetQuad(CardinalDirection.STARBORD).GetThruster(CardinalDirection.STARBORD).SetThrottlePercentage(Math.Abs(thrustPercentage));
                }

                //If aft of CoM
                if (xPos < 0)
                {
                    GetQuad(CardinalDirection.PORT).GetThruster(CardinalDirection.STARBORD).SetThrottlePercentage(Math.Abs(thrustPercentage));
                    GetQuad(CardinalDirection.STARBORD).GetThruster(CardinalDirection.PORT).SetThrottlePercentage(Math.Abs(thrustPercentage));
                }
            }
        }

        public void Step()
        {
            thrusterQuads["Zenith"].Step();
            thrusterQuads["Nadir"].Step();
            thrusterQuads["Port"].Step();
            thrusterQuads["Starboard"].Step();
        }
    }
}
