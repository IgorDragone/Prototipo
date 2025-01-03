using UnityEngine;
using UnityEngine.UI;

public class VolumeManager : MonoBehaviour
{
    [SerializeField] private GameObject startMenu; // Menú inicial
    [SerializeField] private GameObject volumeMenu; // Menú de Volumen
    [SerializeField] private Slider volumeSlider; // Referencia al slider
    public delegate void VolumeChange(float volume);
    public static event VolumeChange OnVolumeChange;
    private float currentVolume = 1f;

    void Start()
    {
        // Configura el slider y su evento de cambio
        volumeSlider.value = currentVolume;
        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    public void SetVolume(float volume)
    {
        currentVolume = volume;
        OnVolumeChange?.Invoke(volume);
    }

    // Método para mostrar el menú de volumen
    public void ShowVolumeMenu()
    {
        // Ocultar el menú inicial
        startMenu.SetActive(false);
        // Mostrar el menú de volumen
        volumeMenu.SetActive(true);
    }

    // Método para volver al menú inicial
    public void ShowStartMenu()
    {
        // Ocultar el menú de volumen
        volumeMenu.SetActive(false);
        // Mostrar el menú inicial
        startMenu.SetActive(true);
    }
}
