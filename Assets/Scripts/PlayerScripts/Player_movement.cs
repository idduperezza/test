using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_movement : MonoBehaviour
{
    public float playerSpeed = 30f, minSpeed = 30f; // velocità base del personaggio
    private float maxSpeed = 110f; // velocità massima
    private float rotazione = 0.0f; // accelerazione di rotazione
    public float boostCounter; // contatore boost
    public RawImage dynamicBoostBar;
    private Vector3 oldPos;
    private int maxFov = 100; // fov massimo
    private int minFov = 90; // fov minimo
    public Transform playerBody; // corpo del personaggio
    public CharacterController playerController; // controller del personaggio

    private void Awake(){
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        oldPos = dynamicBoostBar.transform.localPosition;
        boostCounter = 100;
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer(); // ad ogni frame muovo il personaggio
        RotatePlayer(); // ad ogni frame ruoto il personaggio
        SpeedBoost(); // ad ogni frame controllo se sta venendo utilizzato il boost
    }

    void MovePlayer(){
        float x = Input.GetAxis("Horizontal"); // acquisisco l'input orizzontale (A,D)
        float z = Input.GetAxis("Vertical"); // acquisisco l'input verticale (W,S)
        float y = 0.0f; // valore che andrà ad influenza il movimento in altezza del personaggio

        if(Input.GetKey(KeyCode.Space)){ // se il player preme spazio
            y = 1f; // velocità con la qual il player si sposta verso l'alto
        } else if (Input.GetKey(KeyCode.LeftControl)){ // se il player preme ctrl
            y = -1f; // velocità con la quale il player si sposta verso il basso
        }

        Vector3 move = (transform.right * x) + (transform.forward * z) + (transform.up * y); // imposto il vettore con il quale muovo il personaggio

        playerController.Move(move * playerSpeed * Time.deltaTime); // muovo il personaggio
    }

    void RotatePlayer(){
        // controllo l'input del player
        if(Input.GetKey(KeyCode.Q)) rotazione = 60f; // se preme Q imposto la rotazione in senso ANTIORARIO
        else if(Input.GetKey(KeyCode.E)) rotazione = -60f; // se premre E imposto la rotazione in senso ORARIO
        else rotazione = 0f; // altrimenti, se il player non preme alcun tasto imposto la rotazione a 0

        playerBody.Rotate(Vector3.forward  * rotazione * Time.deltaTime); // effettuo la rotazione
    }

    void SpeedBoost(){

        UpdateBoostBar(boostCounter);

        // controllo se il player sta premendo shift mentre si muove
        if(!transform.GetComponentInChildren<MouseLook>().scoped){
            if(Input.GetKey(KeyCode.LeftShift) &&
            (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) &&
            boostCounter > 0){
                // accelero gradualmente fino al limite massimo di velocità
                if(playerSpeed < maxSpeed){
                    playerSpeed += 0.5f;
                }

                // incremento gradualmente il campo visivo della camera in modo da dare un effetto di velocità
                if(Camera.main.fieldOfView < maxFov){
                    Camera.main.fieldOfView += 0.5f;
                }
                
                // sposto la barra verso sinistra
                
                dynamicBoostBar.transform.localPosition = new Vector3(
                    dynamicBoostBar.transform.localPosition.x,
                    dynamicBoostBar.transform.localPosition.y - 15.5f * Time.deltaTime,
                    dynamicBoostBar.transform.localPosition.z
                );
                
                
                boostCounter -= 20f * Time.deltaTime; // decremento il boost
            } else { 
                // se il player non preme shift decremento gradualmente la velocità e il campo visivo
                if(playerSpeed > minSpeed){
                    playerSpeed -= 1f;
                }

                if(Camera.main.fieldOfView > minFov){
                    Camera.main.fieldOfView -= 1f;
                }
            }
        }
    }

    void UpdateBoostBar(float boost){
        dynamicBoostBar.transform.localScale = new Vector3(dynamicBoostBar.transform.localScale.x, boostCounter, dynamicBoostBar.transform.localScale.z);
    }

    public void AddBoost(){
        if(boostCounter < 100){

            boostCounter += 10;

            UpdateBoostBar(boostCounter);

            if(boostCounter > 100){
                boostCounter = 100;
                dynamicBoostBar.transform.localPosition = oldPos;
            } else {
                dynamicBoostBar.transform.localPosition = new Vector3(
                    dynamicBoostBar.transform.localPosition.x,
                    dynamicBoostBar.transform.localPosition.y + 7.75f,
                    dynamicBoostBar.transform.localPosition.z
                );
            }
        }
    }
}
