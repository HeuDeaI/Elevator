using System;
using System.Collections.Generic;

class Building
{
    public Elevator Elevator { get; }
    private readonly int _totalFloors;
    private readonly List<Person> _people;

    public Building(int totalFloors, int elevatorCapacity)
    {
        _totalFloors = totalFloors;
        _people = new List<Person>();
        Elevator = new Elevator(totalFloors, elevatorCapacity);
    }

    public void SpawnPerson(int id)
    {
        var person = new Person(id, _totalFloors);
        _people.Add(person);
        Elevator.AddRequest(person); 
    }
}
