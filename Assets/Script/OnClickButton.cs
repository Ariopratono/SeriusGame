using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnClickButton : MonoBehaviour
{
    // Start is called before the first frame update
    public void PlayGame()
    {
        // Gantilah "MainScene" dengan nama scene permainan Anda
        SceneManager.LoadSceneAsync("SampleScene");
    }

    // Fungsi untuk tombol Exit
    public void ExitGame()
    {
        // Keluar dari permainan
        Application.Quit();
        Debug.Log("Game is exiting");
    }
}
