using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerAsteroide : MonoBehaviour
{
    Vector3 randomRotation; // rotazione asteroide sul posto
    private Vector3 lastPosition = Vector3.zero; // mi serve per calcolare la velocità con la quale viene effettuata la collisione con un asteroide
    private float speed; // velocità con la quale l'asteroide si deve spostare
    public CharacterController controller;
    public LevelManager manager;
    private Rigidbody rb;
    
    // Start is called before the first frame update
    void Start()
    {
        // acquisco il rigibody
        rb = GetComponent<Rigidbody>();
        speed = Random.Range(500f, 1500f);

        // randomizzo una rotazione
        randomRotation = new Vector3(Random.Range(0, 90), Random.Range(0, 90), Random.Range(0, 90)); // creo il vettore di rotazione in modo randomico
        rb.AddForce(transform.forward * speed);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        RuotaAsteroide(); // ruoto l'asteroide
        DestroyAsteroide(); // controllo se devo distruggere l'asteroide
    }

    void RuotaAsteroide(){
        transform.Rotate(randomRotation * Time.deltaTime); // ruoto l'asteroide
    }

    private void OnCollisionEnter(Collision collision){
        // ad avvenuta collisione con l'asteroide:
        speed = (collision.transform.position - lastPosition).magnitude; // acquisico la velocità dell'oggetto che ha colliso

        rb.AddForce(collision.transform.forward * speed * 1.5f); // applico la forza sull'asteroide e lo faccio
    }

    private void DestroyAsteroide(){
        if(transform.childCount == 0){
            Destroy(gameObject);
        }
    }
}
