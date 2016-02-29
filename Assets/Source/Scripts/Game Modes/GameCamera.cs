using UnityEngine;
using System.Collections.Generic;

public class GameCamera : MonoBehaviour
{
    [SerializeField]
    private Vector3 offset = new Vector3(0, 3, -5);
    private List<GameObject> players = new List<GameObject>();

    private void Start()
    {
        for (int i = 0; i < 4; i++)
        {
            GameObject pl = GameObject.FindWithTag("Player" + i);
            if (pl)
                players.Add(pl);
        }
    }

    private void Update()
    {
        Vector3 pos = Vector3.zero;
        if (players.Count > 0)
        {
            for (int i = 0; i < players.Count; i++)
            {
                pos += players[i].transform.position;
            }
            pos /= players.Count;
        }
        transform.position = Vector3.Lerp(transform.position, pos + offset, Time.deltaTime * 2);
    }

    private void LateUpdate()
    {
        for (int i = 0; i < 4; i++)
        {
            GameObject pl = GameObject.FindWithTag("Player" + i);
            if (pl)
                players.Add(pl);
        }
    }
}