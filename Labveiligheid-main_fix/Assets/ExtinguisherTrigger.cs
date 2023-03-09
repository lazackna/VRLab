using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtinguisherTrigger : MonoBehaviour
{
    public ParticleSystem foam;
    public AudioSource audioSource;
    private bool foamOn;

    // Start is called before the first frame update
    void Start()
    {
        foamOn = false;

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "ExTrigger")
        {
            foamOn = true;
            foam.Play();
            if (!this.audioSource.isPlaying) this.audioSource.Play();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        StartCoroutine(StopFoam());
    }

    IEnumerator StopFoam()
    {
        yield return new WaitForSeconds(0f);

        if (foamOn)
        {
            foamOn = false;
            foam.Stop();
            this.audioSource.Stop();
        }
    }
}
