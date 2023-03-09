using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rerotate : MonoBehaviour
{

    private Quaternion oQuaternion;     //original rotation quaternion
    private bool isRotating;            //is rotating back to oQuaternion rotation
    public float rotateSpeed = 1.0f;
    private float timeCount = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        this.oQuaternion = transform.rotation;
    }

    void Update() {

        if (this.isRotating) {
            this.timeCount += Time.deltaTime;
            float partRotated = this.timeCount * this.rotateSpeed;
            if (partRotated >= 1) {
                this.isRotating = false;
                this.timeCount = 0.0f;
            } else {
                transform.rotation = Quaternion.Slerp(transform.rotation, oQuaternion, this.timeCount * this.rotateSpeed);
            }
        }
    }

    public void rotateBack() {
        if (!this.isRotating) this.isRotating = true;
    }
}
