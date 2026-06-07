using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{
    public GameObject ManoIzqAbierta;
    public GameObject ManoDerAbierta;
    public GameObject ManoIzqCerrada;
    public GameObject ManoDerCerrada;

    private bool isGrabbing = true;

    public void AbrirCerrarMano()
    {
        if (isGrabbing == true)
        {
            ManoDerAbierta.SetActive(false);
            ManoIzqAbierta.SetActive(false);
            ManoIzqCerrada.SetActive(true);
            ManoDerCerrada.SetActive(true);
            isGrabbing = false;     
        } else if (isGrabbing == false)
        {
            ManoDerAbierta.SetActive(true);
            ManoIzqAbierta.SetActive(true);
            ManoIzqCerrada.SetActive(false);
            ManoDerCerrada.SetActive(false);
            isGrabbing = true;

        }
    }
}
