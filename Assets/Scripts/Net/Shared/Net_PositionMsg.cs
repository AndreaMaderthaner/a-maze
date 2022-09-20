using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Networking.Transport;
using UnityEngine;

public class Net_PositionMsg : NetMessage
{
    // 0-8 OP CODE
    // 8-256 String Message
    public int id { set; get; }
    public float posX { set; get; }
    public float posY { set; get; }

    public float posZ { set; get; }

    public objTypeCode objType;

    public Net_PositionMsg()
    {
        Code = OpCode.POSITION_MSG;
    }
    public Net_PositionMsg(DataStreamReader reader)
    {
        Code = OpCode.POSITION_MSG;
        Deserialize(reader);
    }
    public Net_PositionMsg(objTypeCode objectType, int id, float x, float y, float z)
    {
        Code = OpCode.POSITION_MSG;
        objType = objectType;
        id = id;
        posX = x;
        posY = y;
        posZ = z;
    }
    public override void Serialize(ref DataStreamWriter writer)
    {
        writer.WriteByte((byte)Code);
        writer.WriteByte((byte)objType);
        writer.WriteInt(id);
        writer.WriteFloat(posX);
        writer.WriteFloat(posY);
        writer.WriteFloat(posZ);

    }
    public override void Deserialize(DataStreamReader reader)
    {
        //the first byte is handled already 
        objType = (objTypeCode)reader.ReadByte();
        id = reader.ReadInt();
        posX = reader.ReadFloat();
        posY = reader.ReadFloat();
        posZ = reader.ReadFloat();


    }
    public override void ReceivedOnServer()
    {
        Debug.Log("SERVER:" + id + "  " + posX);
    }
    public override void ReceivedOnClient()
    {
        if (objType == objTypeCode.PLAYER)
            PositionManager.instance.updatePlayerPos(new Vector3(posX, posY, posZ));
        else if (objType == objTypeCode.ENEMY)
            PositionManager.instance.updateEnemyPos(id, new Vector3(posX, posY, posZ));

    }
}
