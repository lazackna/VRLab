using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterTrigger : MonoBehaviour
{
    public ParticleSystem water;
    public ParticleSystem waterDust;
    private bool waterOn;
    public GameObject waterFlow;
    public AudioSource showerAudio;
    // Start is called before the first frame update
    void Start()
    {
        waterOn = false;
        waterFlow.SetActive(false);
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "WaterTrigger")
        {
            waterOn = true;
            waterFlow.SetActive(true);
            water.Play();
            waterDust.Play();
            if (!showerAudio.isPlaying) showerAudio.Play();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.name == "Pivot part")
        {
            StartCoroutine(StopShower(5.0f));
        }
        else
        {
            StartCoroutine(StopWater());
        }
    }

    IEnumerator StopShower(float time)
    {
        yield return new WaitForSeconds(time);
       
        if (waterOn)
        {
            waterOn = false;
           
            water.Stop();
            waterDust.Stop();
            showerAudio.Stop();
        }
    }

    IEnumerator StopWater()
    {
        yield return new WaitForSeconds(0f);

        if (waterOn)
        {
            waterOn = false;
            waterFlow.SetActive(false);
            water.Stop();
            waterDust.Stop();
            showerAudio.Stop();
        }
    }
}
