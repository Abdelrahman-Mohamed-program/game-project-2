using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameTimer : MonoBehaviour
{
    public float timeRemaining = 240f; // 4 minutes
    public TextMeshProUGUI timerText;
    private bool timerStarted = false;

void Awake()
{
    DontDestroyOnLoad(gameObject);
}
void Update()
{
    // start timer only in Scene2
   if (SceneManager.GetActiveScene().buildIndex == 1)

{

    timerStarted = true;

}

    if (timerStarted && timeRemaining > 0)
    {
        timeRemaining -= Time.deltaTime;
        UpdateTimerUI();
    }
    else if (timeRemaining <= 0)
    {
        TimeUp();
    }
}

    void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void TimeUp()
    {
        timerText.text = "00:00";

        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}