using UnityEngine;
using UnityEngine.SceneManagement;

public class ScrenManager : MonoBehaviour
{
   
   public void StartPlay()
   {
        SceneManager.LoadScene("loading");
   }
    public void Quit()
    {
        Application.Quit();
    }

}
