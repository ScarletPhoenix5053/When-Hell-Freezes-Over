using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BlobAttackManager))]
[RequireComponent(typeof(MotionController))]
public class BlobController : MonoBehaviour
{
    public float AttackRange = 2f;
    public float ChaseRange = 10f;
    public float LeapHeight = 10f;

    public bool IsGrounded { get { return Physics2D.Raycast(transform.position, -Vector2.up, GetComponent<Collider2D>().bounds.extents.y + 0.05f, LayerMask.GetMask("Environment")); } }
    
    private BlobAnimator an;
    private BlobAttackManager am;
    private MotionController mc;
    private PlayerController plr;
    private Behaviour currentBehaviour = Behaviour.Idle;

    private enum Behaviour
    {
        Idle, Attacking, Chasing, Hit
    }

    private float distToPlayer {  get { return Vector2.Distance(transform.position, plr.transform.position); } }
    private bool playerToLeft { get { return plr.transform.position.x < transform.position.x; } }

    private void Awake()
    {
        an = GetComponent<BlobAnimator>();
        am = GetComponent<BlobAttackManager>();
        mc = GetComponent<MotionController>();
        plr = FindObjectOfType<PlayerController>();
    }
    private void LateUpdate()
    {
        FacePlayer();
    }
    private void FixedUpdate()
    {
        DecideAction();
        Act();
        mc.UpdateMotion();
    }
    
    private void DecideAction()
    {
        // if player doesn't exist or is too far away
        if (plr == null || distToPlayer > ChaseRange)
        {
            SetBehaviour(Behaviour.Idle);
            return;
        }
        
        // if attack in progress, stay attacking
        if (am.Attacking)
        {
            SetBehaviour(Behaviour.Attacking);
            return;
        }

        // If player is close enough
        if (distToPlayer < AttackRange)
        {
            // Attack
            SetBehaviour(Behaviour.Attacking);
        }
        // Else
        else
        {
            // Move closer
            SetBehaviour(Behaviour.Chasing);
        }        
    }
    private void Act()
    {
        switch (currentBehaviour)
        {
            case Behaviour.Idle:
                break;

            case Behaviour.Attacking:
                if (!am.Attacking)
                {
                    an.PlayAttack();
                    am.Attack();
                    StartCoroutine(ApplyLeapVelocity());
                }
                break;

            case Behaviour.Chasing:
                if (playerToLeft)
                {
                    mc.InputMotion = -1;
                }
                else
                {
                    mc.InputMotion = 1;
                }
                break;

            case Behaviour.Hit:
                mc.SetInputOverride(true);
                break;

            default:
                break;
        }
    }
    private void FacePlayer()
    {
        if (playerToLeft)
        {
            transform.localScale = new Vector3(1, transform.localScale.y, 1);
        }
        else
        {
            transform.localScale = new Vector3(-1, transform.localScale.y, 1);
        }
    }
    private void SetBehaviour(Behaviour newBehaviour)
    {
        if (newBehaviour == currentBehaviour) return;

        //Debug.Log(name + " changed behaviour state from " + currentBehaviour + " to " + newBehaviour);
        currentBehaviour = newBehaviour;
    }
    private IEnumerator ApplyLeapVelocity()
    {
        yield return new WaitForSeconds(Sierra.Utility.FramesToSeconds(am.Attacks[0].Startup));

        mc.Impulse(new Vector2(0, LeapHeight));

        var frameCount = am.Attacks[0].Startup;
    }
}