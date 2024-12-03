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

        simulation.Run(10);

        elevatorThread.Join();
    }
}
