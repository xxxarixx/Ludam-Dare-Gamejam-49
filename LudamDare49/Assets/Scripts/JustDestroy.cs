using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JustDestroy : MonoBehaviour
{
    public float DestroyTime = 2f;
    public List<GameObject> SpawnOnDestory = new List<GameObject>();
    private void Update()
    {
        DestroyTime -= Time.deltaTime;
        if(DestroyTime <= 0)
        {
            for (int i = 0; i < SpawnOnDestory.Count; i++)
            {
                Instantiate(SpawnOnDestory[i], transform.position, Quaternion.identity);
            }
            Destroy(gameObject);
        }
    }
}
