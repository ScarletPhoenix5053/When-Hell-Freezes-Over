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
        public bool DrawGizmo = false;

        protected State hbState = State.Inactive;
        protected IHitboxResponder responder = null;

        public enum State { Inactive, Active, Colliding }
        /// <summary>
        /// Containter class for hitbox draw colours
        /// </summary>
        [Serializable]
        public class Colours
        {
            public Color Inactive = new Color(0.5f, 0.5f, 0.5f, 0.25f);
            public Color Active = new Color(0.8f, 0, 0, 0.25f);
            public Color Colliding = new Color(1, 0, 0, 0.75f);
        }

        protected void OnDrawGizmos()
        {
            if (!DrawGizmo) return;

            SetGizmoColor();
            DrawHitBox();
        }

        #region Public Methods
        /// <summary>
        /// When state is not <see cref="State.Inactive"/>, checks for an overlap with a hurtbox and calls CollidedWith in <see cref="responder"/>.
        /// </summary>
        public virtual void UpdateHitbox()
        {
            // Cancel if Inactive
            if (hbState == State.Inactive) { return; }

            // Check Hitbox
            Collider2D[] colliders = GetOverlappingColliders();

            // Perform interaction on hit
            for (int i = 0; i < colliders.Length; i++)
            {
                responder?.Hit(colliders[i]);
            }
        }
        /// <summary>
        /// <see cref="UpdateHitbox"/> will call back to the responder set in this method if it overlaps with a hurtbox.
        /// </summary>
        /// <param name="responder"></param>
        public void SetResponder(IHitboxResponder responder)
        {
            this.responder = responder;
        }

        public virtual void SetActive()
        {
            hbState = State.Active;
        }
        public virtual void SetInactive()
        {
            hbState = State.Inactive;
        }
        #endregion
        #region Protected Methods
        protected void DrawHitBox()
        {
            Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.localScale);

            // Drawcube takes the full size, not halfextents
            Gizmos.DrawCube(Vector3.zero, new Vector3(Size.x, Size.y, Size.z));
        }
        /// <summary>
        /// Sets <see cref="Gizmos.color"/> based on the hitboxes state.
        /// </summary>
        protected void SetGizmoColor()
        {
            switch (hbState)
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
        /// <summary>
        /// Find any colliders within the hitbox
        /// </summary>
        /// <returns></returns>
        protected Collider2D[] GetOverlappingColliders()
        {
            var size = new Vector3(Size.x, Size.y, Size.z);
            return Physics2D.OverlapBoxAll(transform.position, size, 0, LayerMask);
        }
        #endregion
    }
    /// <summary>
    /// Interface for classes that respond to hitbox interactions
    /// </summary>
    public interface IHitboxResponder
    {
        void Hit(Collider2D collider);
    }
}