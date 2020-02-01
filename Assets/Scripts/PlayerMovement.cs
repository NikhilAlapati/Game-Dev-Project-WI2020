using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private float moveX, moveY;
    public float speedOnSnow=1;
    private float speedOnIce;
    public bool onSnow;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //set at start because cant initalize the value beforehand
        speedOnIce = speedOnSnow*2;
    }

    // Update is called once per frame
    void Update()
    {
        float actualSpeed;
        if (onSnow)
        {
            moveX = Input.GetAxisRaw("Horizontal");
            moveY = Input.GetAxisRaw("Vertical");
            actualSpeed = speedOnSnow;
        }
        else
        {
            moveX = Input.GetAxis("Horizontal");
            moveY = Input.GetAxis("Vertical");
            actualSpeed = speedOnIce;
        }
        rb.velocity = new Vector2(moveX * actualSpeed, moveY * actualSpeed);
    }
}
