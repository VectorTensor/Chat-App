using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Join : MonoBehaviour
{
    public static string Name;
    [SerializeField] private GameObject TextField;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(JoinTheNetwork);
        
    }

    void JoinTheNetwork(){
        Name = TextField.GetComponent<TMP_InputField>().text;
        //Debug.Log(Name);
        SceneManager.LoadScene(2,LoadSceneMode.Single);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
