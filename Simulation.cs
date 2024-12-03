using System;
using System.Threading;

class Simulation
{
    private readonly Building _building;

    public Simulation(Building building)
    {
        _building = building;
    }

    public void Run(int numberOfPeople)
    {
        Random random = new Random();

        for (int i = 0; i < numberOfPeople; i++)
        {
            _building.SpawnPerson(i);
            Thread.Sleep(random.Next(500, 1500)); 
        }
    }
}
