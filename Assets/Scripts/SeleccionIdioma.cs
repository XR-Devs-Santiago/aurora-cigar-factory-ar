using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeleccionIdioma : MonoBehaviour
{
	
	public GameObject PanelSeleccionLenguaje;
	public GameObject PanelLogin;
    
	public void SeleccionarIdiomaEspanol()
    {
	    Debug.Log("Boton clickeado!");
	    encenderPanel(PanelLogin);
    }
    
	//Este metodo enciende el panel que reciba como argumento
	//Raldy de Jesus 27/09/2021
	public void encenderPanel(GameObject panelEncender)
	{
		//apago panel Seleccion de Idioma
		PanelSeleccionLenguaje.SetActive(false);
		//enciendo el panel recibido como argumento
		panelEncender.SetActive(true);
		
	}
    
}
