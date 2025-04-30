namespace GameLand;

using static System.Console;

class Planner
{
    private List<Action> Actions { get; set; }
    IDictionary<string, bool> WorldState { get; set; }

    public Planner(List<Action> actions, IDictionary<string, bool> worldState)
    {
        Actions = actions;
        WorldState = worldState;
    }

    public Action Plan(KeyValuePair<string, bool> goal)
    {
        var currentState = WorldState.SingleOrDefault(s => s.Key == goal.Key);

        var visited = new HashSet<Action>();

        if (WorldState[goal.Key] == goal.Value)
        {
            WriteLine($"repeat");
            //return new Action(null, null, null);
        }
        ;

        IEnumerable<Action> actions = [.. Actions];

        List<KeyValuePair<string, bool>> goals = new List<KeyValuePair<string, bool>>();

        goals.Add(goal);

        while (true)
        {
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
                var nextAction = _actions
                    .Where(a => a.Preconditions.All(p => WorldState[p.Key] == p.Value))
                    .OrderBy(a => a.Cost)
                    .FirstOrDefault();

                if (nextAction is not null)
                {
                    return nextAction;
                }
                var _goals = _actions.SelectMany(a => a.Preconditions).ToList();
                goals = _goals.Count > 0 ? _goals : goals;
            }
            else
            {
                throw new Exception("The goal is unattainable!!!");
                //return actions.Single(a => a.Preconditions.Count == 0);
            }
        }
    }
}
