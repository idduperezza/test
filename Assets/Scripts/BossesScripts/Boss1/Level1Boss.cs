using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1Boss : MonoBehaviour
{
    public GameObject testaBoss; // testa del boss
    public GameObject corpoBoss; // corpo del boss
    public GameObject coreBoss; // nucleo del boss
    public GameObject shiledBoss; // scudo del boss
    public float vitaBoss = 100f; // vita del boss
    public bool isAttacking; // flag che indica se il boss è nella fase di attacco
    public bool closed; // flag per capire se la piramide è sigillata
    private float maxYhead = 30f; // max distanza percorribile verticalmente dalla testa quando sale
    private float minYhead = 19f; // max distanza perorribile verticalmente dalla testa quando scende
    private float maxYbody = -40f; // max distanza perorribile verticalmente dal corpo quando sale
    private float minYbody = -50; // max distanza perorribile verticalmente dal corpo quando scende
    private Vector3 headRotation; // vettore di rotazione della testa 
    private Vector3 bodyRotation; // vettore di rotazione del corpo
    private float rotationSpeed = 0.25f; // velocità rotazione
    public LevelManager manager; // script del LevelManager
    public ObjectSpawner bombSpawner; // Script dello spawner delle bombe
    private float prevHealth; // vita precedente
    public float delayProceduraDifensiva = 0.5f;
    private Vector3 newPosition;

    // Start is called before the first frame update
    void Start()
    {
        // inizializzo
        Init(); 

        // faccio partire la coroutine della difesa
        StartCoroutine("ProceduraDifensiva");
    }

    // Update is called once per frame
    void Update()
    {
        // modifico il comportamento del boss in base a quello del player
        Comportamento();

        // aggiorno la barra della vita del boss
        UpdateHealthBar();

        // controllo se il boss è ancora vivo
        CheckLife();
    }

    void Attacca(){
        // se sta attaccando la piramide non è chiusa
        closed = false;

        // se sta attacando il core non è visibile
        coreBoss.SetActive(false);

        // ruoto la testa 
        testaBoss.transform.Rotate(headRotation * rotationSpeed * Time.deltaTime);

        // controllo se sono arrivato al massimo dell'altezza
        if(testaBoss.transform.localPosition.y < maxYhead){
            // sposto la testa verso l'alto
            testaBoss.transform.localPosition += new Vector3(0, 0.75f, 0) * Time.deltaTime;
        }

        // ruoto il corpo
        corpoBoss.transform.Rotate(bodyRotation * rotationSpeed * Time.deltaTime);

        // controllo se sono arrivato al minimo dell'altezza
        if(corpoBoss.transform.localPosition.y > minYbody){
            // sposto il corpo verso il basso
            corpoBoss.transform.localPosition += new Vector3(0, -0.75f, 0) * Time.deltaTime;
        }
    }

    void Difendi(){
        // controllo se sono arrivato al massimo dell'altezza
        if(testaBoss.transform.localPosition.y > minYhead){
            // ruoto la testa 
            testaBoss.transform.Rotate(bodyRotation * rotationSpeed * Time.deltaTime);
            // sposto la testa verso il basso
            testaBoss.transform.localPosition += new Vector3(0, -0.75f, 0) * Time.deltaTime;
        } else {
            // la difesa inizia solo una volta che la piramide è chiusa
            // imposto la flag che indica che la piramide è chiusa
            closed = true;

            // rendo visibile il core
            coreBoss.SetActive(true);

            // continuo la rotazione della testa ma in senso opposto
            testaBoss.transform.Rotate(headRotation * rotationSpeed * Time.deltaTime);
        }

        // ruoto il corpo
        corpoBoss.transform.Rotate(headRotation * rotationSpeed * Time.deltaTime);

        // controllo se sono arrivato al minimo dell'altezza
        if(corpoBoss.transform.localPosition.y < maxYbody){
            // sposto il corpo verso il basso
            corpoBoss.transform.localPosition += new Vector3(0, 0.75f, 0) * Time.deltaTime;
        }
    }

    void Comportamento(){

        shiledBoss.transform.Rotate(new Vector3(45, 0, 0) * rotationSpeed * Time.deltaTime);

        if(isAttacking && !manager.gameOver){
            Attacca();
            bombSpawner.enabledSpawning = true;
        }
        else if (!manager.gameOver){
            Difendi();
            bombSpawner.enabledSpawning = false;
        }
        else {
            Destroy(gameObject);
        }
    }

    void UpdateHealthBar(){
        if(vitaBoss < prevHealth){
            manager.UpdateHealthBar(vitaBoss);
        }
        prevHealth = vitaBoss;
    }

    void CheckLife(){
        // controllo la vita del boss
        if(vitaBoss <= 0){
            // se il boss non ha più vita lo distruggo
            manager.bossFight = false;
            Destroy(gameObject);
            manager.dynamicHealthBar.transform.localScale = manager.localScaleAux;
        }
    }

    public IEnumerator ProceduraDifensiva(){
        
        while(vitaBoss > 0){
            if(closed){
                newPosition = new Vector3(transform.localPosition.x + Random.Range(-30, 30),
                transform.localPosition.y + Random.Range(-32,28),
                transform.localPosition.z + Random.Range(-30, 30));

                coreBoss.transform.position = newPosition;
            }

            yield return new WaitForSeconds(delayProceduraDifensiva);
        }
    }

    void Init(){
        isAttacking = true;
        closed = false;

        coreBoss.SetActive(false);

        headRotation = new Vector3(0, 90, 0); // vettore di rotazione testa
        bodyRotation = new Vector3(0, -90, 0); // vettore di rotazione corpo

        testaBoss.transform.localPosition = new Vector3(0, minYhead, 0);
        corpoBoss.transform.localPosition = new Vector3(0, maxYbody, 0);

        prevHealth = vitaBoss;
    }
}
