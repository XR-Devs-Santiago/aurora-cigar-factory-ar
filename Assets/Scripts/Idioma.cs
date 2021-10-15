using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Idioma : MonoBehaviour
{
  
	public int CambiodeIdioma = 0;
    
	public string[] TextEnglish;
	public string[] TextSpanish;
	public TMP_Text[] Text;

	// Update is called once per frame
	void Update()
	{
		if(CambiodeIdioma== 0){
			IdiomaIngles();
		} else if (CambiodeIdioma == 1) {
			IdiomaSpanish();
		}

	}

	public void Change(){
		if (CambiodeIdioma == 0) {
			CambiodeIdioma = 1;
		} else if (CambiodeIdioma== 1) {
			CambiodeIdioma = 0;
		}

	}

	void IdiomaSpanish(){
		for(int i=0;i<=66;i++){
			if (Text[i] != null) {
				Text[i].text = TextEnglish[i];
		}
		}
	}

	void IdiomaIngles(){
		for(int i=0;i<=66;i++){
			if(Text[i] != null)
		{
				Text[i].text = TextSpanish[i];
		}
		}
		
	}
}
