using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public class Client : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        NetworkManager.Singleton.StartClient();

    }

    // Update is called once per frame
    void Update()
    {
    }
}
