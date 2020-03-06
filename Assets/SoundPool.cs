using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPool
{
    public AudioClip[] soundsInPool;
    private int currentSoundIndex = 0;

    public AudioClip GetRandomSound()
    {
        return GetRandomSound(0);
    }

    public AudioClip GetRandomSound(ulong delay)
    {
        if (soundsInPool[0] == null)
        {
            Debug.Log("sounds index 0 is == null");
            return null;
        }
        else if (soundsInPool[soundsInPool.Length - 1] == null)
        {
            Debug.Log("sounds index " + (soundsInPool.Length - 1) + " is == null");
            return null;
        }

        int randomClipIndex = Random.Range(0, soundsInPool.Length -1);
        return GetSoundAtIndex(randomClipIndex, delay);
    }

    public AudioClip GetSoundInOrder()
    {
        return GetSoundInOrder(0);
    }

    public AudioClip GetSoundInOrder(ulong delay)
    {
        if (currentSoundIndex > soundsInPool.Length - 1)
            currentSoundIndex = 0;
        else
            currentSoundIndex++;
        return GetSoundAtIndex(currentSoundIndex, delay);

    }
    private AudioClip GetSoundAtIndex(int index)
    {
        return GetSoundAtIndex(index, 0);
    }


    private AudioClip GetSoundAtIndex(int index, ulong delay)
    {
        if (soundsInPool[index] == null)
        {
            Debug.Log("sounds index " + index + " is == null");
            return null;
        }

        return soundsInPool[index];

    }
}
