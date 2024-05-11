using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallController : MonoBehaviour
{

    public float speed = 1.0f;
    [SerializeField]
    float backgroundSpeedFraction = 0.7f;

    [SerializeField]
    Renderer bgRenderer, rightRenderer, leftRenderer;

    // Start is called before the first frame update
    void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        bgRenderer.material.mainTextureOffset += Vector2.down * speed * backgroundSpeedFraction * Time.deltaTime;
        rightRenderer.material.mainTextureOffset += Vector2.down * speed * Time.deltaTime;
        leftRenderer.material.mainTextureOffset += Vector2.down * speed * Time.deltaTime;
    }


    public void changeSpeed(float newSpeed)
    {
        speed = newSpeed;
    }
}