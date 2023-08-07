using Entities.NPC.ExternalComponents.Actions;
using UnityEngine;

namespace Dialogue
{
    public class SpeechBubble : MonoBehaviour
    {
        [SerializeField] private Speaker m_speaker;
        public Speaker Speaker => m_speaker;
        [SerializeField] private TMPro.TMP_Text m_text;
        [SerializeField] private Vector2 m_dimensions;
        [SerializeField] private RectTransform m_bubblePanel;
        [SerializeField] private RectTransform m_bubbleIndicator;

        private float m_easingTimer = 0f;
        private float m_easingTime = 0.5f;
        private EasingCore.EasingFunction m_easingFunc;

        private void OnValidate()
        {
            m_dimensions = m_bubblePanel.sizeDelta;
        }

        private void OnEnable()
        {
            transform.localScale = new Vector3();
            m_easingTimer = 0;
            m_easingFunc = EasingCore.Easing.Get(EasingCore.Ease.InOutCubic);
        }

        private void Update()
        {
            if (m_easingTimer < m_easingTime)
            {
                m_easingTimer += Time.deltaTime;
                float ease = m_easingFunc(1 / m_easingTime * m_easingTimer);
                transform.localScale = new Vector3(ease, ease, ease);
            } else
            {
                transform.localScale = new Vector3(1f, 1f, 1f);
            }
        }

        public void SetSpeaker(Speaker speaker)
        {
            m_speaker = speaker;
        }
        public void SetText(string text)
        {
            m_text.text = text;
        }

        public void SetScreenPosition(Vector3 pos, bool noLerp = false)
        {
            if (pos.x - m_dimensions.x * m_bubblePanel.pivot.x < 0)
                pos.x = m_dimensions.x * m_bubblePanel.pivot.x;
            if (pos.x + m_dimensions.x * m_bubblePanel.pivot.x > Screen.width)
                pos.x = Screen.width - m_dimensions.x * m_bubblePanel.pivot.x;
            if (pos.y - m_dimensions.y * (m_bubblePanel.pivot.y) < 0)
                pos.y = m_dimensions.y * (m_bubblePanel.pivot.y);
            if (pos.y + m_dimensions.y * (1 - m_bubblePanel.pivot.y) > Screen.height)
                pos.y = Screen.height - m_dimensions.y * (1 - m_bubblePanel.pivot.y);

            if (noLerp)
            {
                transform.position = pos;
                m_bubbleIndicator.anchoredPosition = new Vector2(pos.x - transform.position.x, m_bubbleIndicator.anchoredPosition.y);
            } else
            {
                transform.position = Vector3.Lerp(transform.position, pos, Time.deltaTime * 20f);
                m_bubbleIndicator.anchoredPosition = new Vector2(pos.x - transform.position.x, m_bubbleIndicator.anchoredPosition.y);
            }
        }
    }
}
