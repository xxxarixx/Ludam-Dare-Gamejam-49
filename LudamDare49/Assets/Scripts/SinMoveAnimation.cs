using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinMoveAnimation : MonoBehaviour
{
    float Progress;
    public float MoveSpeed = 3f;
    public float MaxHeighMultiplayer = .5f;
    float StartPositionY;
    private void Start()
    {
        StartPositionY = transform.localPosition.y;
    }
    private void Update()
    {
        Progress += Time.deltaTime * MoveSpeed;
        var moveY = Mathf.Sin(Progress) * MaxHeighMultiplayer;
        transform.localPosition = new Vector3(transform.localPosition.x, StartPositionY + moveY, transform.localPosition.z);
    }
}
