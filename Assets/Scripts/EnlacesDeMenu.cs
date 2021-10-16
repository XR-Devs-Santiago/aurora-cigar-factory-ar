using UnityEngine;

public class EnlacesDeMenu : MonoBehaviour
{
    public void AbrirURL(int BotonPresionado)
    {

        switch (BotonPresionado)
        {
            case 0:
                // Politica privacidad 0
                Application.OpenURL("https://laauroracigarworld.com/politica-privacidad/#:~:text=POL%C3%8DTICA%20DE%20PRIVACIDAD%20Y%20SEGURIDAD%20DEL%20USUARIO%22");
                break;
            case 1:
                // La aurora website 1
                Application.OpenURL("https://www.laaurora.com.do/");
                break;
            case 2:
                // Valora la app TODO: (cambiar el link)
                Application.OpenURL("https://play.google.com/store/apps/details?id=com.google.android.googlequicksearchbox");
                break;
        }
    }
}
