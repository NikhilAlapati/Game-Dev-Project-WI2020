using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    public float speedOnSnow=1;
    private float speedOnIce;
    private bool onSnow = false;
    private SpriteRenderer spriteRenderer;
    private Player player;
    private Animator anim;
    public bool facingUp = true;
    private SnowmanMelt snowmanMelt;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        //set at start because cant initalize the value beforehand
        speedOnIce = speedOnSnow*2;
        spriteRenderer = GetComponent<SpriteRenderer>();
        player = GetComponentInParent<Player>();
        anim = GetComponent<Animator>();
        //Debug.Assert(anim != null, "Must have an animator component for movement");
        if (!player.isAbSnowman)
            snowmanMelt = GetComponent<SnowmanMelt>();
    }


    // Update is called once per frame
    void Update()
    {
        if (!player.isAlive)
            return;

        float moveX, moveY;

        if (Input.GetKeyDown(KeyCode.Space))
            onSnow = !onSnow;

        float actualSpeed;
        if (onSnow)
        {
            moveX = Input.GetAxisRaw(player.getInputName(Player.InputName.LeftHorizontal));
            moveY = Input.GetAxisRaw(player.getInputName(Player.InputName.LeftVertical));
            actualSpeed = speedOnSnow;
        }
        else
        {
            Debug.Assert(player != null);
            moveX = Input.GetAxis(player.getInputName(Player.InputName.LeftHorizontal));
            moveY = Input.GetAxis(player.getInputName(Player.InputName.LeftVertical));

            //moveY = Input.GetAxis("Vertical");
            actualSpeed = speedOnIce;
        }
        //Debug.Log("actual speed: " + actualSpeed);
        if (!player.isAbSnowman)
        {
            actualSpeed *= 7 / (float)(player.playerHealth + 8);
            if (actualSpeed < 0)
                actualSpeed = 0;
            //Debug.Log("act speed after calc: " + actualSpeed);
        }

        if (moveY < 0 && !facingUp)
        {
            if (!player.isAbSnowman)
                snowmanMelt.FaceDirection(true);
            else
                anim.SetBool("FacingForward", true);

            facingUp = true;
        }
        else if (moveY > 0 && facingUp)
        {
            if (!player.isAbSnowman)
                snowmanMelt.FaceDirection(false);
            else
                anim.SetBool("FacingForward", false);
            facingUp = false;
        }

        rb.velocity = new Vector2(moveX * actualSpeed, moveY * actualSpeed);
    }

    // keeps the player facing forward instead of spinning on collisions
    private void UpdateRotation()
    {
        rb.angularVelocity = 0f;
        transform.forward = Vector3.forward;
    }

    public void SetOnSnow(bool isOnSnow)
    {
        onSnow = isOnSnow;
    }
    
}
