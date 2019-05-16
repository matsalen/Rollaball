﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    [SerializeField] private float tempoRestante;

    private float pontos;
    private bool gameOver;
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
            
        }
            else
            {
                if (Input.GetButtonDown("Submit"))
                {
                    Restart();
                }
            }
    }

    public void FimDeJogo(string msg, Color cor)
    {
        EnviaNovoRecord(pontos);
        textoGameOver.text = msg;
        textoGameOver.color = cor;
        textoRestart.text = "Pressione 'ENTER' para reiniciar";
        gameOver = true;
        pc.enabled = false;
        rb.freezeRotation = true;
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
        SceneManager.LoadScene("SampleScene");
    }
}
