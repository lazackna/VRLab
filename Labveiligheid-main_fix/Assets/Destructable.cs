using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructable : MonoBehaviour
{
    public GameObject brokenObject;
    public float magnitudeCol;
    private bool broken = false;
    public AudioClip breakingGlass;
    
    // Start is called before the first frame update
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.relativeVelocity.magnitude > magnitudeCol && !broken)
        {
            broken = true;
            Instantiate(brokenObject, transform.position, transform.rotation);
            
            if(breakingGlass != null)
            {
                GetComponent<AudioSource>().clip = breakingGlass;
                GetComponent<AudioSource>().Play();
            }

            Destroy(gameObject);
        }
    }
}
