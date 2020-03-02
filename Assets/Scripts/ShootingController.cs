using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingController : MonoBehaviour
{
    private Player player;
    public GameObject ammo;
    public float snowBallSpeed = 1000f;
    public float shootingOffset;
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(ammo != null);
        player = GetComponent<Player>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateRotation();

        if (Input.GetButtonDown(player.getInputName(Player.InputName.RightBumper)))
        {
            ThrowSnowball();
        }
    }

    private void UpdateRotation()
    {
        Vector2 rightStick = new Vector2(
            Input.GetAxis(player.getInputName(Player.InputName.RightHorizontal)),
            Input.GetAxis(player.getInputName(Player.InputName.RightVertical))
            );

        float angularVelocity = 12f;

        var direction = new Vector3(rightStick.x, rightStick.y, 0);


        float radialDeadZone = 0.25f;
        if (direction.magnitude > radialDeadZone)
        {
            rb.angularVelocity = 0f;
            var currentRotation = Quaternion.LookRotation(Vector3.forward, direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, currentRotation, Time.deltaTime * angularVelocity);
        }
    }

    private void ThrowSnowball()
    {
        Debug.Assert(ammo != null);
        Vector3 spawnPosition = transform.up * shootingOffset;
        
        GameObject snowball = Instantiate(ammo, transform.position + spawnPosition, transform.rotation);
        Rigidbody2D snowballRb = snowball.GetComponent<Rigidbody2D>();

        snowballRb.velocity = rb.velocity;
        Vector3 motion = snowballRb.transform.up * snowBallSpeed;
        snowballRb.AddForce(new Vector2(motion.x, motion.y));
    }

}
