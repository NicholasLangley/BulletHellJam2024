using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float xSpeed = 0.1f;
    public float ySpeed = 0.1f;

    [SerializeField]
    float minX, maxX;

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
        float moveVertical = Input.GetAxisRaw("Vertical") * xSpeed * Time.deltaTime;
        float moveHorizontal = Input.GetAxisRaw("Horizontal") * ySpeed * Time.deltaTime;

        Vector3 newPos = transform.localPosition;
        newPos.y += moveVertical;
        newPos.x = Mathf.Clamp(newPos.x + moveHorizontal, minX, maxX);

        transform.localPosition = newPos;
    }
}
