using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ContinuousMovementController : MonoBehaviour
{
    public ActionBasedContinuousMoveProvider ContinuousMoveProvider;
    public ActionBasedContinuousTurnProvider ContinuousTurnProvider;
    public XRRayInteractor XRRInteractor;

    public void HabilitarMovimientoContinuo()
    {
        ContinuousMoveProvider.enabled = true;
        ContinuousTurnProvider.enabled = true;
        XRRInteractor.allowAnchorControl = false;
    }

    public void InhabilitarMovimientoContinuo()
    {
        ContinuousMoveProvider.enabled = false;
        ContinuousTurnProvider.enabled = false;
        XRRInteractor.allowAnchorControl = true;
    }
}
