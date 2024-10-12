using ActressMas;

namespace SistemeMultiagent.Lab1.Exemplu1;

public class MyAgent : Agent
{
    private static Random _rand = new Random();

    public override void Setup()
    {
        for (var i = 1; i <= 10; i++)
        {
            Send("monitor", i.ToString());
        }
    }
}

public class MonitorAgent : Agent 
{
    public override void Act(Message message)
    {
        Console.WriteLine("{0}: {1}", message.Sender, message.Content);
    }
}

public static class Exemplu1
{
    public static void Run()
    {
        var env = new EnvironmentMas();
        var a1 = new MyAgent
        {
            Name = "agent1"
        };
        env.Add(a1, a1.Name);
        var a2 = new MyAgent
        {
            Name = "agent2"
        };
        env.Add(a2, a2.Name);
        var m = new MonitorAgent(); env.Add(m, "monitor");
        env.Start();
    }
}