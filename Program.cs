using System;
using System.Threading;

class Program
{
    static void Main(string[] args)
    {
        ElevatorSystem elevatorSystem = new ElevatorSystem(10, 5);

        Thread elevatorThread = new Thread(elevatorSystem.OperateElevator);
        elevatorThread.Start();

        elevatorSystem.SimulateWithDelays(10);

        Thread.Sleep(5000);
        Environment.Exit(0);
    }
}

