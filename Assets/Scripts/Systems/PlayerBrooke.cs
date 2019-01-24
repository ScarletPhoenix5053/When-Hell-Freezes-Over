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
    //public float climbSpeed;
    private float climbVelocity;
    private float gravityStore;

    //RESPAWNING
    public Vector3 respawnPoint;
    protected Health health;

    //FORGES
    public bool atForge;
    public GameObject prompt;

    //ITEMPICKUP
    public float itemSpeed = 6f;
    public Transform target;
    public bool pickedUp;


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
            prompt.SetActive(true);
        }

        //

        if(other.tag == "Health")
        {
            if(health.Hp < 5)
            {
                health.Hp += 2;
                Destroy(other.gameObject);
            }

            else if(health.Hp == 5)
            {
                health.Hp += 1;
                Destroy(other.gameObject);
            }
           
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Interactable")
        {
            pickedUp = true;

            float step = itemSpeed * Time.deltaTime;
            other.transform.position = Vector3.MoveTowards(other.transform.position, target.position, step);

            if(other.transform.position == target.position)
                other.gameObject.SendMessage("PickUp");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Forge")
        {
            atForge = false;
            prompt.SetActive(false);
        }

        if(other.tag == "Interactable")
        {
            pickedUp = false;
        }

    }


}
