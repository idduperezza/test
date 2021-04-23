using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakPointLifecycle : MonoBehaviour
{
    public float durata = 0.7f; // tempo che rimane in vita il target (weak point)
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("DespawnCoroutine");
    }

    IEnumerator DespawnCoroutine()
    {
        yield return new WaitForSeconds(durata);
        Destroy(gameObject);
    }
}
