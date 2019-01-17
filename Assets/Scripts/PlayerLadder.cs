using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLadder : MonoBehaviour
{
    public Rigidbody2D rb;
    PlayerMotionController pM;

    public bool OnLadder;
    public float climbSpeed;
    private float climbVelocity;
    private float gravityStore;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        pM = GetComponent<PlayerMotionController>();
        gravityStore = pM.Gravity;
    }

    private void Update()
    {
        Climbing();
    }

    public void Climbing()
    {
        if(OnLadder)
        {
            if (Input.GetKey(KeyCode.W))
            {
                pM.Gravity = 0f;
            }

            if (Input.GetKey(KeyCode.S))
            {
                pM.Gravity = 0.2f;
            }
            //climbVelocity = climbSpeed * Input.GetAxisRaw("Vertical");
            //rb.velocity = new Vector2(rb.velocity.x, climbVelocity);
        }
        else if(!OnLadder)
        {
            pM.Gravity = gravityStore;
        }
    }
}
