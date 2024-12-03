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
        CurrentFloor = random.Next(1, totalFloors + 1);
        DestinationFloor = CurrentFloor;

        while (DestinationFloor == CurrentFloor)
        {
            DestinationFloor = random.Next(1, totalFloors + 1);
        }
    }
}
