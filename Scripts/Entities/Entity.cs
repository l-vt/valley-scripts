using Entities.InternalComponents;
using UnityEngine;

namespace Entities
{
    public abstract class Entity : MonoBehaviour
    {
        #region Info
        [Header("Info")]
        [SerializeField] private string m_displayName;
        public string DisplayName => m_displayName;
        #endregion
        
        #region Dimensions
        [Header("Dimensions")]
        [SerializeField] private Vector3 m_eyeOffset = new Vector3(0, 1.7f, 0);
        [SerializeField] private float m_radius = 0.5f;
        public Vector3 EyeOffset => m_eyeOffset;
        public Vector3 AbsoluteEyePosition => transform.position + m_eyeOffset;
        public float Radius => m_radius;
        public float Diameter => m_radius * 2;
        #endregion
        
        #region References
        [Header("References")]
        [SerializeField] private Transform m_visualsTransform;
        public Transform VisualsTransform => m_visualsTransform;
        #endregion
        
        #region Components
        [Header("Components")]
        [SerializeField] private MovementSpeed m_movementSpeed;
        [SerializeField] private EntityAnimations m_animations = new EntityAnimations();
        private EntityRigidbody m_rigidbody;
        public EntityAnimations Animations => m_animations;
        public EntityRigidbody Rigidbody => m_rigidbody;
        public MovementSpeed MovementSpeed => m_movementSpeed;
        #endregion
        
        private void Awake()
        {
            m_rigidbody = new EntityRigidbody(gameObject, m_movementSpeed, m_visualsTransform);
            m_rigidbody.Setup();
            
            m_animations.Initialize(m_rigidbody, m_movementSpeed);

            EntityAwake();
        }

        protected abstract void EntityAwake();
    }
}