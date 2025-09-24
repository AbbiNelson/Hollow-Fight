using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class ChatGPTExample : MonoBehaviour
{
    public string apiKey = "YOUR_OPENAI_API_KEY";
    public string prompt = "Hello, AI!";

    public void StartChat()
    {
        StartCoroutine(SendChatRequest(prompt));
    }

    IEnumerator SendChatRequest(string userInput)
    {
        string url = "https://api.openai.com/v1/chat/completions";
        string json = "{\"model\": \"gpt-3.5-turbo\", \"messages\": [{\"role\": \"user\", \"content\": \"" + userInput + "\"}]}";

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", "Bearer " + apiKey);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log(request.downloadHandler.text); // Parse and use response
        }
        else
        {
            Debug.LogError(request.error);
        }
    }
}