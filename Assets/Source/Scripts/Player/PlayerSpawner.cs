using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject playerPrefab;

    private void Start()
    {
        for (int i = 0; i < PlayerManager.currentNumOfPlayers; i++)
        {
            GameObject player = (GameObject)Instantiate(playerPrefab, transform.position, transform.rotation);
            player.GetComponent<PlayerStats>().playerNum = i;
        }
    }

    private void Update()
    {
        GameObject player = null;
        for (int i = 1; i < 5; i++)
        {
            if (XboxCtrlrInput.XCI.GetButtonDown(XboxCtrlrInput.XboxButton.A, (XboxCtrlrInput.XboxController)i))
            {
                if (PlayerManager.PlayerJoined(Player.ControllerType.Controller, i))
                {
                    player = (GameObject)Instantiate(playerPrefab, transform.position, transform.rotation);
                    player.GetComponent<PlayerStats>().playerNum = PlayerManager.currentNumOfPlayers - 1;
                }
            }
        }
    }
}