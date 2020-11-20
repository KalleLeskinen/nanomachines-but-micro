using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerObjectRegistry
{
    static List<PlayerObject> players = new List<PlayerObject>();
    
    public static IEnumerable<PlayerObject> AllPlayers => players;

    static PlayerObject CreatePlayer(BoltConnection connection)
    {
        var player = new PlayerObject {connection = connection};

        if (player.connection != null)
        {
            player.connection.UserData = player;
        }

        players.Add(player);

        return player;
    }

    public static PlayerObject ServerPlayer
    {
        get { return players.Find(player => player.IsServer); }
    }

    public static PlayerObject CreateServerPlayer()
    {
        return CreatePlayer(null);
    }

    public static PlayerObject CreateClientPlayer(BoltConnection connection)
    {
        return CreatePlayer(connection);
    }

    public static PlayerObject GetPlayer(BoltConnection connection)
    {
        if (connection == null)
            return ServerPlayer;

        return (PlayerObject) connection.UserData;
    }
}
