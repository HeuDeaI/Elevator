using System;
using System.Threading;
using System.Collections.Generic;


class Program
{
    static void Main(string[] args)
    {
        Elevator elevator = new Elevator();
        Building building = new Building(10);
        building.AllPersonCallElevator(elevator);
        elevator.BringPeople();
    }
}