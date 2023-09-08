using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ChatController : MonoBehaviour
{
   [SerializeField] GameObject MessageToSend;
   [SerializeField] GameObject SendButton; 

    [SerializeField] GameObject Title;
   [SerializeField] GameObject SocketManagerObject;  

   [SerializeField] GameObject ChatPanel; 

   [SerializeField] GameObject MessagePrefab;

   public static Boolean messageMode;

   [SerializeField] TMP_Dropdown UserToChat;

   List<TMP_Dropdown.OptionData> UserOptions;

   [SerializeField] TextMeshProUGUI PlayerName;

   [SerializeField] ScrollRect scrollrect;

   public string Message;
    // Start is called before the first frame update
    void Start()
    {

        PlayerName.text = Join.Name;
        UserOptions = new List<TMP_Dropdown.OptionData>();
        UserToChat.ClearOptions();
        //SendButton.GetComponent<Button>().onClick.AddListener(SendPublicButtonClicked);
        if (PrivateOrPublicController.PrivateChat){
            Title.GetComponent<TextMeshProUGUI>().text = "Private";
            SendButton.GetComponent<Button>().onClick.AddListener(SendPrivateButtonClicked);

        }
        else{
            Title.GetComponent<TextMeshProUGUI>().text = "Public";
            SendButton.GetComponent<Button>().onClick.AddListener(SendPublicButtonClicked);

        }

    }

    SocketManager GetSocketManager(){

        // Check to see if the SocketManager component exist
        return SocketManagerObject.GetComponent<SocketManager>();

    }

    void SendPublicButtonClicked(){
        Debug.Log("send pulci");
        string message = MessageToSend.GetComponent<TMP_InputField>().text;
        GameObject gm = Instantiate(MessagePrefab);

        gm.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text =  message;
        gm.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text =  "You";


        gm.transform.parent = ChatPanel.transform;
        gm.transform.localPosition = new Vector3(0,0,0);
        gm.transform.localScale = new Vector3(1,1,1);
        //NetworkManager.GetInstance().SendMessageToServer(message);
        GetSocketManager().SendPublicMessage(message);
        
        MessageToSend.GetComponent<TMP_InputField>().text = "";

    }


    public void AddTheUsersOnTheServer(string username){


        UserToChat.ClearOptions();
        UserOptions.Add(new TMP_Dropdown.OptionData(username));  
        Debug.Log(UserOptions);
        UserToChat.AddOptions(UserOptions);


    }

    void SendPrivateButtonClicked(){
        Debug.Log("send private");

        string message = MessageToSend.GetComponent<TMP_InputField>().text;
        GameObject gm = Instantiate(MessagePrefab);

        gm.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text =  message;
        gm.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text =  "You";


        gm.transform.parent = ChatPanel.transform;
        gm.transform.localPosition = new Vector3(0,0,0);
        gm.transform.localScale = new Vector3(1,1,1);
        //NetworkManager.GetInstance().SendMessageToServer(message);
        GetSocketManager().SendPrivateMessage(message,UserToChat.options[UserToChat.value].text);
        
        MessageToSend.GetComponent<TMP_InputField>().text = "";        
    }


    public  void InstantiateMessage(string message,string name){
        Debug.Log("hello");
        GameObject gm = Instantiate(MessagePrefab,new Vector3(0,0,0),Quaternion.identity);

        gm.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text =  message;
        gm.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text =  name;
        gm.transform.SetParent(ChatPanel.transform);
        
        gm.transform.localPosition = new Vector3(0,0,0);

        gm.transform.localScale = new Vector3(1,1,1);
        scrollrect.normalizedPosition = new Vector2(0,0);
    }

    // Update is called once per frame
    void Update()
    {

        
    }

    void OnEnable(){
        SocketManager.OnNewMessage += InstantiateMessage;
    }

    void OnDisable(){
        
        SocketManager.OnNewMessage -= InstantiateMessage;
    
    }

}
