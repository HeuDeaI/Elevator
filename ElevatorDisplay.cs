using System;
using System.Collections.Generic;
using System.Linq;

class ElevatorDisplay
{
    private readonly int _totalFloors;
    private readonly Dictionary<int, List<int>> _peopleWaiting;

    public ElevatorDisplay(int totalFloors)
    {
        _totalFloors = totalFloors;
        _peopleWaiting = new Dictionary<int, List<int>>();

        for (int i = 1; i <= totalFloors; i++)
        {
            _peopleWaiting[i] = new List<int>();
        }
    }

    public void UpdateRequest(int floor, int personId, bool add)
    {
        if (add)
            _peopleWaiting[floor].Add(personId);
        else
            _peopleWaiting[floor].Remove(personId);
    }

    public void Render(int currentFloor, List<Person> insideElevator, Person? droppedOffPerson)
    {
        Console.Clear();
        for (int i = _totalFloors; i >= 1; i--)
        {
            string elevatorSymbol = i == currentFloor ? GetElevatorDisplay(insideElevator) : "           ";
            string waitingPeople = string.Join(" ", _peopleWaiting[i].Select(id => $"#{id}"));
            string leavingPerson = i == currentFloor && droppedOffPerson != null ? $"   #{droppedOffPerson.Id}" : "";
            Console.WriteLine($"{i}: {waitingPeople,-10} {elevatorSymbol}{leavingPerson}");
        }
        Console.WriteLine(new string('-', 30));
    }

    private string GetElevatorDisplay(List<Person> insideElevator)
    {
        var insideDisplay = insideElevator.Select(p => $"#{p.Id}").ToList();
        while (insideDisplay.Count < 5) insideDisplay.Add(" ");
        return $"[{string.Join(" ", insideDisplay)}]";
    }
}
