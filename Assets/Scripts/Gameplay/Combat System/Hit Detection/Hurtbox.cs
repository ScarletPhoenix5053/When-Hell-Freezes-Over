﻿using UnityEngine;
using Sierra;
using System;
using System.Collections;

namespace Sierra.Combat2D
{
    /// <summary>
    /// Interacts with <see cref="Hitbox"/>es to for hit detection.
    /// Must point to a <see cref="Health"/> component for attack system to work.
    /// </summary>
    [RequireComponent(typeof(Collider2D))]    
    public class Hurtbox : MonoBehaviour
    {
        public Health hp;
        public Collider2D Collider;
        public Colours ColliderColour = new Colours();
        public bool DrawGizmo = false;

        public State HurtboxState = State.Vulnerable;
        protected IHitboxResponder caller = null;

        public enum State { Inactive, Vulnerable, Critical, Blocking }
        /// <summary>
        /// Containter class for hurtbox draw colours
        /// </summary>
        [Serializable]
        public class Colours
        {
            public Color Inactive = new Color(0.5f, 0.5f, 0.5f, 0.25f);
            public Color Active = new Color(0, 0.8f, 0, 0.25f);
        }

        protected virtual void Reset()
        {
            Collider = GetComponent<Collider2D>();
        }
        protected virtual void OnDrawGizmos()
        {
            if (Collider == null || !DrawGizmo) return;

            SetGizmoColor();
            DrawHurtbox();
        }
        
        public virtual bool CheckHit(out bool criticalHit)
        {
            criticalHit = false;
            if (HurtboxState != State.Inactive)
            {
                if (HurtboxState == State.Blocking)
                {
                    Debug.Log("Blocked");
                    return false;
                }
                else if (HurtboxState == State.Critical)
                {
                    Debug.Log("Critical Hit!");
                    criticalHit = true;
                    return true;
                }
                return true;
            }
            else
            {
                Debug.Log(name + " is inactive!");
                return false;
            }
        }
        public void SetActive()
        {
            HurtboxState = State.Vulnerable;
        }
        public void SetInactive()
        {
            HurtboxState = State.Inactive;
        }

        protected void DrawHurtbox()
        {
            Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.localScale);

            // Drawcube takes the full size, not halfextents
            Gizmos.DrawCube(
                Vector3.zero,
                new Vector3(
                    Collider.bounds.extents.x * 2,
                    Collider.bounds.extents.y * 2,
                    2
                    ));
        }
        /// <summary>
        /// Sets <see cref="Gizmos.color"/> based on the hitboxes state.
        /// </summary>
        protected void SetGizmoColor()
        {
            switch (HurtboxState)
            {
                case State.Inactive:
                    Gizmos.color = ColliderColour.Inactive;
                    break;

                case State.Vulnerable:
                    Gizmos.color = ColliderColour.Active;
                    break;
            }
        }
    }
}