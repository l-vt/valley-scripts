using System;
using Camera;
using Dialogue;
using Dialogue.ScriptableObjects;
using UnityEngine;

namespace Entities.NPC.ExternalComponents.Actions
{
    public class Speaker : Action
    {
        [SerializeField] private DialogueObject m_dialogueObject;
        private Func<Player.Player, int> m_callback;
        private int m_currentDialogue;

        private DialogueObject m_dialogueInstance;
        private bool m_dialogueStarted;
        private Player.Player m_player;
        private SpeechBubble m_speechBubble;

        private void Update()
        {
            if (!m_dialogueStarted) return;
            if (Input.GetKeyDown(KeyCode.E))
            {
                m_currentDialogue++;
                if (m_currentDialogue >= m_dialogueInstance.Sentences.Count)
                {
                    FinishDialogue();
                    return;
                }

                SetText(m_dialogueInstance.Sentences[m_currentDialogue].Text);
            }
        }

        public void OnDisable()
        {
            RemoveSpeechBubble();
        }

        public void RemoveSpeechBubble(bool playAnimation = false)
        {
            if (m_speechBubble == null) return;
            GameManager.Instance.RemoveSpeechBubble(m_speechBubble);
            m_speechBubble = null;
        }

        public override void PerformAction(Player.Player player, Func<Player.Player, int> callback)
        {
            GameManager.Instance.SetGameMode(GameManager.GameMode.Talking);
            GameManager.Instance.GameCamera.AddTarget(transform, GameCamera.ZoomLevel.Close);

            m_dialogueStarted = true;
            m_currentDialogue = 0;
            m_player = player;
            m_callback = callback;

            m_dialogueInstance = Instantiate(m_dialogueObject);

            if (m_dialogueInstance.Sentences.Count > 0)
                SetText(m_dialogueInstance.Sentences[0].Text);
            else
                FinishDialogue();
        }

        public void SetText(string text)
        {
            if (m_speechBubble == null) m_speechBubble = GameManager.Instance.AddSpeechBubble(this);

            m_speechBubble.SetText(text);
        }

        private void FinishDialogue()
        {
            m_dialogueStarted = false;
            m_callback(m_player);
            GameManager.Instance.GameCamera.RemoveTarget(transform);
            RemoveSpeechBubble(true);
        }
    }
}