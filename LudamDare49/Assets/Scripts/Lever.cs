using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    Interactable interactable;
    
    public SpriteRenderer LeverSpRend;
    public Sprite LeverOpen;
    public Sprite LeverClosed;
   public List<LeverDoor> LeverDoors = new List<LeverDoor>();
    public bool LeverOn = false;
     [System.Serializable]public class LeverDoor
    {
        public bool ReverseOn = false;
        public GameObject LeverDoorGoReference;
        public Sprite LeverDoorOpen;
        public Sprite LeverDoorClosed;
    }
    private void Start()
    {
        interactable = GetComponent<Interactable>();
        interactable.OnInteract += Interactable_OnInteract;
        ChangeLeverState(LeverOn);
    }

    private void Interactable_OnInteract()
    {
        LeverOn = !LeverOn;
        ChangeLeverState(LeverOn);
    }
    public void ChangeLeverState(bool State)
    {
        LeverOn = State;
        #region Doors
        foreach (var leverDoor in LeverDoors)
        {
            bool localLeverOn = State;
            if (leverDoor.ReverseOn) { localLeverOn = !localLeverOn; }
            var collider = leverDoor.LeverDoorGoReference.GetComponent<Collider2D>();
            var spRend = leverDoor.LeverDoorGoReference.GetComponent<SpriteRenderer>();
            if (localLeverOn)
            {
                spRend.sprite = leverDoor.LeverDoorOpen;
                collider.enabled = false;
            }
            else
            {
                spRend.sprite = leverDoor.LeverDoorClosed;
                collider.enabled = true;
            }
        }
        #endregion
        if (LeverOn)
        {
            LeverSpRend.sprite = LeverOpen;
        }
        else
        {
            LeverSpRend.sprite = LeverClosed;
        }
    }
}
