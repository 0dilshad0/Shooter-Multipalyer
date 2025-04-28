using Photon.Pun;
using TMPro;
using UnityEngine;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
public class CreateAndJoin : MonoBehaviourPunCallbacks
{
    public TMP_InputField RoomInput;
    public GameObject LobbyPannel;
    public GameObject RoomPannel;
    public TMP_Text RoomName;

    public RoomItem RoomItemPrefab;
    List<RoomItem> roomItemList = new List<RoomItem>();
    public Transform content;

    public float UpdateTime = 1.5f;
    private float NextUpdatetime;

    public PlayerList playerListPrefab;
    List<PlayerList> PlayerLists = new List<PlayerList>();
    public Transform playerListTransform;

    public GameObject PlayButton;
    private void Start()
    {
        PhotonNetwork.JoinLobby();
        RoomPannel.SetActive(false);
    }
    private void Update()
    {
        if(PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount >=2)
        {
            PlayButton.SetActive(true);
        }
        else
        {
            PlayButton.SetActive(false);
        }
    }

    public void CreateRoom()
    {
        if(RoomInput.text.Length >0)
        {
            PhotonNetwork.CreateRoom(RoomInput.text, new RoomOptions() { MaxPlayers=5} );
        }
       
    }

    public void JoinRoom(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
    }
    public override void OnJoinedRoom()
    {
        LobbyPannel.SetActive(false);
        RoomPannel.SetActive(true);
        RoomName.text ="Room Name: " + PhotonNetwork.CurrentRoom.Name;
        UpdatePlayerList();
    }

    public void OnLeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }
    public override void OnLeftRoom()
    {
        LobbyPannel.SetActive(true);
        RoomPannel.SetActive(false);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        if(Time.time >= NextUpdatetime)
        {
            UpdateRoomList(roomList);
            NextUpdatetime = Time.time + UpdateTime;
        }
       
    }

    private void UpdateRoomList(List<RoomInfo> list)
    {
        foreach(RoomItem item in roomItemList)
        {
            Destroy(item.gameObject);
        }
        roomItemList.Clear();

        foreach(RoomInfo Room in list)
        {
          RoomItem NewRoom = Instantiate(RoomItemPrefab, content);
            NewRoom.setRoomName(Room.Name);
            roomItemList.Add(NewRoom);
        }

    }

    private void UpdatePlayerList()
    {
        foreach(PlayerList playerName in PlayerLists)
        {
            Destroy(playerName.gameObject);
        }
        PlayerLists.Clear();

        if(PhotonNetwork.CurrentRoom == null)
        {
            return;
        }

        foreach(KeyValuePair<int,Player> player in PhotonNetwork.CurrentRoom.Players)
        {
            PlayerList newPlayerItem = Instantiate(playerListPrefab, playerListTransform);
            newPlayerItem.setPlayerName(player.Value);
            PlayerLists.Add(newPlayerItem);
        }

    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdatePlayerList();
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdatePlayerList();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public void OnClickPlay()
    {
        PhotonNetwork.LoadLevel("game");
    }
    public void onclickBack()
    {
        SceneManager.LoadScene("Main Menu");
        PhotonNetwork.LeaveLobby();
        PhotonNetwork.Disconnect();

    }
}
