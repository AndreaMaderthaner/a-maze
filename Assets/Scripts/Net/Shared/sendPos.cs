using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sendPos : MonoBehaviour
{
    // Start is called before the first frame update
    private float lastSend;
    private Vector3 prevPos;
    private BaseServer server;
    public bool isPlayer;
    public int id; //each Enemy or object (objs placed by 2. player) should have a unique id!
    public float sendInterval = 0.2f;

    void Start()
    {
        server = FindObjectOfType<BaseServer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (timePassed() && posChanged())
        {
            Net_PositionMsg msg;
            if (isPlayer)
                msg = new Net_PositionMsg(objTypeCode.PLAYER, id, transform.position.x, transform.position.y, transform.position.z);
            else 
                msg = new Net_PositionMsg(objTypeCode.ENEMY, id, transform.position.x, transform.position.y, transform.position.z);
            server.SendToClient(msg);
            lastSend = Time.time;
            prevPos = transform.position;
        }
    }

    bool timePassed()
    {
        return Time.time - lastSend > sendInterval;
    }
    bool posChanged()
    {
        return prevPos != transform.position;

    }
}
