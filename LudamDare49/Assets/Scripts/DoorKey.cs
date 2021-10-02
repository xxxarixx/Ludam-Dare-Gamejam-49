using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class DoorKey : MonoBehaviour
{
    public List<Lock> LocksID = new List<Lock>();
    public float DistanceToStartOpenAnimation = 2f;
    public SpriteRenderer sprend;
    Interactable interactable;
    Transform player;
    KeysManager Kmanager;
    public Sprite OpenedDoorSprite;
    bool DoorFullyOpened = false;
    [System.Serializable]public class Lock
    {
        public string Lockname;
        public int LockID;
        public Transform KeyPosition;
        public KeysManager.Key KeyInHole;
    }
    private void Start()
    {
        interactable = GetComponent<Interactable>();
        interactable.OnInteract += Interactable_OnInteract;
        player = CharacterMovement.instance.transform;
        Kmanager = KeysManager.instance;
        interactable.enabled = false;
    }

    private void Interactable_OnInteract()
    {
        if (DoorFullyOpened)
        {
            Debug.Log("DoorOpenedCanGo");
             sprend.sprite = OpenedDoorSprite;
            foreach (var mlock in LocksID)
            {
                Destroy(mlock.KeyInHole.KeyFollower);
            }
            //SceneManager.LoadScene(0);
        }
        else
        {
            Debug.Log("MustTakeMoreKeys");
        }
        //Go inside Door
    }
    private void Update()
    {
       if(Vector3.Distance(transform.position, player.position) < DistanceToStartOpenAnimation && !DoorFullyOpened)
        {
            //Check for openingToDoor
            foreach (var key in Kmanager.AllKeys)
            {
                foreach (var mlock in LocksID)
                {
                    if(mlock.KeyInHole == null && mlock.LockID == key.DoorID)
                    {
                        var followScript = key.KeyFollower.GetComponent<FollowPlayer>();
                        followScript.useLerp = true;
                        followScript.useVelocity = false;
                        followScript.SetDestination(mlock.KeyPosition);
                        followScript.DistanceToStopMove = 0f;
                        followScript.OnTargetReachSnapIT = true;
                        var sin = followScript.GetComponentInChildren<SinMoveAnimation>();
                        sin.enabled = false;
                        sin.transform.localPosition = Vector3.zero;
                        followScript.OnEndSetParent(mlock.KeyPosition);
                        mlock.KeyInHole = key;
                    }
                }
            }
            bool isOpened = true;
            foreach (var mlock in LocksID)
            {
                if (mlock.KeyInHole == null) { isOpened = false; break; }
                if (mlock.KeyInHole.KeyFollower.GetComponent<FollowPlayer>().Progress < 1f) { isOpened = false;break; }
            }
            if (isOpened)
            {
                interactable.enabled = true;
                DoorFullyOpened = true;
            }
            
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, DistanceToStartOpenAnimation);
    }
}
