using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PuzzleScript : MonoBehaviour
{
    // Private
    Camera _camera;   // Main Camera
    [SerializeField] Transform emptySpace;  // Empty Space in the puzzle 
    [SerializeField] private TileScript[] tiles;  // Puzzle Tiles storing by Array 
    // private ShuffleScript shuffleScripts;

    private int emptySpaceIndex;    // EmptySpace 
    private int emptySpaceCheck;
    private bool _isFinished;   // Variable for Finish Checking
    float distanceThreshold;

    [SerializeField] private GameObject endPanel = null, newRecordText = null;  // 
    [SerializeField] private TextMeshProUGUI endPanelTimerText, bestTimeText;   // Finished Time and Best Record Time

    // Public

    
    
    // Start is called before the first frame update
    void Start()
    {
        _camera  = Camera.main;  // Assigning Main Camera

        emptySpaceIndex = tiles.Length - 1;
        emptySpaceCheck = emptySpaceIndex;
        // shuffleScripts.Shuffle(emptySpaceIndex, tiles, emptySpace);
        Shuffle();

        if (SceneManager.GetActiveScene().buildIndex == 1)
            distanceThreshold = 2.2f;
        else if (SceneManager.GetActiveScene().buildIndex == 2)
            distanceThreshold = 1.5f;
        else if (SceneManager.GetActiveScene().buildIndex == 3)
            distanceThreshold = 1.3f;

    }



    // Update is called once per frame
    void Update()
    {
        // When mouse Left Button Clicked
        if (Input.GetMouseButtonDown(0))
        {
            // Raycast Checking for the finding the tiles clicked by the Mouse

            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);  // Assining mouse Position by the Ray 
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);  // Assigning Hit Value to the hit
            
            // IF Mouse Hitted the Screen
            if (hit)
            {
                Debug.Log(distanceThreshold);

                // If the distance between the empty tiles and Hitted Tiles less than 2.2f
                if (Vector2.Distance(emptySpace.position, hit.transform.position) < distanceThreshold )
                {
                    Vector2 lastEmptySpacePosition = emptySpace.position;  // EmptySpace as lastEmptySpacePosition 
                    TileScript thisTile = hit.transform.GetComponent<TileScript>();  // Getting the Script of the Hitted Tiles
                    emptySpace.position = thisTile.targetPosition;  // Changin the Position of the Empty Space to Hitted Tiles
                    thisTile.targetPosition = lastEmptySpacePosition;  // Changing Tiles position to  EmptySpace
                    int tileIndex = findIndex(thisTile);  // 
                    tiles[emptySpaceIndex] = tiles[tileIndex];
                    tiles[tileIndex] = null;
                    emptySpaceIndex = tileIndex;
                }
            }
        }

        // Debug.Log(tiles.Length);

        if (!_isFinished)
        {
            int correctTiles = 0;
            foreach (var a in tiles)
            {
                if (a != null)
                {
                    if (a.inRightPlace)
                    {
                        correctTiles++;
                    }
                }
            }

            if (correctTiles == tiles.Length - 1)
            {
                _isFinished = true;
                endPanel.SetActive(true);
                var a = GetComponent<TimerScript>();
                a.StopTimer();
                endPanelTimerText.text = (a.minutes < 10 ? "0" : "") + a.minutes + ":" + (a.seconds < 10 ? "0" : "") + a.seconds;
                int bestTime;
                if (PlayerPrefs.HasKey("bestTime"))
                {
                    bestTime = PlayerPrefs.GetInt("bestTime");
                }
                else
                {
                    bestTime = 999999;
                }
                int playerTime = a.minutes * 60 + a.seconds;
                if (playerTime < bestTime)
                {
                    newRecordText.SetActive(true);
                    PlayerPrefs.SetInt("bestTime", playerTime);
                }
                else
                {
                    int minutes = bestTime / 60;
                    int seconds = bestTime - minutes * 60;
                    bestTimeText.text = (minutes < 10 ? "0" : "") + minutes + ":" + (seconds < 10 ? "0" : "") + seconds;
                    bestTimeText.transform.parent.gameObject.SetActive(true);
                }
            }
        }

    }


    // Reloading Function
    public void PlayAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Calling the Active Scene
    }

    // Next Level
    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); // Calling the Active Scene
    }

    // Main Menu
    public void MainMenu()
    {
        SceneManager.LoadSceneAsync(0);
    }


    // Shuffling System
    public void Shuffle()
    {
        int invertion;
        
        // For Empty Space Index position in 15th Tile 

        if (emptySpaceIndex != emptySpaceCheck)
        {
            var tileOn15LastPos = tiles[emptySpaceCheck].targetPosition;
            tiles[emptySpaceCheck].targetPosition = emptySpace.position;
            emptySpace.position = tileOn15LastPos;
            tiles[emptySpaceIndex] = tiles[emptySpaceCheck];
            tiles[emptySpaceCheck] = null;
            emptySpaceIndex = emptySpaceCheck;
        }

        // Shuffling Tiles
        
        do
        {
            for (int i = 0; i <= emptySpaceCheck - 1; i++)
            {
                var lastPos = tiles[i].targetPosition;
                int randomIndex = Random.Range(0, emptySpaceCheck - 1);
                tiles[i].targetPosition = tiles[randomIndex].targetPosition;
                tiles[randomIndex].targetPosition = lastPos;
                var tile = tiles[i];
                tiles[i] = tiles[randomIndex];
                tiles[randomIndex] = tile;
            }
            invertion = GetInversions();
            Debug.Log("Puzzle Shuffled");
        } while (invertion % 2 != 0);
        
    }


    // finding the index of Tiles in Shuffling

    public int findIndex(TileScript ts)
    {
        for (int i = 0; i < tiles.Length; i++)
        {
            if (tiles[i] != null)
            {
                if (tiles[i] == ts)
                {
                    return i;
                }
            }
        }
        return -1;
    }

    // Inversion Calculating and Checking

    int GetInversions()
    {
        int inversionsSum = 0;
        for (int i = 0; i < tiles.Length; i++)
        {
            int thisTileInvertion = 0;
            for (int j = i; j < tiles.Length; j++)
            {
                if ((tiles[j] != null)) {
                    if (tiles[i].number > tiles[j].number)
                    {
                        thisTileInvertion++;
                    }
                }
            }
            inversionsSum += thisTileInvertion;
        }
        return inversionsSum;
    }  

}
