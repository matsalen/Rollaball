using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class GameController : MonoBehaviour
{
    public static GameController instancia;

    [SerializeField] private Color corTextoDerrota;
    [SerializeField] private Color corTextoVitoria;
    [SerializeField] private PlayerController pc;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Text textoPontos;
    [SerializeField] private Text textoTempo;
    [SerializeField] private Text textoGameOver;
    [SerializeField] private Text textoRestart;
    [SerializeField] private Text textoRecord;
    [SerializeField] private GameObject painelOpcoes;
    [SerializeField] private float tempoRestante;

    private float pontos;
    private bool gameOver;
    private bool pause;
    private int powerUps;
    private float record;

    private void Awake()
    {
        if (instancia == null)
        {
            instancia = this;
        }
        else
        {
            if (instancia != this)
            {
                Destroy(this);
            }
        }
        pontos = 0;
        gameOver = false; 
    }

    // Start is called before the first frame update
    void Start()
    {
        CarregaRecord();
        powerUps = GameObject.FindGameObjectsWithTag("PowerUp").Length;
        AtualizaPontos(0f);
        AtualizaTempo();
        textoRestart.text = "";
        textoGameOver.text = "";
        pause = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameOver)
        {
            tempoRestante -= Time.deltaTime;
            AtualizaTempo( );

            if (tempoRestante < 0f)
            {                
                FimDeJogo("TIME OVER!", corTextoDerrota);
            }

            if (Input.GetButtonDown("Pause"))
            {
                GamePause();
            }
            
        }
        else
        {

            if (Input.GetButtonDown("Submit"))
                {
                    Restart();
                }
        }


    }

    public void GamePause()
    {
        pause = !pause;
        if (pause)
        {
            // PAUSA O JOGO
            Time.timeScale = 0;
            painelOpcoes.SetActive(pause);
            foreach (Transform filho in painelOpcoes.transform)
            {
                if (filho.name == "Continuar")
                    filho.gameObject.GetComponent<Button>().Select();
            }
        }
        else
        {
            // RETOMA O JOGO
            Time.timeScale = 1;
            painelOpcoes.SetActive(pause);
        }
    }

    public void FimDeJogo(string msg, Color cor)
    {
        EnviaNovoRecord(pontos);
        textoGameOver.text = msg;
        textoGameOver.color = cor;
        //textoRestart.text = "Pressione 'ENTER' para reiniciar";
        gameOver = true;
        pc.enabled = false;
        rb.freezeRotation = true;

        painelOpcoes.SetActive(true);
        foreach(Transform filho in painelOpcoes.transform)
        {
            if (filho.name == "Continuar")
                filho.gameObject.GetComponent<Button>().enabled = false;
            if (filho.name == "Reiniciar")
                filho.gameObject.GetComponent<Button>().Select();
        }
    }

    private void CarregaRecord()
    {
        record = 0f;
        if (PlayerPrefs.HasKey("Score"))
        {
            record = PlayerPrefs.GetFloat("Score");
        }
        textoRecord.text = "Recorde: " + record;
    }

    private void EnviaNovoRecord(float NovoRecord)
    {
        if (NovoRecord > record)
        {
            PlayerPrefs.SetFloat("Score", NovoRecord);
            CarregaRecord();
        }
    }

    public void AtualizaPontos(float incremento)
    {
        pontos += incremento;
        textoPontos.text = "Pontos: " + pontos;
        if (pontos >= powerUps)
        {
            FimDeJogo("YOU WIN!!!", corTextoVitoria);
        }
    }

    public void AtualizaTempo()
    {
        textoTempo.text = "Tempo restante: " + Mathf.Round(tempoRestante);
    }

    public void AtualizaTempo(float incremento)
    {
        tempoRestante += incremento;
        AtualizaTempo();
    }

    public void Restart()
    {
        if (pause) GamePause();
        SceneManager.LoadScene("SampleScene");
    }

    public void Sair()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
