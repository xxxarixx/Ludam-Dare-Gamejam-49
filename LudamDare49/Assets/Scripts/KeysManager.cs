using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeysManager : MonoBehaviour
{
    public List<Key> AllKeys = new List<Key>();
    public static KeysManager instance;
    private void Awake()
    {
        instance = this;
    }
    public class Key
    {
        public GameObject KeyFollower;
        public int DoorID;
    }
    public void AddKey(GameObject KeyFollower, int DoorID)
    {
        Key key = new Key();
        key.KeyFollower = KeyFollower;
        key.DoorID = DoorID;
        AllKeys.Add(key);
    }
    public void RemoveKey(Key key)
    {
        AllKeys.RemoveAt(AllKeys.IndexOf(key));
    }
}
