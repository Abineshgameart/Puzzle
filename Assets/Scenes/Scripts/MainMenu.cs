using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    // Private
    private static MainMenu instance;
    AudioManager audioManager;

    [SerializeField] public GameObject canvaMenu = null, mainMenu = null, pauseMenu = null, completedMenu = null, newRecordText = null;
    [SerializeField] private TextMeshProUGUI CompletedMenuTimerText, bestTimeText;   // Finished Time and Best Record Timer
    [SerializeField] public GameObject lastLevelCompletedMenu = null,  lastLevelNewRecordText = null;
    [SerializeField] private TextMeshProUGUI lastCompletedMenuTimerText, lastBestTimeText;
    [SerializeField] private GameObject level2RefImg, level3RefImg;
    [SerializeField] private Button pauseButton;
    TimerScript timerScript;

    // Public
    public TextMeshProUGUI timeText;


    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private void Start()
    {
     
        if (instance == null)
        {
            // If no instance, make this the instance and don't destroy it
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // If an instance already exists, destroy the new one
            Destroy(gameObject);
            return;
        }

        // Initialize timerScript
        if (timerScript == null)
        {
            timerScript = FindObjectOfType<TimerScript>();
            if (timerScript == null)
            {
                Debug.LogError("TimerScript not found in the scene!");
            }
        }

        // Initialize audioManager
        if (audioManager == null)
        {
            audioManager = FindObjectOfType<AudioManager>();
            if (audioManager == null)
            {
                Debug.LogError("AudioManager not found in the scene!");
            }
        }

        // Hide completedMenu at the start of each level
        if (completedMenu != null)
        {
            completedMenu.SetActive(false);  // Ensure the completion menu is hidden when the level starts
        }



        HandleMainMenu();
        SceneManager.sceneLoaded += OnSceneLoaded;


    }

    private void Update()
    {
       Scene activeScene = SceneManager.GetActiveScene();

        if (activeScene.buildIndex == 2)
        {
            level2RefImg.gameObject.SetActive(true);
        }
        else
        {
            level2RefImg.gameObject.SetActive(false);
        }
        
        if (activeScene.buildIndex == 3)
        {
            level3RefImg.gameObject.SetActive(true);
        }
        else
        {
            level3RefImg.gameObject.SetActive(false);
        }

        

    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Update mainMenu's state when a new scene is loaded
        HandleMainMenu();
        
    }

    // Check and update the mainMenu's activation status
    void HandleMainMenu()
    {
        if (mainMenu == null) return;  // Prevent null reference

        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            // Activate the main menu in scene 0
            if (!mainMenu.activeSelf)
            {
                pauseButton.gameObject.SetActive(false);
                mainMenu.SetActive(true);

            }
        }
        else
        {
            // Deactivate the main menu in other scenes
            if (mainMenu.activeSelf)
            {
                mainMenu.SetActive(false);
                timeText.gameObject.SetActive(true);
                pauseButton.gameObject.SetActive(true);

            }
        }
    }

    void OnDestroy()
    {
        // Unsubscribe from the sceneLoaded event to prevent memory leaks
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }



    public void DisplayingMainmenu()
    {
        // Check if the active scene is scene 0 (Main Menu scene)
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            // Check if the mainMenu is NOT already active
            if (!mainMenu.activeSelf)
            {
                // Activate the mainMenu if it's not already active
                mainMenu.SetActive(true);
            }

            // Hide the timeText when in the main menu
            if (timeText != null && timeText.gameObject.activeSelf)
            {
                timeText.gameObject.SetActive(false);
            }
        }
        else
        {
            // Deactivate the main menu in other scenes
            if (mainMenu.activeSelf)
            {
                mainMenu.SetActive(false);
            }

            // Show the timeText when not in the main menu
            if (timeText != null && !timeText.gameObject.activeSelf)
            {
                timeText.gameObject.SetActive(true);
            }
        }
    }

    public void PlayGame()
    {
        timerScript.Invoke("InitializeTimer", 1);
        SceneManager.LoadSceneAsync(1);
    }

    public void MainMenubg()
    {

    }

    public void PauseMenu()
    {
        if (pauseMenu.gameObject.activeSelf)
        {
            pauseMenu.SetActive(false);
        }
        else
        {
            pauseMenu.SetActive(true);
        }
    }


    public void CompletedMenu()
    {
    
        audioManager.PlaySFX(audioManager.levelCompleted);
        Debug.Log("CompletedMenu called"); // To ensure the method is being called

        if (SceneManager.GetActiveScene().buildIndex == 3)
        {
            lastLevelCompletedMenu.SetActive(true);
            CompletedActivities(lastCompletedMenuTimerText, lastBestTimeText, lastLevelNewRecordText);
        } 
        else
        {
            Debug.Log(SceneManager.GetActiveScene().buildIndex);
            completedMenu.SetActive(true);
            CompletedActivities(CompletedMenuTimerText, bestTimeText, newRecordText);
        }

        
        
    }

    void CompletedActivities(TextMeshProUGUI completedTime,TextMeshProUGUI bestTime, GameObject newRecord)
    {
        var a = timerScript;
        a.StopTimer();
        pauseButton.gameObject.SetActive(false);
        completedTime.text = (a.minutes < 10 ? "0" : "") + a.minutes + ":" + (a.seconds < 10 ? "0" : "") + a.seconds;
        int bestTimeCal;
        if (PlayerPrefs.HasKey("bestTime"))
        {
            bestTimeCal = PlayerPrefs.GetInt("bestTime");
        }
        else
        {
            bestTimeCal = 999999;
        }
        int playerTime = a.minutes * 60 + a.seconds;
        if (playerTime < bestTimeCal)
        {
            if (bestTimeText.transform.parent.gameObject.activeSelf)
            {
                bestTimeText.transform.parent.gameObject.SetActive(false);
            }
            newRecord.SetActive(true);
            PlayerPrefs.SetInt("bestTime", playerTime);
        }
        else
        {
            if (newRecord.activeSelf)
            {
                newRecord.SetActive(false);
            }

            int minutes = bestTimeCal / 60;
            int seconds = bestTimeCal - minutes * 60;
            bestTime.text = (minutes < 10 ? "0" : "") + minutes + ":" + (seconds < 10 ? "0" : "") + seconds;
            bestTime.transform.parent.gameObject.SetActive(true);
        }
    }

    void ResettingCompForLevels()
    {
        // Check if completedMenu is not null and set it inactive
        if (completedMenu != null)
        {
            completedMenu.SetActive(false);  // Ensure it's hidden at the start of the level
        }

        if (timerScript != null)
        {
            // timerScript.ResetTimer();
            timerScript.InitializeTimer();
        }
        if (pauseButton != null)
        {
            pauseButton.gameObject.SetActive(true);
        }

        if (pauseMenu.activeSelf)
        {
            pauseMenu.SetActive(false);
        }
    }
    
    // Game Level Navigation
    public void PlayAgain()
    { 

        Debug.Log("Play Again");
        ResettingCompForLevels();

        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Calling the Active Scene
    }

    // Next Level
    public void NextLevel()
    {
        Debug.Log("Play Again");
        ResettingCompForLevels();
        if ((SceneManager.GetActiveScene().buildIndex + 1) == SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); // Calling the Active Scene
        }
        
    }

    void ForToMainMenuResetting()
    {
        timerScript.StopTimer();
        if (completedMenu != null)
        {
            completedMenu.SetActive(false);  // Ensure it's hidden at the start of the level
        }
        if (lastLevelCompletedMenu != null)
        {
            lastLevelCompletedMenu.SetActive(false);  // Ensure it's hidden at the start of the level
        }
        if (pauseMenu.activeSelf)
        {
            pauseMenu.SetActive(false);
        }

    }

    // Main Menu
    public void ToMainMenu()
    {
        ForToMainMenuResetting();
        SceneManager.LoadSceneAsync(0);
    }
}



