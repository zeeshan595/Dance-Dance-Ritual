using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{
    #region Static Varibles

    public static LevelMode mode = LevelMode.Vs;
    public static List<GameObject> players
    {
        get;
        private set;
    }

    #endregion

    #region Serialize Field

    [SerializeField]
    private float BPM = 1.0f;
    [SerializeField]
    private int countDown = 1;
    [SerializeField]
    private int roundTimer = 60;
    [SerializeField]
    private Transform timerClockHand;
    [SerializeField]
    private Sprite[] timerClockHandTexture;
    [SerializeField]
    private Transform[] timerClockSymbol;
    [SerializeField]
    private Sprite[] timerClockSymbolTexture;
    [SerializeField]
    private Text timerText;
    [SerializeField]
    private AudioClip[] startUpSounds;
    [SerializeField]
    private Image[] playerBoxes;
    [SerializeField]
    private Sprite[] textureList;
    [SerializeField]
    private Sprite tick;
    [SerializeField]
    private Sprite cross;
    [SerializeField]
    private Mode[] modes;

    #endregion

    #region Private Variables

    private AudioSource aSource;
    public bool gameStarted
    {
        get;
        private set;
    }
    private Mode currentMode;
    private int initalTime = 0;
    private float deltaTimer = 0;
    private float scoreMultiplier = 2.0f;

    #endregion

    #region Private Methods

    private void Start()
    {
        initalTime = roundTimer;
        deltaTimer = initalTime;
        aSource = GetComponent<AudioSource>();
        players = new List<GameObject>();

        for (int i = 0; i < modes.Length; i++)
        {
            modes[i].manager = this;
            if (mode == modes[i].GetModeType())
            {
                currentMode = modes[i];
            }
        }

        StartCoroutine(Tick());
        StartCoroutine(ArmFlicker());
    }

    private void Update()
    {
        float tim = ((float)deltaTimer / (float)initalTime);
        timerClockHand.rotation = Quaternion.Euler(new Vector3(90, tim * 360.0f, 0));

        if (gameStarted)
            deltaTimer -= Time.deltaTime;

        scoreMultiplier -= Time.deltaTime;
    }

    #endregion

    #region Coroutines

    private IEnumerator Tick()
    {
        scoreMultiplier = 2.0f;

        if (!gameStarted)
        {
            if (countDown <= 0)
            {
                //Start game
                aSource.clip = startUpSounds[Random.Range(0, startUpSounds.Length)];
                aSource.loop = false;
                aSource.Play();
                currentMode.ModeStart();
                StartCoroutine(TickBPM());
                gameStarted = true;
                roundTimer--;
            }
            else
            {
                countDown--;
                if (timerText)
                    timerText.text = countDown.ToString();
            }
        }
        else
        {
            roundTimer--;
            if (timerText)
                timerText.text = roundTimer.ToString();

            if (timerClockHand)
            {
                float tim = ((float)roundTimer / (float)initalTime);

                for (int i = 0; i < timerClockSymbol.Length; i++)
                {
                    if (((float)i / (float)timerClockSymbol.Length) >= tim || i == 0)
                    {
                        timerClockSymbol[i].GetComponent<SpriteRenderer>().sprite = timerClockSymbolTexture[i];
                    }
                }
            }
        }

        if (roundTimer > 0)
        {
            yield return new WaitForSeconds(1);
            StartCoroutine(Tick());
        }
        else
        {
            //End The Game
            Debug.Log("Game Ended");
        }
    }

    private IEnumerator TickBPM()
    {
        currentMode.NextMove();
        yield return new WaitForSeconds(60.0f / BPM);
        StartCoroutine(TickBPM());
    }

    private IEnumerator ArmFlicker()
    {
        int rnd = Random.Range(1, 3);
        if (rnd == 0)
        {
            timerClockHand.gameObject.SetActive(false);
        }
        else if (rnd == 1)
        {
            timerClockHand.gameObject.SetActive(true);
        }
        else if (rnd == 2)
        {
            timerClockHand.gameObject.SetActive(true);
            for (int i = 0; i < timerClockHandTexture.Length; i++)
            {
                timerClockHand.GetComponent<SpriteRenderer>().sprite = timerClockHandTexture[Random.Range(0, timerClockHandTexture.Length)];
            }
        }
        yield return new WaitForSeconds(Random.Range(0.01f, 0.3f));
        StartCoroutine(ArmFlicker());
    }

    #endregion

    #region Public Methods

    public void UpdateUI(MoveType move, int player)
    {
        playerBoxes[player].gameObject.SetActive(true);
        playerBoxes[player].sprite = textureList[(int)move];
    }

    public void UpdateUI(bool gotIt, int player)
    {
        if (gotIt)
            playerBoxes[player].sprite = tick;
        else
            playerBoxes[player].sprite = cross;
    }

    public void PlayerJoined(GameObject g)
    {
        players.Add(g);
        currentMode.PlayerJoined();
    }

    public void ChangeSetting(float BPM)
    {
        this.BPM = BPM;
    }
    
    public float GetScoreMultiplier()
    {
        return scoreMultiplier;
    }

    #endregion
}

#region ENUM

public enum LevelMode
{
    Coop,
    Vs
}

#endregion