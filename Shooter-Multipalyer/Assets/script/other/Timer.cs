using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviourPunCallbacks
{
    public float gameTme=240f;
    public TMP_Text timerText;
    public TMP_Text scoreboardText;
    public GameObject endScreen;
    public GameObject gameCanvas;
    private bool Isleave;
    void Start()
    {
        endScreen.SetActive(false);
        Isleave = false;
    }

   
    void Update()
    {
        gameTme -= Time.deltaTime;
        UpdateTime();
        if (gameTme<=0)
        {
            EndGame();
        }
    }

    private void UpdateTime()
    {
        int minutes = Mathf.FloorToInt(gameTme / 60);
        int seconds = Mathf.FloorToInt(gameTme % 60);

        timerText.text = minutes + " : " + seconds;
    }
    

   

    
    private void EndGame()
    {
        UpdateScoreboard();
        Canvas[] allCanva = FindObjectsOfType<Canvas>(); 
        foreach(Canvas c in allCanva)
        {
           if(c.gameObject.name!="game Canvas")
            {
                c.gameObject.SetActive(false);
            }
        }
        gameCanvas.SetActive(true);
        endScreen.SetActive(true);
        Time.timeScale = 0;
    }

    public void OnClickNext()
    {
       if(!Isleave)
        {
            Isleave = true;
     
            SceneManager.LoadScene("Main Menu");
        }      
    }
    
    
    
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if (changedProps.ContainsKey("Kills"))
        {
            UpdateScoreboard();
        }
    }

    void UpdateScoreboard()
    {
        List<string> playerScores = new List<string>();

        foreach (Player player in PhotonNetwork.PlayerList)
        {
            int kills = 0;
            if (player.CustomProperties.TryGetValue("Kills", out object killCount))
            {
                kills = (int)killCount;
            }
            playerScores.Add($"{player.NickName}: {kills} Kills");
        }

        scoreboardText.text = string.Join("\n", playerScores);
    }
}
