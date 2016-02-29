using UnityEngine;

public class PartPicker : MonoBehaviour
{
    [SerializeField]
    private ReadyCounter readyCounter;
    [SerializeField]
    private GameObject tick;

    private PartPickerSpawner spawner;
    private int partSelected = 0;
    private bool isPressed = false;
    private bool isReady = false;

    private void Start()
    {
        spawner = GetComponent<PartPickerSpawner>();
    }

    private void Update()
    {
        float vertical = 0;
        float horizontal = 0;
        if (PlayerManager.players[spawner.playerNum].controller == Player.ControllerType.Controller)
        {
            //vertical = XboxCtrlrInput.XCI.GetAxis(XboxCtrlrInput.XboxAxis.LeftStickY, (XboxCtrlrInput.XboxController)PlayerManager.players[spawner.playerNum].controllerNum);
            horizontal = XboxCtrlrInput.XCI.GetAxis(XboxCtrlrInput.XboxAxis.LeftStickX, (XboxCtrlrInput.XboxController)PlayerManager.players[spawner.playerNum].controllerNum);
        }
        else
        {
            //horizontal = InputManager.GetAxis(KeyCode.D, KeyCode.A);
            //vertical = InputManager.GetAxis(KeyCode.W, KeyCode.S);
        }

        if ((Mathf.Abs(horizontal) > 0.5f || Mathf.Abs(vertical) > 0.5f))
        {
            if (!isPressed)
            {
                if (Mathf.Abs(horizontal) > 0.5f)
                    ChangePart(Mathf.RoundToInt(Mathf.Clamp(horizontal * 50, -1, 1)));
                else
                    ChangeSelection(Mathf.RoundToInt(Mathf.Clamp(vertical * 50, -1, 1)));

                isPressed = true;
            }
        }
        else
            isPressed = false;


        //Ready
        if (XboxCtrlrInput.XCI.GetButtonDown(XboxCtrlrInput.XboxButton.A, (XboxCtrlrInput.XboxController)PlayerManager.players[spawner.playerNum].controllerNum))
        {
            GetComponent<AudioSource>().Play();
            isReady = !isReady;
            tick.SetActive(isReady);
            if (isReady)
                readyCounter.playerReady++;
            else
                readyCounter.playerReady--;
        }
    }

    private void ChangePart(int change)
    {
        switch(partSelected)
        {
            case 0:
                PlayerManager.players[spawner.playerNum].head += change;
                PlayerManager.players[spawner.playerNum].head = Mathf.Clamp(PlayerManager.players[spawner.playerNum].head, 0, PlayerManager.players[spawner.playerNum].MAX_HEAD - 1);
                break;
            case 1:
                PlayerManager.players[spawner.playerNum].body += change;
                PlayerManager.players[spawner.playerNum].body = Mathf.Clamp(PlayerManager.players[spawner.playerNum].body, 0, PlayerManager.players[spawner.playerNum].MAX_BODY - 1);
                break;
            case 2:
                PlayerManager.players[spawner.playerNum].hand += change;
                PlayerManager.players[spawner.playerNum].hand = Mathf.Clamp(PlayerManager.players[spawner.playerNum].hand, 0, PlayerManager.players[spawner.playerNum].MAX_HAND - 1);
                break;
            case 3:
                PlayerManager.players[spawner.playerNum].skirt += change;
                PlayerManager.players[spawner.playerNum].skirt = Mathf.Clamp(PlayerManager.players[spawner.playerNum].skirt, 0, PlayerManager.players[spawner.playerNum].MAX_SKIRT - 1);
                break;
            case 4:
                PlayerManager.players[spawner.playerNum].legs += change;
                PlayerManager.players[spawner.playerNum].legs = Mathf.Clamp(PlayerManager.players[spawner.playerNum].legs, 0, PlayerManager.players[spawner.playerNum].MAX_LEGS - 1);
                break;
        }

        StartCoroutine(spawner.playerMesh.UpdateMesh());
    }

    private void ChangeSelection(int change)
    {
        partSelected += change;
        Mathf.Clamp(partSelected, 0, 4);
    }
}