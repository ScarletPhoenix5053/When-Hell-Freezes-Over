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
        // Colldier array/list instead?
        public Collider2D col;
        public Colours ColliderColour = new Colours();

        protected State _state = State.Active;
        protected IHitboxResponder _sender = null;

        public enum State { Inactive, Active }
        [Serializable]
        public class Colours
        {
            public Color Inactive = new Color(0.5f, 0.5f, 0.5f, 0.25f);
            public Color Active = new Color(0, 0.8f, 0, 0.25f);
        }

        protected void Reset()
        {
            col = GetComponent<Collider2D>();
        }
        protected void Awake()
        {
            if (col == null) throw new NullReferenceException("This component needs a collider 2D attatched to " +
                "the same game object to work!");
            if (hp == null) throw new NullReferenceException("This component needs to refer to a health script for " +
                "attack system to work!");
        }
        protected void OnDrawGizmos()
        {
            // Make sure a collider is attatched
            if (col == null)
            {                
                return;
            }

            SetGizmoColor();
            Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.localScale);

            // Drawcube takes the full size, not halfextents
            Gizmos.DrawCube(
                Vector3.zero,
                new Vector3(
                    col.bounds.extents.x * 2,
                    col.bounds.extents.y * 2,
                    2
                    ));
        }
        
        public bool CheckHit(int hitStunFrames)
        {
            if (_state != State.Inactive)
            {
                StopCoroutine("HitStun");
                StartCoroutine(HitStun(hitStunFrames));
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
            _state = State.Active;
        }
        public void SetInactive()
        {
            _state = State.Inactive;
        }

        /// <summary>
        /// Sets <see cref="Gizmos.color"/> based on the hitboxes state.
        /// </summary>
        protected void SetGizmoColor()
        {
            switch (_state)
            {
                case State.Inactive:
                    Gizmos.color = ColliderColour.Inactive;
                    break;

                case State.Active:
                    Gizmos.color = ColliderColour.Active;
                    break;
            }
        }
        protected IEnumerator HitStun(int frameDuration)
        {
            var duration = Utility.FramesToSeconds(frameDuration);

            yield return new WaitForSeconds(duration);
            _state = State.Active;
        }
    }
}