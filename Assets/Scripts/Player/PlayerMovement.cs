using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    public float speedOnSnow=1;
    private float speedOnIce;
    public bool onSnow;
    private SpriteRenderer spriteRenderer;
    private Player playerParent;
    private Animator anim;
    public bool facingForward = true;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        //set at start because cant initalize the value beforehand
        speedOnIce = speedOnSnow*2;
        spriteRenderer = GetComponent<SpriteRenderer>();
        this.playerParent = GetComponentInParent<Player>();
        anim = GetComponent<Animator>();
        //Debug.Assert(anim != null, "Must have an animator component for movement");
    }


    // Update is called once per frame
    void Update()
    {
        float moveX, moveY;

        if (Input.GetKeyDown(KeyCode.Space))
            onSnow = !onSnow;

        float actualSpeed;
        if (onSnow)
        {
            moveX = Input.GetAxisRaw(playerParent.getInputName(Player.InputName.LeftHorizontal));
            moveY = Input.GetAxisRaw(playerParent.getInputName(Player.InputName.LeftVertical));
            actualSpeed = speedOnSnow;
        }
        else
        {
            Debug.Assert(playerParent != null);
            moveX = Input.GetAxis(playerParent.getInputName(Player.InputName.LeftHorizontal));
            moveY = Input.GetAxis(playerParent.getInputName(Player.InputName.LeftVertical));

            //moveY = Input.GetAxis("Vertical");
            actualSpeed = speedOnIce;
        }

        //transform.localScale = Vector3.one + new Vector3(Input.GetAxis("P" + playerNumber + "Fire1"), 0, 0);

        //material.color = new Color(Input.GetAxis("P" + playerNumber + "Fire1"), 1, 1);
        if (moveY < 0 && !facingForward)
        {
            anim.SetBool("FacingForward", true);
            facingForward = true;
        }
        else if (moveY > 0 && facingForward)
        {
            anim.SetBool("FacingForward", false);
            facingForward = false;
        }

        rb.velocity = new Vector2(moveX * actualSpeed, moveY * actualSpeed);
    }

    // keeps the player facing forward instead of spinning on collisions
    private void UpdateRotation()
    {
        rb.angularVelocity = 0f;
        transform.forward = Vector3.forward;
    }
}
