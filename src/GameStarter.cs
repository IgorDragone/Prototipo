using System.Collections;
using UnityEngine;
using TMPro;
using System.Linq;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using SimpleJSON;
using UnityEngine.Networking;

public class GameStarter : MonoBehaviour
{
    public GameObject player;
    public int apiQuestionCount = 5;
    private string deepLApiKey = "a0c105cf-0848-41ac-b06b-9c57a8b8d1fe:fx";
    private Vector3 startingPosition = new Vector3(100, 135, 80);
    [SerializeField] private TextMeshProUGUI questionText;
    [SerializeField] private TextMeshProUGUI option1Text;
    [SerializeField] private TextMeshProUGUI option2Text;
    [SerializeField] private TextMeshProUGUI option3Text;
    [SerializeField] private TextMeshProUGUI option4Text;
    //array of option texts
    [SerializeField] private TextMeshProUGUI[] optionTexts;
    [SerializeField] private TextMeshProUGUI scoreText;
    private char currentCorrectOption;
    private int currentQuestionIndex = 0;
    private int score = 0;
    [Serializable]
    public class Question
    {
        public string questionText;
        public string[] options;
        public char correctAnswer;


        public Question(string questionText, string[] options, char correctAnswer)
        {
            this.questionText = questionText;
            this.options = options;
            this.correctAnswer = correctAnswer;
        }
    }
    private List<Question> questions;
    private Question currentQuestion;
    public delegate void OnGameWon();
    public static event OnGameWon OnGameWonEvent;
    public delegate void OnGameLost();
    public static event OnGameLost OnGameLostEvent;
    public AudioSource wrongAnswer;
    public AudioSource correctAnswer;
    public AudioSource gameTheme;

    void Start() {
        questions = new List<Question>
        {
            new Question("¿En qué año llegó Cristóbal Colón a América?", new string[] {"1490", "1492", "1495", "2025 (Antes de ayer)"}, 'B'),
            new Question("¿Quién fue el primer presidente de los Estados Unidos?", new string[] {"Isabel Sánchez", "George Washington", "Thomas Jefferson", "John Adams"}, 'B' ),
            new Question("¿Cuál es el planeta más grande del sistema solar?", new string[] {"Saturno", "Júpiter", "Marte", "Miercole"}, 'B'),
            new Question("¿Cuál es la capital de Australia?", new string[] {"Sídney", "Melbourne", "Canberra", "Añaza"}, 'C'),
            new Question("¿Qué río atraviesa Egipto?", new string[] {"Amazonas", "Támesis", "Danubio", "Nilo"}, 'D'),
            new Question("¿Qué director es conocido por la película 'Titanic'?", new string[] {"Steven Spielberg", "Quentin Tarantino", "James Cameron", "Christopher Nolan"}, 'C'),
            new Question("¿En qué deporte se utiliza una raqueta y una pelota?", new string[] {"Fútbol", "Rugby", "Baloncesto", "Tenis"}, 'D'),
            new Question("¿Cuántos jugadores componen un equipo de fútbol en el campo?", new string[] {"10", "9", "12", "11"}, 'D'),
            new Question("¿Dónde se celebraron los Juegos Olímpicos de 2016?", new string[] {"Tokio", "Río de Janeiro", "Londres", "Atenas"}, 'B'),
            new Question("¿Quién escribió 'Don Quijote de la Mancha'?", new string[] {"Miguel de Cervantes", "Gabriel García Márquez", "Lope de Vega", "Federico García Lorca"}, 'A'),
            new Question("¿Cuál es el metal más abundante en la corteza terrestre?", new string[] {"Aluminio", "Hierro", "Cobre", "Oro"}, 'A'),
            new Question("¿Qué animal es conocido como el rey de la selva?", new string[] {"Tigre", "Elefante", "León", "Lobo"}, 'C'),
            new Question("¿Cuál es el idioma más hablado del mundo?", new string[] {"Español", "Inglés", "Mandarín", "Árabe"}, 'C'),
            new Question("¿Qué gas se encuentra en mayor cantidad en la atmósfera terrestre?", new string[] {"Oxígeno", "Hidrógeno", "Dióxido de carbono", "Nitrógeno"}, 'D'),
            new Question("¿Cuál es el país más grande del mundo?", new string[] {"Canadá", "Estados Unidos", "China", "Rusia"}, 'D'),
            new Question("¿Cuál es el océano más grande del mundo?", new string[] {"Atlántico", "Índico", "Pacífico", "Ártico"}, 'C'),
            new Question("¿Cuándo comenzó la Primera Guerra Mundial?", new string[] {"1914", "1918", "1920", "1939"}, 'A'),
            new Question("¿Quién pintó la Mona Lisa?", new string[] {"Leonardo da Vinci", "Pablo Picasso", "Vincent van Gogh", "Salvador Dalí"}, 'A'),
            new Question("¿Cuál es el continente más grande?", new string[] {"África", "Asia", "Europa", "América"}, 'B'),
            new Question("¿Qué instrumento musical tiene 88 teclas?", new string[] {"Guitarra", "Piano", "Violín", "Flauta"}, 'B'),
            new Question("¿En qué año se llegó a la Luna por primera vez?", new string[] {"1965", "1969", "1972", "1981"}, 'B'),
            new Question("¿Quién escribió 'Cien años de soledad'?", new string[] {"Gabriel García Márquez", "Mario Vargas Llosa", "Julio Cortázar", "Pablo Neruda"}, 'A'),
            new Question("¿Cuál es el país con más población del mundo?", new string[] {"India", "Estados Unidos", "Rusia", "China"}, 'D'),
            new Question("¿Qué océano está entre África y América?", new string[] {"Atlántico", "Pacífico", "Índico", "Ártico"}, 'A'),
            new Question("¿Cuál es el metal más caro del mundo?", new string[] {"Platino", "Oro", "Iridio", "Ródio"}, 'D'),
            new Question("¿En qué país se originó el tango?", new string[] {"Brasil", "Argentina", "Cuba", "México"}, 'B'),
            new Question("¿Quién fue el primer hombre en el espacio?", new string[] {"Neil Armstrong", "Yuri Gagarin", "John Glenn", "Alan Shepard"}, 'B'),
            new Question("¿Cuál es la capital de Japón?", new string[] {"Seúl", "Tokio", "Pekín", "Bangkok"}, 'B'),
            new Question("¿Cuántos colores tiene el arcoíris?", new string[] {"6", "8", "7", "9"}, 'C'),
            new Question("¿Quién pintó la famosa obra 'La noche estrellada'?", new string[] {"Pablo Picasso", "Vincent van Gogh", "Claude Monet", "Salvador Dalí"}, 'B'),
            new Question("¿En qué continente se encuentra el desierto del Sahara?", new string[] {"Asia", "África", "Australia", "América"}, 'B'),
            new Question("¿Cuál es la capital de Italia?", new string[] {"Roma", "Madrid", "París", "Londres"}, 'A'),
            new Question("¿Qué tipo de animal es la ballena?", new string[] {"Pez", "Reptil", "Mamífero", "Aves"}, 'C'),
            new Question("¿Quién fue el autor de la teoría de la relatividad?", new string[] {"Isaac Newton", "Albert Einstein", "Galileo Galilei", "Nikola Tesla"}, 'B'),
            new Question("¿Qué continente tiene más países?", new string[] {"África", "Asia", "Europa", "América"}, 'A'),
            new Question("¿Cuál es el animal terrestre más grande?", new string[] {"Elefante", "Jirafa", "Rinoceronte", "Hipopótamo"}, 'A'),
            new Question("¿Quién fue el primero en pisar la luna?", new string[] {"Yuri Gagarin", "Neil Armstrong", "Buzz Aldrin", "John Glenn"}, 'B'),
            new Question("¿Cuál es la moneda oficial de Japón?", new string[] {"Yuan", "Won", "Yen", "Baht"}, 'C'),
            new Question("¿Qué instrumento musical tiene cuerdas y se toca con un arco?", new string[] {"Piano", "Guitarra", "Violín", "Flauta"}, 'C'),
            new Question("¿En qué año terminó la Segunda Guerra Mundial?", new string[] {"1945", "1940", "1950", "1939"}, 'A'),
            new Question("¿Cuál es el continente más pequeño del mundo?", new string[] {"Europa", "Oceanía", "Antártida", "Asia"}, 'B'),
            new Question("¿Qué planeta es conocido como el planeta rojo?", new string[] {"Júpiter", "Marte", "Venus", "Saturno"}, 'B'),
            new Question("¿Cuál es el océano más pequeño del mundo?", new string[] {"Atlántico", "Índico", "Ártico", "Pacífico"}, 'C'),
            new Question("¿Qué animal tiene el corazón más grande?", new string[] {"Ballena", "Elefante", "Jirafa", "Tiburón"}, 'A'),
            new Question("¿Qué instrumento musical se toca con un arco y tiene 4 cuerdas?", new string[] {"Piano", "Violín", "Saxofón", "Guitarra"}, 'B'),
            new Question("¿Cuál es el segundo planeta más cercano al Sol?", new string[] {"Venus", "Mercurio", "Marte", "Tierra"}, 'A'),
            new Question("¿Quién escribió 'Harry Potter'?", new string[] {"J.R.R. Tolkien", "J.K. Rowling", "George R.R. Martin", "C.S. Lewis"}, 'B'),
            new Question("¿En qué país se originó el sushi?", new string[] {"China", "Japón", "Corea", "Tailandia"}, 'B'),
            new Question("¿Cuál es la ciudad más poblada del mundo?", new string[] {"Tokio", "Ciudad de México", "Nueva York", "Shanghái"}, 'A'),
            new Question("¿Qué gas produce la fotosíntesis?", new string[] {"Oxígeno", "Dióxido de carbono", "Nitrógeno", "Metano"}, 'A'),
            new Question("¿Quién descubrió la penicilina?", new string[] {"Marie Curie", "Alexander Fleming", "Louis Pasteur", "Charles Darwin"}, 'B'),
            new Question("¿Cuál es el símbolo químico del oro?", new string[] {"Ag", "Au", "Pb", "Fe"}, 'B'),
            new Question("¿Cuál es la capital de España?", new string[] {"Madrid", "Barcelona", "Valencia", "Sevilla"}, 'A'),
        };
        optionTexts = new TextMeshProUGUI[] {option1Text, option2Text, option3Text, option4Text};
        
        // Load questions from API
        RequestAPIQuestions(TranslateAPIQuestions);
        // Debug API questions and translations:
        // StartCoroutine(DelayedDebug(2));

        VolumeManager.OnVolumeChange += ChangeVolume;

        correctAnswer.Stop();
        wrongAnswer.Stop();
    }

    public void StartGame()
    {
        player.transform.position = startingPosition;
    }

    private void OnEnable() {
        Tablet.OnScreenGazedEvent += StartQuestion;
    }

    private void OnDisable() {
        Tablet.OnScreenGazedEvent -= StartQuestion;
    }

    void StartQuestion()
    {
        System.Random random = new System.Random();
        questions = questions.OrderBy(q => random.Next()).ToList();
        // we only need 15 questions
        questions = questions.GetRange(0, 15);
        currentQuestionIndex = 0;
        score = 0;
        scoreText.text = score + "/15";
        timeLeft = 60.0f;
        EnableWildcards();
        NextQuestion();
    }

    void NextQuestion() 
    {
        if (askTheAudienceUsed)
        {
            audiencePanel.SetActive(false);
        }
        correctAnswer.Stop();
        currentQuestion = questions[currentQuestionIndex];
        questionText.text = currentQuestion.questionText;
        option1Text.text = currentQuestion.options[0];
        option2Text.text = currentQuestion.options[1];
        option3Text.text = currentQuestion.options[2];
        option4Text.text = currentQuestion.options[3];
        currentCorrectOption = currentQuestion.correctAnswer;
        SetTimer(true);
    }

    public void IsAnswerA()
    {
        //CheckAnswer('A');
        StartCoroutine(CheckAnswer('A'));
    }

    public void IsAnswerB()
    {
        //CheckAnswer('B');
        StartCoroutine(CheckAnswer('B'));
    }

    public void IsAnswerC()
    {
        //CheckAnswer('C');
        StartCoroutine(CheckAnswer('C'));
    }

    public void IsAnswerD()
    {
        //CheckAnswer('D');
        StartCoroutine(CheckAnswer('D'));
    }

    private IEnumerator CheckAnswer(char answer)
    {
        SetTimer(false);
        int answerIndex = (int)answer - 65;
        optionTexts[answerIndex].text = "<color=orange>" + optionTexts[answerIndex].text + "</color>";

        yield return new WaitForSeconds(2);
        int correctIndex = (int)currentCorrectOption - 65;
        if (answer != currentCorrectOption)
        {   
            gameTheme.Stop();
            wrongAnswer.Play();
            optionTexts[correctIndex].text = "<color=green>" + optionTexts[correctIndex].text + "</color>";
        } else {
            correctAnswer.Play();
            optionTexts[answerIndex].text = optionTexts[answerIndex].text.Replace("<color=orange>", "<color=green>");
        }
        
        yield return new WaitForSeconds(2);


        if (answer == currentCorrectOption)
        {
            score++;
            scoreText.text =  score + "/15";
            currentQuestionIndex++;
            if (currentQuestionIndex < 15)
            {   
                //We wait 3 seconds to show the next question
                yield return new WaitForSeconds(3);
                timeLeft = 60.0f;
                NextQuestion();
            }
            else
            {
                // End game with a win
                OnGameWonEvent?.Invoke();
            }
        }
        else
        {
            // End game with a loss
            OnGameLostEvent?.Invoke();
        }
        yield return new WaitForSeconds(2);
    }

    private void ChangeVolume(float volume)
    {
        correctAnswer.volume = volume;
        wrongAnswer.volume = volume;
        gameTheme.volume = volume;
    }

    // Comodines
    private bool fiftyFiftyUsed = false;
    private bool phoneAFriendUsed = false;
    private bool askTheAudienceUsed = false;
    [SerializeField] private Button fiftyFiftyButton;
    [SerializeField] private Button phoneAFriendButton;
    [SerializeField] private Button askTheAudienceButton;

    // Comodín 50/50 (Elimina dos respuestas incorrectas)
    public void FiftyFifty()
    {
        if (fiftyFiftyUsed) return; // Comprobar si ya se ha usado

        fiftyFiftyUsed = true;
        List<int> incorrectOptions = new List<int>();
        
        for (int i = 0; i < 4; i++)
        {
            if (currentCorrectOption - 65 != i)
            {
                incorrectOptions.Add(i);
            }
        }

        // Eliminar dos respuestas incorrectas
        int firstIncorrect = incorrectOptions[UnityEngine.Random.Range(0, 2)];
        int secondIncorrect;

        do
        {
            secondIncorrect = incorrectOptions[UnityEngine.Random.Range(0, 2)];
        } while (secondIncorrect == firstIncorrect);

        optionTexts[firstIncorrect].text = "";
        optionTexts[secondIncorrect].text = "";

        fiftyFiftyButton.interactable = false;
    }

    // Comodín Llamada a un amigo (Sugerir la respuesta correcta)
    public void PhoneAFriend()
    {
        if (phoneAFriendUsed) return; // Comprobar si ya se ha usado

        phoneAFriendUsed = true;
        // Mostrar la respuesta correcta como sugerencia
        int correctIndex = (int)currentCorrectOption - 65;
        optionTexts[correctIndex].text = "<color=green>" + optionTexts[correctIndex].text + "</color>";

        phoneAFriendButton.interactable = false;
    }

    [SerializeField] private GameObject audiencePanel; // Panel for audience results
    [SerializeField] private Image[] percentageBars; // Bar images for each option
    [SerializeField] private TextMeshProUGUI[] percentageTexts; // Texts to show percentages

    // Comodín Ayuda del público (Mostrar distribución de respuestas del público)
    public void AskTheAudience()
    {
        if (askTheAudienceUsed) return;
        ResetBars();
        
        askTheAudienceUsed = true;
        
        int correctIndex = (int)currentCorrectOption - 65;
        int correctPercentage = UnityEngine.Random.Range(50, 90);
        int remainingPercentage = 100 - correctPercentage;
        int[] percentages = new int[4];
        
        // Distribute remaining percentage among incorrect answers
        for (int i = 0; i < 4; i++)
        {
            if (i == correctIndex)
            {
                percentages[i] = correctPercentage;
            }
            else
            {
                // Random distribution for remaining percentage
                percentages[i] = remainingPercentage / 3;
            }
            
            // Animate bar filling
            StartCoroutine(AnimateBar(i, percentages[i]));
        }
        
        askTheAudienceButton.interactable = false;
    }

    [SerializeField] private float animationDuration = 5.0f;
    [SerializeField] private float maxBarHeight = 210.0f;
    [SerializeField] private GameObject[] bars;

    private IEnumerator AnimateBar(int index, float targetPercentage)
    {
        foreach (GameObject bar in bars)
        {
            bar.SetActive(true);
        }

        float startTime = Time.time;
        float endTime = startTime + animationDuration;
        
        while (Time.time < endTime)
        {
            float t = (Time.time - startTime) / animationDuration;
            float newHeight = Mathf.Lerp(0, maxBarHeight * targetPercentage / 100, t);
            percentageBars[index].rectTransform.sizeDelta = new Vector2(percentageBars[index].rectTransform.sizeDelta.x, newHeight);
            percentageTexts[index].text = Mathf.RoundToInt(newHeight / maxBarHeight * 100) + "%";
            yield return null;
        }

        percentageBars[index].rectTransform.sizeDelta = new Vector2(percentageBars[index].rectTransform.sizeDelta.x, maxBarHeight * targetPercentage / 100);
        percentageTexts[index].text = Mathf.RoundToInt(targetPercentage) + "%";
    }

    private void ResetBars()
    {
        foreach (Image bar in percentageBars)
        {
            bar.rectTransform.sizeDelta = new Vector2(bar.rectTransform.sizeDelta.x, 0);
        }

        foreach (TextMeshProUGUI text in percentageTexts)
        {
            text.text = "0%";
        }

        foreach (GameObject bar in bars)
        {
            bar.SetActive(false);
        }
    }

    private void EnableWildcards()
    {
        fiftyFiftyUsed = false;
        phoneAFriendUsed = false;
        askTheAudienceUsed = false;
        fiftyFiftyButton.interactable = true;
        phoneAFriendButton.interactable = true;
        askTheAudienceButton.interactable = true;
    }

    [SerializeField] TextMeshProUGUI timerText;
    private float timeLeft = 60.0f;
    private bool timerRunning = false;

    public void SetTimer(bool value)
    {
        timerRunning = value;
    }

    void Update()
    {
        if (timerRunning)
        {
            timeLeft -= Time.deltaTime;
            timerText.text = timeLeft.ToString("0");
            if (timeLeft < 0)
            {
                OnGameLostEvent?.Invoke();
            }
        }
    }

    private void RequestAPIQuestions(System.Action callback)
    {
        string url = "https://opentdb.com/api.php?amount=" + apiQuestionCount + "&category=9&difficulty=easy&type=multiple";
        StartCoroutine(GetRequest(url, callback)); 
    }

    private IEnumerator GetRequest(string url, System.Action callback)
    {
        UnityWebRequest request = new UnityWebRequest(url, "GET");
        request.downloadHandler = new DownloadHandlerBuffer();

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("OpenTDB request successful");
            JSONNode response = JSON.Parse(request.downloadHandler.text);
            JSONArray results = response["results"].AsArray;
            foreach(JSONNode result in results)
            {
                string questionText = result["question"];
                string correctAnswer = result["correct_answer"];
                JSONArray incorrectAnswers = result["incorrect_answers"].AsArray;
                string[] options = new string[4];
                System.Random random = new System.Random();
                int correctIndex = random.Next(0, 4);
                options[correctIndex] = correctAnswer;
                int i = 0;
                foreach (JSONNode incorrectAnswer in incorrectAnswers)
                {
                    if (i == correctIndex)
                    {
                        i++;
                    }
                    options[i] = incorrectAnswer;
                    i++;
                }
                char correctOption = (char)(correctIndex + 65);
                questions.Add(new Question(questionText, options, correctOption));
            }
            callback();

        } else {
            Debug.Log("OpenTDB request failed");
        }
    }

    private void TranslateAPIQuestions()
    {
        string url = "https://api-free.deepl.com/v2/translate";
        for (int i = questions.Count - apiQuestionCount; i < questions.Count; i++)
        {
            Question question = questions[i];

            WWWForm form = new WWWForm();
            form.AddField("auth_key", deepLApiKey);
            form.AddField("text", question.questionText);
            form.AddField("text", question.options[0]);
            form.AddField("text", question.options[1]);
            form.AddField("text", question.options[2]);
            form.AddField("text", question.options[3]);
            form.AddField("target_lang", "es");
            form.AddField("source_lang", "en");

            StartCoroutine(PostRequest(url, form, i));
        }
    }

    private IEnumerator PostRequest(string url, WWWForm form, int index)
    {
        UnityWebRequest request = UnityWebRequest.Post(url, form);
        request.downloadHandler = new DownloadHandlerBuffer();

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("DeepL request successful");
            JSONNode response = JSON.Parse(request.downloadHandler.text);
            JSONArray translations = response["translations"].AsArray;
            questions[index].questionText = translations[0]["text"];
            questions[index].options[0] = translations[1]["text"];
            questions[index].options[1] = translations[2]["text"];
            questions[index].options[2] = translations[3]["text"];
            questions[index].options[3] = translations[4]["text"];
        } else {
            Debug.Log("DeepL request failed");
        }
    }

    private IEnumerator DelayedDebug(int seconds)
    {
        yield return new WaitForSeconds(seconds);
        Debug.Log(questions[questions.Count - 1].questionText);
        Debug.Log(questions[questions.Count - 1].options[0]);
        Debug.Log(questions[questions.Count - 1].options[1]);
        Debug.Log(questions[questions.Count - 1].options[2]);
        Debug.Log(questions[questions.Count - 1].options[3]);
    }
}
