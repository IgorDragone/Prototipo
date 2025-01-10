using UnityEngine;

public class Tablet : MonoBehaviour
{
    [SerializeField] Canvas tabletCanvas;
    [SerializeField] Canvas instructionsCanvas;
    [SerializeField] Canvas gameCanvas;
    [SerializeField] Canvas playAgainCanvas;
    public GameObject wall;
    public Material StartMaterial;
    public Material GazedAtMaterial;
    public delegate void OnScreenGazed();
    public static event OnScreenGazed OnScreenGazedEvent;
    public AudioSource gameTheme;
    void Start()
    {
        wall.GetComponent<Renderer>().material = StartMaterial;
        gameCanvas.enabled = false;
        tabletCanvas.enabled = false;
        //all the children of gameCanvas are disabled
        foreach (Transform child in tabletCanvas.transform)
        {
            child.gameObject.tag = "Untagged";
        }
        playAgainCanvas.enabled = true;
        foreach (Transform child in playAgainCanvas.transform)
        {
            child.gameObject.tag = "Interactable";
        }
        gameTheme.Stop();
    }

    public void Play()
    {
        // desactivamos el playAgainCanvas
        foreach (Transform child in playAgainCanvas.transform)
        {
            child.gameObject.tag = "Untagged";
        }
        playAgainCanvas.enabled = false;
        gameTheme.Play();
        // activamos el tabletCanvas
        tabletCanvas.enabled = true;
        // desactivamos el instructionsCanvas
        instructionsCanvas.enabled = false;
        // activamos el gameCanvas
        gameCanvas.enabled = true;
        foreach (Transform child in tabletCanvas.transform)
        {
            child.gameObject.tag = "Interactable";
        }
        // cambiamos el material de la pared
        wall.GetComponent<Renderer>().material = GazedAtMaterial;
        OnScreenGazedEvent?.Invoke();
    }
}
