using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    //the total elapsed time
    float elapsedTime;
    //The timer UI element
    TextMeshProUGUI txTimer;
    //A static reference to the timer
    public static Timer instance;
    //Should the timer be paused?
    public bool pause;

    private void Start()
    {
        //Finds the UI element and sets the timer instance to this
        txTimer = GetComponent<TextMeshProUGUI>();
        instance = this;
    }

    //Sets the timer back to zero
    public void ResetTimer()
    {
        elapsedTime = 0;
    }

    void Update()
    {
        //If the timer is paused - Return and do not update
        if (pause)
            return;

        //Add deltaTime to the elapsed time
        elapsedTime += Time.deltaTime;
        //convert the elapsed time float to hours/minutes/seconds
        string hours = Mathf.Floor(elapsedTime / 3600).ToString("00");
        string minutes = Mathf.Floor(elapsedTime / 60).ToString("00");
        string seconds = Mathf.Floor(elapsedTime % 60).ToString("00");
        //Update the UI
        txTimer.text = string.Format("{0}:{1}:{2}", hours, minutes, seconds);
    }
}
