using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class FeedbackController : MonoBehaviour
{
    [Header("Materiales")]
    public Material materialCorrecto;
    public Material materialIncorrecto;
    public Material materialNeutro;

    [Header("Audio")]
    public AudioSource AudioSource;
    public AudioClip sonidoCorrecto;
    public AudioClip sonidoIncorrecto;

    public TextMeshProUGUI porcentaje;
    public TextMeshProUGUI correctas;
    public TextMeshProUGUI incorrectas;
    public TextMeshProUGUI ListaIncorrectas;


    //para guardar el color original de cada herramienta para restaurarlo
    private Dictionary<int, Renderer> socketRenderers = new Dictionary<int, Renderer>();

    private void Start()
    {
        Posicionamiento_Controlador.Instance.onFeedbackInmediato.AddListener(MostrarFeedbackInmediato);
        Posicionamiento_Controlador.Instance.onReporteFinal.AddListener(MostrarReporteFinal);
    }

    private void OnDisable()
    {
        Posicionamiento_Controlador.Instance.onFeedbackInmediato.RemoveListener(MostrarFeedbackInmediato);
        Posicionamiento_Controlador.Instance.onReporteFinal.RemoveListener(MostrarReporteFinal);

    }

    // Modo Practica: feedback inmediato

    private void MostrarFeedbackInmediato(DatoPosicion dato, bool esCorrecta)
    {
        //Buscar la herramienta en la escena por el nombre
        Renderer rendSocket = ObtenerRendererSocket(dato.idSocket);
        if (rendSocket == null)
        {
            Debug.LogWarning($"Socket {dato.idSocket} no tiene renderer");
            return;
        }
        
        rendSocket.material = esCorrecta ? materialCorrecto : materialIncorrecto;
        rendSocket.enabled = true;

        if (AudioSource != null)
        {
            AudioSource.PlayOneShot(esCorrecta ? sonidoCorrecto : sonidoIncorrecto);
        }

        Debug.Log($"Feedback -> {dato.idSocket}: " +
                  $"{(esCorrecta ? "correcto" : "Incorrecto")}");
    }

    //Restaurar socket a sin material
    public void RestaurarSocket(int idSocket)
    {
        Renderer rendSocket = ObtenerRendererSocket(idSocket);
        if (rendSocket == null) return;

        rendSocket.material = materialNeutro;
        rendSocket.enabled = false;

        Debug.Log($"Socket {idSocket}: area restaurada");
    }

    //Mostrar reporte final en modo evaluacion
    
    private void MostrarReporteFinal (ReporteSimulador reporte)
    {
        //conectar mas adelante con UI
        Debug.Log($"=== reporte final===");
        Debug.Log($"Puntuacion : {reporte.puntuacion:0.0}%");
        porcentaje.text = reporte.puntuacion.ToString() + "%"; 
        Debug.Log($"Correctas : {reporte.correctas}/{reporte.total}");
        correctas.text = reporte.correctas.ToString() + " : " + reporte.total.ToString();
        Debug.Log($"Incorrectas : {reporte.incorrectas}/{reporte.total}");
        incorrectas.text = reporte.incorrectas.ToString() + " : " + reporte.total.ToString();

        if  (reporte.herramientasIncorrectas.Count > 0)
        {
            string lista = string.Join(", ", reporte.herramientasIncorrectas);
            Debug.Log($"Herramientas incorrectas: {lista}");
            ListaIncorrectas.text = lista;
        }
        else
        {
            Debug.Log("Ni un solo error");
        }
    }

    private Renderer ObtenerRendererSocket(int idSocket)
    {
        //si ya esta cacheado lo retorna directo
        if (socketRenderers.TryGetValue(idSocket, out Renderer rend))
            return rend;
        SocketValidador[] sockets = FindObjectsOfType<SocketValidador>();
        foreach (var s in sockets)
        {
            if (s.idSocket == idSocket)
            {
                Renderer r = s.GetComponent<Renderer>();
                if (r != null)
                {
                    socketRenderers[idSocket] = r;
                    return r;
                }
            }
            
        }
        return null;
    }
}
