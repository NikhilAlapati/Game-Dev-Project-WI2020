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
    private Material material;
    private Player playerParent;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //set at start because cant initalize the value beforehand
        speedOnIce = speedOnSnow*2;
        material = GetComponent<MeshRenderer>().material;
        this.playerParent = GetComponentInParent<Player>();
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown(playerParent.getInputName(Player.InputName.LeftFace)))
            Debug.Log(playerParent.getInputName(Player.InputName.LeftFace) + " was pressed!");

        if (Input.GetButtonDown(playerParent.getInputName(Player.InputName.RightFace)))
            Debug.Log(playerParent.getInputName(Player.InputName.RightFace) + " was pressed!");

        if (Input.GetButtonDown(playerParent.getInputName(Player.InputName.TopFace)))
            Debug.Log(playerParent.getInputName(Player.InputName.TopFace) + " was pressed!");

        if (Input.GetButtonDown(playerParent.getInputName(Player.InputName.DownFace)))
            Debug.Log(playerParent.getInputName(Player.InputName.DownFace) + " was pressed!");

        if (Input.GetKeyDown(KeyCode.Space))
            onSnow = !onSnow;

        float actualSpeed;
        if (onSnow)
        {
            moveX = Input.GetAxisRaw(playerParent.getInputName(Player.InputName.Horizontal));
            moveY = Input.GetAxisRaw(playerParent.getInputName(Player.InputName.Vertical));
            actualSpeed = speedOnSnow;
        }
        else
        {
            moveX = Input.GetAxis(playerParent.getInputName(Player.InputName.Horizontal));
            moveY = Input.GetAxis(playerParent.getInputName(Player.InputName.Vertical));

            //moveY = Input.GetAxis("Vertical");
            actualSpeed = speedOnIce;
        }

        //transform.localScale = Vector3.one + new Vector3(Input.GetAxis("P" + playerNumber + "Fire1"), 0, 0);

        //material.color = new Color(Input.GetAxis("P" + playerNumber + "Fire1"), 1, 1);

        rb.velocity = new Vector2(moveX * actualSpeed, moveY * actualSpeed);
    }

    void OnMove()
    {
        Debug.Log("moving");
    }
}
