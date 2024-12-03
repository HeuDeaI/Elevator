using System.Collections.Generic;

class Building
{
    private readonly int _totalFloors;
    private readonly List<Person> _people;

    public Building(int totalFloors)
    {
        _totalFloors = totalFloors;
        _people = new List<Person>();
    }

    public void SpawnPerson(int id)
    {
        var person = new Person(id, _totalFloors);
        _people.Add(person);
        Console.WriteLine($"Spawned Person #{person.Id} at floor {person.CurrentFloor} wanting to go to floor {person.DestinationFloor}");
    }

    public Person GetLastSpawnedPerson()
    {
        return _people[^1];
    }
}
