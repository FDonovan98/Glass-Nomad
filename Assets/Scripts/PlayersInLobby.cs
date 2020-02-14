using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public class PlayersInLobby : MonoBehaviour
{
    public List<PlayerInfo> playerInfos = new List<PlayerInfo>();

    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public string[] GetPlayerNames()
    {
        return (from player in playerInfos select player.name).ToArray();
    }

    public bool[] GetPlayerBools()
    {
        return (from player in playerInfos select player.isAlien).ToArray();
    }

    public void PlayerJoinedLobby(string playerName, bool alien)
    {
        playerInfos.Add(new PlayerInfo(playerName, alien));
        Debug.Log(playerName + " has joined the lobby.");
    }

    public void PlayerLeftLobby(string playerName)
    {
        PlayerInfo playerToRemove = playerInfos.Single(player => player.name == playerName);
        
        if (playerToRemove.isAlien && playerInfos.Count > 1)
        {
            // Finds the first player that isn't an alien, and assigns it as the new alien.
            PlayerInfo newAlien = playerInfos.First(player => !player.isAlien);
            AlienChanged(newAlien.name);
        }

        Debug.Log(playerToRemove.name + " has left the lobby.");

        playerInfos.Remove(playerToRemove);
    }

    public bool IsPlayerAlien(string playerName)
    {
        return playerInfos.Single(player => player.name == playerName).isAlien;
    }

    public void AlienChanged(string nameOfNewAlien)
    {
        PlayerInfo oldAlien = playerInfos.Single(player => player.isAlien);
        oldAlien.isAlien = false;

        nameOfNewAlien = nameOfNewAlien.Replace("Room owner: ", "");

        PlayerInfo newAlien = playerInfos.Single(player => player.name == nameOfNewAlien);
        newAlien.isAlien = true;

        Debug.LogFormat("Player '{0}' has replaced '{1}' as the alien.", oldAlien.name, newAlien.name);
    }

    public void ReconstructList(string[] s, bool[] b)
    {
        playerInfos.Clear();
        for (int i = 0; i < s.Length; i++)
        {
            playerInfos.Add(new PlayerInfo(s[i], b[i]));
        }
    }
}

[Serializable]
public class PlayerInfo
{
    public string name;
    public bool isAlien;

    public PlayerInfo(string playerName, bool alien)
    {
        name = playerName;
        isAlien = alien;
    }
}