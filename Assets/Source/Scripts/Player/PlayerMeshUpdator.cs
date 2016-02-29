using UnityEngine;
using System.Collections;

public class PlayerMeshUpdator : MonoBehaviour
{
    [SerializeField]
    public Material[] material;
    [SerializeField]
    public Texture mainTexture;
    [SerializeField]
    private GameObject currentHead;
    [SerializeField]
    private GameObject[] headPrefabs;
    [SerializeField]
    public GameObject[] symbols;

    private PlayerStats player;
    [System.NonSerialized]
    public bool paintingMode = false;

    public void Start()
    {
        player = GetComponent<PlayerStats>();
        StartCoroutine(UpdateMesh());
    }

    public IEnumerator UpdateMesh()
    {
        //Head
        GameObject newHead = (GameObject)Instantiate(headPrefabs[PlayerManager.players[player.playerNum].head]);
        newHead.transform.SetParent(currentHead.transform.parent);
        newHead.transform.localPosition = currentHead.transform.localPosition;
        newHead.transform.localRotation = Quaternion.Euler(0, 0, 90);
        Destroy(currentHead);
        currentHead = newHead;

        //Update Texture
        foreach (MeshRenderer renderer in GetComponentsInChildren<MeshRenderer>())
        {
            renderer.material = material[player.playerNum];
        }

        if (!paintingMode)
        {
            //Symbols
            for (int i = 0; i < symbols.Length; i++)
            {
                if (player.playerNum == i)
                {
                    symbols[i].SetActive(true);
                }
            }

            if (System.IO.File.Exists(Settings.savePath + player.playerNum + ".png"))
            {
                WWW w = new WWW("file://" + Settings.savePath + player.playerNum + ".png");
                yield return w;
                material[player.playerNum].mainTexture = w.texture;
            }
            else
                material[player.playerNum].mainTexture = mainTexture;
        }
        else
        {
            foreach (MeshRenderer renderer in GetComponentsInChildren<MeshRenderer>())
            {
                if (!renderer.gameObject.GetComponent<MeshCollider>())
                {
                    renderer.gameObject.AddComponent<MeshCollider>();
                    renderer.gameObject.tag = "Player" + player.playerNum;
                }
            }
        }
    }
}