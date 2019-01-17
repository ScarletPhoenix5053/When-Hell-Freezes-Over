﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public Sprite beforeReached;
    public Sprite afterReached;

    private SpriteRenderer spriteRenderer;

    public bool checkpointReached;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            spriteRenderer.sprite = afterReached;
            checkpointReached = true;
        }
    }
}
