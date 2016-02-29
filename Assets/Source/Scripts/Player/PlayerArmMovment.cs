using System.Collections;
using UnityEngine;

public enum MoveType
{
    LeftUp,
    Left,
    RightUp,
    Right,
    BothUp,
    HeadLeft,
    HeadRight,
    None
}

public class PlayerArmMovment : MonoBehaviour
{
    [SerializeField]
    private float horizontalMovment = 60;
    [SerializeField]
    private float verticalMovment = 60;
    [SerializeField]
    private float leftArmSpeed = 5.0f;
    [SerializeField]
    private float rightArmSpeed = 5.0f;
    [SerializeField]
    private float headSpeed = 5.0f;

    [SerializeField]
    private GameObject LeftArm;
    [SerializeField]
    private GameObject RightArm;
    [SerializeField]
    private GameObject Head;
    [SerializeField]
    private AudioClip[] soundClips;
    [SerializeField]
    private AudioClip[] swingClip;

    private PlayerStats player;
    private AudioSource aSource;
    private bool playedWooshSound = false;
    private bool playedSound = false;

    private void Start()
    {
        player = GetComponent<PlayerStats>();
        aSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        float horizontal = 0;
        float vertical = 0;
        bool RightTrigger = false;
        bool LeftTrigger = false;

        if (PlayerManager.players[player.playerNum].controller == Player.ControllerType.Controller)
        {
            horizontal = XboxCtrlrInput.XCI.GetAxis(XboxCtrlrInput.XboxAxis.RightStickX, (XboxCtrlrInput.XboxController)PlayerManager.players[player.playerNum].controllerNum);
            vertical = -XboxCtrlrInput.XCI.GetAxis(XboxCtrlrInput.XboxAxis.RightStickY, (XboxCtrlrInput.XboxController)PlayerManager.players[player.playerNum].controllerNum);
            RightTrigger = XboxCtrlrInput.XCI.GetAxis(XboxCtrlrInput.XboxAxis.RightTrigger, (XboxCtrlrInput.XboxController)PlayerManager.players[player.playerNum].controllerNum) > 0.5f;
            LeftTrigger = XboxCtrlrInput.XCI.GetAxis(XboxCtrlrInput.XboxAxis.LeftTrigger, (XboxCtrlrInput.XboxController)PlayerManager.players[player.playerNum].controllerNum) > 0.5f;
        }
        else
        {
            horizontal = (Input.mousePosition.x / Screen.width) - 0.5f;
            vertical = (-Input.mousePosition.y / Screen.height) + 0.5f;
            horizontal *= 2;
            vertical *= 2;
            RightTrigger = Input.GetKey(KeyCode.Mouse1);
            LeftTrigger = Input.GetKey(KeyCode.Mouse0);
        }

        if (vertical == 0 && horizontal == 0)
            playedSound = false;
        else if (!playedSound)
        {
            PlayAudio();
            playedSound = true;
        }

        if (!RightTrigger && !LeftTrigger)
        {
            Quaternion newRot = Quaternion.Euler(new Vector3(0, -(vertical * verticalMovment), horizontal * horizontalMovment));
            Head.transform.localRotation = Quaternion.Lerp(Head.transform.localRotation, newRot, headSpeed * Time.deltaTime);
            RightArm.transform.localRotation = Quaternion.Lerp(RightArm.transform.localRotation, Quaternion.Euler(new Vector3(0, 0, -155)), 10 * Time.deltaTime);
            LeftArm.transform.localRotation = Quaternion.Lerp(LeftArm.transform.localRotation, Quaternion.Euler(new Vector3(0, 0, 155)), 10 * Time.deltaTime);

            if (horizontal > 0.5f)
            {
                player.currentMove = MoveType.HeadRight;
            }
            else if (horizontal < -0.5f)
            {
                player.currentMove = MoveType.HeadLeft;
            }
            else
                player.currentMove = MoveType.None;

            playedWooshSound = false;
        }
        else if (RightTrigger && LeftTrigger)
        {
            Quaternion LnewRot = Quaternion.Euler(new Vector3((horizontal * horizontalMovment) - 20, 0, (vertical * verticalMovment) + 100));
            LeftArm.transform.localRotation = Quaternion.Lerp(LeftArm.transform.localRotation, LnewRot, leftArmSpeed * Time.deltaTime);
            Quaternion RnewRot = Quaternion.Euler(new Vector3((horizontal * horizontalMovment) + 20, 0, -(vertical * verticalMovment) - 100));
            RightArm.transform.localRotation = Quaternion.Lerp(RightArm.transform.localRotation, RnewRot, leftArmSpeed * Time.deltaTime);
            Head.transform.localRotation = Quaternion.Lerp(Head.transform.localRotation, Quaternion.Euler(Vector3.zero), rightArmSpeed * Time.deltaTime);

            if (vertical < -0.5f)
                player.currentMove = MoveType.BothUp;
            else
                player.currentMove = MoveType.None;

            if (!playedWooshSound)
            {
                StartCoroutine(PlaySwingSound());
                playedWooshSound = true;
            }
        }
        else if (LeftTrigger)
        {
            Quaternion newRot = Quaternion.Euler(new Vector3((horizontal * horizontalMovment) - 20, 0, (vertical * verticalMovment) + 100));
            LeftArm.transform.localRotation = Quaternion.Lerp(LeftArm.transform.localRotation, newRot, leftArmSpeed * Time.deltaTime);
            RightArm.transform.localRotation = Quaternion.Lerp(RightArm.transform.localRotation, Quaternion.Euler(new Vector3(0, 0, -155)), 10 * Time.deltaTime);
            Head.transform.localRotation = Quaternion.Lerp(Head.transform.localRotation, Quaternion.Euler(Vector3.zero), rightArmSpeed * Time.deltaTime);

            if (vertical < -0.5f)
                player.currentMove = MoveType.LeftUp;
            else if (vertical > -0.2f && vertical < 0.2f)
                player.currentMove = MoveType.Left;
            else
                player.currentMove = MoveType.None;

            if (!playedWooshSound)
            {
                StartCoroutine(PlaySwingSound());
                playedWooshSound = true;
            }
        }
        else if (RightTrigger)
        {
            Quaternion newRot = Quaternion.Euler(new Vector3((horizontal * horizontalMovment) + 20, 0, -(vertical * verticalMovment) - 100));
            RightArm.transform.localRotation = Quaternion.Lerp(RightArm.transform.localRotation, newRot, leftArmSpeed * Time.deltaTime);
            LeftArm.transform.localRotation = Quaternion.Lerp(LeftArm.transform.localRotation, Quaternion.Euler(new Vector3(0, 0, 155)), 10 * Time.deltaTime);
            Head.transform.localRotation = Quaternion.Lerp(Head.transform.localRotation, Quaternion.Euler(Vector3.zero), rightArmSpeed * Time.deltaTime);

            if (vertical < -0.5f)
                player.currentMove = MoveType.RightUp;
            else if (vertical > -0.2f && vertical < 0.2f)
                player.currentMove = MoveType.Right;
            else
                player.currentMove = MoveType.None;

            if (!playedWooshSound)
            {
                StartCoroutine(PlaySwingSound());
                playedWooshSound = true;
            }
        }
        else
        {
            RightArm.transform.localRotation = Quaternion.Lerp(RightArm.transform.localRotation, Quaternion.Euler(new Vector3(0, 0, -155)), 10 * Time.deltaTime);
            LeftArm.transform.localRotation = Quaternion.Lerp(LeftArm.transform.localRotation, Quaternion.Euler(new Vector3(0, 0, 155)), 10 * Time.deltaTime);
            Head.transform.localRotation = Quaternion.Lerp(Head.transform.localRotation, Quaternion.Euler(Vector3.zero), rightArmSpeed * Time.deltaTime);

            player.currentMove = MoveType.None;
        }
    }

    private void PlayAudio()
    {
        if (!aSource.isPlaying)
        {
            aSource.clip = soundClips[Random.Range(0, soundClips.Length)];
            aSource.Play();
        }
    }

    private IEnumerator PlaySwingSound()
    {
        GameObject g = new GameObject();
        g.transform.position = transform.position;
        AudioSource a = g.AddComponent<AudioSource>();
        a.clip = swingClip[Random.Range(0, swingClip.Length)];
        a.Play();
        yield return new WaitForSeconds(a.clip.length);
        Destroy(g);
    }
}