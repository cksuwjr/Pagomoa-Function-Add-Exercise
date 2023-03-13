using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    private PlayerController controller;
    public float Speed = 40f;

    bool Jump = false;
    float MoveDir = 0;

    Vector3 bepos = Vector3.zero;
    private void Awake()
    {
        controller = GetComponent<PlayerController>();
    }
    private void Update()
    {
        MoveDir = Input.GetAxisRaw("Horizontal");

        if (Input.GetButton("Jump"))
        {
            Jump = true;
        }
    }
    private void FixedUpdate()
    {
        controller.Move(MoveDir, Speed, Jump);
        bepos = transform.position;
        Jump = false;
    }
}
