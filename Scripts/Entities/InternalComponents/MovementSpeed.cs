using System;
using UnityEngine;

namespace Entities.InternalComponents
{
    [Serializable]
    public class MovementSpeed
    {
        [SerializeField] private float m_defaultSpeed;
        [SerializeField] private float m_crouchedSpeedMultiplier;
        [SerializeField] private float m_sprintSpeedMultiplier;

        private bool m_isCrouching = false;
        private bool m_isSprinting = false;
        public bool IsCrouching => m_isCrouching;
        public bool IsSprinting => m_isSprinting;

        public float CalculatedSpeed
        {
            get
            {
                if (m_isCrouching)
                    return m_defaultSpeed * m_crouchedSpeedMultiplier;
                if (m_isSprinting)
                    return m_defaultSpeed * m_sprintSpeedMultiplier;
                return m_defaultSpeed;
            }
        }

        public void SetCrouching(bool isCrouching)
        {
            m_isCrouching = isCrouching;
            if (m_isCrouching)
            {
                m_isSprinting = false;
            }
        }
        public void SetSprinting(bool isSprinting)
        {
            m_isSprinting = isSprinting;
            if (m_isSprinting)
            {
                m_isCrouching = false;
            }
        }
    }
}