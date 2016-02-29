using UnityEngine;

public class PaintingSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject playerPrefab;
    [SerializeField]
    private GameObject spawnPoint;
    [SerializeField]
    private int playerNum = 0;
    [SerializeField]
    private Material renderMaterial;
    [SerializeField]
    private GameObject colors;

    [System.NonSerialized]
    public GameObject spawnedPlayer;

    private void Start()
    {
        if (playerNum < PlayerManager.currentNumOfPlayers)
        {
            spawnedPlayer = (GameObject)Instantiate(playerPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation);
            spawnedPlayer.GetComponent<CharacterController>().enabled = false;
            spawnedPlayer.GetComponent<BoxCollider>().enabled = false;
            spawnedPlayer.GetComponent<PlayerStats>().enabled = false;
            spawnedPlayer.GetComponent<PlayerMovment>().enabled = false;
            spawnedPlayer.GetComponent<PlayerArmMovment>().enabled = false;
            spawnedPlayer.GetComponent<PlayerItem>().enabled = false;
            spawnedPlayer.GetComponent<PlayerStats>().playerNum = playerNum;
            spawnedPlayer.GetComponent<PlayerMeshUpdator>().mainTexture = GetComponent<Painting>().renderTexture;
            spawnedPlayer.GetComponent<PlayerMeshUpdator>().material[playerNum] = renderMaterial;
            spawnedPlayer.GetComponent<PlayerMeshUpdator>().paintingMode = true;
            spawnedPlayer.transform.SetParent(spawnPoint.transform);
            GetComponent<PaintControls>().enabled = true;
            GetComponent<Painting>().enabled = true;
            colors.SetActive(true);
        }
        else
            colors.SetActive(false);
    }
}