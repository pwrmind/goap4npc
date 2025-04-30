namespace GameLand;

using static System.Console;

/// <summary>
/// Simulates the NPC's vision and perception of the game world.
/// Updates the world state based on what the NPC can see.
/// </summary>
class VisionSensor
{
    Random rand = new Random();

    /// <summary>
    /// Updates the world state based on what the NPC can perceive
    /// </summary>
    /// <param name="worldState">Current state of the game world to update</param>
    public void Update(IDictionary<string, bool> worldState)
    {
        // Update enemy detection state
        worldState["Enemy.IsInSight"] = SeeEnemy();
        WriteLine($"\t[{this}] Apply effect: {worldState.Single(s=>s.Key == "Enemy.IsInSight")}");

        // Update food detection state
        worldState["Provision.IsInSight"] = SeeFood();
        WriteLine($"\t[{this}] Apply effect: {worldState.Single(s => s.Key == "Provision.IsInSight")}");
    }

    /// <summary>
    /// Simulates enemy detection logic
    /// </summary>
    /// <returns>True if an enemy is detected</returns>
    private bool SeeEnemy()
    {
        // Simulated enemy detection logic
        return rand.Next(0, 10) > 5; // 50% chance to see an enemy
    }

    /// <summary>
    /// Simulates food detection logic
    /// </summary>
    /// <returns>True if food is detected</returns>
    private bool SeeFood()
    {
        // Simulated food detection logic
        return rand.Next(0, 10) > 5; // 50% chance to see food
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