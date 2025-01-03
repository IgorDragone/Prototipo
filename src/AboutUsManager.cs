using UnityEngine;
using UnityEngine.UI;

public class AboutUsManager : MonoBehaviour
{
    [SerializeField] private GameObject startMenu; // Menú inicial
    [SerializeField] private GameObject aboutUsMenu; // Menú de información

    // Método para mostrar el menú de información
    public void ShowAboutUsMenu()
    {
        // Ocultar el menú inicial
        startMenu.SetActive(false);
        // Mostrar el menú de información
        aboutUsMenu.SetActive(true);
    }

    // Método para volver al menú inicial
    public void ShowStartMenu()
    {
        // Ocultar el menú de información
        aboutUsMenu.SetActive(false);
        // Mostrar el menú inicial
        startMenu.SetActive(true);
    }
}