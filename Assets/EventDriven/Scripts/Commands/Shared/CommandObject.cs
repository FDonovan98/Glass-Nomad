using UnityEngine;

public abstract class CommandObject : ScriptableObject
{
    public abstract void RunCommandOnStart(AgentInputHandler agentInputHandler);
}