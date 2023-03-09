using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        if (
            other.gameObject.tag == "Lucifer"
        && !other.GetComponent<FireScript>().isOn()
        && other.GetComponent<Rigidbody>().velocity.magnitude > 2
        ) {
            other.GetComponent<FireScript>().turnOn();
        }
    }
}
