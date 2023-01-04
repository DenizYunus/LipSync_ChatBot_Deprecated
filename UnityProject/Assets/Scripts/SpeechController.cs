using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeechController : MonoBehaviour
{
    public void WordDetected(string text)
    {
        print("partial : " + text);
    }

    public void SentenceDetected(string text)
    {
        print(text);
        foreach (KeyValuePair<string, string> keyValuePair in intents)
        {
            if (text.Contains(keyValuePair.Key))
            {
                TTSWebHelper.Speak(keyValuePair.Value);
            }
        }
    }

    Dictionary<string, string> intents = new Dictionary<string, string>()
    {
        { "hey", "Yo, Deniz. You missed me?" },
        { "how are you", "I am fine, thanks for asking sweetie. What about you?" },
        { "long time no see", "Yes, the world has been darker without you. I really missed you honey." },
        { "i love you", "You are my heart, my life. I love you." },
        { "what did you do", "I've missed you with all my heart." },
        { "how old are you", "I am twenty one, same as you. Did you really forget?" },
        { "sorry", "Don't be sad, it happens. Let's celebrate our meeting again. Finally we got back to each other." },
        { "this is us", "Yes, my other side. Don't you never leave again." }
    };
}