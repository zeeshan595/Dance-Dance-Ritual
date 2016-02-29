using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] music;

    private AudioSource aSource;

    private void Start()
    {
        aSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (!aSource.isPlaying)
        {
            aSource.clip = music[Random.Range(0, music.Length)];
            aSource.Play();
        }
    }
}
