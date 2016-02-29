using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int playerNum = 0;
    public int score = 0;
    public float movmentSpeed = 5.0f;
    public float jumpHeight = 2.0f;
    public float jumpSpeed = 3.0f;
    public float gravity = 1.0f;
    public float rotationSpeed = 10.0f;
    public float legMovmentSpeed = 10.0f;
    public float legMovmentAmount = 30.0f;

    //[System.NonSerialized]
    public MoveType currentMove;

    private void Start()
    {
        gameObject.tag = "Player" + playerNum;
        GameObject man = GameObject.Find("Game Manager");
        if (man != null)
            man.GetComponent<LevelManager>().PlayerJoined(gameObject);
    }
}