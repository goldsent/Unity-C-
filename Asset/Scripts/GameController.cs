using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameController : MonoBehaviour
{
    public int playerHealth = 100;
    public int enemyHealth = 100;
    private bool isGameRunning = true;

    [SerializeField] private GameObject buttonRestart;
    // Start is called before the first frame update
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        if (playerHealth <= 0 || enemyHealth <= 0)
        {
            StartCoroutine(DelayGameRestart());
        }

    }

    void StopGame()
    {
        isGameRunning = false;
        Time.timeScale = 0f; // This freezes the game
        // Display a UI element to show game over or restart button
    }

    IEnumerator DelayGameRestart()
    {
        yield return new WaitForSeconds(2f);
        StopGame();
        buttonRestart.SetActive(true);
    }
}
