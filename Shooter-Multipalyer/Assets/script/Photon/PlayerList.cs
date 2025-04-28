using Photon.Realtime;
using TMPro;
using UnityEngine;

public class PlayerList : MonoBehaviour
{
    public TMP_Text PlayerName;
    private CreateAndJoin roomManager;
    void Start()
    {
        roomManager = FindAnyObjectByType<CreateAndJoin>();
    }
    public void setPlayerName(Player _playerName)
    {
        PlayerName.text = _playerName.NickName;
    }


    void Update()
    {
        
    }
}
