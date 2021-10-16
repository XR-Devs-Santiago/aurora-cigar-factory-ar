using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Idioma : MonoBehaviour
{
  
	public int CambiodeIdioma ;
    public GameObject panelLenguaje;
	public GameObject panelLogin;
	public string[] TextEnglish;
	public string[] TextSpanish;
	public TMP_Text[] Text;
	public Button englishButton, spanishButton;
	GameObject enUnchecked, enChecked, esUnchecked, esChecked;

	// Start is called on the frame when a script is enabled just before any of the Update methods is called the first time.
	void Start()
	{
		// 999 es el defaultValue que quiere decir que el valor no fue encontrado
		var idioma = PlayerPrefs.GetInt("idioma", 999);

		if (idioma != 999) {
			CambiodeIdioma = idioma;
			panelLenguaje.SetActive(false);
			panelLogin.SetActive(true);
		} else { 
			CambiodeIdioma = 0;
		}

		englishButton.onClick.AddListener(enOnClick);
		spanishButton.onClick.AddListener(esOnClick);
		enUnchecked = englishButton.transform.Find("Unchecked").gameObject;
		enChecked = englishButton.transform.Find("Checked").gameObject;
		esUnchecked = spanishButton.transform.Find("Unchecked").gameObject;
		esChecked = spanishButton.transform.Find("Checked").gameObject;
	}

	// Update is called once per frame
	void Update()
	{		
		if(CambiodeIdioma == 0){
			IdiomaIngles();
		} else if (CambiodeIdioma == 1) {
			IdiomaSpanish();
		}

		initLanguageToggles(CambiodeIdioma);
	}

	public void Change(int valor){
		// Guardando valor seleccionado de idioma en preferencias de jugador.
		PlayerPrefs.SetInt("idioma", valor);

		if (valor != null) {
			CambiodeIdioma = valor;
		} else if (CambiodeIdioma == 0) {
			CambiodeIdioma = 1;
		} else if (CambiodeIdioma== 1) {
			CambiodeIdioma = 0;
		}
	}

	void IdiomaIngles(){
		for(int i=0;i<=66;i++){
			if (Text[i] != null) {
				Text[i].text = TextEnglish[i];
			}
		}
	}

	void IdiomaSpanish(){
		for(int i=0;i<=66;i++){
			if(Text[i] != null)
			{
				Text[i].text = TextSpanish[i];
			}
		}
	}

    void initLanguageToggles(int value)
    {
        Debug.Log(value);
        if (value == 0)
        {
			enChecked.SetActive(true);
			esChecked.SetActive(false);
		}
        else if (value == 1)
        {
			enChecked.SetActive(false);
			esChecked.SetActive(true);
		}
    }

    void enOnClick(){
		if (enChecked.activeSelf) {
			enChecked.SetActive(false);
			esChecked.SetActive(true);
			Change(1);
		} else {
			enChecked.SetActive(true);
			esChecked.SetActive(false);
			Change(0);
		}
	}

	void esOnClick(){		
		if (esChecked.activeSelf) {
			esChecked.SetActive(false);
			enChecked.SetActive(true);
			Change(0);
		} else {
			esChecked.SetActive(true);
			enChecked.SetActive(false);
			Change(1);
		}
	}
}
