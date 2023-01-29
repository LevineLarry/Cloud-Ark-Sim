using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloud_Ark_Sim.lib.Ship
{
    class ShipFlightComputer : Steppable
    {
        private Ship ship;
        private double maximumAngularAcceleration = 10;
        private double maximumAngularVelocity = 2 * Math.PI / 30; //Full rotation in 30 seconds, 0.5rpm

        //PID setup
        PidTune rollTune = new(0.002, 0.000001, 0.07);
        PidTune pitch_yawTune = new(0.3, 0.3, 7);

        //Integral values
        double ri = 0;
        double yi = 0;
        double pi = 0;

        private EulerOrientation2D orientationSetpoint;
        private EulerOrientation2D orientationError;

        public ShipFlightComputer(Ship _ship)
        {
            ship = _ship;

            orientationSetpoint = new();
            orientationError = new();
        }

        public void OrientTo(EulerOrientation2D newOrientation)
        {
            orientationSetpoint = new(newOrientation);
        }

        private EulerOrientation2D GetOrientationError()
        {
            double pitchE = orientationSetpoint.GetPitch() - ship.GetOrientation().GetPitch();
            double yawE = orientationSetpoint.GetYaw() - ship.GetOrientation().GetYaw();

            return new EulerOrientation2D(pitchE, yawE);
        }

        public void Step()
        {
            EulerOrientation2D newOrientationError = GetOrientationError();

            //Just focus on roll for now
            /*
            ri += newOrientationError.GetRoll() * Sim.GetTimestep();
            double rd = (newOrientationError.GetRoll() - orientationError.GetRoll()) / Sim.GetTimestep();
            double rollPercentSetpoint = (kp * newOrientationError.GetRoll()) + (ki * ri) + (kd * rd);
            */

            //Yaw
            yi += newOrientationError.GetYaw() * Sim.GetTimestep();
            double yd = (newOrientationError.GetYaw() - orientationError.GetYaw()) / Sim.GetTimestep();
            double yawPercentSetpoint = (pitch_yawTune.GetP() * newOrientationError.GetYaw()) + (pitch_yawTune.GetI() * ri) + (pitch_yawTune.GetD() * yd);

            //Pitch
            pi += newOrientationError.GetPitch() * Sim.GetTimestep();
            double pd = (newOrientationError.GetPitch() - orientationError.GetPitch()) / Sim.GetTimestep();
            double pitchPercentSetpoint = (pitch_yawTune.GetP() * newOrientationError.GetPitch()) + (pitch_yawTune.GetI() * ri) + (pitch_yawTune.GetD() * pd);

            /*
            if (rollPercentSetpoint > 1) rollPercentSetpoint = 1;
            if (rollPercentSetpoint < -1) rollPercentSetpoint = -1;
            ship.SetRollPercent(rollPercentSetpoint);
            */

            if (yawPercentSetpoint > 1) yawPercentSetpoint = 1;
            if (yawPercentSetpoint < -1) yawPercentSetpoint = -1;
            ship.SetYawPercent(yawPercentSetpoint);

            if (pitchPercentSetpoint > 1) pitchPercentSetpoint = 1;
            if (pitchPercentSetpoint < -1) pitchPercentSetpoint = -1;
            ship.SetPitchPercent(pitchPercentSetpoint);

            orientationError = newOrientationError;
        }
    }
}
