using static System.Console;

using GameLand;
using Action = GameLand.Action;

class Program
{
    static async Task Main()
    {
        VisionSensor visionSensor = new VisionSensor();
        IDictionary<string, bool> worldState = GetWorldState();
        List<Action> actions = GetActions();

        Planner planner = new Planner(actions, worldState);

        var goal = GetNextGoal(worldState);

        visionSensor.Update(worldState);

        while (true)
        {
            var plannedAction = planner.Plan(goal);

            plannedAction.Execute(worldState);

            if (worldState[goal.Key] == goal.Value)
            {
                WriteLine($"The goal has been achieved: {goal}");

                goal = GetNextGoal(worldState);
            }

            await Task.Delay(1000);

            visionSensor.Update(worldState);
        }
    }

    private static KeyValuePair<string, bool> GetNextGoal(IDictionary<string, bool> worldState)
    {
        Random rand = new Random();
        var achievableGoals = worldState.Where(s => s.Key.StartsWith("NPC")).ToList();
        var randomState = achievableGoals.ElementAt(rand.Next(0, achievableGoals.Count));
        var goal = new KeyValuePair<string, bool>(randomState.Key, !randomState.Value);

        WriteLine($"Next goal: {goal}");

        return goal;
    }

    private static IDictionary<string, bool> GetWorldState()
    {
        IDictionary<string, bool> worldState = new Dictionary<string, bool>();

        worldState["NPC.Action.Idle"] = false;
        worldState["NPC.Action.Walk"] = false;
        worldState["NPC.Action.Run"] = false;
        worldState["NPC.Action.Jump"] = false;
        worldState["NPC.Action.Attack"] = false;
        worldState["NPC.Action.Eat"] = false;
        return worldState;
    }

    private static List<Action> GetActions()
    {
        // Действия
        var actions = new List<Action>
        {
            new Action(
                preconditions: new Dictionary<string, bool>(){ },
                effects: new Dictionary<string, bool>(){
                    { "NPC.Action.Idle", true },
                    { "NPC.Action.Walk", false },
                    { "NPC.Action.Run", false },
                    { "NPC.Action.Jump", false },
                    { "NPC.Action.Attack", false },
                    { "NPC.Action.Eat", false },
                },
                cost : 1
            ),
            new Action(
                preconditions : new Dictionary<string, bool>(){ { "NPC.Action.Idle", true } },
                effects: new Dictionary<string, bool>(){ { "NPC.Action.Walk", true }, { "NPC.Action.Idle", false } },
                cost : 2
            ),
            new Action(
                preconditions : new Dictionary<string, bool>(){ { "NPC.Action.Idle", true } },
                effects: new Dictionary<string, bool>(){ { "NPC.Action.Run", true }, { "NPC.Action.Idle", false } },
                cost : 3
            ),
            new Action(
                preconditions : new Dictionary<string, bool>(){ { "NPC.Action.Walk", true } },
                effects: new Dictionary<string, bool>(){ { "NPC.Action.Run", true }, { "NPC.Action.Walk", false } },
                cost : 3
            ),
            new Action(
                preconditions : new Dictionary<string, bool>(){ { "NPC.Action.Run", true } },
                effects: new Dictionary<string, bool>(){ { "NPC.Action.Jump", true }, { "NPC.Action.Run", false } },
                cost : 4
            ),
            new Action(
                preconditions : new Dictionary<string, bool>(){ { "NPC.Action.Run", true } },
                effects: new Dictionary<string, bool>(){ { "NPC.Action.Walk", true }, { "NPC.Action.Run", false } },
                cost : 2
            ),
            new Action(
                preconditions : new Dictionary<string, bool>(){ { "NPC.Action.Walk", true } },
                effects: new Dictionary<string, bool>(){ { "NPC.Action.Jump", true }, { "NPC.Action.Walk", false } },
                cost: 4
            ),
            new Action(
                preconditions : new Dictionary<string, bool>(){ { "NPC.Action.Jump", true } },
                effects: new Dictionary<string, bool>(){ { "NPC.Action.Idle", true }, { "NPC.Action.Jump", false } },
                cost: 1
            ),
            new Action(
                preconditions: new Dictionary<string, bool>(){ { "NPC.Action.Idle", true } },
                effects: new Dictionary<string, bool>(){ { "NPC.Action.Jump", true }, { "NPC.Action.Idle", false } },
                cost: 4
            ),
            // Enemy
            new Action(
                preconditions: new Dictionary<string, bool>(){ { "Enemy.IsInSight", true } },
                effects: new Dictionary<string, bool>(){ { "NPC.Action.Attack", true }, { "Enemy.IsInSight", false } },
                cost: 1
            ),
            new Action(
                preconditions: new Dictionary<string, bool>(){ { "Enemy.IsInSight", false } },
                effects: new Dictionary<string, bool>(){ { "NPC.Action.Walk", true } },
                expectedEffects: new Dictionary<string, bool>(){ { "Enemy.IsInSight", true } },
                cost: 2
            ),
            // Provision
            new Action(
                preconditions: new Dictionary<string, bool>(){ { "Provision.IsInSight", true } },
                effects: new Dictionary<string, bool>(){ { "NPC.Action.Eat", true }, { "Provision.IsInSight", false } },
                cost : 1
            ),
            new Action(
                preconditions: new Dictionary<string, bool>(){ { "Provision.IsInSight", false } },
                effects: new Dictionary<string, bool>(){ { "NPC.Action.Walk", true } },
                expectedEffects: new Dictionary<string, bool>(){ { "Provision.IsInSight", true } },
                cost: 2
            ),
        };
        return actions;
    }
}
