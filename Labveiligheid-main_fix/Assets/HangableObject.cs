using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HangableObject : MonoBehaviour
{
    public Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Hangable")
        {
            rb.isKinematic = true;
        }
    }

    private void onTriggerExit(Collider other)
    {
        if(other.tag == "Hangable")
        {
            rb.isKinematic = false;
        }
    }
}
