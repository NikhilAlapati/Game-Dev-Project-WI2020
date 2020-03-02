using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snowball : MonoBehaviour
{
    public int damage = 50;

    private void Start()
    {
        Invoke("SelfDestruct", 10);
    }

    private void SelfDestruct()
    {
        Destroy(gameObject);
    }
}
