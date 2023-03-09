using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    public int repeatedSpread = 0;
    public GameObject firePrefab;
    public GameObject[] particleObjects;

    public float scaleCap = 5.0f;
    private ParticleCollisionEvent[] CollisionEvents;

    //[SerializeField] public bool fixScaling = true;

    //to extinguish
    [SerializeField, Range(0.0f, 1.0f)]private float currentIntensity = 1.0f;
    private float[] startIntensities;


    // Start is called before the first frame update
    void Start()
    {
        CollisionEvents = new ParticleCollisionEvent[8];
        startIntensities = new float[particleObjects.Length];
        for (int i = 0; i < particleObjects.Length; i++)
        {
            startIntensities[i] = particleObjects[i].GetComponent<ParticleSystem>().emission.rateOverTime.constant;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (particleObjects != null)
        {
            for (int i = 0; i < particleObjects.Length; i++)
            {
                
                if (particleObjects[i].transform.localScale.x < scaleCap && particleObjects[i].transform.localScale.y < scaleCap && particleObjects[i].transform.localScale.z < scaleCap)
                {
                    particleObjects[i].transform.localScale = new Vector3(particleObjects[i].transform.localScale.x + 0.01f, particleObjects[i].transform.localScale.y + 0.01f, particleObjects[i].transform.localScale.z + 0.01f);
                }
            }
            
        }

        ChangeIntensity();
    }

    [System.Obsolete]
    private void OnParticleCollision(GameObject other)
    {
        //Debug.Log("In Particle Collision!");

        if (repeatedSpread < 2)
        {
            if (Random.Range(0.0f, 100.0f) < 0.1f)
            {

                Debug.Log("Spreading...");
                //if (other.tag == "flammable")
                //{
                int collCount = GetComponent<ParticleSystem>().GetSafeCollisionEventSize();
                if (collCount > CollisionEvents.Length)
                {
                    CollisionEvents = new ParticleCollisionEvent[collCount];
                }
                int eventCount = GetComponent<ParticleSystem>().GetCollisionEvents(other, CollisionEvents);
                for (int i = 0; i < eventCount; i++)
                {
                    GameObject fireTMP = firePrefab;
                    fireTMP.transform.GetChild(0).gameObject.GetComponent<Fire>().repeatedSpread = this.repeatedSpread + 1;
                    //fireTMP.GetComponent<Fire>().repeatedSpread = this.repeatedSpread + 1;
                    GameObject fire = Instantiate(fireTMP);
                    fire.transform.SetParent(other.transform, false);
                    fire.transform.position = new Vector3(CollisionEvents[i].intersection.x, CollisionEvents[i].intersection.y, CollisionEvents[i].intersection.z);
                    //fire.transform.localScale = new Vector3(firePrefab.transform.localScale.x / CollisionEvents[i].colliderComponent.gameObject.transform.localScale.x,
                    //                                        firePrefab.transform.localScale.y / CollisionEvents[i].colliderComponent.gameObject.transform.localScale.y,
                    //                                        firePrefab.transform.localScale.z / CollisionEvents[i].colliderComponent.gameObject.transform.localScale.z
                    //                                        );

                    //Quaternion q = new Quaternion
                    //{
                    //    x = -90.0f,
                    //    y = 0.0f,
                    //    z = 0.0f
                    //};
                    //fire.transform.rotation = q;
                }
                //}
            }

        }
    }



    public void TryExtinguish(float amount)
    {
        currentIntensity -= amount;
        ChangeIntensity();

        if (currentIntensity <= 0)
        {
            Destroy(gameObject.transform.parent.gameObject);
        }
    }


    private void ChangeIntensity()
    {
        for (int i = 0; i < particleObjects.Length; i++)
        {
            var emmision = particleObjects[i].GetComponent<ParticleSystem>().emission;
            emmision.rateOverTime = currentIntensity * startIntensities[i];
        }
    }



}
