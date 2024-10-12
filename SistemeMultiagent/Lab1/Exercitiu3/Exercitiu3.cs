using ActressMas;

namespace SistemeMultiagent.Lab1.Exercitiu3;

public class MyAgent : Agent
{
    private List<string> _otherAgents = [];
    private int _seed;
    private Random _random = new(10);

    public MyAgent(string name, int seed)
    {
        Name = name;
        _seed = seed;
    }

    public override async void Setup()
    {
        _random = new Random(_seed);
        var period = _random.Next(500, 5000);
        _otherAgents = [..Environment.AllAgents()];
        _otherAgents.Remove(Name);
        while (true)
        {
            var index = _random.Next(_otherAgents.Count);
            var agent = _otherAgents[index];
            var content = $"[Agent {Name} intreaba] cat este ceasul?";
            Console.WriteLine(content);
            Send(agent, content);
            await Task.Delay(period);
        }
    }

    public override void Act(Message message)
    {
        if (message.Content.Contains("cat este ceasul?"))
        {
            var time = DateTime.Now.ToString("HH:mm:ss");
            var content = $"[Agent {Name} raspunde agentului {message.Sender}] este ora {time}";
            Console.WriteLine(content);
            Send(message.Sender, content);
        }
    }
}

public static class Exercitiu3
{
    public static void Run()
    {
        var environment = new EnvironmentMas();
        for (var i = 1; i <= 5; i++)
        {
            var agent = new MyAgent("Agent" + i, i);
            environment.Add(agent);
        }
        environment.Start();
    }
}