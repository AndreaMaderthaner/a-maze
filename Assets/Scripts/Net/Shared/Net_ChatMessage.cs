using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Networking.Transport;
using UnityEngine;

public class Net_ChatMessage : NetMessage
{
    // 0-8 OP CODE
    // 8-256 String Message
    public FixedString128Bytes ChatMessage { set; get; }
    public Net_ChatMessage()
    {
        Code = OpCode.CHAT_MESSAGE;
    }
    public Net_ChatMessage(DataStreamReader reader)
    {
        Code = OpCode.CHAT_MESSAGE;
        Deserialize(reader);
    }
    public Net_ChatMessage(string msg)
    {
        Code = OpCode.CHAT_MESSAGE;
        ChatMessage = msg;
    }
    public override void Serialize(ref DataStreamWriter writer)
    {
        writer.WriteByte((byte)Code);
        writer.WriteFixedString128(ChatMessage);
    }
    public override void Deserialize(DataStreamReader reader)
    {
        //the first byte is handled already 
        ChatMessage = reader.ReadFixedString128();
    }
    public override void ReceivedOnServer()
    {
        Debug.Log("SERVER:" + ChatMessage);
    }
    public override void ReceivedOnClient()
    {
        Debug.Log("Client:" + ChatMessage);
    }
}
