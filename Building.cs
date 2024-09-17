class Building{
    public List<Person> People = new List<Person>();
    public Building(int countOfPersons)
    {
        for (int i = 0; i < countOfPersons; i++)
        {
            People.Add(new Person(i));
        }
    }

    public void AllPersonCallElevator(Elevator elevator)
    {
        foreach (var person in People)
        {
            person.CallElevator(elevator);
        }
    }
}