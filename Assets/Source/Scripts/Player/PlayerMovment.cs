using UnityEngine;

public class PlayerMovment : MonoBehaviour
{
    public GameObject LeftLeg;
    public GameObject RightLeg;

    private PlayerStats player;
    private CharacterController controller;
    private bool isJumping = false;
    private float jumpHeight = 0;
    private float legAnimator = 0;

    private void Start()
    {
        player = GetComponent<PlayerStats>();
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        float horizontal = 0;
        float vertical = 0;
        if (PlayerManager.players[player.playerNum].controller == Player.ControllerType.Controller)
        {
            horizontal = XboxCtrlrInput.XCI.GetAxis(XboxCtrlrInput.XboxAxis.LeftStickX, (XboxCtrlrInput.XboxController)PlayerManager.players[player.playerNum].controllerNum);
            vertical = XboxCtrlrInput.XCI.GetAxis(XboxCtrlrInput.XboxAxis.LeftStickY, (XboxCtrlrInput.XboxController)PlayerManager.players[player.playerNum].controllerNum);
        }
        else
        {
            //horizontal = InputManager.GetAxis(KeyCode.D, KeyCode.A);
            //vertical = InputManager.GetAxis(KeyCode.W, KeyCode.S);
        }
        //Movment
        Vector3 mov = new Vector3(horizontal, 0, vertical);
        mov *= player.movmentSpeed;
        mov *= Time.deltaTime;

        //Jump
        if (XboxCtrlrInput.XCI.GetButton(XboxCtrlrInput.XboxButton.A, (XboxCtrlrInput.XboxController)PlayerManager.players[player.playerNum].controllerNum) && !isJumping && controller.isGrounded)
        {
            jumpHeight = transform.position.y;
            isJumping = true;
        }
        if (isJumping)
        {
            mov.y = Mathf.Lerp(mov.y, player.jumpHeight, Time.deltaTime * player.jumpSpeed);
            if (Mathf.Abs((player.jumpHeight + jumpHeight) - transform.position.y) < 0.2f)
            {
                isJumping = false;
            }
        }
        else
        {
            //Gravity
            if (!controller.isGrounded)
            {
                mov.y -= player.gravity * Time.deltaTime;
            }
        }

        controller.Move(mov);

        //Rotation
        Vector3 lookRot = new Vector3(mov.x, 0, mov.z);
        if (lookRot != Vector3.zero)
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(lookRot), Time.deltaTime * player.rotationSpeed);

        //Leg Animation
        if (mov != Vector3.zero && controller.velocity.y < 0.2f)
        {
            legAnimator += Mathf.Clamp(Mathf.Abs(mov.x) + Mathf.Abs(mov.z), 0, 1) * player.legMovmentSpeed;
            if (legAnimator > 180)
                legAnimator = 0;
        }
        else
        {
            legAnimator = 0;
        }
        LeftLeg.transform.localRotation = Quaternion.Euler(new Vector3(0, 180 + (Mathf.Sin(legAnimator) * player.legMovmentAmount), 90));
        RightLeg.transform.localRotation = Quaternion.Euler(new Vector3(0, 180 - (Mathf.Sin(legAnimator) * player.legMovmentAmount), 90));
    }
}