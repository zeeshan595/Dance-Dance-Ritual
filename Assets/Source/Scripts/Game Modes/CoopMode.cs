using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CoopMode : Mode
{
    [SerializeField]
    private bool overrideManager = false;
    [SerializeField]
    private float BPM = 30.0f;

    private List<MoveType> moves = new List<MoveType>();
    private List<bool> gotMove = new List<bool>();
    private bool everyoneGotMove = false;

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
            if (!everyoneGotMove)
                manager.UpdateUI(false, i);
            moves[i] = ((MoveType)Random.Range(0, (int)MoveType.None));
            gotMove[i] = false;
            StartCoroutine(UpdateUI());
        }
    }

    private IEnumerator UpdateUI()
    {
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < moves.Count; i++)
        {
            manager.UpdateUI(moves[i], i);
        }
    }

    public override LevelMode GetModeType()
    {
        return LevelMode.Coop;
    }

    private void Update()
    {
        if (manager.gameStarted)
        {
            for (int i = 0; i < moves.Count; i++)
            {
                PlayerStats stats = LevelManager.players[i].GetComponent<PlayerStats>();
                if (stats.currentMove == moves[i])
                {
                    gotMove[i] = true;
                }
                else
                {
                    gotMove[i] = true;
                }
            }

            if (GetGotMoves() == gotMove.Count - 1)
                everyoneGotMove = true;
            else
                everyoneGotMove = false;

            if (everyoneGotMove)
            {
                for (int i = 0; i < moves.Count; i++)
                    manager.UpdateUI(true, i);
            }
        }
    }

    private int GetGotMoves()
    {
        int total = 0;
        for (int i = 0; i < gotMove.Count; i++)
        {
            if (gotMove[i])
                total++;
        }
        return total;
    }
}
