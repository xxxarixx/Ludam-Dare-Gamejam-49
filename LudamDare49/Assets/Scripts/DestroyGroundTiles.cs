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
    public Vector3Int TilePosToDestroy = new Vector3Int(-100000,-10000000,-10000000);
    private void Update()
    {
        /* var hit = Physics2D.Raycast(GroundCheck.transform.position, Vector2.down, Size, GroundLayer);
         if (hit.collider != null)
         {
             var currentHitPointTmp = tmp.WorldToCell(hit.point);

             if(TilePosToDestroy != currentHitPointTmp)
             {

                 StartCoroutine(WaitAndDestory(TilePosToDestroy));
                 TilePosToDestroy = currentHitPointTmp;
             }
             else
             {
                 tmp.SetTileFlags(TilePosToDestroy, TileFlags.LockTransform);
                 tmp.SetColor(TilePosToDestroy, Color.white);
             }


         }
         else
         {
             StartCoroutine(WaitAndDestory(TilePosToDestroy));
         }
 */
        tmp.SetTileFlags(TilePosToDestroy, TileFlags.LockTransform);
        tmp.SetColor(TilePosToDestroy, Color.white);
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Breakable"))
        {
            foreach (ContactPoint2D contact in collision.contacts)
            {
                var currentHitPointTmp = tmp.WorldToCell(contact.point);
                TilePosToDestroy = currentHitPointTmp;
                
                /*var currentHitPointTmp = tmp.WorldToCell(contact.point);
                if (TilePosToDestroy != currentHitPointTmp)
                {

                    StartCoroutine(WaitAndDestory(TilePosToDestroy));
                    TilePosToDestroy = currentHitPointTmp;
                }
                else
                {
                    tmp.SetTileFlags(TilePosToDestroy, TileFlags.LockTransform);
                    tmp.SetColor(TilePosToDestroy, Color.white);
                }*/
            }
        }
    }
    IEnumerator WaitAndDestory(Vector3Int tmpPos)
    {
        yield return new WaitForSeconds(.2f);
        tmp.SetTile(tmpPos, null);
    }
   /* private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(GroundCheck.position, GroundCheck.position + Vector3.down * Size);
    }*/
}
