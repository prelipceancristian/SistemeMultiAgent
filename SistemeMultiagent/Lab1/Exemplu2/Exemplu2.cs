using ActressMas;
using SistemeMultiagent.Lab1.Exemplu1;

namespace SistemeMultiagent.Lab1.Exemplu2;

public class MyAgent : Agent
{
    public override void Act(Message message)
    {
        if (message.Sender == "monitor")
        {
            if (message.Content == "start" || message.Content == "continue")
                TakeTurn();
            else if (message.Content == "stop")
                Stop();
        }
    }

    private void TakeTurn()
    {
        Console.WriteLine(this.Name);
        Send("monitor", "done");
    }
}

public class MonitorAgent : Agent
{
    private Dictionary<string, bool> _finished;
    private int _turn;
    private int _maxTurns;
    private static Random _rand = new Random();
    private List<string> _agentNames;

    public override void Setup()
    {
        _maxTurns = 3;
        _finished = new Dictionary<string, bool>();
        _agentNames = new List<string>();
        _turn = 1;
        Console.WriteLine("monitor: start turn 1");
        foreach (var a in this.Environment.FilteredAgents("agent"))
        {
            _agentNames.Add(a);
            _finished.Add(a, false);
        }

        int[] randPerm = RandomPermutation(_agentNames.Count);
        for (int i = 0; i < _agentNames.Count; i++)
            Send(_agentNames[randPerm[i]], "start");
    }

    public override void Act(Message message)
    {
        if (message.Content == "done")
            _finished[message.Sender] = true;
        if (AllFinished())
        {
            if (++_turn > _maxTurns)
            {
                Broadcast("stop");
                return;
            }

            for (int i = 0; i < _agentNames.Count; i++)
                _finished[_agentNames[i]] = false;
            Console.WriteLine("\r\nmonitor: start turn " + _turn);
            int[] randPerm = RandomPermutation(_agentNames.Count);
            for (int i = 0; i < _agentNames.Count; i++)
                Send(_agentNames[randPerm[i]], "continue");
        }
    }

    private int[] RandomPermutation(int n)
    {
        int[] numbers = new int[n];
        for (int i = 0; i < n; i++)
            numbers[i] = i;
        int[] randPerm = numbers.OrderBy(x => _rand.Next()).ToArray();
        return randPerm;
    }

    private bool AllFinished()
    {
        foreach (string a in _finished.Keys)
            if (!_finished[a])
                return false;
        return true;
    }
}

public class Exemplu2
{
    public static void Run()
    {
        var env = new EnvironmentMas(0, 300);
        var noAgents = 10;
        for (var i = 1; i <= noAgents; i++)
        {
            var a = new MyAgent();
            env.Add(a, "agent" + i);
        }

        var m = new MonitorAgent();
        env.Add(m, "monitor");
        env.Start();
    }
}