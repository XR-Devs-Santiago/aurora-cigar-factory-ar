using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Experiencia3Info : MonoBehaviour
{
	[SerializeField]
	private GameObject[] infos;
	public void AlternarInfo(string info_destino)
	{
		foreach (var info in infos)
		{
			if(info.name == info_destino)
			{
			   if(!info.activeSelf)
	           {
				    info.SetActive(true);
			   }
			   else
			   {
					info.SetActive(false);
			   }
			}
			else
			{
			  	info.SetActive(false);
			}
		}
	}
}
