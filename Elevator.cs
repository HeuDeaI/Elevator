using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

class Elevator
{
    private int _currentFloor = 1;
    private readonly int _maxCapacity;
    private readonly int _totalFloors;
    private readonly ConcurrentQueue<Person> _requestQueue = new ConcurrentQueue<Person>();
    private readonly List<Person> _insideElevator = new List<Person>();

    private const int TimeBetweenFloors = 100;

    public Elevator(int totalFloors, int maxCapacity)
    {
        _totalFloors = totalFloors;
        _maxCapacity = maxCapacity;
    }

    public void AddRequest(Person person)
    {
        _requestQueue.Enqueue(person);
        Console.WriteLine($"Person #{person.Id} requests from floor {person.CurrentFloor} to floor {person.DestinationFloor}");
    }

public void Operate()
{
    while (true)
    {
        if (_requestQueue.IsEmpty && !_insideElevator.Any())
        {
            Thread.Sleep(100); 
            continue;
        }

        while (_requestQueue.TryDequeue(out var person) && person != null)
        {
            MoveToFloor(person.CurrentFloor);
            Console.WriteLine($"Person #{person.Id} entered the elevator at floor {person.CurrentFloor}");
            _insideElevator.Add(person);
        }

        DropOffPassengers();
    }
}


    private void DropOffPassengers()
    {
        foreach (var person in _insideElevator.ToList())
        {
            MoveToFloor(person.DestinationFloor);
            Console.WriteLine($"Person #{person.Id} delivered from floor {person.CurrentFloor} to floor {person.DestinationFloor}");
            _insideElevator.Remove(person);
        }
    }

    private void MoveToFloor(int targetFloor)
    {
        while (_currentFloor != targetFloor)
        {
            Thread.Sleep(TimeBetweenFloors);
            _currentFloor += _currentFloor < targetFloor ? 1 : -1;
            // Console.WriteLine($"Elevator moved to floor {_currentFloor}");
        }
    }
}