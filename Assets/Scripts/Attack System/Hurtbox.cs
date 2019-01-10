using UnityEngine;
using Sierra;
using System;
using System.Collections;

namespace Tutorial.NahuelG_Fighter
{
    public class Hurtbox : MonoBehaviour
    {
        public Health Health;
        // Colldier array/list instead?
        public Collider2D ColliderMain;
        public Colours ColliderColour = new Colours();

        protected State _state = State.Active;
        protected IHitboxResponder _sender = null;

        public enum State { Inactive, Active }
        [Serializable]
        public class Colours
        {
            public Color Inactive = new Color(0.5f, 0.5f, 0.5f, 0.25f);
            public Color Active = new Color(0, 0.8f, 0, 0.25f);
            public Color Blocking = new Color(0.2f, 0, 0.8f, 0.75f);
            public Color ThrowInvulnerable = new Color(0.8f, 0.7f, 0.05f, 0.25f);
        }
        
        protected void OnDrawGizmos()
        {
            SetGizmoColor();
            Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.localScale);

            // Drawcube takes the full size, not halfextents
            Gizmos.DrawCube(
                Vector3.zero,
                new Vector3(
                    ColliderMain.bounds.extents.x * 2,
                    ColliderMain.bounds.extents.y * 2,
                    2
                    ));
        }
        
        public bool CheckHit(int blockStunFrames, int hitStunFrames)
        {
            if (_state != State.Inactive)
            {
                StopCoroutine("HitStun");
                StartCoroutine(HitStun(hitStunFrames));
                return true;
            }
            else
            {
                Debug.Log("Blocked!");
                return false;
            }
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