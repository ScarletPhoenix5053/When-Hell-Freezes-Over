using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBrooke : MonoBehaviour
{
    public Rigidbody2D rb;
    PlayerMotionController pM;

    //LADDERS
    public bool OnLadder;
    public float climbSpeed;
    private float climbVelocity;
    private float gravityStore;

    //RESPAWNING
    public Vector3 respawnPoint;
    public Health health;

    //FORGES
    public bool atForge;
    public Image prompt;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        pM = GetComponent<PlayerMotionController>();
        gravityStore = pM.Gravity;

        health = GetComponent<Health>();
        
    }

    private void Update()
    {
        Climbing();

        if(health.Dead == true)
        {
            transform.position = respawnPoint;
            //Can make a function called RESPAWN if necessary.
            health.Hp = 6;
        }
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Checkpoint")
        {
            respawnPoint = other.transform.position;
        }

        if (other.tag == "Forge")
        {
            atForge = true;
            prompt.enabled = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Forge")
        {
            atForge = false;
            prompt.enabled = false;
        }
    }


}
