using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LaberinthConditions : MonoBehaviour, IConditions
{
    public bool winCon = false;

   public GameObject player;
    void Start()
    {
        GameManager.conditions = this;
    }
    
    void IConditions.Lose()
    {
        if(player == null || !player.activeSelf)
        {
            SceneManager.LoadScene("Scenes/Lose");
        }
    }

    void IConditions.Win()
    {
        if (isWin ())
        {

            SceneManager.LoadScene("Scenes/PuzzleBall");

        }
    }



    private void OnTriggerEnter(Collider other)
    {

        if(other.tag == "Player")
        {
            isStatusGame(true);
        }
        
    }

    public bool isStatusGame(bool isWin)
    {
        winCon = isWin;
        return isWin;

    }

    public bool isWin()
    {
        return winCon;
    }
}
