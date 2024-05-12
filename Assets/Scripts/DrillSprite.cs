using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrillSprite : MonoBehaviour
{

    //vibration animation
    Vector3 originalPos;
    bool vibrating = false;
    public float vibrateAmount;
    public float vibrateTime = 0.1f;

    float timer;

    // Start is called before the first frame update
    void Awake()
    {
        originalPos = transform.localPosition;
        startVibrating();
        timer = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (vibrating && timer > vibrateTime) { vibrate(); timer = 0.0f; }
    }

    void vibrate()
    {
        Vector3 vib = Random.insideUnitSphere * vibrateAmount;
        Vector3 newPos = originalPos;
        newPos.x += vib.x;
        newPos.y += vib.y;
        transform.localPosition = newPos;


    }

    public void startVibrating()
    {
        vibrating = true;
    }

    public void stopVibrating()
    {
        vibrating = false;
        transform.localPosition = originalPos;
    }
}
