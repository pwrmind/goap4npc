namespace GameLand;

using static System.Console;

class VisionSensor
{
    Random rand = new Random();

    public void Update(IDictionary<string, bool> worldState)
    {
        worldState["Enemy.IsInSight"] = SeeEnemy();
        WriteLine($"\t[{this}] Apply effect: {worldState.Single(s=>s.Key == "Enemy.IsInSight")}");
        worldState["Provision.IsInSight"] = SeeFood();
        WriteLine($"\t[{this}] Apply effect: {worldState.Single(s => s.Key == "Provision.IsInSight")}");
    }

    private bool SeeEnemy()
    {
        // Логика обнаружения врага
        return rand.Next(0, 10) > 5; // Например, враг виден
    }

    private bool SeeFood()
    {
        // Логика обнаружения еды
        return rand.Next(0, 10) > 5; // Например, еда не видна
    }
}

//public class Planner
//{
//    private List<Action> Actions { get; set; }
//    private IDictionary<string, bool> WorldState { get; set; }

//    public Planner(List<Action> actions, IDictionary<string, bool> worldState)
//    {
//        Actions = actions ?? throw new ArgumentNullException(nameof(actions));
//        WorldState = worldState ?? throw new ArgumentNullException(nameof(worldState));
//    }

    //public Action Plan(KeyValuePair<string, bool> goal)
    //{
    //    if (WorldState[goal.Key] == goal.Value)
    //    {
    //        WriteLine("Goal already achieved.");
    //        //return null;
    //    }

    //    var queue = new Queue<Action>();
    //    var visited = new HashSet<string>();

    //    foreach (var action in Actions)
    //    {
    //        if (action.Effects.ContainsKey(goal.Key) && action.Effects[goal.Key] == goal.Value
    //            || action.ExpectedEffects.ContainsKey(goal.Key) && action.ExpectedEffects[goal.Key] == goal.Value)
    //        {
    //            queue.Enqueue(action);
    //        }
    //    }

    //    while (queue.Count > 0)
    //    {
    //        var action = queue.Dequeue();
    //        if (action.IsPossible(WorldState))
    //        {
    //            return action;
    //        }
    //        foreach (var precondition in action.Preconditions)
    //        {
    //            if (!visited.Contains(precondition.Key))
    //            {
    //                visited.Add(precondition.Key);
    //                var nextAction = Actions.FirstOrDefault(a => 
    //                    a.Effects.ContainsKey(precondition.Key) && a.Effects[precondition.Key] == precondition.Value
    //                    || a.ExpectedEffects.ContainsKey(precondition.Key) && a.ExpectedEffects[precondition.Key] == precondition.Value);
    //                if (nextAction != null)
    //                {
    //                    queue.Enqueue(nextAction);
    //                }
    //            }
    //        }
    //    }

    //    WriteLine("Goal is unattainable.");
    //    return null;
    //}
//}