using UnityEngine;

public abstract class CommandObject : ScriptableObject
{
    public abstract void Execute(GameObject agent, AgentValues agentValues);
}