using UnityEngine;

public class PartPickerSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject playerPrefab;
    [SerializeField]
    private GameObject spawnPoint;
    [SerializeField]
    public int playerNum;

    [System.NonSerialized]
    public PlayerMeshUpdator playerMesh;

    private void Start()
    {
        GameObject spawnedPlayer;
        if (playerNum < PlayerManager.currentNumOfPlayers)
        {
            spawnedPlayer = (GameObject)Instantiate(playerPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation);
            spawnedPlayer.GetComponent<CharacterController>().enabled = false;
            spawnedPlayer.GetComponent<BoxCollider>().enabled = false;
            spawnedPlayer.GetComponent<PlayerStats>().enabled = false;
            spawnedPlayer.GetComponent<PlayerMovment>().enabled = false;
            spawnedPlayer.GetComponent<PlayerArmMovment>().enabled = false;
            spawnedPlayer.GetComponent<PlayerItem>().enabled = false;
            spawnedPlayer.transform.SetParent(spawnPoint.transform);
            spawnedPlayer.GetComponent<PlayerStats>().playerNum = playerNum;
            playerMesh = spawnedPlayer.GetComponent<PlayerMeshUpdator>();
            GetComponent<PartPicker>().enabled = true;
        }
    }
}