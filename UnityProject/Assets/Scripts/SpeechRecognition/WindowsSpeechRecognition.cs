using Assets.Scripts.SpeechRecognition;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Windows.Speech;

public class WindowsSpeechRecognition : MonoBehaviour, ISpeechRecognition, IDisposable
{
    public ResultCallback onPartialResults = new();
    public ResultCallback onFinalResults = new();

    protected DictationRecognizer dictationRecognizer;

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
            //dictationRecognizer.DictationComplete -= DictationRecognizer_OnDictationComplete;
            dictationRecognizer.DictationResult -= DictationRecognizer_OnDictationResult;
            //dictationRecognizer.DictationError -= DictationRecognizer_OnDictationError;
            if (dictationRecognizer.Status == SpeechSystemStatus.Running)
            {
                dictationRecognizer.Stop();
            }
            dictationRecognizer.Dispose();
        }
    }
}

[System.Serializable]
public class ResultCallback : UnityEvent<string> { };