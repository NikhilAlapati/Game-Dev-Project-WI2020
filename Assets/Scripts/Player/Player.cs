﻿/*
 * Component for each player character
 * Controller and Player number are stored here while defined in PlayerManager
 * UI will use playerHealth, ammoCount, playerNumber, and teamNumber
 * @authors: Nikhil Alapati, Josh Kennedy
 */
using UnityEngine;

public class Player : MonoBehaviour
{
    public int playerHealth = 100;
    public int startHealth = 100;
    public int ammoCount = 50;

    // the controller number assigned by Unity's Input Controller
    private int controllerNumber { get; set; }
    // the player number assigned by PlayerManager dependent on when a player joins
    private int playerNumber { get; set; }
    // chosen by the player
    private int teamNumber { get; set; }
    private Team currentTeam;

    private bool isAlive = true;

    private void Update()
    {
        if (Input.GetButtonDown(getInputName(InputName.RightFace)))
            dropOut();
        /*if (Input.GetButtonDown(getInputName(InputName.TopFace)))
            setTeam(1);*/
        if (Input.GetKeyDown(KeyCode.K))
            killPlayer();
    }

    public enum InputName
    {
        LeftFace, RightFace, TopFace, DownFace,
        LeftHorizontal, LeftVertical,
        RightHorizontal, RightVertical,
        RightBumper
    }

    public string getInputName(InputName inputName)
    {
        return "P" + this.controllerNumber + inputName.ToString();
    }

    public void setTeam(Team team)
    {
        this.teamNumber = team.teamNumber;
        GetComponent<Renderer>().material.color = team.teamColor;
        team = GetComponentInParent<PlayerManager>().GetTeam(this.teamNumber);
        team.addMember();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("2d collision trigger enter");
        Team teamAssignment = collision.gameObject.GetComponent<Team>();

        if (teamAssignment != null && teamAssignment != currentTeam)
            setTeam(collision.gameObject.GetComponent<Team>());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Snowball snowBall = collision.gameObject.GetComponent<Snowball>();
        if (snowBall != null)
            damagePlayer(snowBall.damage);

    }


    public void dropOut()
    {
        //team.removeMember();
        GetComponentInParent<PlayerManager>().DropPlayer(this.playerNumber);
    }

    public int getPlayerNumber()
    {
        return this.playerNumber;
    }
    public void setPlayerNumber(int number)
    {
        this.playerNumber = number;
    }

    public int getControllerNumber()
    {
        return this.controllerNumber;
    }
    public void setControllerNumber(int number)
    {
        this.controllerNumber = number;
    }


    public void increaseAmmo(int amount)
    {
        this.ammoCount += amount;
    }

    public bool decreaseAmmo(int amount)
    {
        if (this.ammoCount - amount < 0)
            return false;
        else
        {
            this.ammoCount -= amount;
            return true;
        }
    }

    public void damagePlayer(int damage)
    {
        this.playerHealth -= damage;
        if (this.playerHealth < 0)
            killPlayer();
    }

    private void killPlayer()
    {
        Debug.Log("Player " + this.playerNumber + " is dead");
        currentTeam.memberDied();
        // Play death animation
        // Remove object
        gameObject.SetActive(false);
    }

    private void revivePlayer()
    {
        currentTeam.memberRevived();
        // Set health to above 0
    }

    public float getHealthPercentage()
    {
        return this.playerHealth / this.startHealth * 100;
    }

    public int getAmmoCount()
    {
        return this.ammoCount;
    }

}
