using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
public float movementSpeed = 5.0f;
public Rigidbody rb;

void Start ()
{
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
}

void Update ()
{
        if(Input.GetKey(KeyCode.W))
        {
                transform.position += transform.forward * movementSpeed * Time.deltaTime;
        }
        else if(Input.GetKey(KeyCode.S))
        {
                transform.position -= transform.forward * movementSpeed * Time.deltaTime;
        }
        else if(Input.GetKey(KeyCode.A))
        {
                transform.position -= transform.right * movementSpeed * Time.deltaTime;
        }
        else if(Input.GetKey(KeyCode.D))
        {
                transform.position += transform.right * movementSpeed * Time.deltaTime;
        }
}
}
