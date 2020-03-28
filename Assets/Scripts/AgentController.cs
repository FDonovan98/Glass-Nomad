using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentController : MonoBehaviourPunCallbacks
{
    public delegate void RunCommandOnAwake();
    public RunCommandOnAwake runCommandOnAwake;

    void Awake()
    {

    }
}
