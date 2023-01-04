using System.Collections;
using UnityEngine;

public class LipSync : MonoBehaviour
{
    public static LipSync Instance;

    // Use this for initialization

    /*Class for implementing Lips Syncronisation*/

    public WWW www;

    public AudioSource source;
    public AudioClip source_clip;
    public string audioFilePath;

    public bool soundPlayed = true;

    public float[] freqData;
    public int nSamples = 256;
    int fMax = 24000;

    public Transform upmouth0_M, upmouth01_L, upmouth02_R, downmouth1_M, downmouth11_L, downmouth12_R;
    float volume = 0.4f; //----------------1000 ----- was 1
    //  float freqLow = 200;
    //  float freqHigh = 800;
    //value change

    float freqLow = 5; //was 200
    float freqHigh = 800; //was 1600

    int sizeFilter = 5;
    float[] filter;
    float filterSum;
    int posFilter = 0;
    int qSample = 0;

    public int video_Length, secCounter;

    float y0, y1;

    void OnEnable()
    {
        if (Instance == null) Instance = this; else Destroy(this);

        secCounter = 0;

        //      y0 = mouth0.localPosition.y;
        //      y1 = mouth1.localPosition.y;

        y0 = upmouth0_M.localPosition.z;
        y0 = upmouth01_L.localPosition.z;
        y0 = upmouth02_R.localPosition.z;
        y1 = downmouth1_M.localPosition.y;
        y1 = downmouth11_L.localPosition.y;
        y1 = downmouth12_R.localPosition.y;

        //freqData = new float[nSamples];
        ///////////source_clip = SetFace.voiceOver;
        //////////      //GetComponent<AudioSource>().clip = Rec_voice.instance.voiceFeed.clip;
        //source.Play();
        //video_Length = Mathf.CeilToInt(source_clip.length);
    }

    float BandVol(float fLow, float fHigh)
    {
        fLow = Mathf.Clamp(fLow, 5, fMax); //mid was 20
        fHigh = Mathf.Clamp(fHigh, fLow, fMax);

        source.GetSpectrumData(freqData, 0, FFTWindow.BlackmanHarris);

        int n1 = Mathf.FloorToInt(fLow * nSamples / fMax);
        int n2 = Mathf.FloorToInt(fHigh * nSamples / fMax);

        float sum = 0;

        for (int i = n1; i <= n2; i++)
        {
            sum = freqData[i];
        }

        return sum;
    }

    float MovingAverage(float sample)
    {
        if (qSample == 0)
            filter = new float[sizeFilter];

        filterSum += sample - filter[posFilter];
        filter[posFilter++] = sample;

        if (posFilter > qSample)
        {
            qSample = posFilter;
        }

        posFilter = posFilter % sizeFilter;
        return filterSum / qSample;
    }

    void Start()
    {
        source = GetComponent<AudioSource>();
    }


    public void activateSound(string soundURL)
    {
        UnityMainThreadDispatcher.Instance().Enqueue(startSound(soundURL));
        //StartCoroutine(startSound(soundURL));

        /*UnityMainThread.wkr.AddJob(() =>
        {
            source.clip = www.GetAudioClip();
            source_clip = www.GetAudioClip();
            freqData = new float[nSamples];
            source.Play();
            video_Length = Mathf.CeilToInt(source_clip.length);
        });*/
    }

    IEnumerator startSound(string soundURL)
    {
        www = new WWW(soundURL);
        yield return www;
        source.clip = www.GetAudioClip();
        freqData = new float[nSamples];
        source.Play();
        //video_Length = Mathf.CeilToInt(source_clip.length);
        yield return new WaitForSeconds(www.GetAudioClip().length);
        SpeechRecognitionSelector.recognizer.Resume();
    }

    float limValue;

    void Update()
    {
        if (!source.isPlaying)
            return;
        //if (source.clip != null) {
        //    if (!source.isPlaying && source.clip.isReadyToPlay && soundPlayed == false)
        //    {
        //        source.Play();
        //        soundPlayed = true;
        //    }
        //}

        float band_vol = BandVol(freqLow, freqHigh);//--------------------------------------------------------
        //print(band_vol);
        float val = MovingAverage(band_vol) * volume;
        //limValue = val;//Mathf.Clamp (val, 0, 0.1f);
        //limValue = Mathf.Clamp (val, 0, 10f);
        //check new lip movement abd set clamp val
        limValue = Mathf.Clamp(val, 0, 25); //-----------------------------------------------25 ---------- 0.02f
        //Debug.Log(limValue);//-------------------------------------------------------------------------------------------
        //if (Input.GetKeyDown(KeyCode.Escape))
        //{
        //    Application.Quit();
        //}
        /*  mouth0.position = new Vector3 (mouth0.position.x, y0 - MovingAverage (band_vol) * volume, mouth0.position.z);
        mouth1.position = new Vector3 (mouth1.position.x, y1 + MovingAverage (band_vol) * volume * 0.3f, mouth1.position.z);*/
    }

    void LateUpdate()
    {
        //      mouth0.localPosition = new Vector3 (mouth0.localPosition.x, y0 - limValue, mouth0.localPosition.z);
        //      mouth1.localPosition = new Vector3 (mouth1.localPosition.x, y1 + limValue, mouth1.localPosition.z);
        upmouth0_M.localPosition = new Vector3(upmouth0_M.localPosition.x, upmouth0_M.localPosition.y, y0 + limValue);
        upmouth01_L.localPosition = new Vector3(upmouth01_L.localPosition.x, upmouth01_L.localPosition.y, y0 + limValue);
        upmouth02_R.localPosition = new Vector3(upmouth02_R.localPosition.x, upmouth02_R.localPosition.y, y0 + limValue);
        downmouth1_M.localPosition = new Vector3(downmouth1_M.localPosition.x, y1 + limValue, downmouth1_M.localPosition.z);
        downmouth11_L.localPosition = new Vector3(downmouth11_L.localPosition.x, y1 + limValue, downmouth11_L.localPosition.z);
        downmouth12_R.localPosition = new Vector3(downmouth12_R.localPosition.x, y1 + limValue, downmouth12_R.localPosition.z);

        //upmouth0_M.localPosition = new Vector3(upmouth0_M.localPosition.x, y0 - limValue, upmouth0_M.localPosition.z);
        //upmouth01_L.localPosition = new Vector3(upmouth01_L.localPosition.x, y0 - limValue, upmouth01_L.localPosition.z);
        //upmouth02_R.localPosition = new Vector3(upmouth02_R.localPosition.x, y0 - limValue, upmouth02_R.localPosition.z);
        //downmouth1_M.localPosition = new Vector3(downmouth1_M.localPosition.x, y1 + limValue, downmouth1_M.localPosition.z);
        //downmouth11_L.localPosition = new Vector3(downmouth11_L.localPosition.x, y1 + limValue, downmouth11_L.localPosition.z);
        //downmouth12_R.localPosition = new Vector3(downmouth12_R.localPosition.x, y1 + limValue, downmouth12_R.localPosition.z);
    }

}