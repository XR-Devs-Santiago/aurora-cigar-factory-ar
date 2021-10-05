using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnOffTracking : NYImageTrackerEventHandler
{
	//public Animator targetAnim;

    public string onFoundAnimName;
	public string onLostAnimName;
	public GameObject panel_alerta, modelo_experiencia, alerta_escaneo, alerta_codigo_incorrecto;

    public override void OnTrackingFound()
    {
	    //targetAnim.Play(onFoundAnimName);
	    
	    if(!escaneo_correcto())
	    {
		    alerta_codigo_incorrecto.SetActive(true);
		    alerta_escaneo.SetActive(false);
	    }
	    else
	    {
		    alerta_codigo_incorrecto.SetActive(false);
		    panel_alerta.SetActive(false);
		    alerta_escaneo.SetActive(true);
		    modelo_experiencia.SetActive(true);
	    }
	    
    }

    public override void OnTrackingLost()
    {
	    //targetAnim.Play(onLostAnimName);
	    panel_alerta.SetActive(true);
	    modelo_experiencia.SetActive(false);
    }
    
	public bool escaneo_correcto()
	{
		bool result = true;
		if(modelo_experiencia.gameObject.name == "experiencia1_animada" && ValoresGlobales.experiencia_seleccionada !=	Experiencias.experiencia1_arado)
		{
			result = false;
		}
		if(modelo_experiencia.gameObject.name == "Cubo" && ValoresGlobales.experiencia_seleccionada !=	Experiencias.experiencia2_crecimiento)
		{
			result = false;
		}
		if(modelo_experiencia.gameObject.name == "esfera" && ValoresGlobales.experiencia_seleccionada !=	Experiencias.experiencia3_partes_planta)
		{
			result = false;
		}
		return result;
	}
	
}
