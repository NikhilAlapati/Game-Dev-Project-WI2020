using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snowball : MonoBehaviour
{

    public int damage = 1;
    public Player thrower;
    public Team team;
    public Animator anim;
    public AudioSource audioSource;

    public AudioClip[] playerHitSounds;
    public SoundPool playerHitSoundPool = new SoundPool();

    public AudioClip[] wallHitSounds;
    public SoundPool wallHitSoundPool = new SoundPool();

    private Rigidbody2D rb;

    private bool alreadyHit = false;

    private void Start()
    {
        playerHitSoundPool.soundsInPool = playerHitSounds;
        wallHitSoundPool.soundsInPool = wallHitSounds;

        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        Invoke("Drop", 1f);
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (alreadyHit)
            return;

        // hitting a player or another snowball or wall
        if (collision.gameObject.GetComponent<Player>() != null || collision.gameObject.GetComponent<Snowball>() != null || collision.gameObject.CompareTag("wall"))
            audioSource.clip = playerHitSoundPool.GetRandomSound();
        // disregard
        else
        {
            return;
        }

        alreadyHit = true;
        audioSource.Play();
        Explode();
    }

    private void Drop()
    {
        // slows the snowball down as if it hit the ground
        rb.velocity = rb.velocity * 0.2f;
        Explode();
    }

    private void Explode()
    {
        // play sound
        rb.freezeRotation = true;
        transform.Rotate(Vector3.forward, Random.Range(0, 360));
        anim.SetTrigger("Explode");
    }

    private void SelfDestruct()
    {
        Destroy(gameObject);
    }
}
