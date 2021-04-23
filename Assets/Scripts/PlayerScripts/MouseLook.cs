using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float mouseSens = 50;
    private float prev_sens = 50;
    private float prev_fov;
    public Transform playerBody;
    public GameObject rifle;
    public bool scoped = false;
    public GameObject reticolo;

    // Start is called before the first frame update
    void Start()
    {
        reticolo.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked; // nascondo il cursore
        prev_fov = Camera.main.fieldOfView; // salvo il fov precedente
        prev_sens = mouseSens; // salvo la sens di base
    }

    // Update is called once per frame
    void Update()
    {
        LookAround();
        Zoom();
    }

    void LookAround(){
        float mouseX = Input.GetAxis("Mouse X") * mouseSens * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSens * Time.deltaTime;
        
        mouseY = -mouseY;
    
        playerBody.Rotate(Vector3.up * mouseX);
        playerBody.Rotate(Vector3.right * mouseY);
    }

    void Zoom(){

        if(Input.GetButtonDown("Fire2") && !Input.GetKey(KeyCode.LeftShift)){
            scoped = !scoped;
            rifle.GetComponent<Animator>().SetBool("Scoped", scoped);

            if(scoped){
                ZoomIn();
            } else {
                ZoomOut();
            }
        }
    }

    IEnumerator OnScoped(){
        yield return new WaitForSeconds(0.15f);
        Camera.main.fieldOfView = 15f;
        reticolo.SetActive(true);
    }

    void ZoomIn(){
        mouseSens = 10f;
        StartCoroutine(OnScoped());
    }

    void ZoomOut(){
        mouseSens = prev_sens;
        Camera.main.fieldOfView = prev_fov;
        reticolo.SetActive(false);
    }
}
