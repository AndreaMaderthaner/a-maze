using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using ParrelSync;
using UnityEngine.SceneManagement;
public class Host : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
     
            //Is this unity editor instance opening a clone project?
            if (ClonesManager.IsClone())
            {
                Debug.Log("This is a clone project.");
                SceneManager.LoadScene("Player2", LoadSceneMode.Single);
            }
            else
            {
            NetworkManager.Singleton.StartHost();

        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
