using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARTrackedImageManager))]
public class DeteccionImagenes : MonoBehaviour
{
	[SerializeField]
	private GameObject[] objetos_a_agregar;
	
	private Dictionary<string,GameObject> objetos_showroom = new Dictionary<string, GameObject>();
	
	private ARTrackedImageManager administrador_imagenes;
	
	public GameObject PanelAlertas;
	public GameObject PanelExperienciasShowroom;
	
	// Awake is called when the script instance is being loaded.
	protected void Awake()
	{
		administrador_imagenes = FindObjectOfType<ARTrackedImageManager>();
		foreach (var objeto in objetos_a_agregar)
		{
			var objeto_creado = Instantiate(objeto,Vector3.zero,Quaternion.identity);
			objeto_creado.name = objeto.name;
			objeto_creado.SetActive(false);
			objetos_showroom.Add(objeto.name,objeto_creado);
		}
	}
	
	// This function is called when the object becomes enabled and active.
	protected void OnEnable()
	{
		administrador_imagenes.trackedImagesChanged += DeteccionImagen;
	}
	
	// This function is called when the behaviour becomes disabled () or inactive.
	protected void OnDisable()
	{
		administrador_imagenes.trackedImagesChanged -= DeteccionImagen;
	}
	
	private void DeteccionImagen(ARTrackedImagesChangedEventArgs args)
	{
		foreach (var imagen_detectada in args.added)
		{
			ActualizarObjeto(imagen_detectada);
		}
		foreach (var imagen_detectada in args.updated)
		{
			if(imagen_detectada.trackingState == UnityEngine.XR.ARSubsystems.TrackingState.Tracking)
				ActualizarObjeto(imagen_detectada);
			else {
				PanelAlertas.SetActive(true);
				PanelExperienciasShowroom.SetActive(false);
				objetos_showroom[imagen_detectada.referenceImage.name].SetActive(false);
			}
		}
		foreach (var imagen_detectada in args.removed)
		{
			PanelAlertas.SetActive(true);
			PanelExperienciasShowroom.SetActive(false);
			objetos_showroom[imagen_detectada.referenceImage.name].SetActive(false);
		}
	}
	
	private void ActualizarObjeto(ARTrackedImage imagen_detectada)
	{
		if(objetos_showroom != null)
		{
			var nombre_imagen = imagen_detectada.referenceImage.name;
			var posicion_imagen = imagen_detectada.transform.position;
			//var posicion_central = new Vector3(Screen.width/2,Screen.height/2,Camera.main.nearClipPlane);
		
			var objeto_a_mostrar = objetos_showroom[nombre_imagen];
			objeto_a_mostrar.transform.position = posicion_imagen;
			objeto_a_mostrar.SetActive(true);
			PanelExperienciasShowroom.SetActive(true);
			PanelAlertas.SetActive(false);
			// administrador_imagenes.trackedImagePrefab
			foreach (var objeto in objetos_showroom.Values)
			{
				if(objeto.name != nombre_imagen)
				{
					objeto.SetActive(false);
				}
			}
		}
		
	}
}
