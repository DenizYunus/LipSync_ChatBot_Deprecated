using System.Collections;
using UnityEngine;
using static SpeechController;

namespace Assets.Scripts.TTS
{
    [RequireComponent(typeof(AudioSource))]
    public class SpeechSynthesizer : MonoBehaviour
    {
        public static ITTS tts;
        public SpeechGenerator generatorType;

        [HideInInspector]
        public AudioSource source;

        public static SpeechSynthesizer Instance;

        private void OnEnable()
        {
            if (Instance != null) { Destroy(this); }
            else Instance = this;

            source = GetComponent<AudioSource>();
            Initialize();
            //StartCoroutine(Test());
        }

        IEnumerator Test()
        {
            yield return new WaitForSeconds(3);
            Speak("I am your sweetie, am I?");
        }

        public void Initialize()
        {
            if (generatorType == SpeechGenerator.GoogleTTS)
            {
                gameObject.AddComponent<GoogleTTS>();
                tts = GetComponent<GoogleTTS>();
            }
            else if (generatorType == SpeechGenerator.TTSWebHelper)
            {
                gameObject.AddComponent<TTSWebHelper>();
                tts = GetComponent<TTSWebHelper>();
            }
        }

        public void Speak(string text)
        {
            tts.Speak(text);
        }
    }
}
