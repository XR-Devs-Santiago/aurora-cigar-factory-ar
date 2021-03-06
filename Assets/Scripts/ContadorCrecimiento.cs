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
	public	float	EscalaDeTiempo = 12, EscalaDeTiempoCierreCompletado = 60;
	
	public	TMP_Text MyText;
	private	float TiempoDelFrameConTimeScale = 0f;
	private	float TiempoAMostrarEnDias = 0f, tiempo_cierre = 0f;
	private float EscalaDeTiempoAlPausar, EscalaDeTiempoInicial;
	private	bool EstaPausado = false;
	private int segundos_cierre = 0;
	
	public GameObject Panel1, Panel2, Panel3, Panel4, AlertaCompletado;
	
	// This function is called when the object becomes enabled and active.
	protected void OnEnable()
	{
		EscalaDeTiempoInicial = EscalaDeTiempo;
		MyText = GetComponent<TMP_Text>();
		TiempoAMostrarEnDias = tiempoInicial;	
		ActualizarReloj(tiempoInicial);
	}
    // Start is called before the first frame update
    //void Start()
    //{
	//    EscalaDeTiempoInicial = EscalaDeTiempo;
	//    MyText = GetComponent<TMP_Text>();
	//    TiempoAMostrarEnDias = tiempoInicial;	
	//    ActualizarReloj(tiempoInicial); 
    //}

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
		switch (semanas)
		{
			case 01:
					activar_panel(Panel1);
					break;
			case 02:
				activar_panel(Panel2);
					break;
			case 03:
				activar_panel(Panel3);
					break;
			case 04:
				if(EstaPausado == false)
				activar_panel(Panel4);
					break;
		}

		if(semanas == 04 && EstaPausado == false){
			if(AlertaCompletado.active == false)
			{
				pausar();
				Panel4.SetActive(false);
				AlertaCompletado.SetActive(true);
			}
			
		}
		if(EscalaDeTiempo == 0)
		{
			if(segundos_cierre < 5)
			{
				tiempo_cierre += Time.deltaTime*EscalaDeTiempoCierreCompletado;
				segundos_cierre = (int)tiempo_cierre/60;
				if(segundos_cierre == 4)
				{
					AlertaCompletado.SetActive(false);
				}
			}
		}
	}
	
	public	void pausar(){
		if (!EstaPausado){
			EstaPausado = true;
			EscalaDeTiempoAlPausar = EscalaDeTiempo;
			EscalaDeTiempo = 0;
		}
	}
	
	private void activar_panel(GameObject panel_activado)
	{
		desactivar_todos();
		panel_activado.SetActive(true);
	}
	
	private void desactivar_todos()
	{
		Panel1.SetActive(false);
		Panel2.SetActive(false);
		Panel3.SetActive(false);
		Panel4.SetActive(false);
	}
	// This function is called when the behaviour becomes disabled () or inactive.
	protected void OnDisable()
	{
		AlertaCompletado.SetActive(false);
		desactivar_todos();
		EscalaDeTiempo = 12;
		EscalaDeTiempoCierreCompletado = 60;
		tiempo_cierre = 0f;
		TiempoAMostrarEnDias = 0f;
		TiempoDelFrameConTimeScale = 0f;
		segundos_cierre = 0;
		EstaPausado = false;
	}
	
}
