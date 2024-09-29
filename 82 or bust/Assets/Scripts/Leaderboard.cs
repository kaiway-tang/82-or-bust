using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Leaderboard : MonoBehaviour
{
    string postUrl = "https://docs.google.com/forms/d/e/1FAIpQLSceQoCbutZCGbHmsc9fZPd1DFEplU1UkYeRQ20Xj2vSIWoltQ/formResponse";
    string nameField = "entry.554190033";
    string scoreField = "entry.1605142753";
    string getUrl = "https://docs.google.com/spreadsheets/d/1Xr67yY-JMkPiwOWICbjKbXDBiD9Z1pfyBLeBBHaHlHo/export?format=csv";
    string ascendingListId = "&gid=678367919";
    string descendingListId = "&gid=1288437903";
    // Start is called before the first frame update
    void Start()
    {
        SendResults("testnAMe", 70);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Upload(string name, int score)
    {
        WWWForm form = new WWWForm();
        form.AddField(nameField, name);  
        form.AddField(scoreField, score);

        using (UnityWebRequest www = UnityWebRequest.Post(postUrl, form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error); 
            }
            else
            {
                Debug.Log("Form upload complete!");
                StartCoroutine(GetRequest(true));
            }
        }
    }

    IEnumerator GetRequest(bool ascending = false)
    {
        string reqUrl = ascending ? getUrl + ascendingListId : getUrl + descendingListId;
        using (UnityWebRequest webRequest = UnityWebRequest.Get(reqUrl))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = reqUrl.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                    DisplayResults(webRequest.downloadHandler.text);
                    break;
            }
        }
    }

    public void SendResults(string name, int score)
    {
        StartCoroutine(Upload(name.ToLower(), score));
    }

    void DisplayResults(string res)
    {

    }
}
