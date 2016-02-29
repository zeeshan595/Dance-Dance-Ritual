using UnityEngine;

public class PlayerItem : MonoBehaviour
{
    [SerializeField]
    private GameObject itemHand;
    [SerializeField]
    private GameObject rig;
    [SerializeField]
    private AudioClip[] damageClips;
    [SerializeField]
    public GameObject healthBar;

    private GameObject itemHover = null;
    private GameObject itemHolding = null;
    private PlayerStats player;
    private AudioSource aSource;

    private void Start()
    {
        player = GetComponent<PlayerStats>();
        aSource = GetComponent<AudioSource>();
        PlayerManager.players[player.playerNum].health = 100;
    }

    private void Update()
    {

        if (itemHover != null)
        {
            if (XboxCtrlrInput.XCI.GetButtonDown(XboxCtrlrInput.XboxButton.X, (XboxCtrlrInput.XboxController)PlayerManager.players[player.playerNum].controllerNum) && itemHolding == null)
            {
                itemHolding = itemHover;
                itemHover = null;
                itemHolding.transform.SetParent(itemHand.transform);
                itemHolding.transform.localPosition = Vector3.zero;
                itemHolding.transform.localRotation = Quaternion.Euler(0, 0, 180);
                itemHolding.GetComponent<Rigidbody>().isKinematic = true;
                itemHolding.GetComponent<AudioSource>().Play();
            }
        }
        else if (itemHolding != null)
        {
            if (XboxCtrlrInput.XCI.GetButtonDown(XboxCtrlrInput.XboxButton.X, (XboxCtrlrInput.XboxController)PlayerManager.players[player.playerNum].controllerNum))
            {
                itemHolding.transform.position = transform.position + transform.forward;
                itemHolding.transform.SetParent(null);
                itemHolding.GetComponent<Rigidbody>().isKinematic = false;
                itemHolding = null;
            }
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.tag == "Item")
        {
            if (hit.collider.attachedRigidbody.isKinematic && hit.collider.gameObject != itemHolding)
            {
                if (!aSource.isPlaying)
                {
                    aSource.clip = damageClips[Random.Range(0, damageClips.Length)];
                    aSource.Play();
                }
                PlayerManager.players[player.playerNum].health -= 25 * Time.deltaTime;
                float healthPos = (PlayerManager.players[player.playerNum].health / 100);
                healthBar.transform.localPosition = (Vector3.right * (-healthPos + (healthPos / 2))) + new Vector3(0, 0, 0.1f);
                healthBar.transform.localScale = new Vector3(healthPos / 2, 1, 1);
                if (PlayerManager.players[player.playerNum].health <= 0)
                {
                    Instantiate(rig, transform.position, transform.rotation);
                    Destroy(gameObject);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Item")
        {
            itemHover = col.gameObject;
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.tag == "Item")
        {
            itemHover = null;
        }
    }
}