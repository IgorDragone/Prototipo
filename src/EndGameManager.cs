using UnityEngine;
using TMPro;    

public class EndGameManager : MonoBehaviour
{
    public GameObject wall;
    public Material winMaterial;
    public Material loseMaterial;
    [SerializeField] Canvas gameCanvas;
    [SerializeField] Canvas tabletCanvas;
    [SerializeField] Canvas playAgainCanvas;
    [SerializeField] private TextMeshProUGUI playAgainText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void OnEnable()
    {
        GameStarter.OnGameWonEvent += Win;
        GameStarter.OnGameLostEvent += Lose;
    }

    void OnDisable()
    {
        GameStarter.OnGameWonEvent -= Win;
        GameStarter.OnGameLostEvent -= Lose;
    }

    void Win()
    {
        //Debug.Log("Win");
        gameCanvas.enabled = false;
        wall.GetComponent<Renderer>().material = winMaterial;
        PlayAgain();
    }

    void Lose()
    {
        //Debug.Log("Lose");
        gameCanvas.enabled = false;
        wall.GetComponent<Renderer>().material = loseMaterial;
        PlayAgain();
    }

    void PlayAgain()
    {
        //Debug.Log("PlayAgain");
        tabletCanvas.enabled = false;
        foreach (Transform child in gameCanvas.transform)
        {
            child.gameObject.tag = "Untagged";
        }
        playAgainText.text = "PLAY AGAIN";
        playAgainCanvas.enabled = true;
        foreach (Transform child in playAgainCanvas.transform)
        {
            child.gameObject.tag = "Interactable";
        }
    }
}
