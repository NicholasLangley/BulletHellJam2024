using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{
    [Header("Custom Cursor")]
    public Texture2D cursorTexture;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;

    [Header("Pause Menu")]
    [SerializeField]
    GameObject pauseMenu;
    [SerializeField]
    TextMeshProUGUI timeTheftText, warningText;

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

    [Header("Monsters")]
    [SerializeField]
    LaserMonster laserMonster;
    [SerializeField]
    Monster batPrefab, wallMonsterPrefab;
    public float batHeight, wallMonLeft, wallMonRight, wallMonHeight;
    float batTimer, wallMonTimer, batSpawnTime, wallMonSpawnTime;

    [Header("Colors")]
    [SerializeField]
    Gradient levelGradient;
    public float scorePerLevel;
    float percentageIncrease = 12.5f;
    float currentPercentage = 10.0f;
    float levelScore;
    int level;

    [Header("sound stuff")]
    [SerializeField]
    AudioSource killSound;
    [SerializeField]
    AudioSource finalExplosionSound,startChargingSound, stopChargingSound;

    [SerializeField]
    public AudioSource swordSwingSound;
    public AudioSource SwordDashSound;
    public AudioSource paintAttackSound;
    public AudioSource paintPaintSound;

    public enum PLAYER_TYPE { SWORD_PLAYER, PAINT_PLAYER, PONG_PLAYER, MISSILE_PLAYER}

    void Awake()
    {
        goToMainMenu();
        closeSelectCharMenu();
        timeSpentPaused = 0.0f;
        gameOver = false;
        wallController.transitionWallColor(levelGradient.Evaluate(currentPercentage / 100.0f));
        level = 0;
        batTimer = 0;
        wallMonTimer = 0;
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
        else if (paused) { timeSpentPaused += Time.unscaledDeltaTime; timeTheftText.text = "Time Theft Commited: " + (int)timeSpentPaused + "s"; if (timeSpentPaused > 15.0f) { warningText.text = "Warning: Excessive Time Theft WILL be garnished from your Life Insurance Payout"; } }
        else if (gameOver) { gameOverTimer += Time.unscaledDeltaTime; Time.timeScale = 1.0f - Mathf.Lerp(0, 1, gameOverTimer / slowMoTime); }
        else 
        { 
            increaseScore();

            batTimer += Time.deltaTime;
            wallMonTimer += Time.deltaTime;
            if(batTimer > batSpawnTime)
            {
                spawnBat();
                batTimer = 0;
            }
            if (wallMonTimer > wallMonSpawnTime)
            {
                spawnWallMonster(Random.Range(0,2) == 1);
                wallMonTimer = 0;
            }
        }
        if(Input.GetKeyDown(KeyCode.Escape) && !atMainMenu && !gameOver)
        {
            if (!paused) { PauseGame(); }
            else { ResumeGame(); }
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
                SwordPlayer sp = GameObject.Instantiate(swordPlayerPrefab) as SwordPlayer;
                sp.swingSound = swordSwingSound;
                sp.dashSound = SwordDashSound;
                currentPlayerChracter = sp;
                break;
            case PLAYER_TYPE.PAINT_PLAYER:
                PainterPlayer pp = GameObject.Instantiate(paintPlayerPrefab) as PainterPlayer;
                pp.attackSound = paintAttackSound;
                pp.paintSound = paintPaintSound;
                currentPlayerChracter = pp;
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
        currentPlayerChracter.startChargingSound = startChargingSound;
        currentPlayerChracter.stopChargingSound = stopChargingSound;
        
        timeSpentPaused = 0.0f;
        warningText.text = "";
        drill.Repair(99999);
        gameOver = false;
        gameOverTimer = 0.0f;
        score = 0.0f;
        levelScore = 0.0f;
        currentPercentage = 10f;
        wallController.transitionWallColor(levelGradient.Evaluate(currentPercentage / 100.0f));
        level = 0;
        batTimer = 0;
        wallMonTimer = 0;
        batSpawnTime = 2.5f;
        wallMonSpawnTime = 8;
        laserMonster.deactivate();
        paintAttackSound.Stop();
        paintPaintSound.Stop();
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
        foreach (Monster m in monsters) { GameObject.Destroy(m.gameObject); }

        Object[] lines = FindObjectsOfType<PaintLine>();
        foreach (PaintLine l in lines) { GameObject.Destroy(l.gameObject); }

        Object[] lasers = FindObjectsOfType<Laser>();
        foreach (Laser l in lasers) { GameObject.Destroy(l.gameObject); }
    }


    public void endGame()
    {
        //only play once
        if (gameOver == false) { finalExplosionSound.Play(); }
        gameOver = true;
        gameOverMenu.SetActive(true);
        
    }

    //call start game with enum, since you cant do this directly from the button due to using an enum parameter
    public void startSwordPlayer() => startGame(PLAYER_TYPE.SWORD_PLAYER);
    public void startPaintPlayer() => startGame(PLAYER_TYPE.PAINT_PLAYER);
    public void startPongPlayer() => startGame(PLAYER_TYPE.PONG_PLAYER);
    //public void startMissilePlayer() => startGame(PLAYER_TYPE.MISSILE_PLAYER);

    void increaseScore()
    {
        score += scoreIncreaseSpeed * Time.deltaTime;
        scoreText.text = "Depth: " + (int)score + "m";

        levelScore += scoreIncreaseSpeed * Time.deltaTime;
        if(levelScore >= scorePerLevel)
        {
            level++;
            laserMonster.activate();
            levelScore = 0.0f;
            currentPercentage += percentageIncrease;
            if (currentPercentage > 100) { currentPercentage = 10f; }
            wallController.transitionWallColor(levelGradient.Evaluate(currentPercentage / 100.0f));
            batSpawnTime -= 0.15f;
            wallMonSpawnTime -= 0.4f;
        }
    }

    void spawnBat()
    {
        Monster newBat = GameObject.Instantiate(batPrefab);
        newBat.transform.position = new Vector3(Random.Range(wallMonLeft, wallMonRight), batHeight + Random.Range(-1.0f, 1.0f), 0.0f);
        newBat.killSound = killSound;
        newBat.level = level;
        newBat.setStartPos();
    }

    void spawnWallMonster(bool isRightSide)
    {
        WallMonster newWallMon = GameObject.Instantiate(wallMonsterPrefab) as WallMonster;
        newWallMon.transform.position = new Vector3((isRightSide ? wallMonRight : wallMonLeft), wallMonHeight + Random.Range(-1.0f, 1.0f), 2.5f);
        if (isRightSide) {newWallMon.transform.localScale = new Vector3(-1, 1, 1); }
        newWallMon.killSound = killSound;
        newWallMon.rightSide = isRightSide;
        newWallMon.level = level;
        newWallMon.setStartPos();
    }
}
