using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class ButtonRestart : MonoBehaviour
{
    public Button myButton;

    void Start()
    {
        // Get a reference to the button component on this game object
        myButton = GetComponent<Button>();

        // Add a listener to the button to handle clicks
        myButton.onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        // This code will be executed when the button is clicked
        Restart();
    }

    public void Restart()
    {
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
