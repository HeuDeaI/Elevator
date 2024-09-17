class Person{
    public int CurrentFloor;
    public int EndFloor = 1;
    public int Id;
    public int MaxFloor = 10;

    public Person(int id)
    {
        Id = id + 1;
        Random random = new Random();
        CurrentFloor = random.NextDouble() < 0.5 ? 1 : random.Next(2, MaxFloor);
        if (CurrentFloor == 1)
        {
            EndFloor = random.Next(2, MaxFloor);
        }
    }

    public void CallElevator(Elevator elevator)
    {
        Console.WriteLine($"Person №{Id} at floor {CurrentFloor} calls the elevator to {EndFloor} floor.");
        elevator.AddToQueue(this);
    }
}