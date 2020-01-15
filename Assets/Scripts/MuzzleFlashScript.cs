using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MuzzleFlashScript
{
    private Vector3 instantiatePosition;
    
    public IEnumerator Flash(Vector3 position, Quaternion rotation)
    {
        instantiatePosition = position;
        GameObject flash = PhotonNetwork.Instantiate("Muzzle_Flash", instantiatePosition, rotation);
        yield return new WaitForSeconds(0.1f);
        PhotonNetwork.Destroy(flash);
    }
}