using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Code initially based on code from here:
// https://answers.unity.com/questions/155907/basic-movement-walking-on-walls.html
public class AlienMovement : MonoBehaviour
{
    public float movementSpeed = 6;
    // Turn speed is in degrees per second.
    public float turnSpeed = 90; 
    // Smoothing speed.
    public float lerpSpeed = 10;
    public float gravity = 10;
    // Is the alien in contact with the ground.
    public bool isGrounded;
    // The initial speed of a jump.
    public float jumpSpeed = 10;
    // The range at which to detect a wall to stick to.
    public float jumpRange = 10;
}
