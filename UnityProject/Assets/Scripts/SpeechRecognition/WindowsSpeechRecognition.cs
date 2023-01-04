using Assets.Scripts.SpeechRecognition;
using System;
using UnityEngine;
using UnityEngine.Events;
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
using UnityEngine.Windows.Speech;
#endif

public class WindowsSpeechRecognition : MonoBehaviour, ISpeechRecognition, IDisposable
{
    public static WindowsSpeechRecognition Instance;

    public ResultCallback onPartialResults = new();
    public ResultCallback onFinalResults = new();

#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
    protected DictationRecognizer dictationRecognizer;
#endif

    private void OnEnable()
    {
        if (Instance == null) Instance = this; else Destroy(this);
    }

    public void Initialize(ResultCallback sentenceComplete, ResultCallback wordComplete)
    {
        onPartialResults = wordComplete;
        onFinalResults = sentenceComplete;

        StartDictationEngine();
    }

    private void DictationRecognizer_OnDictationHypothesis(string text)
    {
        onPartialResults.Invoke(text);
    }

#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
    private void DictationRecognizer_OnDictationResult(string text, ConfidenceLevel confidence)
    {
        onFinalResults.Invoke(text);
    }
#endif

    private void StartDictationEngine()
    {
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
        dictationRecognizer = new DictationRecognizer();
        dictationRecognizer.DictationHypothesis += DictationRecognizer_OnDictationHypothesis;
        dictationRecognizer.DictationResult += DictationRecognizer_OnDictationResult;
        //dictationRecognizer.DictationComplete += DictationRecognizer_OnDictationComplete;
        //dictationRecognizer.DictationError += DictationRecognizer_OnDictationError;
        dictationRecognizer.Start();
#endif
    }

    public void Dispose()
    {
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
        if (dictationRecognizer != null)
        {
            dictationRecognizer.DictationHypothesis -= DictationRecognizer_OnDictationHypothesis;
            dictationRecognizer.DictationResult -= DictationRecognizer_OnDictationResult;
            if (dictationRecognizer.Status == SpeechSystemStatus.Running)
            {
                dictationRecognizer.Stop();
            }
            dictationRecognizer.Dispose();
        }
#endif
    }

    public void Pause()
    {
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
        dictationRecognizer?.Stop();
#endif
    }

    public void Resume()
    {
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
        dictationRecognizer?.Start();
#endif
    }
}

[System.Serializable]
public class ResultCallback : UnityEvent<string> { };