using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class UIManager : MonoBehaviour
{
    public static UIManager Instance; //Declares and instance of this object as static
    public TMP_InputField inputField;
    public string playerName;

    public void Awake() //Makes this instance of the main manager to prevail in other scenes
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    
    // Start is called before the first frame update
    void Start()
    {
        inputField = FindObjectOfType<TMP_InputField>();
    }

    // Update is called once per frame
    void Update()
    {
        if (inputField != null)
        {
            SetPlayerName();
        }
    }

    public void SetPlayerName()
    {
        string nameInputField = inputField.text;
        playerName = nameInputField;
    }
    public void StartNew()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitApplication()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}
