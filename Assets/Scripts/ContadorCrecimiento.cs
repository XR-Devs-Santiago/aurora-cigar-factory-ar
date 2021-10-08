using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ContadorCrecimiento : MonoBehaviour
{
	//en caso de querer reiniciar el conteo o hacerlo inverso
	[Tooltip("Tiempo inicial en segundos")]
	public	int tiempoInicial;
	
	[Tooltip("Escala del tiempo del reloj")]
	[Range(-12.0f,12.0f)]
	public	float	EscalaDeTiempo = 1;
	
	public	TMP_Text MyText;
	private	float TiempoDelFrameConTimeScale = 0f;
	private	float TiempoAMostrarEnDias = 0f;
	private float EscalaDeTiempoAlPausar, EscalaDeTiempoInicial;
	private	bool EstaPausado = false;
	

    // Start is called before the first frame update
    void Start()
    {
	    EscalaDeTiempoInicial = EscalaDeTiempo;
	    MyText = GetComponent<TMP_Text>();
	    TiempoAMostrarEnDias = tiempoInicial;	
	    ActualizarReloj(tiempoInicial); 
    }

    // Update is called once per frame
    void Update()
    {
	    //variable que representa el tiempo de cada frame considerando la escala de tiempo
	    TiempoDelFrameConTimeScale = Time.deltaTime*EscalaDeTiempo;
	    
	    //variable que acumula el tiempo transcurrido para luego mostrarlo en el reloj
	    TiempoAMostrarEnDias += TiempoDelFrameConTimeScale;
	    ActualizarReloj(TiempoAMostrarEnDias);
    }
    
    
	public void ActualizarReloj(float tiempoensegundos){
		int semanas =0;
		int dias=0;
		string TextoReloj;
		
		//Validando que el tiempo no sea negativo
		if(tiempoensegundos<0) tiempoensegundos=0;
		
		//calcular minutos y segundos
		semanas = (int)tiempoensegundos/30;
		dias = (int)tiempoensegundos%30;
		
		//crear la cadena de caracteres con 2 digitos para los minutos y segundos,separados por ":"
		TextoReloj =( semanas.ToString("00") + " Semanas\n"+ dias.ToString(" 00")+" Días");
		
		//actualizar el elemento de text de ui con la cadena de caracteres
		MyText.text = TextoReloj;
		if(semanas == 04){
			pausar();
		}
	}
	
	public	void pausar(){
		if (!EstaPausado){
			EstaPausado = true;
			EscalaDeTiempoAlPausar = EscalaDeTiempo;
			EscalaDeTiempo = 0;
		}
	}
	
}
