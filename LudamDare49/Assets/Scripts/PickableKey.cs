using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableKey : MonoBehaviour
{
    public GameObject KeyFollower;
    public int DoorID;
    Interactable interactable;
    public AudioClip sfx;
    
    private void Start()
    {
        interactable = GetComponent<Interactable>();
        interactable.OnInteract += Interactable_OnInteract;
    }
    private void Interactable_OnInteract()
    {
        var spawnedFollower = Instantiate(KeyFollower, transform.position, Quaternion.identity);
        spawnedFollower.GetComponentInChildren<SpriteRenderer>().sprite = GetComponentInChildren<SpriteRenderer>().sprite;
        KeysManager.instance.AddKey(spawnedFollower, DoorID);
        SfxCreator.instance.PlaySound(sfx);
        Destroy(gameObject);
        Debug.Log("ShouldGrabKey");
    }
    
}
