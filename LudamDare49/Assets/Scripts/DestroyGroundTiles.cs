using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class DestroyGroundTiles : MonoBehaviour
{
    public Transform GroundCheck;
    public LayerMask GroundLayer;
    public float Size;
    public Tilemap tmp;
    //public List<List<Vector3Int>> TilePosToDestroy = new List<List<Vector3Int>>();
    public List<Vector3Int> LastUpdatedContacts = new List<Vector3Int>();
    public bool DebugIT = false;
    private void Update()
    {
        bool grounded = Physics2D.Raycast(GroundCheck.transform.position, Vector2.down, Size, GroundLayer);
        /*tmp.SetTileFlags(TilePosToDestroy, TileFlags.LockTransform);
        tmp.SetColor(TilePosToDestroy, Color.white);*/
        if (!grounded)
        {
            foreach (var TmpContactPos in LastUpdatedContacts)
            {
                StartCoroutine(WaitAndDestory(TmpContactPos));
            }
            LastUpdatedContacts.Clear();
        }

    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Breakable"))
        {
            //get current contacts and hilight them
            var currentContactPoints = new List<Vector3Int>();
            foreach (ContactPoint2D contact in collision.contacts)
            {
                var currentHitPointTmp = tmp.WorldToCell(contact.point) - new Vector3Int(0, 1, 0);
                currentContactPoints.Add(currentHitPointTmp);
                tmp.SetTileFlags(currentHitPointTmp, TileFlags.LockTransform);
                tmp.SetColor(currentHitPointTmp, Color.white);
            }

            if (LastUpdatedContacts.Count > 0)
            {
                //check if last updated contact points are equal to current updated contact points
                bool isEqual = true;
                if (LastUpdatedContacts.Count < currentContactPoints.Count) { isEqual = false; } 
                else if (currentContactPoints.Count < LastUpdatedContacts.Count) { isEqual = false; }
                else
                {
                    for (int i = 0; i < currentContactPoints.Count; i++)
                    {
                        if (LastUpdatedContacts[i] != currentContactPoints[i]) { isEqual = false; }
                    }
                }
                        
                if (isEqual)
                {
                    if (DebugIT) { Debug.Log("Lists equal"); }
                }
                else
                {
                    //when conact points are not equal check find tiles that are not in current updated contact points
                    List<Vector3Int> NotEqualPositions = new List<Vector3Int>();
                    foreach (var lastCheckPos in LastUpdatedContacts)
                    {
                        if (!currentContactPoints.Contains(lastCheckPos))
                        {
                            NotEqualPositions.Add(lastCheckPos);
                        }
                    }
                    foreach (var pos in NotEqualPositions)
                    {
                        StartCoroutine(WaitAndDestory(pos));
                    }
                    //recreate list
                    LastUpdatedContacts.Clear();
                    LastUpdatedContacts.AddRange(currentContactPoints);
                    if (DebugIT) { Debug.Log("Lists not equal"); }
                }
            }
            else
            {
                //recreate list
                if (DebugIT) { Debug.Log("Lists equal"); }
                LastUpdatedContacts.Clear();
                LastUpdatedContacts.AddRange(currentContactPoints);
            } //equal

        }
        else
        {
            //just clrear
            if(LastUpdatedContacts != null)
            {
                foreach (var tmpContacts in LastUpdatedContacts)
                {
                    StartCoroutine(WaitAndDestory(tmpContacts));
                }   
            }
            LastUpdatedContacts.Clear();
        }
    }
    IEnumerator WaitAndDestory(Vector3Int tmpPos)
    {
        yield return new WaitForSeconds(0f);
        tmp.SetTile(tmpPos, null);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(GroundCheck.position, GroundCheck.position +Vector3.down * Size);
    }
}

