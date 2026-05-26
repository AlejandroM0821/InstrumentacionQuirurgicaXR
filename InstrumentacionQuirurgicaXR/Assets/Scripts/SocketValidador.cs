using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SocketValidador : MonoBehaviour
{
    [Header("Configuracion del socket")]
    public string herramientaEsperada;
    public int idSocket;

    [Header("Posiciones")]
    public Vector3 posicionEstimada;
    public Vector3 posicionFinal;

    private XRSocketInteractor socket;

    private void Awake()
    {
        socket = GetComponent<XRSocketInteractor>();
        posicionFinal = transform.position;
    }

    private void Start()
    {
        herramientaEsperada = gameObject.name.Replace("Socket_", "");
        idSocket = transform.GetSiblingIndex();

        Posicionamiento_Controlador.Instance.RegistrarSocket(this);
    }

    private void OnEnable()
    {
        socket.selectEntered.AddListener(OnHerramientaColocada);
        socket.selectExited.AddListener(OnHerramientaRetirada);
    }

    private void OnDisable()
    {
        socket.selectEntered.RemoveListener(OnHerramientaColocada);
        socket.selectExited.RemoveListener(OnHerramientaRetirada);
    }

    private void OnHerramientaColocada(SelectEnterEventArgs args)
    {
        HerramientaID herramienta = args.interactableObject.transform.GetComponent<HerramientaID>();
        if (herramienta == null) return;

        posicionEstimada = args.interactableObject.transform.position;
        bool esCorrecta = (herramienta.nombreHerramienta == herramientaEsperada);

        Posicionamiento_Controlador.Instance.RegistrarPosicion(
            idSocket,
            herramienta.nombreHerramienta,
            posicionEstimada,
            posicionFinal,
            esCorrecta
        );
    }


    private void OnHerramientaRetirada(SelectExitEventArgs args)
    {
        FeedbackController feedback = FindObjectOfType<FeedbackController>();
        if (feedback != null)
            feedback.RestaurarSocket(idSocket);
            
        Posicionamiento_Controlador.Instance.EliminarPosicion(idSocket);
        posicionEstimada = Vector3.zero;
    }

}
