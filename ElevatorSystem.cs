using System;
using System.Threading;

class ElevatorSystem
{
    private readonly Building _building;
    private readonly Elevator _elevator;

    public ElevatorSystem(int totalFloors, int elevatorCapacity)
    {
        _building = new Building(totalFloors);
        _elevator = new Elevator(totalFloors, elevatorCapacity);
    }

    public void SimulateWithDelays(int numberOfPeople)
    {
        Random random = new Random();

        for (int i = 0; i < numberOfPeople; i++)
        {
            _building.SpawnPerson(i);
            var person = _building.GetLastSpawnedPerson();
            _elevator.AddRequest(person);

            Thread.Sleep(random.Next(500, 1500));
        }
    }

    public void OperateElevator()
    {
        _elevator.Operate();
    }
}