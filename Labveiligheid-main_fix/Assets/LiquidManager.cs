using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiquidManager : MonoBehaviour
{

    private float filledPercentage = 50.0f;
    private float minPercentage = 0.5f;
    public float filledLiters = 1.5f;
    public float maxFillLiters = 2.0f;
    private float lOut = 0.05f;

    public ParticleSystem pourSystem;
    public ParticleSystem steamSystem;
    public float timeToSteam;
    public GameObject fill;
    private float scaleFill;

    private float lastAngle = 0.0f;
    private Quaternion lastQuaternion;
    private float tiltPerc;
    private bool empty = false;
    private bool isPouring = false;
    private bool heatingUp = false;
    private float heatingUpTime = 0.0f;

    private Quaternion offsetAngle;

    // Start is called before the first frame update
    void Start()
    {
        this.calculateFilledPercentage();
        this.pourSystem.GetComponent<ParticleSystem>().Stop();
        this.steamSystem.GetComponent<ParticleSystem>().Stop();
        this.scaleFill = this.fill.transform.localScale.y;
        this.offsetAngle = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (this.heatingUp == false) { this.steamSystem.Stop(); }
        if (this.heatingUp == true) { this.heatingUp = false; }

        Quaternion quaternion = transform.rotation;

        if (quaternion != this.lastQuaternion)
        {
            
            Quaternion x = this.offsetAngle * Quaternion.Inverse(quaternion);
            //Quaternion x = quaternion * Quaternion.Inverse(this.offsetAngle);
            //Vector3 x = quaternion.ToEulerAngles() - this.offsetAngle.ToEulerAngles();
            this.lastQuaternion = quaternion;
            // FIX: offset angle to calculate how much  it should turn, idk
            bool facingdown = Vector3.Dot(x * transform.up, -Vector3.down) > 0;
            if (facingdown && this.empty == false && this.isPouring == false)
            {

                this.isPouring = true;
                this.pourSystem.Play();

            } else if (facingdown == false && this.isPouring == true)
            {

                this.isPouring = false;
                this.pourSystem.Stop();
            }            
        }

        if (this.isPouring == true && this.empty == false)
        {

            //Calculate
            this.filledLiters -= this.lOut * Time.deltaTime;

            //this.raycastPour();
            this.calculateFilledPercentage();
            this.rescaleFill();
        }

        if (this.filledPercentage < this.minPercentage && this.empty == false)
        {
            Debug.Log("LiquidContainer '" + gameObject.name + "' is empty!");
            this.filledLiters = 0.0f;
            this.filledPercentage = 0.0f;
            this.pourSystem.Stop();
            this.isPouring = false;
            this.empty = true;
            this.fill.SetActive(false);
        }
    }

    private void rescaleFill()
    {
        Vector3 tempS = this.fill.transform.localScale;
        tempS.y = this.scaleFill * this.filledPercentage / 100.0f;

        if (tempS.y > 0) this.fill.transform.localScale = tempS;

        Vector3 tempP = this.fill.transform.position;
        tempP -= (this.fill.transform.up * (this.scaleFill * this.lOut * 5 * Time.deltaTime));


        this.fill.transform.position = tempP;
    }

    private void calculateFilledPercentage()
    {
        this.filledPercentage = this.filledLiters / this.maxFillLiters * 100.0f;
    }

    private void raycastPour() {
        RaycastHit hit;
        if (Physics.Raycast(this.pourSystem.transform.position, Vector3.down, out hit, 500))
        {
            hit.transform.gameObject.GetComponent<LiquidManager>()?.fillLiters(this.lOut * Time.deltaTime);
        }
    }

    public void fillLiters(float liters)
    {
        this.filledLiters += liters;
        if (this.filledLiters > this.maxFillLiters) this.filledLiters = this.maxFillLiters;
        this.calculateFilledPercentage();
        if (this.filledPercentage > this.minPercentage)
        {
            this.empty = false;
            this.fill.SetActive(true);
        }
        this.rescaleFill();
    }

    public void startHeatUp() {
        this.heatingUp = true;
        this.heatingUpTime += Time.deltaTime;
        if (this.heatingUpTime >= this.timeToSteam && this.empty == false) {
            if  (!this.steamSystem.isPlaying) this.steamSystem.Play();
            this.filledLiters -= this.lOut * 0.5f * Time.deltaTime;
            this.calculateFilledPercentage();
            this.rescaleFill();

            if (this.filledPercentage < this.minPercentage) {
                Debug.Log("LiquidContainer '" + gameObject.name + "' is empty!");
                this.filledLiters = 0.0f;
                this.filledPercentage = 0.0f;
                this.pourSystem.Stop();
                this.isPouring = false;
                this.empty = true;
                this.fill.SetActive(false);
                this.heatingUp = false;
                return;
            }
            Debug.Log("Heat Up");
        }
    }

    public void stopHeatUp() {
        this.heatingUp = false;
        this.heatingUpTime = 0.0f;
        this.steamSystem.Stop();
    }
}
