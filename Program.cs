using System;
using System.Threading;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("[System]    Simulation started.");

        Building building = new Building(10);
        Simulation simulation = new Simulation(building);

        Thread elevatorThread = new Thread(building.Elevator.Operate);
        elevatorThread.Start();

        simulation.Run(10);

        building.Elevator.Shutdown();

        elevatorThread.Join();
        Console.WriteLine("[System]    Simulation ended.");
    }
}
