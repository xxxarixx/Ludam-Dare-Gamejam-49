using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ChangeSceneEvent : MonoBehaviour
{
    public void ChangeScene(int SceneIndex) { SceneManager.LoadScene(SceneIndex); }

}
