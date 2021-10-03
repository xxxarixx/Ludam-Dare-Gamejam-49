using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SecretGain : MonoBehaviour
{
    Interactable interactable;
    public AudioClip clip;
    private void Start()
    {
        interactable = GetComponent<Interactable>();
        interactable.OnInteract += Interactable_OnInteract;
    }

    private void Interactable_OnInteract()
    {
        SfxCreator.instance.PlaySound(clip);
        SecretsGainedCounter.instance.AddSecretGained(SceneManager.GetActiveScene().buildIndex);
        Destroy(gameObject);
    }
}
