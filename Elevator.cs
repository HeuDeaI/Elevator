using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

class Elevator
{
    private int _currentFloor = 1;
    private readonly int _maxCapacity = 5;
    private readonly int _totalFloors;
    private readonly ConcurrentQueue<Person> _requestQueue = new ConcurrentQueue<Person>();
    private readonly List<Person> _insideElevator = new List<Person>();
    private bool _running = true;

    private const int TimeBetweenFloors = 400;

    public Elevator(int totalFloors)
    {
        _totalFloors = totalFloors;
    }

    public void AddRequest(Person person)
    {
        _requestQueue.Enqueue(person);
        Console.WriteLine($"[Request]   Person #{person.Id}  From: {person.CurrentFloor}  To: {person.DestinationFloor}");
    }

    public void Shutdown()
    {
        _running = false; 
    }

    public void Operate()
    {
        while (_running || !_requestQueue.IsEmpty || _insideElevator.Any())
        {
            if (_requestQueue.IsEmpty && !_insideElevator.Any())
            {
                Thread.Sleep(100); 
                continue;
            }

            while (_requestQueue.TryDequeue(out var person))
            {
                MoveToFloor(person.CurrentFloor);
                Console.WriteLine($"[Pickup]    Person #{person.Id}  Entered Elevator");
                _insideElevator.Add(person);
            }

            DropOffPassengers();
        }

        Console.WriteLine("[System]    Elevator shut down.");
    }

    private void DropOffPassengers()
    {
        foreach (var person in _insideElevator.OrderBy(p => p.DestinationFloor).ToList())
        {
            MoveToFloor(person.DestinationFloor);
            Console.WriteLine($"[Dropoff]   Person #{person.Id}  Floor: {person.DestinationFloor}");
            _insideElevator.Remove(person);
        }
    }

    private void MoveToFloor(int targetFloor)
    {
        while (_currentFloor != targetFloor)
        {
            Thread.Sleep(TimeBetweenFloors);
            _currentFloor += _currentFloor < targetFloor ? 1 : -1;
            Console.WriteLine($"[Move]      Elevator at Floor {_currentFloor}");
        }
    }
}
