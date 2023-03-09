using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetFire : MonoBehaviour
{
    public GameObject firePrefab;
    public Collider colliderForCollision;
    public ParticleSystem psInObject;

    //TODO:
    //  - check if there are other fire components on the other gameobject
    //  - check which fire object is the closest to the contact point
    //  - check the distance between the closest point 
    //  - if the distance is above a certain threshold, then if can spawn a new fire!

    private void OnCollisionEnter(Collision collision)
    {
        Collider collider = collision.GetContact(0).thisCollider;
        if (collider == colliderForCollision)
        {
            if (psInObject.isPlaying)
            {

                //Debug.Log("Collision with: " + collision.gameObject.name + ", position: " + collision.contacts[0].point.x + ", " + collision.contacts[0].point.y + ", " + collision.contacts[0].point.z);
                GameObject closestFire = null;
                //Debug.Log("childCount: " + collision.gameObject.transform.childCount);
                for (int i = 0; i < collision.gameObject.transform.childCount; i++)
                {
                    GameObject child = collision.gameObject.transform.GetChild(i).gameObject;
                    //if (child.GetComponent<Fire>() != null)
                    if(child.layer == LayerMask.NameToLayer("Fire"))
                    {
                        if (closestFire == null)
                        {
                            closestFire = child;
                        }
                        if (closestFire != null || (Vector3.Distance(child.transform.position, transform.position) < Vector3.Distance(closestFire.transform.position, transform.position)))
                        {
                            closestFire = child;
                        }
                    }
                }
                if (closestFire == null)
                {
                    //Debug.Log("SPAWNING FIREEEEE");
                    GameObject fire = Instantiate(firePrefab);
                    fire.transform.position = new Vector3(collision.contacts[0].point.x, collision.contacts[0].point.y, collision.contacts[0].point.z);
                    fire.transform.SetParent(collision.gameObject.transform, true);
                    //fire.transform.localScale = new Vector3(firePrefab.transform.localScale.x / collision.gameObject.transform.localScale.x,
                    //                                        firePrefab.transform.localScale.y / collision.gameObject.transform.localScale.y,
                    //                                        firePrefab.transform.localScale.z / collision.gameObject.transform.localScale.z
                    //                                        );
                }
                else if (Vector3.Distance(closestFire.transform.position, transform.position) > 0.5f)
                {
                    //Debug.Log("SPAWNING FIREEEEE");
                    GameObject fire = Instantiate(firePrefab);
                    fire.transform.position = new Vector3(collision.contacts[0].point.x, collision.contacts[0].point.y, collision.contacts[0].point.z);
                    fire.transform.SetParent(collision.gameObject.transform, true);
                    //fire.transform.localScale = new Vector3(firePrefab.transform.localScale.x / collision.gameObject.transform.localScale.x,
                    //                                        firePrefab.transform.localScale.y / collision.gameObject.transform.localScale.y,
                    //                                        firePrefab.transform.localScale.z / collision.gameObject.transform.localScale.z
                    //                                        );
                }


                //Quaternion q = new Quaternion
                //{
                //    x = -90.0f,
                //    y = 0.0f,
                //    z = 0.0f
                //};
                //fire.transform.rotation = q;



            }
        }
    }
}
