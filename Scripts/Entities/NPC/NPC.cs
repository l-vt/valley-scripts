using System;
using Entities.InternalComponents;
using Entities.NPC.InternalComponents;
using Entities.NPC.States;
using Environment;
using UnityEngine;
using Utils;
using Utils.StateMachine;

namespace Entities.NPC
{
    public abstract class NPC : Entity
    {
        #region References
        [Header("NPC References")]
        [SerializeField] private TriggerBox<Entity> m_visionTrigger;
        public TriggerBox<Entity> Trigger => m_visionTrigger;
        #endregion
        
        #region Internal Components
        [Header("NPC Components")]
        [SerializeField] private Interacting m_interacting;
        [SerializeField] private Pathfinding m_pathfinding;
        public Pathfinding Pathfinding => m_pathfinding;
        public Interacting Interacting => m_interacting;
        #endregion
        
        private StateMachine<NPC> m_stateMachine;

        protected override void EntityAwake()
        {
            m_pathfinding = new Pathfinding(this);
        }

        private void Start()
        {
            InitializeStates(ref m_stateMachine);
        }
        
        protected abstract void InitializeStates(ref StateMachine<NPC> stateMachine);
        
        public void PerformAction(Player.Player player)
        {
            Interacting.PerformAction(player);
        }

        private void Update()
        {
            m_stateMachine.Update();
            Pathfinding.Update();
            Rigidbody.Update();
            Animations.Update();
        }
    }
}