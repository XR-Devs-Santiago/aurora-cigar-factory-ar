using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAnimationOnOffTracking : NYImageTrackerEventHandler
{
    public Animator targetAnim;

    public string onFoundAnimName;
	public string onLostAnimName;
	public GameObject panel_alerta, experiencia;

    public override void OnTrackingFound()
    {
	    //targetAnim.Play(onFoundAnimName);
	    panel_alerta.SetActive(false);
	    experiencia.SetActive(true);
    }

    public override void OnTrackingLost()
    {
	    //targetAnim.Play(onLostAnimName);
	    panel_alerta.SetActive(true);
	    experiencia.SetActive(false);
    }
	
}
