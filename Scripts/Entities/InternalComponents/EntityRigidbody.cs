using UnityEngine;

namespace Entities.InternalComponents
{
    public class EntityRigidbody
    {
        private readonly Rigidbody m_rigidbody;
        private readonly MovementSpeed m_movementSpeed;
        private readonly Transform m_visualsTransform;
        
        private const float MassInKg = 70f;
        private const float Drag = 50f;
        private const float RotationLookUpdateThreshold = 0.3f;

        private readonly bool m_rotateTowardsMovementDirection = false;

        public Vector3 Velocity => m_rigidbody.velocity;
        public EntityRigidbody(GameObject gameObject, MovementSpeed movementSpeed, Transform visuals = null)
        {
            m_rigidbody = gameObject.AddComponent<Rigidbody>();
            m_movementSpeed = movementSpeed;
            m_visualsTransform = visuals;

            m_rotateTowardsMovementDirection = visuals != null;
        }
        
        public void ApplyMovement(Vector3 direction)
        {
            if (m_rigidbody.velocity.sqrMagnitude > m_movementSpeed.CalculatedSpeed)
            {
                return;
            }
            direction.y = 0;
            direction.Normalize();

            direction *= m_movementSpeed.CalculatedSpeed * 2;
            
            
            m_rigidbody.velocity += direction * Time.deltaTime;
        }

        public void Setup()
        {
            m_rigidbody.freezeRotation = true;
            m_rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
            m_rigidbody.mass = MassInKg;
            m_rigidbody.drag = 0;
            m_rigidbody.angularDrag = 0;
            m_rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
        }

        private void TurnTowardsRotation()
        {
            var velocityNoUp = Vector3.Scale(new Vector3(1f, 0, 1f), Velocity);
            var velocitySoftUp = Vector3.Scale(new Vector3(1f, -0.1f, 1f), Velocity);

            var vel = velocityNoUp.magnitude;
            if (vel > RotationLookUpdateThreshold)
            {
                m_visualsTransform.LookAt(
                    m_rigidbody.gameObject.transform.position + velocitySoftUp,
                    Vector3.up
                );
            }
        }
        

        public void Update()
        {
            Vector3 velocity = m_rigidbody.velocity;
            
            Vector3 newVelocity = velocity - (Drag * velocity / MassInKg) * Time.deltaTime;
            // We don't want drag to affect gravity
            newVelocity.y = velocity.y;

            m_rigidbody.velocity = newVelocity;

            if (m_rotateTowardsMovementDirection)
            {
                TurnTowardsRotation();
            }
        }
    }
}