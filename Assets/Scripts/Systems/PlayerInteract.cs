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

    private bool forgeUnfreeze;

    [Header("Item Pickup")]
    public float itemSpeed = 6f;
    public Transform target;
    public bool pickedUp;

    [Header("KeysAndDoors")]
    public bool hasKey;
    public bool doorOpen;
    public Transform forgeRoom;
    public Transform mainRoom;
    public Sprite doorOpenSprite;
    bool hasTeleported;
    bool atDoor;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        pM = GetComponent<PlayerMotionController>();
        gravityStore = pM.Gravity;

        health = GetComponent<Health>();
        
    }

    private void Update()
    {
        if (health.Dead)
        {
            transform.position = respawnPoint;
            GetComponent<PlayerController>().Respawn();
            health.Hp = 6;
        }

        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, Vector2.up, distance, ladder);

        if(hitInfo.collider != null)
        {
            if (InputManager.Jump())
            {
                isClimbing = true;
            }
        }
        else
        {
                isClimbing = false;
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

        if (doorOpen && atDoor)
        {
            if (InputManager.Interact())
            {
                transform.position = forgeRoom.transform.position;
                hasTeleported = true;
            }
        }

        else if (!hasKey && InputManager.Interact())
        {
            FindObjectOfType<AudioManager>().Play("DoorLocked");
            //Show a message saying the door is locked.
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
            FindObjectOfType<AudioManager>().Play("Forge");
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
                FindObjectOfType<AudioManager>().Play("HealthPickup");
                Destroy(other.gameObject);
            }

            else if(health.Hp == 5)
            {
                health.Hp += 1;
                FindObjectOfType<AudioManager>().Play("HealthPickup");
                Destroy(other.gameObject);
            }
        }

        //KEY
        if(other.tag == "Key")
        {
            hasKey = true;

            //if (other.transform.position == target.position)
                other.gameObject.SendMessage("PickUp");
        }

        //DOOR
        if(other.tag == "ForgeDoor") //&& !hasTeleported)
        {
            SpriteRenderer otherSprite;
            otherSprite = other.GetComponent<SpriteRenderer>();

            atDoor = true;
            bool hasPlayed = false;

            if (hasKey)
            {
                doorOpen = true;
                otherSprite.sprite = doorOpenSprite;
                hasTeleported = false;

                if (!hasPlayed)
                {
                    FindObjectOfType<AudioManager>().Play("DoorUnlock");
                    hasPlayed = true;
                }
            }  
        }

        if (other.tag == "Door" && !hasTeleported)
        {
            transform.position = mainRoom.transform.position;
            hasTeleported = true;
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

        if(other.tag == "ForgeDoor")
        {
            atDoor = false;
        }

        if(other.tag == "Door")
        {
            hasTeleported = false;
        }
    }

    public void BackToCheckpoint()
    {
        transform.position = respawnPoint;
    }

   

}
