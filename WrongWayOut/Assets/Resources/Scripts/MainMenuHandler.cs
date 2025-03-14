using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuHandler : MonoBehaviour
{
    public void startGame(){
        Debug.Log("Starting Game");
        SceneManager.LoadScene("WrongWayOutMain");
    }
}
