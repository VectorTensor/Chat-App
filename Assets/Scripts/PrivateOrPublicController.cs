using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class PrivateOrPublicController : MonoBehaviour
{

    [SerializeField] Button PrivateButton;
    [SerializeField] Button PublicButton;

    public static Boolean PrivateChat;

    
    // Start is called before the first frame update
    void Start()
    {
        PrivateButton.onClick.AddListener(PrivateButtonCallback);
        PublicButton.onClick.AddListener(PublicButtonCallback);

    }

    void PrivateButtonCallback(){
        PrivateChat = true;
        SceneManager.LoadScene("Chat", LoadSceneMode.Single);
    }

    void PublicButtonCallback(){
        PrivateChat = false;
        SceneManager.LoadScene("Chat", LoadSceneMode.Single);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
