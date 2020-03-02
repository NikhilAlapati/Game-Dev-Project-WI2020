using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Team : MonoBehaviour
{   
    public int teamNumber;
    private bool active;

    private int totalMembers = 0;
    private int activeMembers = 0;

    public int gameWins = 0;
    public int gameLosses = 0;

    public int roundWins = 0;
    public int roundLosses = 0;

    public Color teamColor;
    public Text scoreboard;

    private void Start()
    {
        teamColor = GetComponent<Renderer>().material.color;
    }

    public Team(int teamNumber)
    {
        this.teamNumber = teamNumber;
        this.active = true;
    }
    public bool isTeamActive()
    {
        return this.active;
    }

    public void addMember()
    {
        ++totalMembers;
    }
    public void removeMember()
    {
        --totalMembers;
    }
    // call when a round starts
    public void activateTeam()
    {
        activeMembers = totalMembers;
        this.active = true;
    }
    public void memberDied()
    {
        Debug.Log("teammember died: " + activeMembers);
        --activeMembers;
        if (activeMembers < 1)
            teamLostRound();
    }
    public void memberRevived()
    {
        ++activeMembers;
        if (activeMembers > totalMembers)
            Debug.LogError("active members on a team is greater than the total members");
    }

    private void teamLostRound()
    {
        Debug.Log("team lost round" + activeMembers);

        this.active = false;
        ++roundLosses;
        PlayerManager.Instance.TeamLostRound(this.teamNumber);
        // TODO: call game manager
        Debug.Log("Team lost");
    }

    public void teamWonRound()
    {
        ++roundWins;
        if (scoreboard != null)
            scoreboard.text = roundWins.ToString();
        else
            Debug.Log("scoreboard is not set team number: " + teamNumber);
    }

    public void teamLostGame()
    {
        --gameLosses;
    }

    public void teamWonGame()
    {
        ++gameWins;
    }


    


}
