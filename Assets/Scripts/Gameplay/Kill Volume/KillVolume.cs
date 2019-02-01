using System;
using UnityEngine;

public class KillVolume : MonoBehaviour
{
    public bool DrawVolume;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (Math.Pow(2, other.gameObject.layer) == LayerMask.GetMask("Player"))
        {
            var plr = other.GetComponent<PlayerController>();
            if (plr != null)
            {
                plr.GetComponent<Health>().Die();
            }
            else
            {
                throw new NullReferenceException("Gameobject " + other.name + " does not contain a player controller. Why is it on layer \"Player\"?");
            }
        }
        else if (Math.Pow(2, other.gameObject.layer) == LayerMask.GetMask("Enemy"))
        {
            var enm = other.GetComponent<EnemyController>();
            if (enm != null)
            {

                enm.Die();
            }
            else
            {
                throw new NullReferenceException("Gameobject " + other.name + " does not contain an enemy controller. Why is it on layer \"Enemy\"?");
            }
        }
        else if (Math.Pow(2, other.gameObject.layer) != LayerMask.GetMask("Environment"))
        {
            Destroy(other.gameObject);
        }
    }
    private void OnDrawGizmos()
    {
        if (DrawVolume)
        {
            var col = GetComponent<BoxCollider2D>();

            Gizmos.color = new Color(0.8f, 0, 0, 0.25f);
            Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.localScale);

            // Drawcube takes the full size, not halfextents
            Gizmos.DrawCube(Vector3.zero, new Vector3(col.size.x, col.size.y, 1));
        }
    }
}
