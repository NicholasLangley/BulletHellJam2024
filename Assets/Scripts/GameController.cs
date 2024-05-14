using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [Header ("Custom Cursor")]
    public Texture2D cursorTexture;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;

    bool paused;
    float timeSpentPaused;

    public enum PLAYER_TYPE { SWORD_PLAYER, PAINT_PLAYER, PONG_PLAYER, MISSILE_PLAYER}

    void Awake()
    {
        PauseGame();
        timeSpentPaused = 0.0f;
    }

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


    // Update is called once per frame
    void Update()
    {
        if (paused) { timeSpentPaused += Time.unscaledDeltaTime; }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (!paused) { PauseGame(); }
            else { ResumeGame(); }
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            startGame(PLAYER_TYPE.SWORD_PLAYER);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            startGame(PLAYER_TYPE.PAINT_PLAYER);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            startGame(PLAYER_TYPE.PONG_PLAYER);
        }
    }

    public void startGame(PLAYER_TYPE pType)
    {
        if(currentPlayerChracter != null)
        {
            GameObject.Destroy(currentPlayerChracter.gameObject);
            currentPlayerChracter = null;
        }
        
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
        ResumeGame();
    }

    public void PauseGame()
    {
        paused = true;
        Time.timeScale = 0;
    }
    public void ResumeGame()
    {
        paused = false;
        Time.timeScale = 1;
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
}
