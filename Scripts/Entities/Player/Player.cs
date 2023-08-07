using System;
using Entities.InternalComponents;
using Entities.Player.InternalComponents;
using Environment;
using UnityEngine;
using UnityEngine.Serialization;
using Utils;

namespace Entities.Player
{
    public class Player : Entity
    {
        #region References
        [Header("Player References")]
        [SerializeField] private TriggerBox<Interactable> m_interactableTrigger;
        private UnityEngine.Camera m_camera;
        #endregion
        
        #region Internal Components
        private QuestStore m_questStore;
        public QuestStore QuestStore => m_questStore;
        #endregion

        #region Movement Input

        private const KeyCode KeyBindCrouch = KeyCode.C; 
        private const KeyCode KeyBindSprint = KeyCode.LeftShift; 
        private const KeyCode KeyBindInteract = KeyCode.E; 
        
        private Vector3 m_direction;
        private Vector3 m_forward;
        private Vector3 m_sidewards;
        #endregion
        
        private Reactive<Interactable> m_interactable = new Reactive<Interactable>(null);
        public Reactive<Interactable> CurrentInteractable => m_interactable;

        protected override void EntityAwake()
        {
            m_questStore ??= new QuestStore();
            
            m_interactableTrigger.OnEnter.AddListener(SetOption);
            m_interactableTrigger.OnExit.AddListener(RemoveOption);
        }

        private void Start()
        {
            m_camera = GameManager.Instance.GameCamera.ViewCamera;
        }

        private void Update()
        {
            HandleInput();
            HandleMovement();
            
            Rigidbody.Update();
            Animations.Update();
        }

        private void HandleMovement()
        {
            m_forward = m_camera.transform.forward;
            m_forward.y = 0;
            m_forward.Normalize();
            m_forward *= m_direction.z;

            m_sidewards = m_camera.transform.right;
            m_sidewards.y = 0;
            m_sidewards.Normalize();
            m_sidewards *= m_direction.x;
            
            Rigidbody.ApplyMovement(m_forward + m_sidewards);
        }

        private void OnDestroy()
        {
            m_interactableTrigger.OnEnter.RemoveListener(SetOption);
            m_interactableTrigger.OnExit.RemoveListener(RemoveOption);
        }

        private void SetOption(Interactable i)
        {
            m_interactable.Value = i;
        }

        private void RemoveOption(Interactable i)
        {
            if (m_interactable.Value == i) m_interactable.Value = null;
        }

        private void HandleInput()
        {
            // Moving
            int vertical = (Input.GetKey(KeyCode.W) ? 1 : 0) - (Input.GetKey(KeyCode.S) ? 1 : 0);
            int horizontal = (Input.GetKey(KeyCode.D) ? 1 : 0) - (Input.GetKey(KeyCode.A) ? 1 : 0);
            m_direction = new Vector3(horizontal, 0, vertical).normalized;

            // Crouching
            if (Input.GetKeyDown(KeyBindCrouch))
            {
                MovementSpeed.SetCrouching(!MovementSpeed.IsCrouching);
            }
            
            // Sprinting
            MovementSpeed.SetSprinting(Input.GetKey(KeyBindSprint));

            // Interacting
            if (
                Input.GetKeyDown(KeyBindInteract) &&
                m_interactable.Value != null
            )
            {
                m_interactable.Value.Execute(this);
                RemoveOption(m_interactable.Value);
            }
        }
    }
}