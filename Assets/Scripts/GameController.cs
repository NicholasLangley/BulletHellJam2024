using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    bool paused;
    float timeSpentPaused;
    // Start is called before the first frame update
    void Awake()
    {
        paused = false;
        Time.timeScale = 1;
        timeSpentPaused = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (paused) { timeSpentPaused += Time.unscaledDeltaTime; }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (!paused) { PauseGame(); }
            else { ResumeGame(); }
        }
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
}
