using Assets.Scripts.SpeechRecognition;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
using UnityEngine.Windows.Speech;
#endif

public class WindowsSpeechRecognition : MonoBehaviour, ISpeechRecognition, IDisposable
{
    public static WindowsSpeechRecognition Instance;

    public ResultCallback onPartialResults = new();
    public ResultCallback onFinalResults = new();

    public Text speechRecognitionStatusText;

    bool starting = true;
    bool running = false;

#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
    protected DictationRecognizer dictationRecognizer;
    //KeywordRecognizer keywordRecognizer;
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

    private void Update()
    {
        if (speechRecognitionStatusText != null) speechRecognitionStatusText.text = "Status: " + dictationRecognizer.Status.ToString();
        if (starting && dictationRecognizer.Status == SpeechSystemStatus.Running)
            starting = false;

        if ((running && dictationRecognizer.Status == SpeechSystemStatus.Failed) || (running == true && starting == false && (Input.GetKeyDown(KeyCode.R) || dictationRecognizer.Status == SpeechSystemStatus.Stopped)))
        {
            print(running + " " + starting);
            StartCoroutine(RestartSpeechRecognition());
        }
        //print(dictationRecognizer.Status);
    }

    IEnumerator RestartSpeechRecognition()
    {
        starting = true;
        Dispose();
        yield return new WaitForSeconds(2);
        StartDictationEngine();
        print("Reinitialized.");
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
        dictationRecognizer.DictationError += (error, hresult) =>
        {
            Debug.LogErrorFormat("Dictation error: {0}; HResult = {1}.", error, hresult);
        };
        dictationRecognizer.AutoSilenceTimeoutSeconds = 600;
        dictationRecognizer.InitialSilenceTimeoutSeconds = 600;
        dictationRecognizer.Start();
        running = true;
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
        running = false;
#endif
    }

    public void Resume()
    {
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
        if (dictationRecognizer.Status != SpeechSystemStatus.Running)
            dictationRecognizer?.Start();
        running = true;
        starting = true;
#endif
    }
}

[System.Serializable]
public class ResultCallback : UnityEvent<string> { };