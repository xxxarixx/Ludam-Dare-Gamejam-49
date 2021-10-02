using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Collider2D))]
public class Interactable : MonoBehaviour
{
    public GameObject InteractPopup;
    public Transform SpawnPos;
    MainControlls controlls;
    public delegate void Interacted();
    public event Interacted OnInteract;
    GameObject SpawnedPopup;
    private void Awake()
    {
        if (SpawnPos == null) { SpawnPos = transform; }
        controlls = new MainControlls();
        controlls.Player.Interact.performed += ctx => PressedInteract();
    }
    private void OnEnable()
    {
        controlls.Enable();
    }
    private void OnDisable()
    {
        controlls.Disable();
    }
    void PressedInteract()
    {
        if (SpawnedPopup == null) { return; }
        OnInteract?.Invoke();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) { return; }
        if (CharacterMovement.instance.PlayerIsInteracting) { return; }
        CharacterMovement.instance.PlayerIsInteracting = true;
        SpawnedPopup = Instantiate(InteractPopup, SpawnPos.position, Quaternion.identity);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) { return; }
        if (SpawnedPopup == null) { return; }
        CharacterMovement.instance.PlayerIsInteracting = false;
        Destroy(SpawnedPopup);
    }
}
