using UnityEngine;
using UnityEngine.Windows.Speech;
using System.Collections.Generic;
using System.Linq;


public class VoiceCommandListener : MonoBehaviour
{
    public PlayerInputHandler playerInput;

    private KeywordRecognizer keywordRecognizer;
    private Dictionary<string, System.Action> voiceActions;

    void Start()
    {
        voiceActions = new Dictionary<string, System.Action>
        {
            { "forward", () => playerInput.voiceForward = true },
            { "back", () => playerInput.voiceBack = true },
            { "left", () => playerInput.voiceLeft = true },
            { "right", () => playerInput.voiceRight = true },
        };

        keywordRecognizer = new KeywordRecognizer(voiceActions.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += OnVoiceCommand;
        keywordRecognizer.Start();
    }

    private void OnVoiceCommand(PhraseRecognizedEventArgs args)
    {
        if (voiceActions.TryGetValue(args.text, out var action))
        {
            action.Invoke();
            Debug.Log($"Voice command recognized: {args.text}");
        }
    }

    private void OnApplicationQuit()
    {
        if (keywordRecognizer != null && keywordRecognizer.IsRunning)
        {
            keywordRecognizer.Stop();
            keywordRecognizer.Dispose();
        }
    }
}
