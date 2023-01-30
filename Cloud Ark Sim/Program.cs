using System;
using Cloud_Ark_Sim.lib;
using Cloud_Ark_Sim.lib.Ship;
using Cloud_Ark_Sim.lib.Propellants;
using Cloud_Ark_Sim.lib.Configuration;
using System.Collections.Generic;
using Cloud_Ark_Sim.lib.IPC;
using System.Threading;

namespace Cloud_Ark_Sim
{
    class Program
    {
        static void Main(string[] args)
        {
            double framerate = 1/30.0;
            SocketServer.Start();
            Sim.Init(framerate);
            Ship ship = new(9, 27);
            ship.GetFlightComputer().OrientTo(new EulerOrientation2D(-Math.PI/4, Math.PI/2));
            //ship.GetFlightComputer().OrientTo(new EulerOrientation2D(-Math.PI / 4, 0));

            double[] yaw = new double[500_000];
            double[] time = new double[500_000];
            double[] pitch = new double[500_000];

            var plt = new ScottPlot.Plot(400, 300);
            plt.AddScatter(time, yaw);
            plt.AddScatter(time, pitch);

            double t = 0;
            int i = 1;

            long lastTick = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

            while (ship.GetTotalOxidizer() > 0 && ship.GetTotalFuel() > 0 && t < 500)
            {
                long currentTime = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
                if (currentTime > lastTick + (framerate * 1000))
                {
                    if(t > 150)
                    {
                        ship.GetFlightComputer().OrientTo(new EulerOrientation2D(Math.PI / 4, Math.PI / 4));
                    }
                    SocketServer.wssv.WebSocketServices["/Data"].Sessions.Broadcast(t + ";" + ship.GetOrientation().GetYaw() + ";" + ship.GetOrientation().GetPitch());
                    yaw[i] = ship.GetOrientation().GetYaw() * (180 / Math.PI);
                    pitch[i] = ship.GetOrientation().GetPitch() * (180 / Math.PI);
                    time[i] = t;

                    //Console.WriteLine("T: " + Math.Round(t, 2) + "; Orientation: " + Math.Round(ship.GetOrientation().GetYaw() * (180 / Math.PI), 2) + " " + Math.Round(ship.GetOrientation().GetPitch() * (180 / Math.PI), 2) + "; Ang. Acc.: " + Math.Round(ship.GetAngularAcceleration().GetYaw() * (180 / Math.PI), 2) + "; Fuel: " + Math.Round(ship.GetTotalFuel(), 2));
                    //Console.WriteLine("T: " + t + "; Oxidizer Amount: " + Math.Round(oxidizerTank.GetAmountKG(),2) + "kg; Fuel Amount: " + Math.Round(fuelTank.GetAmountKG(),2) + "kg");
                    //Console.WriteLine(t + " " + oxidizerTank.GetAmountKG() + " " + quad.GetThruster(CardinalDirection.FORE).GetThrottlePercentage());
                    ship.Step();

                    lastTick = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
                    t += Sim.GetTimestep();
                    i++;
                }
            }

            plt.SaveFig("figure.png");
        }
    }
}
