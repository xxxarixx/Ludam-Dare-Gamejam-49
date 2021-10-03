using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class MenuController : MonoBehaviour
{
    public static MenuController instance;
    public bool secretLevelUnlocked = false;
    public int howMuchSecretsDiscovered = 0;
    public TextMeshProUGUI SecretCount;
    public SecretsGainedCounter secretsCounter;
    private void Awake()
    {
        instance = this;

    }
    private void Update()
    {
        SecretCount.text = howMuchSecretsDiscovered + "/" + secretsCounter.Secrets.Count;
    }
    public void OnExitButton()
    {
        Application.Quit();
        UnityEditor.EditorApplication.ExitPlaymode();
    }
    public void LoadScene(int SceneIndex)
    {
        SceneManager.LoadScene(SceneIndex);
    }
    public void LoadSecretLevel(int SceneIndex)
    {
        if (!secretLevelUnlocked) { return; }
        SceneManager.LoadScene(SceneIndex);
    }
}
