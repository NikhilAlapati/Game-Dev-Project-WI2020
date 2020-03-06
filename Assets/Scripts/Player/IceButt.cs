using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceButt : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private void Start()
    {
        playerMovement = GetComponentInParent<PlayerMovement>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("IceTile"))
            playerMovement.SetOnSnow(false);

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("IceTile"))
            playerMovement.SetOnSnow(true);
    }
}
