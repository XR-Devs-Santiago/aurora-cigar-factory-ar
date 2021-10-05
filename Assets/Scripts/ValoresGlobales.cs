using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValoresGlobales : MonoBehaviour
{
	public static Experiencias experiencia_seleccionada;
	public void SetExperiencia(int exp){
		switch (exp)
		{
				case 1:
					experiencia_seleccionada =	Experiencias.experiencia1_arado;
						break;
				case 2:
					experiencia_seleccionada =	Experiencias.experiencia2_crecimiento;
						break;
				case 3:
					experiencia_seleccionada =	Experiencias.experiencia3_partes_planta;
					break;
				case 4:
					experiencia_seleccionada =	Experiencias.experiencia4_secado;
						break;
				case 5:
					experiencia_seleccionada =	Experiencias.experiencia5_tipos_hojas;
						break;
				case 6:
					experiencia_seleccionada =	Experiencias.experiencia6_transporte;
						break;
				case 7:
					experiencia_seleccionada =	Experiencias.experiencia7_enrolado;
						break;
		}
	}
}

public enum Experiencias
{
	experiencia1_arado,
	experiencia2_crecimiento,
	experiencia3_partes_planta,
	experiencia4_secado,
	experiencia5_tipos_hojas,
	experiencia6_transporte,
	experiencia7_enrolado,
}