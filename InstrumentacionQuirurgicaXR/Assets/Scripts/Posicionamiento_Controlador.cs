using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class DatoPosicion
{
    public int idSocket;
    public string nombreHerramienta;
    public Vector3 posicionEstimada;
    public Vector3 posicionFinal;
    public bool esCorrecta;
}

[System.Serializable]
public class ReporteSimulador
{
    public int total;
    public int correctas;
    public int incorrectas;
    public float puntuacion;
    public List<string> herramientasIncorrectas;
}

public class Posicionamiento_Controlador : MonoBehaviour
{
    public static Posicionamiento_Controlador Instance;

    [Header("Modo del simulador")]
    public ModoSimulador modoActual = ModoSimulador.Practica;

    [Header("Arrays de posiciones")]
    public List<DatoPosicion> arrayPosicionEstimada = new List<DatoPosicion>();
    public List<DatoPosicion> arrayPosicionFinal = new List<DatoPosicion>();

    [Header("Progreso")]
    public int totalHerramientas;
    public int herramientasCorrectas;

    [Header("Eventos")]
    public UnityEvent<DatoPosicion, bool> onFeedbackInmediato;
    public UnityEvent<ReporteSimulador> onReporteFinal;

    private List<SocketValidador> socketsMesa = new List<SocketValidador>();

    public GameObject PanelFinal;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        if (GameManager.Instance != null)
            modoActual = GameManager.Instance.modoSeleccionado;
    }

    //Registro de sockets
    public void RegistrarSocket(SocketValidador socket)
    {
        socketsMesa.Add(socket);
        totalHerramientas = socketsMesa.Count;
        Debug.Log($"Socket registrado: ID {socket.idSocket} " +
                  $"| Espera: {socket.herramientaEsperada} " +
                  $"| Total sockets: {totalHerramientas} ");
    }

    //Cambio de modo
    public void CambiarModo(ModoSimulador nuevoModo)
    {
        modoActual = nuevoModo;
        LimpiarTodo();
        Debug.Log($"Modo cambiado a: {nuevoModo}");
    }

    // AgregarPosiciones
    public void RegistrarPosicion(int idSocket, string nombre,
                                    Vector3 posEst, Vector3 posFinal,
                                    bool esCorrecta)
    {
        DatoPosicion dato = new DatoPosicion
        {
            idSocket = idSocket,
            nombreHerramienta = nombre,
            posicionEstimada = posEst,
            posicionFinal = posFinal,
            esCorrecta = esCorrecta
        };
        arrayPosicionEstimada.Add(dato);
        if (esCorrecta)
        {
            arrayPosicionFinal.Add(dato);
            herramientasCorrectas++;
        }
        ReportarPosicion(dato);

        if (modoActual == ModoSimulador.Practica)
            onFeedbackInmediato?.Invoke(dato, esCorrecta);
        if (modoActual == ModoSimulador.Evaluacion)
            VerificarCompletadosSilencioso();
    }

    //Quitar Posicion
    public void EliminarPosicion(int idSocket)
    {
        DatoPosicion enEstimada = arrayPosicionEstimada.Find(d => d.idSocket == idSocket);
        if (enEstimada != null)
        {
            if (enEstimada.esCorrecta) herramientasCorrectas--;
            arrayPosicionEstimada.Remove(enEstimada);
        }

        DatoPosicion enFinal = arrayPosicionFinal.Find(d => d.idSocket == idSocket);
        if (enFinal != null)
            arrayPosicionFinal.Remove(enFinal);

        Debug.Log($"Socket {idSocket}: herramienta retirada.");
    }

    //Actualizar
    public void ActualizarPosicion(int idSocket, Vector3 nuevaPosEstimada, bool nuevaValidez)
    {
        DatoPosicion dato = arrayPosicionEstimada.Find(d => d.idSocket == idSocket);
        if (dato != null)
        {
            dato.posicionEstimada = nuevaPosEstimada;
            dato.esCorrecta = nuevaValidez;
            Debug.Log($"Socket {idSocket}: posicion actualizada");
        }
    }

    //Reporte de posicion
    public void ReportarPosicion(DatoPosicion dato)
    {
        Debug.Log($"[Reporte] Socket {dato.idSocket} | " +
                  $"Herramienta: {dato.nombreHerramienta} | " +
                  $"Pos. Estimada: {dato.posicionEstimada} | " +
                  $"Pos. Final: {dato.posicionFinal} | " +
                  $"Correcta: {dato.esCorrecta} | ");
    }

    // Boton Verificar Mesa / Solo debe funcionar en modo evaluacion
    [ContextMenu("Verificar Mesa")]
    public void VerificarMesa()
    {
        if(modoActual != ModoSimulador.Evaluacion)
        {
            Debug.LogWarning("Verificar Mesa solo funciona en modo evaluacion");
            return;
        }

        ReporteSimulador reporte = GenerarReporte();
        onReporteFinal?.Invoke(reporte);

        Debug.Log($"===Reporte Final===\n" +
                  $"Correctas: {reporte.correctas}/{reporte.total}\n" +
                  $"Incorrectas: {reporte.incorrectas}/{reporte.total}\n" +
                  $"Puntuacion: {reporte.puntuacion:0.0}%");

        PanelFinal.SetActive( true );
    }

    //Verificacion silenciosa
    private void VerificarCompletadosSilencioso()
    {
        if (arrayPosicionEstimada.Count >= totalHerramientas)
            Debug.Log("Todas las herramientas colocadas. " +
                      "Presiona 'Verificar Mesa' para ver el resultado");
    }

    //Generar reporte
    private ReporteSimulador GenerarReporte()
    {
        int correctas = 0;
        int incorrectas = 0;
        List<string> incorrectasList = new List<string>();

        foreach (var dato in arrayPosicionEstimada)
        {
            if (dato.esCorrecta) correctas++;
            else
            {
                incorrectas++;
                incorrectasList.Add(dato.nombreHerramienta);
            }
        }

        return new ReporteSimulador
        {
            total = totalHerramientas,
            correctas = correctas,
            incorrectas = incorrectas,
            puntuacion = (float)correctas / totalHerramientas * 100f,
            herramientasIncorrectas = incorrectasList
        };
    }

    //Limpiar
    public void LimpiarTodo()
    {
        arrayPosicionEstimada.Clear();
        arrayPosicionFinal.Clear();
        herramientasCorrectas = 0;
        Debug.Log("Arrays Limpiados");
    }

    //Ver estado en inspector
    [ContextMenu("Mostrar Estado Arrays")]
    public void MostrarEstado()
    {
        Debug.Log($"=== Array Estimada ({arrayPosicionEstimada.Count}) === ");
        foreach (var d in arrayPosicionEstimada)
            Debug.Log($" Socket {d.idSocket}: {d.nombreHerramienta} " +
                      $"| Correcta: {d.esCorrecta}");

        Debug.Log($"=== Array Final ({arrayPosicionFinal.Count}) === ");
        foreach (var d in arrayPosicionFinal)
            Debug.Log($" Socket {d.idSocket}: {d.nombreHerramienta}");
    }
}
