using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneController : MonoBehaviour
{
    private int EscenaActual;

    public void ReiniciarEscena()
    {
        EscenaActual = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(EscenaActual);
    }

    public void IrMenu()
    {
        SceneManager.LoadScene("Menu Inicio");
    }
}
