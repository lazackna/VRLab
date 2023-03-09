using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireScript : MonoBehaviour
{
    private bool isLit = false;
    public ParticleSystem fireSpread;
    public ParticleSystem fire;
    public ParticleSystem fireSmoke;
    public Material burntMat;
    public float lifeTime = 8.0f;

    // Start is called before the first frame update
    void Start()
    {
        this.fire.GetComponent<ParticleSystem>().Stop();
        this.fireSpread.GetComponent<ParticleSystem>().Stop();
        this.fireSmoke.GetComponent<ParticleSystem>().Stop();

        
    }

    // Update is called once per frame
    void Update()
    {
        if (this.isLit && (gameObject.GetComponent<Rigidbody>().velocity.x > 8.0f || gameObject.GetComponent<Rigidbody>().velocity.y > 8.0f || gameObject.GetComponent<Rigidbody>().velocity.z > 8.0f))
        {
            Debug.Log("velocity reached, unlit fire!");
            UnLitTip();
        }
    }

    public bool isOn()
    {
        return this.isLit;
    }
    public void turnOn()
    {
        if (!this.isLit)
        {
            this.isLit = true;
            this.fire.Play();
            //this.fireSpread.Play();
            Invoke("setSpreader", 1.0f);
            this.fireSmoke.Play();

            Invoke("UnLitTip", lifeTime);
        }
    }

    public void turnOff()
    {
        if (this.isLit)
        {
            this.isLit = false;
            this.fire.Stop();
            this.fireSpread.Stop();
            this.fireSmoke.Stop();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        //Debug.Log("OnTrigger: " + other.gameObject.name);
        if (other.gameObject.name == "LuciferTrigger" && this.isLit)
        {
            //Debug.Log("Invoking fireTrigger");
            other.GetComponent<FireTrigger>().fireTrigger.Invoke();
        }


        if (other.gameObject.name == "WaterFlow" && isLit)
        {
            //Debug.Log("Collision with: " + other.gameObject.name);
            UnLitTip();
        }
    }

    private void UnLitTip()
    {
        GetComponent<MeshRenderer>().material = burntMat;
        turnOff();

    }

    private void setSpreader()
    {
        if (this.isLit)
        {
            this.fireSpread.Play();
        }
    }
}