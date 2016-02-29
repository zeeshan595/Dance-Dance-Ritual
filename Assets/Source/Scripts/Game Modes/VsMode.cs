using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VsMode : Mode
{
    [SerializeField]
    private bool overrideManager = false;
    [SerializeField]
    private float BPM = 30.0f;

    private List<MoveType> moves = new List<MoveType>();
    private List<bool> gotMove = new List<bool>();

    public override void ModeStart()
    {
        enabled = true;
        if (overrideManager)
            manager.ChangeSetting(BPM);
    }

    public override void PlayerJoined()
    {
        moves.Add((MoveType)Random.Range(0, (int)MoveType.None));
        gotMove.Add(false);
        UpdateUI();
    }

    public override void NextMove()
    {
        for (int i = 0; i < moves.Count; i++)
        {
            if (!gotMove[i])
                manager.UpdateUI(false, i);
            moves[i] = ((MoveType)Random.Range(0, (int)MoveType.None));
            gotMove[i] = false;
            StartCoroutine(UpdateUI());
        }
    }

    public override LevelMode GetModeType()
    {
        return LevelMode.Vs;
    }

    private IEnumerator UpdateUI()
    {
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < moves.Count; i++)
        {
            manager.UpdateUI(moves[i], i);
        }
    }

    private void Update()
    {
        if (manager.gameStarted)
        {
            for (int i = 0; i < moves.Count; i++)
            {
                PlayerStats stats = LevelManager.players[i].GetComponent<PlayerStats>();
                if (!gotMove[i] && stats.currentMove == moves[i])
                {
                    manager.UpdateUI(true, i);
                    gotMove[i] = true;
                    stats.score += 10;
                }
            }
        }
    }
}
