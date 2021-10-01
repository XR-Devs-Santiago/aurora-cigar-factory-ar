using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BarraProgreso : MonoBehaviour
{
	Slider Barra;
	public float max, act;
	public TMP_Text ValorBarraProgreso, NumEstaciones;
	public	bool[] advarr =new bool[7];
	private Color ColorBarra = Color.green;
	
	

	private void Awake()
	{
		Barra = GetComponent<Slider>();
	}
	// Start is called before the first frame update
	void Start()
	{
		for(int a=0;a<8;a++){
			advarr[a] = false;
		}
	}

	// Update is called once per frame
	void Update()
	{
		ActualizarBarra(max, act);
	}

	void ActualizarBarra(float ValorMax, float ValorAct)
	{
		float porcentaje;
		int exp;
		porcentaje = ValorAct / ValorMax;
		Barra.value = porcentaje;
		porcentaje =(Mathf.Round(porcentaje*100));
		ValorBarraProgreso.text = porcentaje +"%";
		NumEstaciones.text = act.ToString();
		
	}
    
	public void ValorBarra(int i){
		
		for (int x = 0; x < 8; x++) {
			if (i==x)
			{
				if (advarr[x] == false)
				{	
					act ++;
					advarr[x] = true;
				}
			
			}
		}
	}
	
	
}