using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowmanMelt : MonoBehaviour
{
    public Player player;
    public Sprite[] forwardSprites;
    public Sprite[] backSprites;
    private SpriteRenderer spriteRenderer;
    /// <summary>
    /// 0 is dead state, max is maxHealth
    /// </summary>
    private int meltIndex = 0;
    private int maxIndex;
    private bool faceForward = true;
    // Start is called before the first frame update
    void Start()
    {
        maxIndex = forwardSprites.Length;
        player = GetComponent<Player>();

        spriteRenderer = GetComponent<SpriteRenderer>();
        meltIndex = player.maxHealth;
        updateSprite();
    }

    public void UpdateMeltStatus(int health)
    {
        if (meltIndex != health && health >= 0)
        {
            meltIndex = health;
            updateSprite();
        }
    }

    public void FaceDirection(bool forward)
    {
        if (forward != faceForward)
        {
            faceForward = forward;
            updateSprite();
        }
    }

    public void updateSprite()
    {
        if (meltIndex < 0 || meltIndex >= maxIndex)
            Debug.Log("out of bounds somehow. MeltIndex: " + meltIndex + " MaxIndex: " + maxIndex);
        if (faceForward)
            spriteRenderer.sprite = forwardSprites[meltIndex];
        else
            spriteRenderer.sprite = backSprites[meltIndex];
        
    }
}
