using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using KKSpeech;
using UnityEngine.Events;
using Assets.Scripts.SpeechRecognition;
using System;

namespace KKSpeech
{
    public class MobileSpeechRecognizerListener : MonoBehaviour, ISpeechRecognition, IDisposable
    {
        public static MobileSpeechRecognizerListener Instance;

        [System.Serializable]
        public class AuthorizationCallback : UnityEvent<AuthorizationStatus> { };

        [System.Serializable]
        public class AvailabilityCallback : UnityEvent<bool> { };

        [System.Serializable]
        public class ErrorCallback : UnityEvent<string> { };

        [System.Serializable]
        public class SupportedLanguagesCallback : UnityEvent<List<LanguageOption>> { };

        public SupportedLanguagesCallback onSupportedLanguagesFetched = new SupportedLanguagesCallback();
        public AuthorizationCallback onAuthorizationStatusFetched = new AuthorizationCallback();
        public ResultCallback onPartialResults = new();
        public ResultCallback onFinalResults = new();
        // iOS-only
        public AvailabilityCallback onAvailabilityChanged = new AvailabilityCallback();
        public ErrorCallback onErrorDuringRecording = new ErrorCallback();
        public ErrorCallback onErrorOnStartRecording = new ErrorCallback();
        // Android-only
        public UnityEvent onEndOfSpeech = new UnityEvent();
        public UnityEvent onReadyForSpeech = new UnityEvent();

        private void OnEnable()
        {
            if (Instance == null) Instance = this; else Destroy(this);
        }

        void AvailabilityDidChange(string available)
        {
            Debug.Log("AvailabilityDidChange " + available);
            onAvailabilityChanged?.Invoke(available.Equals("1"));
        }

        void GotPartialResult(string result)
        {
            Debug.Log("GotPartialResult " + result);
            onPartialResults.Invoke(result);
        }

        void GotFinalResult(string result)
        {
            Debug.Log("GotFinalResult " + result);
            onFinalResults.Invoke(result);
        }

        void FailedToStartRecording(string reason)
        {
            Debug.Log("FailedToStartRecording " + reason);
            onErrorOnStartRecording?.Invoke(reason);
        }

        void FailedDuringRecording(string reason)
        {
            Debug.Log("FailedDuringRecording " + reason);
            onErrorDuringRecording?.Invoke(reason);
        }

        public void SupportedLanguagesFetched(string langs)
        {
            string[] components = langs.Split('|');

            List<LanguageOption> languageOptions = new List<LanguageOption>();
            foreach (string component in components)
            {
                string[] idAndName = component.Split('^');
                var option = new LanguageOption(idAndName[0], idAndName[1]);
                languageOptions.Add(option);
            }

            onSupportedLanguagesFetched.Invoke(languageOptions);
        }

        // Android-only
        void OnEndOfSpeech(string dummy)
        {
            Debug.Log("End Of Speech");
            onEndOfSpeech?.Invoke();
        }

        void OnReadyForSpeech(string dummy)
        {
            Debug.Log("Ready For Speech");
            onReadyForSpeech?.Invoke();
        }

        void AuthorizationStatusFetched(string status)
        {
            Debug.Log("AuthorizationStatusFetched" + status);
            AuthorizationStatus authStatus = AuthorizationStatus.NotDetermined;
            switch (status)
            {
                case "denied":
                    authStatus = AuthorizationStatus.Denied;
                    break;
                case "authorized":
                    authStatus = AuthorizationStatus.Authorized;
                    break;
                case "restricted":
                    authStatus = AuthorizationStatus.Restricted;
                    break;
                case "notDetermined":
                    authStatus = AuthorizationStatus.NotDetermined;
                    break;
            }

            onAuthorizationStatusFetched?.Invoke(authStatus);
        }

        public void Initialize(ResultCallback sentenceComplete, ResultCallback wordComplete)
        {
            onPartialResults = wordComplete;
            onFinalResults = sentenceComplete;
            SpeechRecognizer.StartRecording(true);
        }

        public void Dispose()
        {
            onPartialResults.RemoveAllListeners();
            onFinalResults.RemoveAllListeners();
        }

        public void Pause()
        {
            SpeechRecognizer.StopIfRecording();
        }

        public void Resume()
        {
            SpeechRecognizer.StartRecording(true);
        }
    }
}