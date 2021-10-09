using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Android;

public class Camara : MonoBehaviour
{
	public GameObject[] ObjetosAOcultar;
	public GameObject panelEscaner;
	public GameObject panelFoto;
	public GameObject objetoImagen;
	public GameObject AdvertenciaGuardar;
	public GameObject AdvertenciaDescartar;
	
	Texture2D lastImageTexture;
	string lastFileName = "";
	
	
	// Start is called before the first frame update
	void Start()
	{
		NativeGallery.CheckPermission(NativeGallery.PermissionType.Write);
		SetActivesObjetos(true);
		EncenderUnPanel(panelEscaner);
	}
	
	void OnEnable(){
		if(!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite)){
			Permission.RequestUserPermission(Permission.ExternalStorageWrite);
		}
	}
    
	public void EncenderUnPanel(GameObject panel){
		panelEscaner.SetActive(false);
		panelFoto.SetActive(false);
		panel.SetActive(true);
		
		if(panel == panelFoto){
			UpdateImage();
		}
	}
	
	void UpdateImage(){
		lastImageTexture = GetScreenshotImage();
		Sprite sp = Sprite.Create(lastImageTexture, new Rect(0,0, lastImageTexture.width, lastImageTexture.height), new Vector2(0.5f, 0.5f));
		objetoImagen.GetComponent<Image>().sprite = sp;
	}
	
	Texture2D GetScreenshotImage(){
		Texture2D texture = null;
		byte[] fileBytes;
		fileBytes = File.ReadAllBytes(Application.persistentDataPath+"/"+lastFileName);
		texture = new Texture2D(2,2, TextureFormat.RGB24, false);
		texture.LoadImage(fileBytes);
		return texture;
	}
	
	void Update(){
		if(File.Exists(Application.persistentDataPath+"/"+lastFileName)){
			SetActivesObjetos(true);
			EncenderUnPanel(panelFoto);
		}
	}
	
    
	void SetActivesObjetos(bool valor){
		for(int i = 0; i<ObjetosAOcultar.Length; i++){
			ObjetosAOcultar[i].SetActive(valor);
		}
	}

	public void TakeScreenShot(){
		StartCoroutine("CaptureIt");
	}
	
	IEnumerator CaptureIt(){
		SetActivesObjetos(false);
		string timeStamp = System.DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss");
		string fileName = "MuralAR-"+timeStamp+".png";
		lastFileName = fileName;
		ScreenCapture.CaptureScreenshot(fileName);
		yield return new WaitForEndOfFrame();
	}
	
	public void Descartar(){
		File.Delete(Application.persistentDataPath+"/"+lastFileName);
		SetActivesObjetos(true);
		AdvertenciaDescartar.SetActive(false);
		EncenderUnPanel(panelEscaner);
	}
	
	
	public void Guardar(){
		//NativeToolkit.SaveImage(lastImageTexture, "Almacenamiento interno/DCIM/Camera", ".png");
		
		File.WriteAllBytes( "/storage/emulated/0/Mural/tabaco.png",lastImageTexture.EncodeToPNG());
	}
	
	void OnImageSaved(string loquesea){
	}
	
	void DespuesDeGuardar(bool sucess, string path){
		if(sucess){
			AdvertenciaGuardar.SetActive(true);
			Descartar();
		}
	}
	
	public void DesactivarAdvertencia(){
		AdvertenciaDescartar.SetActive(false);
		AdvertenciaGuardar.SetActive(false);
	}
	
	
	public void Compartir(){
		new NativeShare().AddFile( lastImageTexture, lastFileName)
			.SetSubject( "MuralAR" ).SetText( "Mural Photo" )
			.SetCallback( ( result, shareTarget ) => Debug.Log( "Share result: " + result + ", selected app: " + shareTarget ) )
			.Share();
	}
	
	public void volverApaneles(){
		SceneManager.LoadScene("paneles");
	}
	
}