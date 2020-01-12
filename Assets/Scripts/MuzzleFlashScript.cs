using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuzzleFlashScript : MonoBehaviour
{
    public GameObject muzzleFlash;

    public void Flash()
    {
        if (muzzleFlash != null)
        {
            var flash = Instantiate(muzzleFlash, gameObject.transform);
            flash.transform.position = new Vector3(flash.transform.position.x, flash.transform.position.y, flash.transform.position.z + 0.52f);
            Destroy(flash, 0.1f);
        }
    }
}
