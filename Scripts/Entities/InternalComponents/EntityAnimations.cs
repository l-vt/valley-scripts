using System;
using UnityEngine;

namespace Entities.InternalComponents
{
    [Serializable]
    public class EntityAnimations
    {
        private MovementSpeed m_movementSpeed;
        private EntityRigidbody m_rigidbody;

        [SerializeReference] private Animator m_animator = null;

        private bool m_disabled = false;

        // hashes are more memory efficient
        private static readonly int StrVelocity = Animator.StringToHash("Velocity");
        private static readonly int StrIsCrouching = Animator.StringToHash("IsCrouching");
        private static readonly int StrIsSprinting = Animator.StringToHash("IsSprinting");

        public void Initialize(EntityRigidbody rigidbody, MovementSpeed movementSpeed)
        {
            m_rigidbody = rigidbody;
            m_movementSpeed = movementSpeed;

            if (m_animator == null)
            {
                m_disabled = true;
            }
        }

        public void Update()
        {
            if (m_disabled)
            {
                return;
            }
            Vector3 velocityNoUp = Vector3.Scale(new Vector3(1f, 0, 1f), m_rigidbody.Velocity);

            var speed = velocityNoUp.magnitude;
            
            m_animator.SetFloat(StrVelocity, speed);
            m_animator.SetBool(StrIsCrouching, m_movementSpeed.IsCrouching);
            m_animator.SetBool(StrIsSprinting, m_movementSpeed.IsSprinting);
        }
    }
}