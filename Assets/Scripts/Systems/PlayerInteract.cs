using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteract : MonoBehaviour
{
    PlayerMotionController pM;

    [Header("Ladders")]
    private float inputVertical;
    public Rigidbody2D rb;
    float gravityStore;
    float distance = 5;
    public float climbingSpeed = 2;
    public LayerMask ladder;
    public bool isClimbing;

    [Header("Checkpoints")]
    public Vector3 respawnPoint;
    protected Health health;

    [Header("Forges")]
    public bool atForge;
    public GameObject prompt;
    public Sprite unfrozenForge;
    private SpriteRenderer forgeRenderer;

    [Header("Item Pickup")]
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
        GlobalControl.Instance.playerPosition = transform.position;

        if (health.Dead == true)
        {
            transform.position = respawnPoint;
            health.Hp = 6;
        }

        //Climbing();

        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, Vector2.up, distance, ladder);

        if(hitInfo.collider != null)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                isClimbing = true;
            }
        }
        else
        {
            //if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
            //{
                isClimbing = false;
            //}
        }

        if(isClimbing) // && hitInfo.collider != null)
        {
            inputVertical = Input.GetAxisRaw("Vertical");
            rb.velocity = new Vector2(rb.velocity.x, inputVertical * climbingSpeed);
            pM.Gravity = 0f;
        }
        else if(isClimbing == false)
        {
            pM.Gravity = gravityStore;
        }
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //CHECKPOINTS
        if(other.tag == "Checkpoint")
        {
            respawnPoint = other.transform.position;
        }

        //FORGES
        if (other.tag == "Forge")
        {
            atForge = true;
            prompt.SetActive(true);

            //Do I have to create a seperate script to sit on the forge that does this magical change?
            forgeRenderer = other.GetComponent<SpriteRenderer>();
            forgeRenderer.sprite = unfrozenForge;
        }

        //HEALTH PICKUPS
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
        //ITEM PICKUPS
        if (other.tag == "Interactable")
        {
            float step = itemSpeed * Time.deltaTime;
            other.transform.position = Vector3.MoveTowards(other.transform.position, target.position, step);
            pickedUp = true;

            if (other.transform.position == target.position)
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
