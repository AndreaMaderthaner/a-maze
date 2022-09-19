using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Networking.Transport;
using UnityEngine;

public class Net_PlayerPosition : NetMessage
{
    // 0-8 OP CODE
    // 8-256 String Message
    public int PlayerId { set; get; } 
    public float posX { set; get; }
    public float posY { set; get; }

    public float posZ { set; get; }

    public Net_PlayerPosition()
    {
        Code = OpCode.PLAYER_POSITION;
    }
    public Net_PlayerPosition(DataStreamReader reader)
    {
        Code = OpCode.PLAYER_POSITION;
        Deserialize(reader);
    }
    public Net_PlayerPosition(int playerId, float x, float y, float z)
    {
        Code = OpCode.PLAYER_POSITION;
        PlayerId = playerId;
        posX = x;
        posY = y;
        posZ = z;
    }
    public override void Serialize(ref DataStreamWriter writer)
    {
        writer.WriteByte((byte)Code);
        writer.WriteInt(PlayerId);
        writer.WriteFloat(posX);
        writer.WriteFloat(posY);
        writer.WriteFloat(posZ);

    }
    public override void Deserialize(DataStreamReader reader)
    {
        //the first byte is handled already 
        PlayerId = reader.ReadInt();
        posX = reader.ReadFloat();
        posY = reader.ReadFloat();
        posZ = reader.ReadFloat();


    }
    public override void ReceivedOnServer()
    {
        Debug.Log("SERVER:" + PlayerId+"  "+posX);
    }
    public override void ReceivedOnClient()
    {
        Debug.Log("CLIENT:" + PlayerId + "  " + posX);
    }
}
