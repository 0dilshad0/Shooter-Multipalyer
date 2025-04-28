using TMPro;
using UnityEngine;

public class RoomItem : MonoBehaviour
{
    public TMP_Text RoomName;
    private CreateAndJoin roomManager;

    private void Start()
    {
        roomManager = FindAnyObjectByType<CreateAndJoin>();
    }

    public void setRoomName(string _roomName)
    {
        RoomName.text = _roomName;
    }

    public void OnItemClick()
    {
        roomManager.JoinRoom(RoomName.text);
    }
   
}
