using UnityEngine;

public class PlayerManager
{
    public const int MAX_PLAYERS = 4;

    public static int currentNumOfPlayers = 0;
    public static Player[] players = new Player[MAX_PLAYERS] {
        null,
        null,
        null,
        null
    };

    public static bool PlayerJoined(Player.ControllerType type, int controllerNum)
    {
        if (type == Player.ControllerType.Controller)
        {
            for (int i = 0; i < currentNumOfPlayers; i++)
            {
                if (players[i].controller == Player.ControllerType.Controller && controllerNum == players[i].controllerNum)
                {
                    return false;
                }
            }
        }
        else if (type == Player.ControllerType.Keyboard)
        {
            for (int i = 0; i < currentNumOfPlayers; i++)
            {
                if (players[i].controller == Player.ControllerType.Keyboard)
                {
                    return false;
                }
            }
        }

        if (currentNumOfPlayers < 4)
        {
            players[currentNumOfPlayers] = new Player(type);
            players[currentNumOfPlayers].controllerNum = controllerNum;
            currentNumOfPlayers++;
            return true;
        }
        else
            return false;
    }

    public static void PlayerLeft(int id)
    {
        players[id] = null;
        for (int i = 0; i < MAX_PLAYERS; i++)
        {
            if (players[i] == null)
            {
                players[i] = players[i + 1];
                break;
            }
        }
        currentNumOfPlayers--;
    }
}

public class Player
{
    public enum ControllerType
    {
        Controller,
        Keyboard,
        Phone
    }

    public ControllerType controller;
    public int controllerNum = -1;
    public float health = 100;
    public int head = 0;
    public int body = 0;
    public int hand = 0;
    public int skirt = 0;
    public int legs = 0;

    public int MAX_HEAD = 3;
    public int MAX_BODY = 1;
    public int MAX_HAND = 1;
    public int MAX_SKIRT = 1;
    public int MAX_LEGS = 1;

    public Player(ControllerType controller)
    {
        this.controller = controller;
    }
}