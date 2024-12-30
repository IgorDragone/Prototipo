using System.Collections;
using UnityEngine;
using TMPro;
using System.Linq;
using System;
using System.Collections.Generic;

public class GameStarter : MonoBehaviour
{
    public GameObject player;
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
        };
        optionTexts = new TextMeshProUGUI[] {option1Text, option2Text, option3Text, option4Text};

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
        NextQuestion();
    }

    void NextQuestion() 
    {
        correctAnswer.Stop();
        currentQuestion = questions[currentQuestionIndex];
        questionText.text = currentQuestion.questionText;
        option1Text.text = currentQuestion.options[0];
        option2Text.text = currentQuestion.options[1];
        option3Text.text = currentQuestion.options[2];
        option4Text.text = currentQuestion.options[3];
        currentCorrectOption = currentQuestion.correctAnswer;
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
        //yield return new WaitForSeconds(2);
    }
}
