using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    private const int MAX_PLAYERS = 8;
    // all the child objects with player components
    Player[] players;

    // just left and right team at the moment
    //private const int MAX_TEAMS = 2;
    public GameObject teamHolder;
    private Team[] teams;

    bool isCountDown = false;
    int ticksPerSecond = 60;
    int countDownTicks = 0;
    int countDownSeconds = 3;
    public Text countDownNumber;

    // a mapping of controllers to players
    // controllerMappings[controllerNumber] = playerNumber
    int[] controllerMappings = new int[MAX_PLAYERS+1];
    
    // Singleton implementation
    private static PlayerManager _instance;
    public static PlayerManager Instance { get { return _instance; } }

    public int roundCount = 0;

    private void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(this.gameObject);
        else
            _instance = this;
    }
    // End of Singleton Implementation

    void Start()
    {
        teams = teamHolder.GetComponentsInChildren<Team>();
        Debug.Assert(teams.Length > 1, "Must have more than one team for a game");
        for (int i = 0; i < teams.Length; i++)
            teams[i].teamNumber = i;

        players = GetComponentsInChildren<Player>();
        // ensure there are no active players in the beginning
        foreach (Player player in players)
        {
            player.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        for (int controllerNum = 1; controllerNum < MAX_PLAYERS + 1; controllerNum++)
        {
            // if controller is unassigned to a player and has pressed the downface button
            if (controllerMappings[controllerNum] == 0
                && Input.GetButtonDown("P" + controllerNum + "DownFace"))
                AssignPlayer(controllerNum);
        }

        if (Input.GetKeyDown(KeyCode.Space))
            StartNewRoundTimer();

        CountDownTimer();
    }

    private void CountDownTimer()
    {
        if (!isCountDown)
            return;

        ++countDownTicks;
        if (countDownTicks >= ticksPerSecond)
        {
            countDownTicks = 0;
            --countDownSeconds;

            if (countDownSeconds > 0)
                countDownNumber.text = countDownSeconds.ToString();

            else if (countDownSeconds == 0)
            {
                countDownNumber.text = "Go!";
                StartNewRound();
            }
            else
            {
                countDownNumber.text = "";
                isCountDown = false;
            }
        }
    }

    private void AssignPlayer(int controllerNumber)
    {
        for (int playerNumber = 0; playerNumber < players.Length; playerNumber++)
        {
            // if player is unassigned
            if (!players[playerNumber].gameObject.activeInHierarchy)
            {
                // activate the player object
                players[playerNumber].gameObject.SetActive(true);
                // assign player playerNumber and controllerNumber
                players[playerNumber].setPlayerNumber(playerNumber + 1);
                players[playerNumber].setControllerNumber(controllerNumber);
                // add it to the controller mappings
                controllerMappings[controllerNumber] = playerNumber + 1;
                return;
            }
        }
    }
    public void DropPlayer(int playerNumber)
    {
        // remove the mapping
        controllerMappings[players[playerNumber-1].getControllerNumber()] = 0;

        // TODO: reset the player position and stats

        // turn off the player
        players[playerNumber-1].gameObject.SetActive(false);
    }

    public Team GetTeam(int teamNumber)
    {
        return teams[teamNumber];
    }

    public void StartNewRoundTimer()
    {
        countDownSeconds = 3;
        countDownNumber.text = countDownSeconds.ToString();
        isCountDown = true;
    }

    public void StartNewRound()
    {
        ++roundCount;
        Debug.Log("Round " + roundCount + " has begun");
        ReviveAllPlayers();
        foreach (Team team in teams)
            team.activateTeam();
    }

    public void ReviveAllPlayers()
    {
        foreach(Player player in players)
        {
            player.fullRevive();
        }
    }

    public void TeamLostRound(int loserNumber)
    {
        int teamsRemaining = 0;
        int winningTeam = -1;
        foreach (Team team in teams) { 
            if (team.isTeamActive())
            {
                Debug.Log("team " + team.teamNumber + " is active");
                ++teamsRemaining;
                winningTeam = team.teamNumber;
            }
        }
        Debug.Log("There are a total of " + teams.Length + " teams and there are " + teamsRemaining + " teams remaining");
        if (teamsRemaining == 1)
        {
            Debug.Log("Team " + winningTeam + " has won");
            teams[winningTeam].teamWonRound();
        }
    }
}
