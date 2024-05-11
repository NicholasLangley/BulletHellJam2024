using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float xSpeed = 0.1f;
    public float ySpeed = 0.1f;

    // Start is called before the first frame update
    void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        move();
    }


    public void move()
    {
        float moveVertical = Input.GetAxisRaw("Vertical") * xSpeed;
        float moveHorizontal = Input.GetAxisRaw("Horizontal") * ySpeed;

        Vector3 newPos = transform.localPosition;
        newPos.y += moveVertical;
        newPos.x += moveHorizontal;

        transform.localPosition = newPos;
    }
}
