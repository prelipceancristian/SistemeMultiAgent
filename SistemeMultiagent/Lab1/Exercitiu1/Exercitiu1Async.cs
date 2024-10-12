using ActressMas;

namespace SistemeMultiagent.Lab1.Exercitiu1;

public class MyAgentAsync : Agent
{
    private readonly int _seed;
    private Random _random;
    private int _millisecondDelay;
    private int _reachedValue = 0;
    
    public MyAgentAsync(int seed)
    {
        _seed = seed;
    }
    
    public override async void Setup()
    {
        _random = new Random(_seed);
        _millisecondDelay = _random.Next(500, 5000);
        Console.WriteLine("Setup done");
        foreach (var index in Enumerable.Range(1, 100))
        {
            Send("monitor", index.ToString());
            await Task.Delay(_millisecondDelay);
            Console.WriteLine("ActDefault shoo");
        }
    }
}

public class MonitorAgentAsync : Agent
{
    public override void Setup()
    {
        Console.WriteLine("monitor setup");
    }
    
    public override void Act(Message message)
    {
        Console.WriteLine("monitor act");
        Console.WriteLine($"Agentul {message.Sender} a numarat pana la valoarea {message.Content}");
    }
}


public class Exercitiu1Async
{
    public static void Run()
    {
        var environment = new EnvironmentMas();
        for (var i = 0; i < 5; i++)
        {
            var agent = new MyAgentAsync(i);
            environment.Add(agent, $"agent{i}");
        }
        var monitorAgent = new MonitorAgentAsync();
        environment.Add(monitorAgent, "monitor");
        environment.Start();
    }
}