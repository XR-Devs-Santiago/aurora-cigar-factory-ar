using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_exp2 : MonoBehaviour
{
	public GameObject panel_experiencias, experiencia2, panel_alertas; 
	// This function is called when the object becomes enabled and active.
	protected void OnEnable()
	{
		panel_alertas.SetActive(false);
		panel_experiencias.SetActive(true);
		experiencia2.SetActive(true);
	}
	
	// This function is called when the behaviour becomes disabled () or inactive.
	protected void OnDisable()
	{
		panel_alertas.SetActive(true);
		panel_experiencias.SetActive(false);
		experiencia2.SetActive(false);
	}
}
