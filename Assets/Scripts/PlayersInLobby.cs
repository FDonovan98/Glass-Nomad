using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public class PlayersInLobby : ScriptableObject
{
    List<PlayerInfo> playerInfos = new List<PlayerInfo>();

    public void PlayerJoinedLobby(string playerName, bool alien)
    {
        playerInfos.Add(new PlayerInfo(playerName, alien));
    }

    public void PlayerLeftLobby(string playerName)
    {
        PlayerInfo playerToRemove = playerInfos.Single(player => player.name == playerName);
        
        if (playerToRemove.isAlien && playerInfos.Count > 1)
        {
            // allocate new alien
        }

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

        PlayerInfo newAlien = playerInfos.Single(player => player.name == nameOfNewAlien);
        newAlien.isAlien = true;
    }
}

[Serializable]
class PlayerInfo
{
    public string name;
    public bool isAlien;

    public PlayerInfo(string playerName, bool alien)
    {
        name = playerName;
        isAlien = alien;
    }
}