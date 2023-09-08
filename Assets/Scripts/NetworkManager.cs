using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System;
using System.Threading;

public class NetworkManager : MonoBehaviour
{
    private Socket sck;
    public static NetworkManager instance;

    public bool talking = true;
    void Awake()
    {
        if(instance == null)
            instance = this;
    }

    public static NetworkManager GetInstance()
    {
        return instance;
    }

    // Start is called before the first frame update
    void Start()
    {
        talking = true;
        sck = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);
        IPEndPoint ip = new IPEndPoint(IPAddress.Parse("127.0.0.1"),2004);

        try {
            sck.Connect(ip);
        }
        catch{
            Debug.Log("Some error occured !!!");
        }
        SendName();
        Thread t = new Thread(new ThreadStart(ListenForMessages));
        t.Start();
        //StartCoroutine(ListenForMessages());

    }
    

    void ListenForMessages()
{
    while(talking){
        Byte[] data = new Byte[2048];
        
        sck.Receive(data);
        Debug.Log(Encoding.UTF8.GetString(data));

    }
        
    
}

    void SendName(){
        byte[] data = Encoding.ASCII.GetBytes(Join.Name);
        sck.Send(data);
       // sck.Close();  

    }

    public void SendMessageToServer(string message){
        byte[] data = Encoding.ASCII.GetBytes(message);
        sck.Send(data);
    }

    void OnDestroy(){


        sck.Close(); 

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
