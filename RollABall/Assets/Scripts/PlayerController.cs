﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private float velocidade;
    private float movimentoH;
    private float movimentoV;
    private Vector3 movimento;
    private Rigidbody rb;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        movimentoH = Input.GetAxis("Horizontal");
        movimentoV = Input.GetAxis("Vertical");

        movimento = new Vector3(movimentoH, 0, movimentoV);
        rb.AddForce(movimento * velocidade);


    }

    /*
     * private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PowerUp"))
        {
            other.gameObject.SetActive(false);
            GameController.instancia.AtualizaTempo(1f);
            GameController.instancia.AtualizaPontos(1f);
            
        }
    }
    */
}
