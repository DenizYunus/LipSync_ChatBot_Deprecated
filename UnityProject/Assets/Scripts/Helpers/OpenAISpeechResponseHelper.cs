using Assets.Scripts.TTS;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class OpenAISpeechResponseHelper : MonoBehaviour
{
    HttpWebRequest request;
    readonly string apiURL = @"https://api.openai.com/v1/completions";

    SpeechController sc;

    public OpenAISpeechResponseHelper(SpeechController _sc)
    {
        sc = _sc;
    }

    public void GetResponse(string text)
    {
        StartCoroutine(GetRequest(apiURL, text));
    }

    IEnumerator GetRequest(string apiURLL, string text)
    {
        string apiURL = @"https://api.openai.com/v1/completions";

        Dictionary<string, object> formData = new()
        {
            { "model", "text-curie-001" },
            { "prompt", text },
            { "temperature", 0.7 },
            { "max_tokens", 70 },
            { "top_p", 1 },
            { "presence_penalty", 0 },
            { "frequency_penalty", 0 }
        };

        print(apiURL);

        string jsonData = JsonConvert.SerializeObject(formData);
        print(jsonData);

        using UnityWebRequest webRequest = UnityWebRequest.Post(apiURL, string.Empty);

        webRequest.SetRequestHeader("Content-Type", "application/json");
        webRequest.SetRequestHeader("User-Agent", $"hexthedev/openai_api_unity");
        webRequest.SetRequestHeader("Authorization", "Bearer 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");

        webRequest.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(jsonData));

        yield return webRequest.SendWebRequest();

        string[] pages = apiURL.Split('/');
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
                print(webRequest.downloadHandler.text);
                OpenAIResponseJsonClass data = JsonConvert.DeserializeObject<OpenAIResponseJsonClass>(webRequest.downloadHandler.text);
                SpeechSynthesizer.Instance.Speak(data.choices[0].text);
                break;
        }
    }
}

// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
public class Choice
{
    public string text { get; set; }
    public int index { get; set; }
    public object logprobs { get; set; }
    public string finish_reason { get; set; }
}

public class OpenAIResponseJsonClass

{
    public string id { get; set; }
    public string @object { get; set; }
    public int created { get; set; }
    public string model { get; set; }
    public List<Choice> choices { get; set; }
    public Usage usage { get; set; }
}

public class Usage
{
    public int prompt_tokens { get; set; }
    public int completion_tokens { get; set; }
    public int total_tokens { get; set; }
}