/*
 * This class is a backbone for the player statistics it stores and keeps track of health and number of bullets
 * methods:
 * addBullets(int bullets) adds bullets can be negative but canot net <0
 * damage(int damageAmt) decreases player health if it nets <0 it will throw an exception which should be handled client side as a death screen
 * settrs/getters
 * getHealthPercentage() returns the percentage of health left in double
 * setHealth(int health) sets health has to be greater than 0
 * getHealth() returns health
 * getBullets() returns bullets
 * setBullets(int bullets) sets the bullets has to be greater than 0
 * @author Nikhil Alapati
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    /*
     * Instance variables
     * constrains: 
     * health cannot be negative
     * bullets cannot be negative
     */
    private int playerHealth;
    private int startHealth;
    private int gunBullets;
    /*
     * Default constructor sets health to 100 and bullets to 50
     */
    public Player()
    {
        this.playerHealth = 100;
        this.startHealth = 100;
        this.gunBullets = 50;
    }
    /*
     * C'tor with health and bulltes as parameters
     * health and bullets >0
     */
    public Player(int startingHealth, int bullets)
    {
        if (startingHealth >0 && bullets >0)
        {
            this.playerHealth = startingHealth;
            this.startHealth = startHealth;
            this.gunBullets = bullets;
        }
        else
        {
            throw (new System.ArgumentOutOfRangeException("starting health or bullets cannot be less than 0"));
        }
    }
    /*
     * copy c'tor
     */
    public Player(Player p)
    {
        this.setHealth = p.getHealth;
        this.setBullets = p.getBullets;
        this.startHealth = p.startHealth;
    }
    /*
     * adds bullets (can subtract too if input < 0 but cannot net to < 0)
     */
    public static void addBullets(int bullets)
    {
        if (this.gunBullets - bullets<0)
        {
            throw new System.ArgumentException("cannot have a negative result");
        }
        this.gunBullets += bullets;
    }
    /*
     * decreases health input has to be greater than 0
     */
    public static void damage(int damageAmt)
    {
        if (this.playerHealth - damageAmt>0)
        {
            this.playerHealth -= damageAmt;
        }
        else
        {
            throw (throw new System.ArgumentException("DEATH"));
        }
    }

    /*
     * getters and setters
     */
    public static double getHealthPercent()
    {
        return (this.playerHealth / this.startHealth)*100;
    }
    public void setBullets(int bullets)
    {
        if (bullets>0)
        {
            this.gunBullets = bullets;
        }
        throw (new System.ArgumentOutOfRangeException("bullets cannot be less than 1"));
    }
    public int getBullets()
    {
        return this.gunBullets;
    }

    public int getHealth()
    {
        return this.playerHealth;
    }
    public void setHealth(int health)
    {
        if (health >0) {
            this.playerHealth = health;
        }
        else
        {
            throw (new System.ArgumentOutOfRangeException("health cannot be negative dummy"));
        }
    }
}
