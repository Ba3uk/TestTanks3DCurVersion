using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class GameManager : MonoBehaviour {

    public const string PLAYER_PRIFIX_ID = "Player "; 
    public static Dictionary<string, TankController> players = new Dictionary<string, TankController>();

    public static void RegisterPlayer(string _netID , TankController playerObj)
    {
        string _playerID = playerObj.tag + _netID;
        players.Add(_playerID, playerObj);
        playerObj.transform.name = _playerID;
    }

    public static void UnRegisterPlayer(string _playerID)
    {
        players.Remove(_playerID);
    }

    public static TankController getPlayer(string _playerID)
    {
        return players[_playerID];
    }



}
