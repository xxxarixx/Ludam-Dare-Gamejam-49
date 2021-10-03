using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public bool useVelocity = true;
    public bool useLerp = false;
    public float MoveSpeed = 2f;
    public float LerpSpeed;
    float currentMoveSpeed;
    public float DistanceToStopMove = .2f;
    public bool OnTargetReachSnapIT = false;
    Rigidbody2D rb;
    Transform target;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentMoveSpeed = MoveSpeed;
        target = CharacterMovement.instance.transform;
    }
    private void FixedUpdate()
    {
        if (!useVelocity) { rb.velocity = Vector2.zero; return; }
        if (Vector3.Distance(transform.position, target.position) < DistanceToStopMove) 
        { 
            rb.velocity = Vector2.zero;
            if (OnTargetReachSnapIT) { transform.position = target.position; }
            if (parentToSet != null)
            {
                transform.SetParent(parentToSet);
                transform.localPosition = Vector2.zero;
                parentToSet = null;
            }
            return; 
        }
        rb.velocity = getDirectionToplayer() * currentMoveSpeed * Time.deltaTime;
        currentMoveSpeed = MoveSpeed * (Vector3.Distance(transform.position, target.position) / 2);
    }
    public float Progress = 0f;
    private void Update()
    {
        if (!useLerp) { return; }
        if (Vector3.Distance(transform.position, target.position) < DistanceToStopMove) { rb.velocity = Vector2.zero; if (OnTargetReachSnapIT) { transform.position = target.position; } return; }
       
        if(Progress >= 1f)
        {
            if (OnTargetReachSnapIT) { transform.position = target.position;  }
            Progress = 1f;
            if (parentToSet != null)
            {
                transform.SetParent(parentToSet);
                transform.localPosition = Vector2.zero;
                parentToSet = null;
            }
        }
        else
        {
            rb.position = Vector3.Lerp(rb.position, target.position, Progress);
            Progress += Time.deltaTime * LerpSpeed;
            if (Vector3.Distance(rb.position, target.position) < .1f) { Progress = 1f;  }
        }
    }
    Transform parentToSet;
    public void OnEndSetParent(Transform parent)
    {
        parentToSet = parent;
    }
    Vector2 getDirectionToplayer()
    {
        return (target.position - transform.position).normalized;
    }
    public void SetDestination(Transform destination)
    {
        target = destination;
    }
}
