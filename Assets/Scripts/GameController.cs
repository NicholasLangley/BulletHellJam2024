using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{
    [Header ("Custom Cursor")]
    public Texture2D cursorTexture;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;

    [Header("Pause Menu")]
    [SerializeField]
    GameObject pauseMenu;
    [SerializeField]
    TextMeshProUGUI timeTheftText;

    [Header("Character Select Menu")]
    [SerializeField]
    GameObject charSelectMenu;

    [Header("Main Menu")]
    [SerializeField]
    GameObject mainMenu;
    [SerializeField]
    float menuTextFlashOn, menuTextFlashOff;
    bool atMainMenu;
    float mainMenuTimer;

    bool paused;
    float timeSpentPaused;

    [Header("Game Over Menu")]
    [SerializeField]
    GameObject gameOverMenu;
    [SerializeField]
    float slowMoTime = 1.0f;
    bool gameOver;
    float gameOverTimer;

    [Header("Wall Controller")]
    [SerializeField]
    WallController wallController;

    [Header("Player Stuff")]
    [SerializeField]
    Player swordPlayerPrefab;
    [SerializeField]
    Player paintPlayerPrefab, pongPlayerPrefab; //missilePlayerPrefab,
    [SerializeField]
    ResourceBar energyBar;
    Player currentPlayerChracter = null;

    [Header("Drill")]
    [SerializeField]
    Drill drill;

    [Header("Score Section")]
    [SerializeField]
    TextMeshProUGUI scoreText;
    float score;
    public float scoreIncreaseSpeed;

    public enum PLAYER_TYPE { SWORD_PLAYER, PAINT_PLAYER, PONG_PLAYER, MISSILE_PLAYER}

    void Awake()
    {
        goToMainMenu();
        closeSelectCharMenu();
        timeSpentPaused = 0.0f;
        gameOver = false;
    }

 


    // Update is called once per frame
    void Update()
    {
        if (atMainMenu)
        {
            mainMenuTimer += Time.unscaledDeltaTime;
            if (mainMenuTimer > menuTextFlashOff + menuTextFlashOn) { mainMenuTimer = 0.0f; mainMenu.transform.Find("flashingText").gameObject.SetActive(true); }
            else if (mainMenuTimer > menuTextFlashOn) { mainMenu.transform.Find("flashingText").gameObject.SetActive(false); }

            if (Input.anyKeyDown)
            {
                openSelectCharMenu();
            }
        }
        else if (paused) { timeSpentPaused += Time.unscaledDeltaTime; timeTheftText.text = "Time Theft Commited: " + (int)timeSpentPaused + "s"; }
        else if (gameOver) { gameOverTimer += Time.unscaledDeltaTime; Time.timeScale = 1.0f - Mathf.Lerp(0, 1, gameOverTimer / slowMoTime); }
        else { score += scoreIncreaseSpeed * Time.deltaTime;  scoreText.text = "Depth: " + (int)score + "m"; }
        if(Input.GetKeyDown(KeyCode.Escape) && !atMainMenu && !gameOver)
        {
            if (!paused) { PauseGame(); }
            else { ResumeGame(); }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            wallController.transitionWallColor(Color.red);
        }
        else if (Input.GetKeyDown(KeyCode.B))
        {
            wallController.transitionWallColor(Color.blue);
        }
        else if (Input.GetKeyDown(KeyCode.G))
        {
            wallController.transitionWallColor(Color.green);
        }
    }

    public void startGame(PLAYER_TYPE pType)
    {
        destroyCurrentGame();

        atMainMenu = false;
        mainMenu.SetActive(false);
        gameOverMenu.SetActive(false);

        switch (pType)
        {
            case PLAYER_TYPE.SWORD_PLAYER:
                currentPlayerChracter = GameObject.Instantiate(swordPlayerPrefab);
                break;
            case PLAYER_TYPE.PAINT_PLAYER:
                currentPlayerChracter = GameObject.Instantiate(paintPlayerPrefab);
                break;
            case PLAYER_TYPE.PONG_PLAYER:
                currentPlayerChracter = GameObject.Instantiate(pongPlayerPrefab);
                break;
            //case PLAYER_TYPE.MISSILE_PLAYER:
                //GameObject.Instantiate(missilePlayerPrefab);
                //break;
            default:
                return;
        }

        currentPlayerChracter.setEnergyBar(energyBar);
        
        timeSpentPaused = 0.0f;
        drill.Repair(99999);
        gameOver = false;
        gameOverTimer = 0.0f;
        score = 0.0f;
        ResumeGame();
    }

    public void PauseGame()
    {
        paused = true;
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
        enablePauseButtons();
    }
    public void ResumeGame()
    {
        paused = false;
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        charSelectMenu.SetActive(false);
    }

    //custom cursor stuff
    void OnMouseEnter()
    {
        Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
    }
    void OnMouseExit()
    {
        // Pass 'null' to the texture parameter to use the default system cursor.
        Cursor.SetCursor(null, Vector2.zero, cursorMode);
    }

    public void openSelectCharMenu()
    {
        charSelectMenu.SetActive(true);
        disablePauseButtons();
    }

    public void closeSelectCharMenu()
    {
        charSelectMenu.SetActive(false);
        enablePauseButtons();
    }

    void enablePauseButtons()
    {
        pauseMenu.transform.Find("Buttons").gameObject.SetActive(true);
    }

    void disablePauseButtons()
    {
        pauseMenu.transform.Find("Buttons").gameObject.SetActive(false);
    }

    public void goToMainMenu()
    {
        atMainMenu = true;
        mainMenuTimer = 0.0f;
        mainMenu.SetActive(true);
        gameOverMenu.SetActive(false);
        PauseGame();
    }

    //destroy any spawned bullets/monsters/etc.
    void destroyCurrentGame()
    {
        if (currentPlayerChracter != null)
        {
            GameObject.Destroy(currentPlayerChracter.gameObject);
            currentPlayerChracter = null;
        }

        Object[] bullets = FindObjectsOfType<Bullet>();
        foreach (Bullet b in bullets) { GameObject.Destroy(b.gameObject); }

        Object[] monsters = FindObjectsOfType<Monster>();
        //foreach (Monster m in monsters) { GameObject.Destroy(m.gameObject); }

        Object[] lines = FindObjectsOfType<PaintLine>();
        foreach (PaintLine l in lines) { GameObject.Destroy(l.gameObject); }
    }


    public void endGame()
    {
        gameOver = true;
        gameOverMenu.SetActive(true);
    }

    //call start game with enum, since you cant do this directly from the button due to using an enum parameter
    public void startSwordPlayer() => startGame(PLAYER_TYPE.SWORD_PLAYER);
    public void startPaintPlayer() => startGame(PLAYER_TYPE.PAINT_PLAYER);
    public void startPongPlayer() => startGame(PLAYER_TYPE.PONG_PLAYER);
    //public void startMissilePlayer() => startGame(PLAYER_TYPE.MISSILE_PLAYER);


}
