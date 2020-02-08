using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    private const int MAX_PLAYERS = 16;
    // all the child objects with player components
    Player[] players;

    // just left and right team at the moment
    private const int MAX_TEAMS = 2;
    Team[] teams = new Team[MAX_TEAMS];

    // a mapping of controllers to players
    // controllerMappings[controllerNumber] = playerNumber
    int[] controllerMappings = new int[MAX_PLAYERS+1];

    void Start()
    {
        for (int i = 0; i < MAX_TEAMS; i++)
            teams[i] = new Team(this, i);

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
            StartRound();
    }

    private void AssignPlayer(int controllerNumber)
    {
        Debug.Log("assigning player");
        for (int playerNumber = 0; playerNumber < MAX_PLAYERS; playerNumber++)
        {
            // if player is unassigned
            if (!players[playerNumber].gameObject.activeInHierarchy)
            {
                Debug.Log("Player Assigned!");
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

    private void StartRound()
    {
        foreach (Team team in teams)
            team.activateTeam();
    }

    public void TeamLostRound(int loserNumber)
    {
        int teamsRemaining = 0;
        int winningTeam = -1;
        for (int i = 0; i < teams.Length; i++) {
            if (teams[i].isTeamActive())
            {
                ++teamsRemaining;
                winningTeam = i;
            }
        }

        if (teamsRemaining == 1)
        {
            teams[winningTeam].teamWonRound();
        }
    }
}
