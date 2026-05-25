using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Estado Actual")]

    public string procedimientoSeleccionado;
    public ModoSimulador modoSeleccionado = ModoSimulador.Practica;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); //persiste entre escena el GameManager
        }
        else Destroy(gameObject);
    }

    //seleccion de procedimiento

    public void SeleccionarProcedimiento(string nombreProcedimiento)
    {
        procedimientoSeleccionado = nombreProcedimiento;
        Debug.Log($"Procedimiento seleccionado: {nombreProcedimiento}");
    }

    //seleccion de modo

    public void SeleccionarModo(string modo)
    {
        modoSeleccionado = modo == "Practica"
            ? ModoSimulador.Practica
            : ModoSimulador.Evaluacion;
        Debug.Log($"Modo seleccionado: {modoSeleccionado}");
    }

    //Iniciar Simulador

    public void IniciarSimulador(string nombreEscena)
    {
        if (string.IsNullOrEmpty(procedimientoSeleccionado))
        {
            Debug.LogWarning("no hay procedimiento seleccionado");
            return;
        }

        Debug.Log($"Cargando escena: {nombreEscena} | " +
                  $"Procedimiento: {procedimientoSeleccionado} | " +
                  $"Modo: {modoSeleccionado}");

        SceneManager.LoadScene(nombreEscena);
    }

    public void VolverAlMenu(string nombreEscenaMenu)
    {
        procedimientoSeleccionado = "";
        modoSeleccionado = ModoSimulador.Practica;
        SceneManager.LoadScene(nombreEscenaMenu);
    }
}
