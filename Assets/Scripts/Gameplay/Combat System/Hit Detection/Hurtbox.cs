using UnityEngine;
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

        public State CurrentState = State.Vulnerable;
        protected IHitboxResponder caller = null;

        public enum State { Inactive, Vulnerable, Critical, Blocking, Armored}
        /// <summary>
        /// Containter class for hurtbox draw colours
        /// </summary>
        [Serializable]
        public class Colours
        {
            public Color Inactive = new Color(0.5f, 0.5f, 0.5f, 0.25f);
            public Color Vulnerable = new Color(0, 0.8f, 0, 0.25f);
            public Color Critical = new Color(0.8f, 1, 0, 0.25f);
            public Color Blocking = new Color(0, 0, 1, 0.25f);
            public Color Armored = new Color(1, 1, 0, 0.25f);
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

        public virtual State GetHurtboxState()
        {
            return CurrentState;
        }

        public virtual bool CheckHit()
        {
            if (CurrentState != State.Inactive)
            {
                if (CurrentState == State.Blocking)
                {
                    Debug.Log("Blocked");
                    return false;
                }
                return true;
            }
            else
            {
                Debug.Log(name + " is inactive!");
                return false;
            }
        }
        public virtual bool CheckHit(out bool criticalHit, out bool tanked)
        {
            criticalHit = false;
            tanked = false;
            return false;
        }
        public virtual void SetState(State newState)
        {
            if (newState != CurrentState) CurrentState = newState;
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
            switch (CurrentState)
            {
                case State.Inactive:
                    Gizmos.color = ColliderColour.Inactive;
                    break;

                case State.Vulnerable:
                    Gizmos.color = ColliderColour.Vulnerable;
                    break;

                case State.Critical:
                    Gizmos.color = ColliderColour.Critical;
                    break;

                case State.Blocking:
                    Gizmos.color = ColliderColour.Blocking;
                    break;

                case State.Armored:
                    Gizmos.color = ColliderColour.Armored;
                    break;
            }
        }
    }
}