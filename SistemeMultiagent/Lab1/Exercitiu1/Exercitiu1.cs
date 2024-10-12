using ActressMas;

namespace SistemeMultiagent.Lab1.Exercitiu1;

public class MyAgent : Agent
{
    private readonly int _seed;
    private Random _random;
    private int _millisecondDelay;
    private int _reachedValue = 0;
    
    public MyAgent(int seed)
    {
        _seed = seed;
    }
    
    public override void Setup()
    {
        _random = new Random(_seed);
        _millisecondDelay = _random.Next(500, 5000);
        Console.WriteLine("Setup done");
    }

    public override void ActDefault()
    {
        Console.WriteLine("ActDefault start");
        if (_reachedValue <= 100)
        {
            _reachedValue += 1;
            Send("monitor", _reachedValue.ToString());
            Thread.Sleep(_millisecondDelay);
        }
        else
        {
            Stop();
        }
        // foreach (var index in Enumerable.Range(1, 100))
        // {
        //     Send("monitor", index.ToString());
        //     Thread.Sleep(_millisecondDelay);
        //     Console.WriteLine("ActDefault shoo");
        // }
    }
}

public class MonitorAgent : Agent
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

public static class Exercitiu1
{
    public static void Run()
    {
        var environment = new EnvironmentMas();
        // var environment = new EnvironmentMas(0, 0, true, null, false);
        var agent1 = new MyAgent(1);
        var agent2 = new MyAgent(2);
        var agent3 = new MyAgent(3);
        var agent4 = new MyAgent(4);
        var monitorAgent = new MonitorAgent();
        environment.Add(agent1, "agent1");
        environment.Add(agent2, "agent2");
        environment.Add(agent3, "agent3");
        environment.Add(agent4, "agent4");
        environment.Add(monitorAgent, "monitor");
        environment.Start();
    }
}