using Assets.Scripts.SpeechRecognition;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Windows.Speech;

public class WindowsSpeechRecognition : MonoBehaviour, ISpeechRecognition, IDisposable
{
    public static WindowsSpeechRecognition Instance;

    public ResultCallback onPartialResults = new();
    public ResultCallback onFinalResults = new();

    protected DictationRecognizer dictationRecognizer;

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

    private void DictationRecognizer_OnDictationResult(string text, ConfidenceLevel confidence)
    {
        onFinalResults.Invoke(text);
    }

    private void StartDictationEngine()
    {
        dictationRecognizer = new DictationRecognizer();
        dictationRecognizer.DictationHypothesis += DictationRecognizer_OnDictationHypothesis;
        dictationRecognizer.DictationResult += DictationRecognizer_OnDictationResult;
        //dictationRecognizer.DictationComplete += DictationRecognizer_OnDictationComplete;
        //dictationRecognizer.DictationError += DictationRecognizer_OnDictationError;
        dictationRecognizer.Start();
    }

    public void Dispose()
    {
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
    }

    public void Pause()
    {
        dictationRecognizer?.Stop();
    }

    public void Resume()
    {
        dictationRecognizer?.Start();
    }
}

[System.Serializable]
public class ResultCallback : UnityEvent<string> { };