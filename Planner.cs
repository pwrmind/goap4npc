namespace GameLand;

using static System.Console;

/// <summary>
/// The GOAP planner that finds sequences of actions to achieve goals.
/// Uses a backward-chaining approach to find the most efficient plan.
/// </summary>
class Planner
{
    /// <summary>
    /// List of all possible actions the NPC can perform
    /// </summary>
    private List<Action> Actions { get; set; }

    /// <summary>
    /// Current state of the game world
    /// </summary>
    IDictionary<string, bool> WorldState { get; set; }

    /// <summary>
    /// Creates a new planner with the given actions and world state
    /// </summary>
    /// <param name="actions">List of possible actions</param>
    /// <param name="worldState">Current world state</param>
    public Planner(List<Action> actions, IDictionary<string, bool> worldState)
    {
        Actions = actions;
        WorldState = worldState;
    }

    /// <summary>
    /// Plans a sequence of actions to achieve the given goal
    /// </summary>
    /// <param name="goal">The desired world state to achieve</param>
    /// <returns>The next action to perform to work towards the goal</returns>
    public Action Plan(KeyValuePair<string, bool> goal)
    {
        var currentState = WorldState.SingleOrDefault(s => s.Key == goal.Key);
        var visited = new HashSet<Action>();

        if (WorldState[goal.Key] == goal.Value)
        {
            WriteLine($"repeat");
        }

        IEnumerable<Action> actions = [.. Actions];
        List<KeyValuePair<string, bool>> goals = new List<KeyValuePair<string, bool>>();
        goals.Add(goal);

        while (true)
        {
            // Find actions that can help achieve the current goals
            var _actions = actions.Where(a =>
                (a.Effects.Intersect(goals.AsEnumerable()).Count() > 0)
                || (a.ExpectedEffects.Intersect(goals.AsEnumerable()).Count() > 0)
            );

            if (goals.Count == 0)
            {
                throw new Exception("Empty goals!");
            }

            if (_actions.Any())
            {
                // Find the cheapest action that can be performed
                var nextAction = _actions
                    .Where(a => a.Preconditions.All(p => WorldState[p.Key] == p.Value))
                    .OrderBy(a => a.Cost)
                    .FirstOrDefault();

                if (nextAction is not null)
                {
                    return nextAction;
                }

                // If no action can be performed, add their preconditions as new goals
                var _goals = _actions.SelectMany(a => a.Preconditions).ToList();
                goals = _goals.Count > 0 ? _goals : goals;
            }
            else
            {
                throw new Exception("The goal is unattainable!!!");
            }
        }
    }
}
