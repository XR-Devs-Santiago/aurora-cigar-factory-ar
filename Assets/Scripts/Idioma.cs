using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Idioma : MonoBehaviour
{
  
	public int CambiodeIdioma = 1;
    
	public string[] TextEnglish;
	public string[] TextSpanish;
	public Text[] Text;

	// Update is called once per frame
	void Update()
	{
		if(CambiodeIdioma== 0){
			IdiomaIngles();
		}else if(CambiodeIdioma == 1){
			IdiomaSpanish();
		}

	}

	public void Change(){
		if(CambiodeIdioma == 0){
			CambiodeIdioma = 1;
		}else if(CambiodeIdioma== 1){
			CambiodeIdioma = 0;
		}

	}

	void IdiomaIngles(){
		if(Text[0] != null){
			Text[0].text = TextEnglish[0];
		}
	
        
	}

	void IdiomaSpanish(){
		if(Text[0] != null){
			Text[0].text = TextSpanish[0];
		}

	}
}
