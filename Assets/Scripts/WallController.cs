using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallController : MonoBehaviour
{
    public float colorTransitionLength;
    float transitionTimer;
    bool transitioning;
    [SerializeField]
    Color oldColor;
    Color newColor;

    [SerializeField]
    Light spotlight;


    public float speed = 1.0f;
    [SerializeField]
    float backgroundSpeedFraction = 0.7f;

    [SerializeField]
    Renderer bgRenderer, rightRenderer, leftRenderer;
    [SerializeField]
    SpriteRenderer horde, drill;

    // Start is called before the first frame update
    void Awake()
    {
        rightRenderer.material.mainTextureOffset += Vector2.down * 0.4f;
        transitioning = false;
        oldColor = bgRenderer.material.color;
    }

    // Update is called once per frame
    void Update()
    {
        bgRenderer.material.mainTextureOffset += Vector2.down * speed * backgroundSpeedFraction * Time.deltaTime;
        rightRenderer.material.mainTextureOffset += Vector2.down * speed * Time.deltaTime;
        leftRenderer.material.mainTextureOffset += Vector2.down * speed * Time.deltaTime;

        if (transitioning)
        {
            transitionTimer += Time.deltaTime;
            bgRenderer.material.color = Color.Lerp(oldColor, newColor, transitionTimer / colorTransitionLength);
            rightRenderer.material.color = Color.Lerp(oldColor, newColor, transitionTimer / colorTransitionLength);
            leftRenderer.material.color = Color.Lerp(oldColor, newColor, transitionTimer / colorTransitionLength);

            horde.color = Color.Lerp(oldColor, newColor, transitionTimer / colorTransitionLength);
            drill.color = Color.Lerp(oldColor, newColor, transitionTimer / colorTransitionLength);

            spotlight.color = Color.Lerp(oldColor, newColor, transitionTimer / colorTransitionLength);

            if (transitionTimer >= colorTransitionLength)
            {
                oldColor = newColor;
                transitioning = false;
            }
        }
    }


    public void changeSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    public void transitionWallColor(Color c)
    {
        transitioning = true;
        newColor = c;
        transitionTimer = 0.0f;
    }
}