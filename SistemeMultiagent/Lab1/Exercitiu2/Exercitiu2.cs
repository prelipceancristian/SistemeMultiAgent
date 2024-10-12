using ActressMas;

namespace SistemeMultiagent.Lab1.Exercitiu2;

public class EmitterAgent : Agent
{
    private readonly List<int> _numbers = [];
    private List<string> _agents = [];
    private int _index;

    public EmitterAgent(string name)
    {
        Name = name;
    }
    
    public override void Setup()
    {
        _agents = [..Environment.FilteredAgents("agent")];
        var random = new Random(10);
        foreach (var _ in Enumerable.Range(1, 10000))
        {
            _numbers.Add(random.Next(1, 11));
        }
    }

    public override void Act(Message message)
    {
        if (_index < _numbers.Count)
        {
            var number = _numbers[_index++];
            Send(message.Sender, number.ToString());
        }
        else
        {
            Send(message.Sender, "stop");
            _agents.Remove(message.Sender);
        }

        if (_agents.Count == 0)
        {
            Stop();
        }
    }
}

public class MyAgent : Agent
{
    public int PartialSum { get; set; }
    public int NumberCounter { get; set; }

    public MyAgent(string name)
    {
        Name = name;
    }
    
    public override void Act(Message message)
    {
        if (message.Content == "stop")
        {
            Console.WriteLine($"Suma partiala agent {Name}: {PartialSum}. Numere procesate: {NumberCounter}");
            Stop();
            return;
        }
        var number = int.Parse(message.Content);
        PartialSum += number;
        NumberCounter++;
        if (NumberCounter % 100 == 0)
        {
            Console.WriteLine(
                $"Agentul {Name} a procesat {NumberCounter} și a obținut rezultat partial {PartialSum}");
        }
        Send(message.Sender, "give");
    }

    public override void Setup()
    {
        Send("emitter", "give");
    }
}

public static class Exercitiu2
{
    public static void Run()
    {
        var environment = new EnvironmentMas();
        var emitterAgent = new EmitterAgent("emitter");
        var agent1 = new MyAgent("agent1");
        var agent2 = new MyAgent("agent2");
        environment.Add(emitterAgent);
        environment.Add(agent1);
        environment.Add(agent2);
        environment.Start();
        double totalSum = agent1.PartialSum + agent2.PartialSum;
        var totalCount = agent1.NumberCounter + agent2.NumberCounter;
        Console.WriteLine("Suma totala: " + totalSum);
        Console.WriteLine("Medie: " + totalSum / totalCount);
    }
}