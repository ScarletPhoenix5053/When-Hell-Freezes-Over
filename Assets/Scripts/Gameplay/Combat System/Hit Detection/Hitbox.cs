using System;
using UnityEngine;

namespace Sierra.Combat2D
{
    public class Hitbox : MonoBehaviour
    {
        public Vector3 Size;
        public Quaternion Rotation = Quaternion.identity;
        public LayerMask LayerMask;
        public Colours BoxColour = new Colours();

        protected State _state = State.Inactive;
        protected IHitboxResponder responder = null;

        public enum State { Inactive, Active, Colliding }
        public enum Shape { Box, Sphere }
        [Serializable]
        public class Colours
        {
            public Color Inactive = new Color(0.5f, 0.5f, 0.5f, 0.25f);
            public Color Active = new Color(0.8f, 0, 0, 0.25f);
            public Color Colliding = new Color(1, 0, 0, 0.75f);
        }

        // Can use OnDrawGizmosSelected to only render hitboxes when selected
        protected void OnDrawGizmos()
        {
            SetGizmoColor();
            
            Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.localScale);

            // Drawcube takes the full size, not halfextents
            Gizmos.DrawCube(Vector3.zero, new Vector3(Size.x, Size.y, Size.z));

        }

        /// <summary>
        /// Check if the hitbox is overlapping a hurtbox
        /// </summary>
        public void CheckCollision()
        {
            // Cancel if Inactive
            if (_state == State.Inactive) { return; }

            // Check Hitbox
            Collider2D[] colliders = GetOverlappingColliders();

            if (colliders.Length > 0)
            {
                _state = State.Colliding;
                Debug.Log("We hit something");
            }
            else
            {
                _state = State.Active;
            }
        }
        /// <summary>
        /// When state is not <see cref="State.Inactive"/>, checks for an overlap with a hurtbox and calls CollidedWith in <see cref="responder"/>.
        /// </summary>
        public void UpdateHitbox()
        {
            // Cancel if Inactive
            if (_state == State.Inactive) { return; }

            // Check Hitbox
            Collider2D[] colliders = GetOverlappingColliders();

            // Perform interaction on hit
            for (int i = 0; i < colliders.Length; i++)
            {
                Collider2D collider = colliders[i];
                responder?.Hit(collider);
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
        /// Set this to the object expecting a response from the hitbox.
        /// </summary>
        /// <param name="responder"></param>
        public void SetResponder(IHitboxResponder responder)
        {
            this.responder = responder; 
        }

        /// <summary>
        /// Sets <see cref="Gizmos.color"/> based on the hitboxes state.
        /// </summary>
        protected void SetGizmoColor()
        {
            switch (_state)
            {
                case State.Inactive:
                    Gizmos.color = BoxColour.Inactive;
                    break;

                case State.Active:
                    Gizmos.color = BoxColour.Active;
                    break;

                case State.Colliding:
                    Gizmos.color = BoxColour.Colliding;
                    break;
            }
        }
        protected Collider2D[] GetOverlappingColliders()
        {
            var size = new Vector3(Size.x, Size.y, Size.z);
            return Physics2D.OverlapBoxAll(transform.position, size, 0, LayerMask);
        }
    }
    public interface IHitboxResponder
    {
        void Hit(Collider2D collider);
    }
}