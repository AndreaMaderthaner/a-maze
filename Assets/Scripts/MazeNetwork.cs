using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class MazeNetwork : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(GetComponent<NetworkObject>());
        GetComponent<NetworkObject>().Spawn();
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
