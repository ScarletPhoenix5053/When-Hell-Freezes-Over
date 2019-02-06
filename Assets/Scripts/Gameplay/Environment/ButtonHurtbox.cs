using UnityEngine;
using Sierra;
using System;
using System.Collections;

namespace Sierra.Combat2D
{
    /// <summary>
    /// Interacts with <see cref="Hitbox"/>es to for hit detection.
    /// </summary>
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(EventRaiser))]
    public class ButtonHurtbox : Hurtbox
    {
        protected EventRaiser EventRaiser;

        protected void Awake()
        {
            EventRaiser = GetComponent<EventRaiser>();
        }

        public override bool CheckHit()
        {
            EventRaiser?.RaiseEvent();
            SetState(State.Inactive);
            return true;
        }
    }
}