/*
 * Component for each player character
 * Controller and Player number are stored here while defined in PlayerManager
 * UI will use playerHealth, ammoCount, playerNumber, and teamNumber
 * @authors: Nikhil Alapati, Josh Kennedy
 */
using UnityEngine;

public class Player : MonoBehaviour
{
    public int playerHealth = 5;
    public int maxHealth = 5;
    //public int ammoCount = 50;

    // the controller number assigned by Unity's Input Controller
    private int controllerNumber { get; set; }
    // the player number assigned by PlayerManager dependent on when a player joins
    private int playerNumber { get; set; }
    // chosen by the player
    private int teamNumber { get; set; }
    private Team currentTeam;

    private SpriteRenderer spriteRenderer;

    public bool isAlive = true;
    public bool reviving = false;
    public bool isAbSnowman = true;
    private SnowmanMelt snowmanMelt;
    private Animator anim;
    private Rigidbody2D rb;
    public AudioClip deathSound;
    private AudioSource audioSource;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        if (!isAbSnowman)
            snowmanMelt = GetComponent<SnowmanMelt>();
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
    }

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
        LeftBumper, RightBumper
    }

    public string getInputName(InputName inputName)
    {
        return "P" + this.controllerNumber + inputName.ToString();
    }

    public void setTeam(Team team)
    {
        if (team == currentTeam)
            return;
        if (currentTeam != null)
            currentTeam.removeMember(this);

        this.teamNumber = team.teamNumber;
        spriteRenderer.color = team.teamColor;
        currentTeam = GetComponentInParent<PlayerManager>().GetTeam(this.teamNumber);

        // only abSnowman players are important for wins/losses
        if (isAbSnowman)
            currentTeam.addMember();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Team teamAssignment = collision.gameObject.GetComponent<Team>();

        if (teamAssignment != null && teamAssignment != currentTeam)
            setTeam(collision.gameObject.GetComponent<Team>());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Snowball snowBall = collision.gameObject.GetComponent<Snowball>();
        if (snowBall != null)
            hitBySnowball(snowBall);

    }

    private void hitBySnowball(Snowball snowball)
    {
        if (isAbSnowman)
            AbSnowmanHitBySnowball(snowball);
        else
            SnowmanHitBySnowBall(snowball);

    }

    private void SnowmanHitBySnowBall(Snowball snowball)
    {
        healPlayer(snowball.damage);
    }

    private void AbSnowmanHitBySnowball(Snowball snowball)
    {
        if (snowball.thrower == this)
            return;
        if (this.currentTeam != null && snowball.thrower.currentTeam == this.currentTeam) // no friendly fire at the moment
            return;

        damagePlayer(snowball.damage);
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


    public void damagePlayer(int damage)
    {
        if (!isAlive)
            return;

        this.playerHealth -= damage;

        
        if (this.playerHealth < 0)
            killPlayer();

        else if (!isAbSnowman)
            snowmanMelt.UpdateMeltStatus(playerHealth);
    }

    public void healPlayer(int healthIncrease)
    {
        if (!isAlive && isAbSnowman)
            return;
        else if (!isAbSnowman)
        {
            revivePlayer(0);
        }
        // already at max
        if (playerHealth >= maxHealth)
            return;
        playerHealth += healthIncrease;
        // can't go higher than max
        if (playerHealth > maxHealth)
            playerHealth = maxHealth;
        if (!isAbSnowman)
            snowmanMelt.UpdateMeltStatus(playerHealth);
    }

    private void killPlayer()
    {
        Debug.Log("Player " + this.playerNumber + " is dead");
        rb.velocity = Vector2.zero;
        isAlive = false;
        if (deathSound != null)
        {
            audioSource.clip = deathSound;
            audioSource.PlayOneShot(deathSound, 0.5f);
        }
        spriteRenderer.color = Color.gray;
        if (currentTeam == null)
        {
            slowRevive(3);
            return;
        }
        // only abSnowman players are important for wins/losses
        if (isAbSnowman)
            currentTeam.memberDied();
        // Play death animation
        
        
    }

    public void slowRevive(float seconds)
    {
        if (!reviving && !isAlive)
        {
            reviving = true;
            Invoke("fullRevive", seconds);
        }
    }

    public void fullRevive()
    {
        playerHealth = maxHealth;
        revivePlayer(maxHealth);
    }

    public void revivePlayer(int healthAmount)
    {
        reviving = false;
        playerHealth += healthAmount;
        if (playerHealth > maxHealth)
            playerHealth = maxHealth;
        if (!isAbSnowman)
            GetComponent<SnowmanMelt>().UpdateMeltStatus(playerHealth);

        if (!isAlive)
        {
            if (currentTeam != null)
            {
                if (isAbSnowman)
                    currentTeam.memberRevived();

                spriteRenderer.color = currentTeam.teamColor;
            }
            else
            {
                spriteRenderer.color = Color.white;
            }
            isAlive = true;
        }
        if (!isAbSnowman)
            snowmanMelt.UpdateMeltStatus(playerHealth);

    }

    public void respawn()
    {
        if (currentTeam == null)
            transform.position = PlayerManager.Instance.transform.position;
        else
            transform.position = currentTeam.getSpawnPoint();
    }

    public float getHealthPercentage()
    {
        return this.playerHealth / this.maxHealth * 100;
    }

}
