using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
public class Door : MonoBehaviour
{
    private Collider2D col;
    private SpriteRenderer sr;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
    }
    public void Open()
    {
        col.enabled = false;
        if (sr != null) sr.enabled = false;
    }
    public void Close()
    {
        col.enabled = true;
        if (sr != null) sr.enabled = true;
    }
}
