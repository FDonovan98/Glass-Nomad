using UnityEngine;

public class GeneratorPowerOn : MonoBehaviour
{
    /// <summary>
    /// Once a player enters the generator room, all doors are locked open.
    /// </summary>
    /// <param name="coll"></param>
    private void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            Debug.Log("All doors are now opened");
            GameObject[] doors = GameObject.FindGameObjectsWithTag("Door");
            foreach (GameObject door in doors)
            {
                DoorTriggerScript doorTrigger = door.GetComponent<DoorTriggerScript>();
                
                // If the door isn't already open, then open it.
                if (!doorTrigger.GetDoorOpen()) { doorTrigger.ChangeDoorState(); }
                
                // Keeps the door open, forever.
                doorTrigger.LockDoor();
            }
            this.gameObject.SetActive(false);
        }
    }
}
