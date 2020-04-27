using Photon.Pun;
using UnityEngine;

public class AmmoRacksTrigger : TriggerInteractionScript
{
    public int maxAmmoGiven = 30;
    [ReadOnly] public int currAmmoGiven = 0;
}
