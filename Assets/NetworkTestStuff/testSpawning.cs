using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public class testSpawning : NetworkBehaviour
{
    [SerializeField]
    public List<GameObject> objs;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;
        if (Input.GetKey(KeyCode.T)) //test pawning object and sending it to the server
        {
            Vector3 pos = new Vector3(0, 0, 0);

            RequestFireServerRpc(pos);
            spawnObj(pos);
        }
    }
    [ServerRpc(RequireOwnership = false)]
    private void RequestFireServerRpc(Vector3 pos)
    {
        FireClientRpc(pos);
    }

    [ClientRpc]
    private void FireClientRpc(Vector3 pos)
    {
        spawnObj(pos);
    }

    private void spawnObj(Vector3 pos)
    {
        Instantiate(objs[0], pos, Quaternion.Euler(0, 30, 0));

    }
}


