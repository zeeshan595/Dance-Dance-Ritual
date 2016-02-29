using UnityEngine;
using System.Collections;

public class PlayerHealthBar : MonoBehaviour
{
    private Camera mainCam;

    private void Start()
    {
        mainCam = Camera.main;
    }

    private void Update()
    {
        if (mainCam != null)
            transform.LookAt(mainCam.transform.position);
        else
            this.enabled = false;
    }
}