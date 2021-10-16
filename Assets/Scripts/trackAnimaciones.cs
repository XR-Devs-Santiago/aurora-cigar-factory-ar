using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class trackAnimaciones : MonoBehaviour
{
	public Animator anim;
	public Button esp;
	public Button eng;
	public Button esp2;
	public Button eng2;
	public Button googleBTN;
	public Button FacebookBTN;
	public Button SinCuentaBTN;
	public Button experiencia1;
	public Button experiencia2;
	public Button experiencia3;
	public Button experiencia4;
	public Button experiencia5;
	public Button experiencia6;
	public Button experiencia7;
	
    // Start is called before the first frame update
    void Start()
	{
		//animacion de login
	    esp.onClick.AddListener(espanol);
		eng.onClick.AddListener(espanol);
	    
		esp2.onClick.AddListener(espanol);
		eng2.onClick.AddListener(espanol);
		
		//Animaciones panel principal
		googleBTN.onClick.AddListener(encenderAnimacionLogin);
		FacebookBTN.onClick.AddListener(encenderAnimacionLogin);
		SinCuentaBTN.onClick.AddListener(encenderAnimacionLogin);
		
		//Animacion de la alerta
		experiencia1.onClick.AddListener(encenderAnimacionAlert);
		experiencia2.onClick.AddListener(encenderAnimacionAlert);
		experiencia3.onClick.AddListener(encenderAnimacionAlert);
		experiencia4.onClick.AddListener(encenderAnimacionAlert);
		experiencia5.onClick.AddListener(encenderAnimacionAlert);
		experiencia6.onClick.AddListener(encenderAnimacionAlert);
		experiencia7.onClick.AddListener(encenderAnimacionAlert);
	    
    }
    
    
	void espanol(){
		anim.Play("Login anim");
	}

	void encenderAnimacionLogin(){
		anim.Play("panelprincipal anim");
	}
	
	void encenderAnimacionAlert(){
		anim.Play("alert escanear codigo");
	}
    // Update is called once per frame
    void Update()
    {
	   
    }
}
