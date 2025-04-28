using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ConnectToServer : MonoBehaviourPunCallbacks
{
    public TMP_InputField UserName;
    public TMP_Text buttonText;
    

    public void Onclick()
    {
        if(UserName.text.Length>=1)
        {
            PhotonNetwork.NickName = UserName.text;
            buttonText.text = "Connecting...";
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.ConnectUsingSettings();
        }
    }
    public void onclickBack()
    {
        SceneManager.LoadScene("Main Menu");
    }
    public override void OnConnectedToMaster()
    {
        SceneManager.LoadScene("lobby");
    }
}
    

