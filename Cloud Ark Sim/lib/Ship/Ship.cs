using Cloud_Ark_Sim.lib.Configuration;
using Cloud_Ark_Sim.lib.Propellants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloud_Ark_Sim.lib.Ship
{
    //Long axis is x, cross axis is y, nadir-zenith axis is z
    //TODO: Implement pitch & yaw control
    //TODO: Implement RollTo, PitchTo, and YawTo
    class Ship : Steppable
    {
        //Burn (time, throttle setting)
        //Get state
        //Get fuel state
        //Set diameter
        //Set length
        //Get position
        //Get speed
        //Mass

        //Store propellant mixture ratio, molar mass, and amount (mass)

        //Thruster[] thrusters;
        //thrusters[0] = new Thruster(0, 445);

        private EulerOrientation2D orientation;
        private EulerOrientation2D angularVelocity;
        private EulerOrientation2D angularAcceleration;

        private StateVector stateVector;

        private double mass;
        private double diameter;
        private double length;

        private double propellantTankEmptyMass = 15;
        private int numPropellantTanks = 8;

        private ThrusterRing foreRing;
        private ThrusterRing aftRing;
        private ShipFlightComputer flightComputer;
        public Ship(double _diam, double _length, StateVector initialConditions)
        {
            PropellantTypes.Init();

            diameter = _diam;
            length = _length;
            orientation = new();
            angularVelocity = new();
            angularAcceleration = new();
            stateVector = new(initialConditions);

            //These PropellantTank objects are copied for each ThrusterQuad, so they are just reffered to as blueprints
            PropellantTank oxidizerTankBlueprint = new(88, TankTypes.OXIDIZER, propellantTankEmptyMass);
            PropellantTank fuelTankBlueprint = new(45.36, TankTypes.FUEL, propellantTankEmptyMass);

            PropellantMixture propMixture = new(1 / 2.05, PropellantTypes.propellants["Aerozine50"], PropellantTypes.propellants["NitrogenTetroxide"]);
            ThrusterConfiguration config = new(281, 445, fuelTankBlueprint, oxidizerTankBlueprint, propMixture);

            //https://www.enginehistory.org/Rockets/RPE09.45/RPE09.45.shtml
            foreRing = new ThrusterRing(_length/2, config, _diam, 0);
            aftRing = new ThrusterRing(-_length/2, config, _diam, 0);

            flightComputer = new ShipFlightComputer(this);
        }

        public void Step()
        {
            foreRing.ResetThrusters();
            aftRing.ResetThrusters();

            flightComputer.Step();
            mass = GetTotalFuel() + GetTotalOxidizer() + (propellantTankEmptyMass * numPropellantTanks); //Update ships mass

            //Apply linear kinematics
            stateVector.position.Apply(
                (stateVector.velocity.GetX() * Sim.GetTimestep()) + (0.5 * stateVector.velocity.GetX() * Math.Pow(Sim.GetTimestep(), 2)),
                (stateVector.velocity.GetY() * Sim.GetTimestep()) + (0.5 * stateVector.velocity.GetY() * Math.Pow(Sim.GetTimestep(), 2)),
                (stateVector.velocity.GetZ() * Sim.GetTimestep()) + (0.5 * stateVector.velocity.GetZ() * Math.Pow(Sim.GetTimestep(), 2))
            );

            Vect3D oldAcceleration = new(stateVector.acceleration);
            stateVector.acceleration = new(SpaceUtils.EarthOrbit.GetAccelerationVector(stateVector.position, mass));
            stateVector.velocity.Apply(
                0.5 * (stateVector.acceleration.GetX() + oldAcceleration.GetX()) * Sim.GetTimestep(),
                0.5 * (stateVector.acceleration.GetY() + oldAcceleration.GetY()) * Sim.GetTimestep(),
                0.5 * (stateVector.acceleration.GetZ() + oldAcceleration.GetZ()) * Sim.GetTimestep()
            );

            //Apply rotational kinematics
            double mi_long = 0.5 * mass * Math.Pow((diameter / 2), 2); //Moment of inertia about ship long axis (roll)
            double mi_short = (1 / 12.0) * mass * ((3 * Math.Pow((diameter / 2), 2)) + Math.Pow(length, 2)); //Moment of inertia about ship short axes (yaw & pitch)

            orientation.Apply(
                (angularVelocity.GetPitch() * Sim.GetTimestep()) + (0.5 * angularAcceleration.GetPitch() * Math.Pow(Sim.GetTimestep(), 2)), 
                (angularVelocity.GetYaw() * Sim.GetTimestep()) + (0.5 * angularAcceleration.GetYaw() * Math.Pow(Sim.GetTimestep(), 2))
            );

            EulerOrientation2D oldAngularAcceleration = new(angularAcceleration);

            angularAcceleration = new(
                GetPitchTorque() / mi_short, 
                GetYawTorque() / mi_short
            ); //Get angular acceleration (rad/s^2)

            angularVelocity.Apply(
                0.5 * (angularAcceleration.GetPitch() + oldAngularAcceleration.GetPitch()) * Sim.GetTimestep(), 
                0.5 * (angularAcceleration.GetYaw() + oldAngularAcceleration.GetYaw()) * Sim.GetTimestep()
            );

            foreRing.Step();
            aftRing.Step();
        }

        public double GetRollTorque()
        {
            return foreRing.GetRollTorque() + aftRing.GetRollTorque();
        }

        public double GetYawTorque()
        {
            return foreRing.GetYawTorque() + aftRing.GetYawTorque();
        }
        public double GetPitchTorque()
        {
            return foreRing.GetPitchTorque() + aftRing.GetPitchTorque();
        }

        public double GetTotalFuel()
        {
            double total = 0;

            total += foreRing.GetQuad(CardinalDirection.ZENITH).GetFuelTank().GetAmountKG();
            total += foreRing.GetQuad(CardinalDirection.PORT).GetFuelTank().GetAmountKG();
            total += foreRing.GetQuad(CardinalDirection.NADIR).GetFuelTank().GetAmountKG();
            total += foreRing.GetQuad(CardinalDirection.NADIR).GetFuelTank().GetAmountKG();

            total += aftRing.GetQuad(CardinalDirection.ZENITH).GetFuelTank().GetAmountKG();
            total += aftRing.GetQuad(CardinalDirection.PORT).GetFuelTank().GetAmountKG();
            total += aftRing.GetQuad(CardinalDirection.NADIR).GetFuelTank().GetAmountKG();
            total += aftRing.GetQuad(CardinalDirection.NADIR).GetFuelTank().GetAmountKG();

            return total;
        }

        public double GetTotalOxidizer()
        {
            double total = 0;

            total += foreRing.GetQuad(CardinalDirection.ZENITH).GetOxidizerTank().GetAmountKG();
            total += foreRing.GetQuad(CardinalDirection.PORT).GetOxidizerTank().GetAmountKG();
            total += foreRing.GetQuad(CardinalDirection.NADIR).GetOxidizerTank().GetAmountKG();
            total += foreRing.GetQuad(CardinalDirection.NADIR).GetOxidizerTank().GetAmountKG();

            total += aftRing.GetQuad(CardinalDirection.ZENITH).GetOxidizerTank().GetAmountKG();
            total += aftRing.GetQuad(CardinalDirection.PORT).GetOxidizerTank().GetAmountKG();
            total += aftRing.GetQuad(CardinalDirection.NADIR).GetOxidizerTank().GetAmountKG();
            total += aftRing.GetQuad(CardinalDirection.NADIR).GetOxidizerTank().GetAmountKG();

            return total;
        }

        public void SetRollPercent(double thrustPercentage)
        {
            if (thrustPercentage > 1 || thrustPercentage < -1) throw new ArgumentOutOfRangeException("thrustPercentage must be between -1 and 1, inclusive");
            foreRing.SetRollPercent(thrustPercentage);
            aftRing.SetRollPercent(thrustPercentage);
        }

        public void SetYawPercent(double thrustPercentage)
        {
            if (thrustPercentage > 1 || thrustPercentage < -1) throw new ArgumentOutOfRangeException("thrustPercentage must be between -1 and 1, inclusive");
            foreRing.SetYawPercent(thrustPercentage);
            aftRing.SetYawPercent(thrustPercentage);
        }

        public void SetPitchPercent(double thrustPercentage)
        {
            if (thrustPercentage > 1 || thrustPercentage < -1) throw new ArgumentOutOfRangeException("thrustPercentage must be between -1 and 1, inclusive");
            foreRing.SetPitchPercent(thrustPercentage);
            aftRing.SetPitchPercent(thrustPercentage);
        }

        public EulerOrientation2D GetOrientation()
        {
            return orientation;
        }

        public EulerOrientation2D GetAngularVelocity()
        {
            return angularVelocity;
        }

        public EulerOrientation2D GetAngularAcceleration()
        {
            return angularAcceleration;
        }

        public ShipFlightComputer GetFlightComputer()
        {
            return flightComputer;
        }

        public StateVector GetStateVector()
        {
            return stateVector;
        }
    }
}
