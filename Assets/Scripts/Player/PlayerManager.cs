using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private const int MAX_PLAYERS = 8;
    // all the child objects with player components
    Player[] players;

    // just left and right team at the moment
    private const int MAX_TEAMS = 2;
    public Dictionary<int, Team> teams;

    // a mapping of controllers to players
    // controllerMappings[controllerNumber] = playerNumber
    int[] controllerMappings = new int[MAX_PLAYERS+1];
    
    // Singleton implementation
    private static PlayerManager _instance;
    public static PlayerManager Instance { get { return _instance; } }

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
        Debug.Assert(teams.Count > 1, "Must have more than one team for a game");

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
            if (controllerMappings[controllerNum] == 0 && Input.GetButtonDown("P" + controllerNum + "DownFace"))
                AssignPlayer(controllerNum);
        }
        if (Input.GetKeyDown(KeyCode.Space))
            StartNewRound();
    }

    private void AssignPlayer(int controllerNumber)
    {
        for (int playerNumber = 0; playerNumber < MAX_PLAYERS; playerNumber++)
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

    public void StartNewRound()
    {
        foreach (KeyValuePair<int, Team> team in teams)
            team.Value.activateTeam();
    }

    public void TeamLostRound(int loserNumber)
    {
        int teamsRemaining = 0;
        int winningTeam = -1;
        foreach (KeyValuePair<int, Team> team in teams) { 
            if (team.Value.isTeamActive())
            {
                ++teamsRemaining;
                winningTeam = team.Key;
            }
        }

        if (teamsRemaining == 1)
        {
            teams[winningTeam].teamWonRound();
        }
    }
}
