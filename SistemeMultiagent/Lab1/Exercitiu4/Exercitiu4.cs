using ActressMas;

namespace SistemeMultiagent.Lab1.Exercitiu4;

public static class Commands
{
    public const string Ping = "Ping";
    public const string Pong = "Pong";
    public const string Stop = "Stop";
}

public class PingPongAgent : Agent
{
    public PingPongAgent(string name)
    {
        Name = name;
    }
    
    public override void Act(Message message)
    {
        var messageWords = message.Content.Split("->");
        if (messageWords[1] == Commands.Stop)
        {
            Stop();
            return;
        }

        Send(message.Sender, Commands.Pong);
    }
}

public class StopperAgent : Agent
{
    private readonly int _seed;

    public StopperAgent(string name, int seed)
    {
        Name = name;
        _seed = seed;
    }
    
    public override async void Setup()
    {
        var random = new Random(_seed);
        var period = random.Next(500, 5000);
        while (true)
        {
            var availablePingAgents = Environment.FilteredAgents("ping") ?? [];
            if (availablePingAgents.Count == 0)
            {
                Stop();
                return;
            }
            var pingAgent = availablePingAgents.ElementAt(random.Next(availablePingAgents.Count));
            Console.WriteLine($"{Name}: trimit STOP catre {pingAgent}");
            Send(pingAgent, $"{Name}->{Commands.Stop}");
            await Task.Delay(period);
        }
    }
}

public class ManagerAgent : Agent
{
    private readonly int _seed;

    public ManagerAgent(int seed)
    {
        _seed = seed;
        Name = "Manager";
    }
    
    public override async void Setup()
    {
        var random = new Random(_seed);
        var period = random.Next(500, 5000);
        while (true)
        {
            var availablePingAgents = Environment.FilteredAgents("ping") ?? [];
            if (availablePingAgents.Count == 0)
            {
                Stop();
                return;
            }
            var agent = availablePingAgents.ElementAt(random.Next(availablePingAgents.Count));
            var content = $"{Name}->{Commands.Ping}";
            Send(agent, content);
            await Task.Delay(period);
        }
    }

    public override void Act(Message message)
    {
        Console.WriteLine($"Manager: {message.Sender} a raspuns {message.Content}");
    }
}

public class Exercitiu4
{
    public static void Run()
    {
        var environment = new EnvironmentMas();
        for (var i = 1; i <= 4; i++)
        {
            var pingAgent = new PingPongAgent("pingAgent" + i);
            environment.Add(pingAgent);
        }

        for (var i = 1; i <= 2; i++)
        {
            var stopperAgent = new StopperAgent("stopAgent" + i, i);
            environment.Add(stopperAgent);
        }

        var managerAgent = new ManagerAgent(1);
        environment.Add(managerAgent);
        
        environment.Start();
    }

}