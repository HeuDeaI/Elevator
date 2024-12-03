using System;
using System.Threading;

class Program
{
    static void Main(string[] args)
    {
        Building building = new Building(9, 5);
        Simulation simulation = new Simulation(building);

        Thread elevatorThread = new Thread(building.Elevator.Operate);
        elevatorThread.Start();
        Thread.Sleep(1000);

        simulation.Run(10);
        building.Elevator.Shutdown();
        elevatorThread.Join();
    }
}
