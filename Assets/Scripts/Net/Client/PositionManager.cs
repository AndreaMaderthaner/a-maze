using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionManager : MonoBehaviour
{
    public static PositionManager instance { get; private set; }
    public Transform player;
    public List<Transform> enemies;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void updatePlayerPos(Vector3 pos)
    {
        player.position = pos;
    }
    public void updateEnemyPos(int id, Vector3 pos)
    {
        //on server-side (VR) enemies should be numbered (id in sendPos)
        enemies[id].position = pos;
    }
}
