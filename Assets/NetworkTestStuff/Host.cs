using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;
public class Host : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
     
            //Is this unity editor instance opening a clone project?
       
            NetworkManager.Singleton.StartHost();

        

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
