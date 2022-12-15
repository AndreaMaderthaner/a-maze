using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;


public class scroll : MonoBehaviour
{
    public RiddleBlock riddleRoom;
    
    private MeshRenderer mesh;
    private Transform trans;
    // Start is called before the first frame update
    void Start()
    {
        mesh = GetComponent<MeshRenderer>();
        trans = GetComponent<Transform>();
        mesh.enabled = false;
        trans.GetChild(0).gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (RiddleBlock.instance)
        {
            if (RiddleBlock.instance.playerPosStatus == 2)
            {
                mesh.enabled = true;
                trans.GetChild(0).gameObject.SetActive(true);
            }
            else
            {
                mesh.enabled = false;
                trans.GetChild(0).gameObject.SetActive(false);
            }
        }
        
    }
}
