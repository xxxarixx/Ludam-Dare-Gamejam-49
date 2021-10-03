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
    public Tilemap DecorationTmp;
    //public List<List<Vector3Int>> TilePosToDestroy = new List<List<Vector3Int>>();
    public List<Vector3Int> LastUpdatedContacts = new List<Vector3Int>();
    public bool DebugIT = false;
    public Tile JumpPad;
    public float JumpPadForce = 5;
    Rigidbody2D rb;
    public Color TilesColor = Color.white;
    [Header("Audio")]
    public AudioClip DestoryBlockSFX;
    public AudioClip JumpPadSFX;
    private void Start()
    {
        rb = GetComponentInParent<Rigidbody2D>();
    }
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
                tmp.SetColor(currentHitPointTmp, TilesColor);
                if(tmp.GetTile<Tile>(currentHitPointTmp) == JumpPad)
                {
                    rb.velocity = new Vector2(rb.velocity.x, JumpPadForce);
                    SfxCreator.instance.PlaySound(JumpPadSFX, .15f);
                }
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
        yield return new WaitForSeconds(.1f);
        if (tmp.GetTile<Tile>(tmpPos) != null)
        { 
            SfxCreator.instance.PlaySound(DestoryBlockSFX, .1f);
            GameObject FallingBlockVFX_Parent = new GameObject();
            {
                FallingBlockVFX_Parent.AddComponent<JustDestroy>().DestroyTime = .5f;
                var blockRb = FallingBlockVFX_Parent.AddComponent<Rigidbody2D>();
                FallingBlockVFX_Parent.transform.position = tmp.CellToWorld(tmpPos) + new Vector3(.5f, .5f);
            }
            GameObject SpriteHolder = new GameObject();
            {
                SpriteHolder.transform.SetParent(FallingBlockVFX_Parent.transform);
                SpriteHolder.transform.localPosition = Vector3.zero;
                var spRend = SpriteHolder.AddComponent<SpriteRenderer>();
                spRend.sprite = tmp.GetTile<Tile>(tmpPos).sprite;
                spRend.sortingOrder = -1;
                var sinAnimation = SpriteHolder.AddComponent<SinMoveAnimation>();
                sinAnimation.MaxHeighMultiplayer = 2f;
                sinAnimation.MoveSpeed = 6f;
                sinAnimation.OnlyOneJump = true;
            }
            tmp.SetTile(tmpPos, null);
            if (DecorationTmp != null) { DecorationTmp.SetTile(tmpPos, null); }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(GroundCheck.position, GroundCheck.position +Vector3.down * Size);
    }
}

