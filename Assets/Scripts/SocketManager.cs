using System.Threading.Tasks;
using System;
using System.Net.Sockets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class SocketManager : MonoBehaviour
{
    SocketIOUnity client;

    Boolean JoinedPublicNetwork;

    [SerializeField] GameObject ChatController;

    public string Message ;

    [SerializeField] GameObject ChatPanel; 

    [SerializeField] GameObject MessagePrefab;  

    public static  event Action<string,string> OnNewMessage;

    // Start is called before the first frame update
    void Start()
    {
        JoinedPublicNetwork = false;
      //  ConectToServerFirst();
        client = new SocketIOUnity("http://127.0.0.1:3000/");
        client.Connect();
        //GameObject gm = Instantiate(MessagePrefab);

        client.OnConnected +=  (sender, e) =>
        { 
            

            if (!PrivateOrPublicController.PrivateChat){
                JoinPublicNetwork();
                Debug.Log("Join Public network");

            } else{
                Debug.Log("Join private network");

                client.Emit("GetUsers");
                
                
                client.OnUnityThread("UsersList",(response) =>{
                    
                    Debug.Log(response.GetValue<List<string>>());
                    List<string> Clients = response.GetValue<List<string>>();
                  //  GetChatController().AddTheUsersOnTheServer(response.GetValue<string>());
                    Debug.Log("new Message");
                     for (int i =0 ; i<Clients.Count; i++){
                         GetChatController().AddTheUsersOnTheServer(Clients[i]);
                         Debug.Log("new Message");

                     }


                });


                JoinPrivateNetwork();
                

                client.OnUnityThread("NewClientAdded",(response)=>{

                Debug.Log(response);

                string name = response.GetValue<string>();
                GetChatController().AddTheUsersOnTheServer(name);
                

                });
                

            } 
            

            

            

            client.OnUnityThread("NewMessageFromServer",(response)=>{
                
                Debug.Log(response.ToString());

                string message =  response.GetValue<string>();
                string name = response.GetValue<string>(1);
                Debug.Log(message.ToString());
                
                OnNewMessage?.Invoke(message,name);
                //test = response;

                
                Debug.Log(message.ToString());
            });

            client.OnUnityThread("NewPrivateMessage",(response)=>{
                Debug.Log(response.ToString());

                string message =  response.GetValue<string>();
                string name = response.GetValue<string>(1);
                Debug.Log(message.ToString());
                
                OnNewMessage?.Invoke(message,name);
                //test = response;

                
                Debug.Log(message.ToString());

            });
        };
        
        
        
    }

    ChatController GetChatController(){

        // Check to see if the Chatcontroller component exist
        return ChatController.GetComponent<ChatController>();
    }

    class JsonData{
        public string status;
        public JsonData(string s){
            status = s;
        }
    };


    async void JoinPublicNetwork(){
    await client.EmitAsync("JoinPublic", response =>{
        //Debug.Log(response.ToString());
        JsonData jd = JsonUtility.FromJson<JsonData>(response.ToString());

        if (jd.status == "ok"){
            JoinedPublicNetwork = true;
        }
        //Debug.Log("hello");
    }, Join.Name);

    
    }
    async void JoinPrivateNetwork(){

        await client.EmitAsync("JoinPrivate", response =>{
        //Debug.Log(response.ToString());
        JsonData jd = JsonUtility.FromJson<JsonData>(response.ToString());

        if (jd.status == "ok"){
            JoinedPublicNetwork = false;
        }
        Debug.Log("hello");
        }, Join.Name);


    }


    
    public void SendPublicMessage(string message){
            client.EmitAsync("messagePublic",message);

    }


    public void SendPrivateMessage(string message , string Sendto){

        client.Emit("PrivateMessage",message, Sendto,Join.Name);
    }

    
    // Update is called once per frame
    void Update()
    {

    }
}
