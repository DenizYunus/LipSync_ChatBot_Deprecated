using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class TTSWebHelper : MonoBehaviour
{
    public static TTSWebHelper Instance;

    [SerializeField]
    LipSync lipSync;
    [SerializeField]
    AudioSource source;

    private void OnEnable()
    {
        if (Instance == null) Instance = this; else Destroy(this);
    }

    void Start()
    {
        //StartCoroutine(Upload("I want you to marry me. Please, please, please, would you do it?"));
    }

    void SpeakStartCoroutine(string text)
    {
        StartCoroutine(Instance.Upload(text));
    }


    IEnumerator Upload(string text)
    {

        WWWForm form = new();
        form.AddField("msg", text);
        form.AddField("lang", "Salli");
        form.AddField("source", "ttsmp3");

        using UnityWebRequest www = UnityWebRequest.Post("https://ttsmp3.com/makemp3_new.php", form);

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
            StartCoroutine(Instance.Upload(text));
        }
        else
        {
            //if (MobileSpeechRecognizerListener.Instance != null) MobileSpeechRecognizerListener.Instance.Pause();
            //if (WindowsSpeechRecognition.Instance != null) WindowsSpeechRecognition.Instance.Pause();
            print(www.downloadHandler.text.Replace("\\/", "/"));
            lipSync.activateSound(getURLfromJSON(www.downloadHandler.text.Replace("\\/", "/")));
        }
    }

    string getURLfromJSON(string json)
    {
        AudioURLResponseJsonClass data = JsonUtility.FromJson<AudioURLResponseJsonClass>(json);
        return data.URL;
    }

    public static void Speak(string text)
    {
        if (!Instance.source.isPlaying)
            Instance.SpeakStartCoroutine(text);
    }
}

public class AudioURLResponseJsonClass
{
    public int Error;
    public string Speaker;
    public int Cached;
    public string Text;
    public string tasktype;
    public string URL;
    public string MP3;
}