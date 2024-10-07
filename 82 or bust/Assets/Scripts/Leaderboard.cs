using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Leaderboard : MonoBehaviour
{
    string postUrl = "https://docs.google.com/forms/d/e/1FAIpQLSceQoCbutZCGbHmsc9fZPd1DFEplU1UkYeRQ20Xj2vSIWoltQ/formResponse";
    string nameField = "entry.554190033";
    string scoreField = "entry.1605142753";
    string getUrl = "https://docs.google.com/spreadsheets/d/1Xr67yY-JMkPiwOWICbjKbXDBiD9Z1pfyBLeBBHaHlHo/export?format=csv";
    string ascendingListId = "&gid=678367919";
    string descendingListId = "&gid=1288437903";

    [SerializeField] TMP_Text leaderboardDisplay;
    [SerializeField] TMP_Text scoreDisplay;
    [SerializeField] TMP_Text loserName;
    [SerializeField] TMP_Text loserScore;
    [SerializeField] TMP_InputField nameInput;
    [SerializeField] TMP_Text runSummary;
    [SerializeField] TMP_Text finalScore;
    // Start is called before the first frame update
    void Start()
    {
        // SendResults("testnAMe", 70);
    }

    private void OnEnable()
    {
        StartCoroutine(GetRequest());
        StartCoroutine(GetRequest(true));
        finalScore.text = GameManager.self.score.ToString();
        Cursor.lockState = CursorLockMode.None;
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
                StartCoroutine(GetRequest());
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
                    DisplayResults(webRequest.downloadHandler.text, !ascending);
                    break;
            }
        }
    }

    public void SubmitName()
    {
        SendResults(nameInput.text, GameManager.self.score);
    }

    public void SendResults(string name, int score)
    {
        StartCoroutine(Upload(name.ToLower(), score));
    }

    public void ResetGame()
    {
        GameManager.self.Restart();
    }

    public void ExitToMenu()
    {
        SceneManager.LoadScene("StartScene");
    }

    void DisplayResults(string res, bool leader = true)
    {
        // Split the CSV into lines
        string[] lines = res.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

        // This will store the formatted result
        string displayText = "";

        string scoreText = "";

        int ctr = 0;
        // Loop through each line
        foreach (string line in lines)
        {
            // Split the line by commas to get individual elements
            string[] elements = line.Split(',');

            // Ensure the line has at least 3 elements (adjust based on your CSV format)
            if (elements.Length >= 3)
            {
                // Get the second and third elements (adjust index if CSV structure differs)
                string element2 = elements[1].Trim();  // 2nd element
                string element3 = elements[2].Trim();  // 3rd element

                // Append them to the display text (format this as needed)
                displayText += $"{element2}\n";
                scoreText += $"{element3}\n";
            }
            ++ctr;
            if (ctr >= 10)  // Only display top 10
            {
                break;
            }
        }
        if (leader)
        {
            leaderboardDisplay.text = displayText;
            scoreDisplay.text = scoreText;
        } else
        {
            loserName.text = displayText;
            loserScore.text = scoreText;
        }
        
    }
}
