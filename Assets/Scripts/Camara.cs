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
	public GameObject[] ocultarObjCamara;
	
	string filePath;
	
	
	// Start is called before the first frame update
	void Start()
	{
		NativeGallery.CheckPermission(NativeGallery.PermissionType.Write);
		NativeGallery.RequestPermission(NativeGallery.PermissionType.Write);

	}
	
	void OnEnable(){
		if(!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite)){
			Permission.RequestUserPermission(Permission.ExternalStorageWrite);
		}
	}
    
    
	public void SetActivesObjetos(bool valor){
		for(int i = 0; i<ObjetosAOcultar.Length; i++){
			ObjetosAOcultar[i].SetActive(valor);
		}
	}
	
	
	public void SetObjetosCamara(bool valor){
		for(int i = 0; i<ObjetosAOcultar.Length; i++){
			ocultarObjCamara[i].SetActive(valor);
		}
	}
	
	
	public void TakeScreenShot(){
		SetActivesObjetos(false);
		StartCoroutine("CaptureIt");
	}

	IEnumerator CaptureIt(){
		
		yield return new WaitForEndOfFrame();
		Texture2D ss = new Texture2D( Screen.width, Screen.height, TextureFormat.RGB24, false );
		ss.ReadPixels( new Rect( 0, 0, Screen.width, Screen.height ), 0, 0 );
		ss.Apply();
		filePath = Path.Combine( Application.temporaryCachePath, "shared img.png" );
		File.WriteAllBytes( filePath, ss.EncodeToPNG() );

		
		// To avoid memory leaks
		Destroy( ss );

		SetActivesObjetos(true);
		
		
		new NativeShare().AddFile( filePath )
			.SetSubject( "MuralAR" ).SetText( "Mural Photo" )
			.SetCallback( ( result, shareTarget ) => Debug.Log( "Share result: " + result + ", selected app: " + shareTarget ) )
			.Share();
		
	}
	
	public void Descartar(){

		SetActivesObjetos(true);
	}
	
	public void Guardar(){
		NativeGallery.SaveImageToGallery();

	}
}