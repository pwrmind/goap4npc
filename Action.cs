namespace GameLand;

using static System.Console;

/// <summary>
/// Represents a single action that an NPC can perform in the game world.
/// Each action has preconditions that must be met and effects that change the world state.
/// </summary>
public class Action
{
    /// <summary>
    /// Conditions that must be true for the action to be possible
    /// </summary>
    public IDictionary<string, bool> Preconditions { get; private set; }

    /// <summary>
    /// Changes to the world state that occur when the action is executed
    /// </summary>
    public IDictionary<string, bool> Effects { get; private set; }

    /// <summary>
    /// Potential outcomes that might occur when the action is executed
    /// </summary>
    public IDictionary<string, bool> ExpectedEffects { get; private set; }

    /// <summary>
    /// Cost of performing this action (used for planning efficiency)
    /// </summary>
    public int Cost { get; private set; }

    /// <summary>
    /// Creates a new action with the specified preconditions and effects
    /// </summary>
    /// <param name="preconditions">Conditions that must be true to perform the action</param>
    /// <param name="effects">Changes to the world state when action is executed</param>
    /// <param name="expectedEffects">Potential outcomes of the action</param>
    /// <param name="cost">Cost of performing the action</param>
    public Action(
        IDictionary<string, bool> preconditions,
        IDictionary<string, bool> effects,
        IDictionary<string, bool> expectedEffects = null,
        int cost = 1
    )
    {
        Preconditions = preconditions ?? throw new ArgumentNullException(nameof(preconditions));
        Effects = effects ?? throw new ArgumentNullException(nameof(effects));
        ExpectedEffects = expectedEffects ?? new Dictionary<string, bool>();
        Cost = cost;
    }

    /// <summary>
    /// Checks if this action can be performed in the given world state
    /// </summary>
    /// <param name="worldState">Current state of the game world</param>
    /// <returns>True if all preconditions are met</returns>
    public bool IsPossible(IDictionary<string, bool> worldState)
    {
        if (worldState == null)
        {
            throw new ArgumentNullException(nameof(worldState));
        }
        return Preconditions.All(p => worldState[p.Key] == p.Value);
    }

    /// <summary>
    /// Executes the action, modifying the world state according to its effects
    /// </summary>
    /// <param name="worldState">Current state of the game world</param>
    public void Execute(IDictionary<string, bool> worldState)
    {
        if (worldState == null)
        {
            throw new ArgumentNullException(nameof(worldState));
        }
        if (IsPossible(worldState))
        {
            foreach (var effect in Effects)
            {
                worldState[effect.Key] = effect.Value;
                WriteLine($"\t[{this}] Apply effect: {effect}");
            }
        }
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