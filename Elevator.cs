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
    private bool _running = true;

    private const int TimeBetweenFloors = 300;
    private readonly Dictionary<int, List<int>> _peopleWaiting;

    private Person _leavingPerson = null; 

    public Elevator(int totalFloors, int maxCapacity)
    {
        _totalFloors = totalFloors;
        _maxCapacity = maxCapacity;
        _peopleWaiting = new Dictionary<int, List<int>>();
        for (int i = 1; i <= totalFloors; i++)
        {
            _peopleWaiting[i] = new List<int>();
        }
    }

    public void AddRequest(Person person)
    {
        _requestQueue.Enqueue(person);
        _peopleWaiting[person.CurrentFloor].Add(person.Id);
        Console.WriteLine($"[Request]   Person #{person.Id}  From: {person.CurrentFloor}  To: {person.DestinationFloor}");
        RenderDisplay();
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
                _insideElevator.Add(person);
                Thread.Sleep(300);
                _peopleWaiting[person.CurrentFloor].Remove(person.Id);
                RenderDisplay();
            }

            DropOffPassengers();
        }
    }

    private void DropOffPassengers()
    {
        foreach (var person in _insideElevator.OrderBy(p => p.DestinationFloor).ToList())
        {
            MoveToFloor(person.DestinationFloor);
            _insideElevator.Remove(person);
            _leavingPerson = person; 
            RenderDisplay();
            Thread.Sleep(300);
            _leavingPerson = null;
            RenderDisplay(); 
        }
    }

    private void MoveToFloor(int targetFloor)
    {
        while (_currentFloor != targetFloor)
        {
            Thread.Sleep(TimeBetweenFloors);
            _currentFloor += _currentFloor < targetFloor ? 1 : -1;
            RenderDisplay();
        }
    }

    private string GetElevatorDisplay()
    {
        var insideDisplay = _insideElevator.Select(p => $"#{p.Id}").ToList();
        while (insideDisplay.Count < _maxCapacity) insideDisplay.Add(" ");
        return $"[{string.Join(" ", insideDisplay)}]";
    }

    private void RenderDisplay()
    {
        Console.Clear();
        for (int i = _totalFloors; i >= 1; i--)
        {
            string elevatorSymbol = i == _currentFloor ? GetElevatorDisplay() : "           ";
            string waitingPeople = string.Join(" ", _peopleWaiting[i].Select(id => $"#{id}"));
            string leavingPerson = i == _currentFloor && _leavingPerson != null ? $"   #{_leavingPerson.Id}" : "";
            Console.WriteLine($"{i}: {waitingPeople,-10} {elevatorSymbol}{leavingPerson}");
        }
        Console.WriteLine(new string('-', 20));
    }
}
