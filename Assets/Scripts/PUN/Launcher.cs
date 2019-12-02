using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

public class Launcher : MonoBehaviourPunCallbacks
{
    private byte maxPlayersPerRoom = 5;

    [SerializeField]
    private GameObject controlPanel;
    [SerializeField]
    private GameObject progressLabel;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
