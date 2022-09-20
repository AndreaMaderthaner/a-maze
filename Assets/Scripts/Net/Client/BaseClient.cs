
using UnityEngine;
using Unity.Networking.Transport;
using System.Collections.Generic;
using Unity.Collections;

public class BaseClient : MonoBehaviour
{
    public NetworkDriver driver;
    protected NetworkConnection connection;
    private void Start()
    {
        Init();
    }
    private void Update()
    {
        UpdateClient();
    }
    private void OnDestroy()
    {
        Shutdown();
    }
    public virtual void Init()
    {
        //initialize driver
        driver = NetworkDriver.Create();
        connection = default(NetworkConnection);
        NetworkEndPoint endpoint;
        NetworkEndPoint.TryParse("192.168.0.111", 5522, out endpoint); //anyone can connect
                                                                       // endpoint.Port = 5522;
        connection = driver.Connect(endpoint);
    }
    public virtual void UpdateClient()
    {
        driver.ScheduleUpdate().Complete();
        CheckAlive();
        UpdateMessagePump();
    }

    private void CheckAlive()
    {
        if (!connection.IsCreated)
        {
            Debug.Log("Lost connection to server");
        }
    }
    protected virtual void UpdateMessagePump()
    {
        DataStreamReader stream;
        NetworkEvent.Type cmd;
        while ((cmd = driver.PopEventForConnection(connection, out stream)) != NetworkEvent.Type.Empty)
        {
            if (cmd == NetworkEvent.Type.Data)
            {
                OnData(stream);
            }
            else if (cmd == NetworkEvent.Type.Connect)
            {
                Debug.Log("connected to server");
            }
            else if (cmd == NetworkEvent.Type.Disconnect)
            {
                Debug.Log("Client got disconnected from server");
                connection = default(NetworkConnection);
            }
        }
    }

    public virtual void OnData(DataStreamReader stream)
    {
        NetMessage msg = null;
        var opCode = (OpCode)stream.ReadByte();
        switch (opCode)
        {
            case OpCode.CHAT_MESSAGE: msg = new Net_ChatMessage(stream); break;
            case OpCode.POSITION_MSG: msg = new Net_PositionMsg(stream); break;

            default:
                Debug.Log("message received had no OpCode");
                break;
        }
        msg.ReceivedOnClient();
    }
    public virtual void Shutdown()
    {
        driver.Dispose();
    }
    public virtual void SendToServer(NetMessage msg)
    {
        DataStreamWriter writer;
        driver.BeginSend(connection, out writer);
        msg.Serialize(ref writer);
        driver.EndSend(writer);
    }
}
