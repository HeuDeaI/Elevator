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
    private readonly ElevatorDisplay _display;

    private const int ActionDelay = 300;
    private bool _shouldShutdown = false; 

    public Elevator(int totalFloors, int maxCapacity, ElevatorDisplay display)
    {
        _totalFloors = totalFloors;
        _maxCapacity = maxCapacity;
        _display = display;
    }

    public void AddRequest(Person person)
    {
        _requestQueue.Enqueue(person);
        _display.UpdateRequest(person.CurrentFloor, person.Id, true);
    }

    public void Operate()
    {
        bool movingUp = true;

        while (true)
        {
            if (_requestQueue.IsEmpty && !_insideElevator.Any())
            {
                Thread.Sleep(500); 
                if (_requestQueue.IsEmpty && !_insideElevator.Any())
                {
                    _shouldShutdown = true; 
                    return;
                }
            }

            DropOffPassengers(ref movingUp);
            PickupPassengers(ref movingUp);
        }
    }

    private void DropOffPassengers(ref bool movingUp)
    {
        foreach (var person in _insideElevator.ToList())
        {
            if (_currentFloor == person.DestinationFloor)
            {
                _insideElevator.Remove(person);
                _display.Render(_currentFloor, _insideElevator, person);
                Thread.Sleep(ActionDelay);
            }
        }
    }

    private void PickupPassengers(ref bool movingUp)
    {
        List<Person> peopleToPickup = new List<Person>();

        while (_requestQueue.TryDequeue(out var person))
        {
            if (person.CurrentFloor == _currentFloor)
            {
                if (_insideElevator.Count < _maxCapacity)
                {
                    _insideElevator.Add(person);
                    _display.UpdateRequest(person.CurrentFloor, person.Id, false);
                    Thread.Sleep(ActionDelay);
                }
                else
                {
                    _requestQueue.Enqueue(person); 
                }
            }
            else
            {
                peopleToPickup.Add(person); 
            }
        }

        foreach (var person in peopleToPickup)
            _requestQueue.Enqueue(person);

        MoveToNextStop(ref movingUp);
    }

    private void MoveToNextStop(ref bool movingUp)
    {
        var nextStops = _insideElevator.Select(p => p.DestinationFloor)
            .Concat(_requestQueue.Select(p => p.CurrentFloor))
            .OrderBy(f => f)
            .ToList();

        if (!nextStops.Any())
            return;

        if (movingUp)
        {
            var nextUp = nextStops.FirstOrDefault(f => f > _currentFloor);
            if (nextUp > 0)
            {
                MoveToFloor(nextUp, ref movingUp);
            }
            else
            {
                movingUp = false;
                MoveToNextStop(ref movingUp);
            }
        }
        else
        {
            var nextDown = nextStops.LastOrDefault(f => f < _currentFloor);
            if (nextDown > 0)
            {
                MoveToFloor(nextDown, ref movingUp);
            }
            else
            {
                movingUp = true;
                MoveToNextStop(ref movingUp);
            }
        }
    }

    private void MoveToFloor(int targetFloor, ref bool movingUp)
    {
        while (_currentFloor != targetFloor)
        {
            Thread.Sleep(ActionDelay);
            _currentFloor += _currentFloor < targetFloor ? 1 : -1;
            _display.Render(_currentFloor, _insideElevator, null);
        }
    }

    public bool ShouldShutdown() => _shouldShutdown; 
}
