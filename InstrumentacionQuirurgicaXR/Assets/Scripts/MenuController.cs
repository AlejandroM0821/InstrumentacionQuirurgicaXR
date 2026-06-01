using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [Header("Panel seleccion de modo")]
    public GameObject panelModo; //Panel que aparece para elegir modo

    [Header("Botones de modo")]
    public Button BotonPractica;
    public Button BotonEvaluacion;
    public Button BotonInicio;
    public Button Atras;

    [Header("Texto Confirmación")]
    public TextMeshProUGUI txtProcedimientoSeleccionado;

    // resaltado visual de activo
    public Color colorActivo = Color.blue;
    public Color colorInactivo = Color.white;

    private void Start()
    {
        panelModo.SetActive(false);

        BotonPractica.onClick.AddListener(() => OnModoSeleccionado("Practica"));
        BotonEvaluacion.onClick.AddListener(() => OnModoSeleccionado("Evaluacion"));
    }

    //Seleccion de procedimiento

    public void OnProcedimientoSeleccionado(string nombreProcedimiento)
    {
        GameManager.Instance.SeleccionarProcedimiento(nombreProcedimiento);

        txtProcedimientoSeleccionado.text = $"Procedimiento: {nombreProcedimiento}";
        panelModo.SetActive(true);

        OnModoSeleccionado("Practica");
    }

    //Seleccion modo

    public void OnModoSeleccionado(string modo)
    {
        GameManager.Instance.SeleccionarModo(modo);

        BotonPractica.GetComponent<Image>().color =
            modo == "Practica" ? colorActivo : colorInactivo;

        BotonEvaluacion.GetComponent<Image>().color =
            modo == "Evaluacion" ? colorActivo : colorInactivo;
    }

    public void OnIniciar()
    {
        string escena = ObtenerEscena(GameManager.Instance.procedimientoSeleccionado);
        GameManager.Instance.IniciarSimulador(escena);
    }

    private string ObtenerEscena(string procedimiento)
    {
        switch (procedimiento)
        {
            case "Facil": return "CirugiaFacil";
            case "Medio": return "CirugiaMedia";
            case "Dificil": return "CirugiaDificil";

            default:
                Debug.LogWarning($"Escena no encontrada para: {procedimiento}");
                return "";
        }
    }

    public void botonAtras()
    {
        panelModo.SetActive(false);
    }
}
