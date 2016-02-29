using UnityEngine;
using UnityEngine.SceneManagement;

public class ReadyCounter : MonoBehaviour
{
    [System.NonSerialized]
    public int playerReady = 0;

    private int everyoneReady = 3;

    private void Start()
    {
        InvokeRepeating("Tick", 1, 1);
    }

    private void Tick()
    {
        everyoneReady--;
        if (everyoneReady == 0)
        {
            LoadingManager.ChangeScene("Main");
        }
    }

    private void Update()
    {
        if (PlayerManager.currentNumOfPlayers != playerReady)
            everyoneReady = 3;
    }
}