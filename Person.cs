using System;

class Person
{
    public int Id { get; }
    public int CurrentFloor { get; }
    public int DestinationFloor { get; }

    public Person(int id, int totalFloors)
    {
        Id = id + 1;
        Random random = new Random();
        CurrentFloor = random.Next(0, 2) == 0 ? 1 : random.Next(2, totalFloors + 1);
        DestinationFloor = CurrentFloor == 1
            ? random.Next(2, totalFloors + 1) 
            : random.Next(1, CurrentFloor); 
    }
}