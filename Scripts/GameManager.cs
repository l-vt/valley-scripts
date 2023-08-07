using System.Collections;
using System.Collections.Generic;
using Camera;
using Dialogue;
using Entities.NPC.ExternalComponents.Actions;
using UnityEngine;
using Utils;

public class GameManager : Singleton<GameManager>
{
    public enum GameMode
    {
        Walking,
        Talking,
        Driving,
        GameOver
    }

    [Header("Session")]
    [SerializeField] private Reactive<GameMode> m_currentGameMode = new Reactive<GameMode>(GameMode.Walking);
    public Reactive<GameMode> CurrentGameMode => m_currentGameMode;

    [Header("References")]
    [SerializeField] private GameCamera m_gameCamera;
    public GameCamera GameCamera => m_gameCamera;

    [SerializeField] private Transform m_speechBubbleContainer;
    public Transform SpeechBubbleContainer => m_speechBubbleContainer;
    [SerializeField] private List<SpeechBubble> m_speechBubbles = new List<SpeechBubble>();
    public List<SpeechBubble> SpeechBubbles => m_speechBubbles;

    [Header("Prefabs")]
    [SerializeField] private SpeechBubble m_prefabSpeechBubble;
    public SpeechBubble PrefabSpeechBubble => m_prefabSpeechBubble;

    private void OnValidate()
    {
        if (m_gameCamera == null)
        {
            m_gameCamera = FindObjectOfType<GameCamera>();
        }
    }

    public void SetGameMode(GameMode mode) {
        m_currentGameMode.Value = mode;
    }

    public void SetGameOver()
    {
        m_currentGameMode.Value = GameMode.GameOver;
    }

    public SpeechBubble AddSpeechBubble(Speaker speaker)
    {
        SpeechBubble bub = Instantiate(PrefabSpeechBubble);
        bub.transform.SetParent(SpeechBubbleContainer);
        bub.transform.position = new Vector3();
        bub.SetScreenPosition(m_gameCamera.ViewCamera.WorldToScreenPoint(speaker.transform.position + new Vector3(0, 2f, 0)), true);
        bub.SetSpeaker(speaker);
        SpeechBubbles.Add(bub);

        return bub;
    }
    public void RemoveSpeechBubble(SpeechBubble bub)
    {
        SpeechBubbles.Remove(bub);
        Destroy(bub.gameObject);
    }

    private void Update()
    {
        foreach (var bub in SpeechBubbles)
        {
            bub.SetScreenPosition(m_gameCamera.ViewCamera.WorldToScreenPoint(bub.Speaker.transform.position + new Vector3(0, 2f, 0)));
        }
    }
}
