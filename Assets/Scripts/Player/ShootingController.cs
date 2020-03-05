using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingController : MonoBehaviour
{
    private Player player;
    public GameObject ammo;
    public float snowBallSpeed = 1000f;
    public float shootingOffset = 1f;
    private Rigidbody2D rb;
    private Animator anim;
    public GameObject reticle;
    public float reticleOffset = 2f;
    private PlayerMovement playerMovement;
    private SnowmanMelt snowmanMelt;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(ammo != null);
        player = GetComponent<Player>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        if (!player.isAbSnowman)
            snowmanMelt = GetComponent<SnowmanMelt>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!player.isAlive)
            return;

        if (Input.GetButtonDown(player.getInputName(Player.InputName.RightBumper)))
        {
            ThrowSnowball();
        }

        Vector2 rightStick = getRightStick();
        updatePlayerDirection(rightStick);
        updateReticlePosition(rightStick);
        updateReticleRotation(rightStick);
    }

    private Vector2 getRightStick()
    {
        return new Vector2(
            Input.GetAxis(player.getInputName(Player.InputName.RightHorizontal)),
            Input.GetAxis(player.getInputName(Player.InputName.RightVertical))
            );

    }

    private void ThrowSnowball()
    {
        Debug.Assert(ammo != null);
        if (player.isAbSnowman)
            anim.SetTrigger("Throw");

        else
            player.damagePlayer(1);



        // direction to throw snowball
        Vector3 spawnPosition = reticle.transform.position;
        Vector3 heading  = reticle.transform.position - transform.position;

        updatePlayerDirection(heading);


        // constant distance from player
        heading.Normalize();
        heading *= shootingOffset;

        // new snowball
        GameObject snowball = Instantiate(ammo, transform.position + heading, reticle.transform.rotation);
        // set thrower to avoid self-damage and friendly-fire
        snowball.GetComponent<Snowball>().thrower = player;

        // give the snowball a relative velocity based on the player's velocity
        Rigidbody2D snowballRb = snowball.GetComponent<Rigidbody2D>();
        snowballRb.velocity = rb.velocity;
        Vector3 motion = snowballRb.transform.up * snowBallSpeed;
        snowballRb.AddForce(new Vector2(motion.x, motion.y));
    }

    private void updatePlayerDirection(Vector2 rightStick)
    {
        if (rightStick.y > 0)
        {
            if (player.isAbSnowman)
                anim.SetBool("FacingForward", false);
            else
                snowmanMelt.FaceDirection(false);

            playerMovement.facingUp = false;
        }
        else if (rightStick.y < 0)
        {
            if (player.isAbSnowman)
                anim.SetBool("FacingForward", true);
            else
                snowmanMelt.FaceDirection(true);

            playerMovement.facingUp = true;
        }

    }

    private void updateReticlePosition(Vector2 rightStick)
    {
        Vector3 displacement = new Vector3(rightStick.x, rightStick.y, 0);
        if (rightStick.x == 0 && rightStick.y == 0)
            return;

        displacement.Normalize();
        displacement *= reticleOffset;
        reticle.transform.position = transform.position + displacement;
    }

    private void updateReticleRotation(Vector2 rightStick)
    {
        float angularVelocity = 12f;

        var direction = new Vector3(rightStick.x, rightStick.y, 0);

        float radialDeadZone = 0.25f;
        if (direction.magnitude > radialDeadZone)
        {
            var currentRotation = Quaternion.LookRotation(Vector3.forward, direction);
            reticle.transform.rotation = Quaternion.Lerp(reticle.transform.rotation, currentRotation, Time.deltaTime * angularVelocity);
        }
    }

}
