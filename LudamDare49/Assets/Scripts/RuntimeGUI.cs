using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class RuntimeGUI : MonoBehaviour
{
    public GameObject ThanksForPlaying;
    public GameObject ThanksForPlayingWithSecrets;
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void SkipLevel()
    {
        if (SceneManager.GetActiveScene().buildIndex == 10) { Instantiate(ThanksForPlaying, Vector3.zero, Quaternion.identity); }
        else
              if (SceneManager.GetActiveScene().buildIndex == 11) { Instantiate(ThanksForPlayingWithSecrets, Vector3.zero, Quaternion.identity); }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
