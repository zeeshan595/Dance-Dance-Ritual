using UnityEngine;
using System.Collections;

#pragma warning disable 0649

public class PaintControls : MonoBehaviour
{
    [System.NonSerialized]
    public bool isReady = false;

    [SerializeField]
    private Camera playerCam;
    [SerializeField]
    private int playerNum;
    [SerializeField]
    private GameObject tickBox;
    [SerializeField]
    private ReadyCounter readyCounter;
    private Vector2 cursor = Vector2.zero;
    private PaintingSpawner spawner;

    private void Start()
    {
        cursor += new Vector2(playerCam.pixelWidth / 2, playerCam.pixelWidth / 2) + new Vector2(playerCam.rect.x * Screen.width, 0);
        spawner = GetComponent<PaintingSpawner>();
    }

    private void Update()
    {
        //Change Brush Size
        if (PlayerManager.players[playerNum].controller == Player.ControllerType.Controller)
        {
            if (XboxCtrlrInput.XCI.GetDPad(XboxCtrlrInput.XboxDPad.Left, (XboxCtrlrInput.XboxController)PlayerManager.players[playerNum].controllerNum))
            {
                GetComponent<Painting>().brushSize -= Time.deltaTime;
            }
            else if (XboxCtrlrInput.XCI.GetDPad(XboxCtrlrInput.XboxDPad.Right, (XboxCtrlrInput.XboxController)PlayerManager.players[playerNum].controllerNum))
            {
                GetComponent<Painting>().brushSize += Time.deltaTime;
            }
        }

        //Zoom + Rotate
        float vertical = 0;
        float horizontal = 0;
        if (PlayerManager.players[playerNum].controller == Player.ControllerType.Controller)
        {
            vertical = -XboxCtrlrInput.XCI.GetAxis(XboxCtrlrInput.XboxAxis.RightStickY, (XboxCtrlrInput.XboxController)PlayerManager.players[playerNum].controllerNum);
            horizontal = XboxCtrlrInput.XCI.GetAxis(XboxCtrlrInput.XboxAxis.RightStickX, (XboxCtrlrInput.XboxController)PlayerManager.players[playerNum].controllerNum);
        }
        else
        {
            vertical = Input.GetAxis("MouseScrollWheel") * -5;
            //horizontal = InputManager.GetAxis(KeyCode.Q, KeyCode.E);
        }

        playerCam.orthographicSize += vertical * Time.deltaTime;
        playerCam.orthographicSize = Mathf.Clamp(playerCam.orthographicSize, 0.5f, 2.0f);

        spawner.spawnedPlayer.transform.Rotate(new Vector3(0, horizontal, 0) * Time.deltaTime * 45);

        //Ready/Unready
        if (XboxCtrlrInput.XCI.GetButtonDown(XboxCtrlrInput.XboxButton.A, (XboxCtrlrInput.XboxController)PlayerManager.players[playerNum].controllerNum) && !GetComponent<Painting>().isSaving)
        {
            GetComponent<AudioSource>().Play();
            isReady = !isReady;
            tickBox.SetActive(isReady);
            if (isReady)
            {
                StartCoroutine(GetComponent<Painting>().SaveTexture());
                readyCounter.playerReady++;
            }
            else
                readyCounter.playerReady--;
        }
    }

    public Vector2 cursorPos()
    {
        if (PlayerManager.players[playerNum].controller == Player.ControllerType.Controller)
        {
            float horizontal = XboxCtrlrInput.XCI.GetAxis(XboxCtrlrInput.XboxAxis.LeftStickX, (XboxCtrlrInput.XboxController)PlayerManager.players[playerNum].controllerNum);
            float vertical = XboxCtrlrInput.XCI.GetAxis(XboxCtrlrInput.XboxAxis.LeftStickY, (XboxCtrlrInput.XboxController)PlayerManager.players[playerNum].controllerNum);
            cursor += new Vector2(horizontal, vertical);
        }
        else if (PlayerManager.players[playerNum].controller == Player.ControllerType.Keyboard)
        {
            cursor = Input.mousePosition;
        }

        return cursor;
    }
}