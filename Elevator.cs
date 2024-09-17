class Elevator{
    public int CurrentFloor = 1;
    private int _timeToChangeFloor = 500; 
    public int MoveDirection = 1;
    private List<Person> _waitingToDownPersons = new List<Person>();
    private List<Person> _waitingToUpPersons = new List<Person>();
    private List<Person> _personsIntoElevator = new List<Person>();
    private int MaxCapacity = 5;

    public void MoveToFloor(Person person)
    {
        if (person.CurrentFloor < CurrentFloor)
        {
            MoveDirection = -1;
        }
        else
        {
            MoveDirection = 1;
        }
        while (CurrentFloor != person.CurrentFloor)
        {
            Thread.Sleep(_timeToChangeFloor);
            CurrentFloor += MoveDirection;
            Console.WriteLine($"Elevator moves to {CurrentFloor} floor");
        }
    }

    public void MoveToEndFloor()
    {
        foreach (var person in _personsIntoElevator)
        {
            if (person.EndFloor < CurrentFloor)
            {
                MoveDirection = -1;
            }
            else
            {
                MoveDirection = 1;
            }
            while (CurrentFloor != person.EndFloor)
            {
                Thread.Sleep(_timeToChangeFloor);
                CurrentFloor += MoveDirection;
                Console.WriteLine($"Elevator moves to {CurrentFloor} floor");
            }
            Console.WriteLine($"Elevator arrive person №{person.Id} to {CurrentFloor} floor");
            Thread.Sleep(_timeToChangeFloor);
        }
    }

    public void MoveToFirstFloor()
    {
        MoveDirection = -1;
        while (CurrentFloor != 1)
        {
            Thread.Sleep(_timeToChangeFloor);
            CurrentFloor += MoveDirection;
            Console.WriteLine($"Elevator moves to {CurrentFloor} floor");
        }
    }

    public void AddToQueue(Person person)
    {
        if (person.CurrentFloor != 1)
        {
            _waitingToDownPersons.Add(person);
        }
        else
        {
            _waitingToUpPersons.Add(person);
        }
    }

    public void BringPeople()
    {
        while (_waitingToDownPersons.Count + _waitingToUpPersons.Count != 0)
        {
            BringPersonToUp();
            BringPersonToDown();
        }
        MoveToFirstFloor();
    }

    public void BringPersonToUp()
    {
        MoveToFirstFloor();
        _waitingToUpPersons.Sort((person1, person2) => person1.EndFloor.CompareTo(person2.EndFloor));
        int elevatorLimit = Math.Min(_waitingToUpPersons.Count, MaxCapacity);
        for (int i = 0; i < elevatorLimit; i++)
        {
            _personsIntoElevator.Add(_waitingToUpPersons[i]);
        }
        _waitingToUpPersons.RemoveRange(0, elevatorLimit);
        MoveToEndFloor();
        _personsIntoElevator.Clear();
    }

    public void BringPersonToDown()
    {
        _waitingToDownPersons.Sort((person1, person2) => person1.CurrentFloor.CompareTo(person2.CurrentFloor));
        int elevatorLimit = Math.Min(_waitingToDownPersons.Count, MaxCapacity);
        for (int i = 0; i < elevatorLimit; i++)
        {
            MoveToFloor(_waitingToDownPersons[i]);
            _personsIntoElevator.Add(_waitingToDownPersons[i]);
        }
        _waitingToDownPersons.RemoveRange(0, elevatorLimit);
        MoveToEndFloor();
        _personsIntoElevator.Clear();
    }
}