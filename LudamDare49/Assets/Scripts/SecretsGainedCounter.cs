using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class SecretsGainedCounter : MonoBehaviour
{
    public List<Secret> Secrets = new List<Secret>();
    public static SecretsGainedCounter instance;
    public TextMeshProUGUI SecretCounter;
    [System.Serializable]public class Secret
    {
        public bool Gained = false;
    }
    private void Awake()
    {
        instance = this;
        UpdateSecretsCount();
    }
    public void AddSecretGained(int ID)
    {
        Secrets[ID].Gained = true;
        UpdateSecretsCount();
    }
    private void Start()
    {
        var finded = FindObjectOfType<SecretsGainedCounter>();
        if (finded != null && finded != this) { Destroy(gameObject); } else { DontDestroyOnLoad(gameObject); }
    }
    public void UpdateSecretsCount()
    {
        int SecretCount = 0;
        foreach (var secret in Secrets)
        {
            if (secret.Gained)
            {
                SecretCount++;
            }
        }
        SecretCounter.text = "Secrets:" + SecretCount + "/" + Secrets.Count;
    }
    private void Update()
    {
        
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            int SecretCount = 0;
            foreach (var secret in Secrets)
            {
                if (secret.Gained)
                {
                    SecretCount++;
                }
            }
            MenuController.instance.howMuchSecretsDiscovered = SecretCount;
            if (SecretCount >= Secrets.Count) { MenuController.instance.secretLevelUnlocked = true; }
        }
    }

}
