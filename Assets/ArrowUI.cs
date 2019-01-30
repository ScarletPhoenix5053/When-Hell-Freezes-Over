using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArrowUI : MonoBehaviour
{
    PlayerAttackManager pam;
    public Text arrowText;

    void Start()
    {
        pam = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAttackManager>();
    }

    // Update is called once per frame
    void Update()
    {
        arrowText.text = pam.Arrows.ToString();
    }
}
