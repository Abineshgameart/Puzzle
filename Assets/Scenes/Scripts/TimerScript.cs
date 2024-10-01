using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TimerScript : MonoBehaviour
{
    
    // Private
    [SerializeField] TextMeshProUGUI timeText;
    MainMenu menu;

    // Public
    public int seconds, minutes;


    // Start is called before the first frame update
    void Start()
    {
        //timeText = GameObject.FindGameObjectWithTag("Timer").GetComponent<TextMeshProUGUI>();
        // AddToSecond();
        // Invoke("AddToSecond", 2);

        // Start the timer with a small delay to ensure all components are initialized

        Invoke("InitializeTimer", 1);  // Delayed initialization for 1 second
        // InitializeTimer();
    }

    // Initialize the timer and check for timeText
    public void InitializeTimer()
    {
        if (SceneManager.GetActiveScene().buildIndex > 0)
        {
            Debug.Log("InitializeTimer Called");
            if (timeText == null)
            {
                // Debug.LogError("timeText is null! Trying to find it again.");
                timeText = GameObject.FindGameObjectWithTag("Timer")?.GetComponent<TextMeshProUGUI>();

                if (timeText == null)
                {
                    Debug.LogError("Failed to find timeText.");
                    return;  // Exit if timeText cannot be found
                }
            }

            ResetTimer();  // Ensure the timer is reset when starting
            Invoke("AddToSecond", 1); // Start the timer
        }
            
    }


    private void AddToSecond()
    {
        // Debug.Log("Add To Second Called");
        // Cancel any previous invokes to prevent multiple triggers
        CancelInvoke(nameof(AddToSecond));


        //if (timeText == null)
        //{
        //    Debug.LogError("timeText is null! Attempting to find it again.");
        //    timeText = GameObject.FindGameObjectWithTag("Timer").GetComponent<TextMeshProUGUI>();

        //    if (timeText == null)
        //    {
        //        Debug.LogError("Failed to find timeText.");
        //        return;  // Exit if it still can't be found
        //    }
        //} 


        seconds++;
        if (seconds > 59)
        {
            minutes++;
            seconds = 0;
        }

        // Update the timer display
        timeText.text = (minutes < 10 ? "0" : "") + minutes + ":" + (seconds < 10 ? "0" : "") + seconds;

        // Reinvoke the AddToSecond method after 1 second
        Invoke(nameof(AddToSecond), 1);
    }

    public void StopTimer()
    {
        CancelInvoke(nameof(AddToSecond));
        if (timeText != null)
        {
            timeText.gameObject.SetActive(false);
        }
        // timeText.gameObject.SetActive(false);
    }

    public void ResetTimer()
    {

        // Reset the seconds and minutes
        seconds = 0;
        minutes = 0;

        // Reset the timeText display if it's assigned
        if (timeText != null)
        {
            // If the timeText object is inactive, activate it
            if (!timeText.gameObject.activeSelf)
            {
                timeText.gameObject.SetActive(true);
            }

            // Reset the timeText display
            timeText.text = "00:00";

        }
        else
        {
            Debug.LogError("timeText is null! Cannot reset the timer.");
        }
    }
}
