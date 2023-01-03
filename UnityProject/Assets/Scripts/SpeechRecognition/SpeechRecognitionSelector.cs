using Assets.Scripts.SpeechRecognition;
using KKSpeech;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeechRecognitionSelector : MonoBehaviour
{
    public static Platforms platform;
    ISpeechRecognition recognizer;
    GameObject recognizerObject;

    public ResultCallback onPartialResults = new();
    public ResultCallback onFinalResults = new();

    void Awake()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        platform = Platforms.Android;
#endif

#if UNITY_EDITOR
        platform = Platforms.Editor;
#endif

#if UNITY_STANDALONE_WIN
        platform = Platforms.Windows;
#endif

#if UNITY_IOS && !UNITY_EDITOR
        platform = Platforms.IOS;
#endif

        if (platform == Platforms.Android || platform == Platforms.IOS)
        {
            recognizer = transform.GetComponentInChildren<MobileSpeechRecognizerListener>(true);
            recognizerObject = transform.GetComponentInChildren<MobileSpeechRecognizerListener>(true).gameObject;
        }
        else if (platform == Platforms.Windows || platform == Platforms.Editor)
        {
            recognizer = transform.GetComponentInChildren<WindowsSpeechRecognition>(true);
            recognizerObject = transform.GetComponentInChildren<WindowsSpeechRecognition>(true).gameObject;
        }
        recognizerObject.SetActive(true);
        recognizer.Initialize(onFinalResults, onPartialResults);
    }
}


public enum Platforms
{
    Android,
    IOS,
    Editor,
    Windows,
    Linux,
    WebGL
}