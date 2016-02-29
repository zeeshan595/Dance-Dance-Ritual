using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevel : MonoBehaviour
{
    [SerializeField]
    private LevelMode gameMode = LevelMode.Coop;
    [SerializeField]
    private string levelToLoad = "Forest";

    private int players = 0;

    private void Update()
    {
        if (players == PlayerManager.currentNumOfPlayers)
        {
            for (int i = 0; i < PlayerManager.currentNumOfPlayers; i++)
            {
                if (XboxCtrlrInput.XCI.GetButtonDown(XboxCtrlrInput.XboxButton.A, (XboxCtrlrInput.XboxController)PlayerManager.players[i].controllerNum))
                {
                    LevelManager.mode = gameMode;
                    LoadingManager.ChangeScene(levelToLoad);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerStats>())
            players++;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerStats>())
            players--;
    }
}