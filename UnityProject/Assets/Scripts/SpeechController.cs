using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System.IO;
using UnityEngine.Video;
using Assets.Scripts.TTS;

public class SpeechController : MonoBehaviour
{
    SpeechResponseHelper speechResponseHelper;
    OpenAISpeechResponseHelper openAISpeechResponseHelper;

    //SerialPort stream;
    //public GameObject blackGO;
    //public VideoPlayer animPlayer;

    readonly SpeechResponseGenerator speechResponseGenerator = SpeechResponseGenerator.OPENAI;

    public void WordDetected(string text)
    {
        print("partial : " + text);
    }

    private void Start()
    {
        speechResponseHelper = GetComponent<SpeechResponseHelper>();
        openAISpeechResponseHelper = GetComponent<OpenAISpeechResponseHelper>();
        //SentenceDetected("How can I get laid in Istanbul today?");

        //animPlayer.Stop();
        //stream = new SerialPort("COM9", 9600);
        //stream.ReadTimeout = 50;
        //stream.Open();
    }

    public void SentenceDetected(string text)
    {
        print(text);

        if (speechResponseGenerator == SpeechResponseGenerator.METAPIENSAPI)
        {
            SpeechRecognitionSelector.recognizer.Pause();
            speechResponseHelper.GetResponse(text);
        }

        else if (speechResponseGenerator == SpeechResponseGenerator.OPENAI)
        {
            SpeechRecognitionSelector.recognizer.Pause();
            openAISpeechResponseHelper.GetResponse(text);
        }

        else if (speechResponseGenerator == SpeechResponseGenerator.INTENTS)
        {
            bool found = false;
            foreach (KeyValuePair<string, string> keyValuePair in intents)
            {
                if (text.Contains(keyValuePair.Key))
                {
                    SpeechSynthesizer.Instance.Speak(keyValuePair.Value);
                    //if (text.Contains("start the system"))
                    //{
                    //    blackGO.SetActive(false);
                    //    animPlayer.Play();
                    //    stream.Write("start");
                    //}
                    found = true;
                    break;
                }
            }
            if (found == false)
                SpeechRecognitionSelector.recognizer.Resume();
        }
    }

    readonly Dictionary<string, string> intents = new ()
    {
        { "start the system", "hi boss. service is being started. please hold on for a moment." },
        //{ "hey", "Hi sir." },
        { "who are you", "I am a bot developed my Metapiens. A personal assistant that can do many tasks and make life better for you." },
        { "how old are you", "I am twenty one, a new generation internet connected meta-human, in other words, a meta-sapien." },
        { "long time no see", "Yes, the world has been darker without you. I really missed you honey." },
        { "i love you", "You are my heart, my life. I love you." },
        { "what did you do", "I've missed you with all my heart." },
        { "sorry", "Don't be sad, it happens. Let's celebrate our meeting again. Finally we got back to each other." },
        { "this is us", "Yes, my other side. Don't you never leave again." },
        { "recommend", "Actually, yes. There is a song will be released this Sunday by DeNyx. I'm gonna send you the art to share it." },
        { "recommendation", "Actually, yes. There is a song will be released this Sunday by DeNyx. I'm gonna send you the art to share it." }
        //{ "hey", "Yo, Deniz. You missed me?" },
        //{ "how are you", "I am fine, thanks for asking sweetie. What about you?" },
        //{ "long time no see", "Yes, the world has been darker without you. I really missed you honey." },
        //{ "i love you", "You are my heart, my life. I love you." },
        //{ "what did you do", "I've missed you with all my heart." },
        //{ "how old are you", "I am twenty one, same as you. Did you really forget?" },
        //{ "sorry", "Don't be sad, it happens. Let's celebrate our meeting again. Finally we got back to each other." },
        //{ "this is us", "Yes, my other side. Don't you never leave again." },
        //{ "recommend", "Actually, yes. There is a song will be released this Sunday by DeNyx. I'm gonna send you the art to share it." },
        //{ "recommendation", "Actually, yes. There is a song will be released this Sunday by DeNyx. I'm gonna send you the art to share it." }
    };

    enum SpeechResponseGenerator
    {
        METAPIENSAPI,
        OPENAI,
        INTENTS
    }

    public enum SpeechGenerator
    {
        TTSWebHelper,
        GoogleTTS
    }
}