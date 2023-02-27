using Assets.Scripts.TTS;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class SpeechResponseHelper : MonoBehaviour
{
    HttpWebRequest request;
    string apiURL = @"https://31f9-88-254-139-27.eu.ngrok.io/aila/dataset/?q=";

    SpeechController sc;

    public SpeechResponseHelper(SpeechController _sc)
    {
        sc = _sc;
        apiURL = @"https://31f9-88-254-139-27.eu.ngrok.io/aila/dataset/?q=";
    }

    public void GetResponse(string text)
    {
        apiURL = @"https://31f9-88-254-139-27.eu.ngrok.io/aila/dataset/?q=";
        string reqURL = apiURL + getTextWithoutSpaces(text);

        print(reqURL);
        StartCoroutine(GetRequest(reqURL));
    }

    string getTextWithoutSpaces(string text)
    {
        return text.Replace(" ", "+");
    }

    IEnumerator GetRequest(string uri)
    {
        using UnityWebRequest webRequest = UnityWebRequest.Get(uri);

        webRequest.SetRequestHeader("ngrok-skip-browser-warning", "1");

        yield return webRequest.SendWebRequest();

        string[] pages = uri.Split('/');
        int page = pages.Length - 1;

        print(webRequest.result);
        switch (webRequest.result)
        {
            case UnityWebRequest.Result.ConnectionError:
                print("Connection error.");
                break;
            case UnityWebRequest.Result.DataProcessingError:
                Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                break;
            case UnityWebRequest.Result.ProtocolError:
                Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                break;
            case UnityWebRequest.Result.Success:
                ApiResponseJsonClass data = JsonUtility.FromJson<ApiResponseJsonClass>(webRequest.downloadHandler.text);
                SpeechSynthesizer.Instance.Speak(data.response);
                break;
        }
    }
}

public class ApiResponseJsonClass
{
    public string response;
    public string userid;
}