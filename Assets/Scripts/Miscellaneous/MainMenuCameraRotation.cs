<<<<<<< HEAD
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
=======
﻿using UnityEngine;
>>>>>>> master

public class MainMenuCameraRotation : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 1f;

    private void Update()
    {
        transform.localEulerAngles += new Vector3(0f, rotationSpeed * Time.deltaTime, 0f);
    }
}
