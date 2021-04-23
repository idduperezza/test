using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour
{
    // Start is called before the first frame update
    public CharacterController controller; // controller del proiettile
    public float speed; // velocità proiettile/bomba
    public GameObject target; // bersaglio del proiettile/bomba
    public GameObject[] targets; // array contenente i possibili targets
    public float rotationSpeed = 60f; // velocità rotazione
    private Quaternion zRotation = Quaternion.identity; // bho preso da internet 
    
    void Start()
    {
        controller = GetComponent<CharacterController>(); // ottengo il character controller
    }

    // Update is called once per frame
    void Update()
    {
        // acquisico il target
        AcquireTarget();

        // controllo l'esito dell'acquisizione
        if(target != null){
            Rotate(); // ruoto il proiettile/bomba
            Move(); // muovo il poiettile/bomba
        } else {
            // se nonostante l'acquisizione del target non si trova nulla
            // distruggo la bomba
            Destroy(gameObject);
        }

    }

    void AcquireTarget(){
        if(target == null){
            // se il gameobjet è un proiettile
            if(gameObject.tag == "EnemyProjectile")
            {
                target = GameObject.FindGameObjectWithTag("TargetPlayer"); // ottengo il target player
            }

            // se il gameobject è una bomba
            if(gameObject.tag == "Bomb" || gameObject.tag == "Bomb2")
            {
                // controllo che esistano ancora targets
                if((targets = GameObject.FindGameObjectsWithTag("EnemyTarget")).Length > 0){
                    target = targets[Random.Range(0, (targets.Length))]; // scelgo in modo casuale il target su cui far puntare le bombe
                }
            }
        }
    }

    void Rotate()
    {
        // se il gameObject in questione è una bomba:
        if(gameObject.tag == "Bomb" || gameObject.tag == "Bomb2"){
            // controllo che il target non sia stato distrutto
            var lookAtTarget = target.transform.position - transform.position; // calcolo la posizione in cui far guardare la bomba
            var localForward = zRotation * Vector3.forward;
            var lookAt = Quaternion.LookRotation(lookAtTarget, localForward);
            var zDelta = Quaternion.AngleAxis(rotationSpeed * Time.deltaTime, localForward);
            zRotation = zRotation * zDelta; // calcolo la rotazione sull'asse delle z
            transform.rotation = lookAt * zRotation; // effettuo la rotazione
        }
        // se il gameObject in questione è un proiettile rivolto verso il giocatore
        if(gameObject.tag == "EnemyProjectile")
        {
            transform.LookAt(target.transform); // punto il proiettile verso il giocatore
        }
    }

    void Move()
    {
        Vector3 direzione = transform.TransformDirection(Vector3.forward); // aggiorno la direzione in cui andare
        controller.Move(direzione * speed * Time.deltaTime); // muovo il proiettile
    }
}
