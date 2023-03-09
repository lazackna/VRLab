using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectObjectInArea : MonoBehaviour
{
    public Transform snapPoint;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter(Collider other)
    {
        //a little of the hacky side way to find specific object with speciic names, could be more efficient and cleaner. But it works for now
        if (other.transform.tag.Equals("Snappable"))
        {
            Transform groundPoint = other.transform.Find("SnapPoint");
            if (groundPoint == null)
            {
                Debug.Log("GroundPoint not found!");
                return;
            }

            other.gameObject.GetComponent<Rerotate>()?.rotateBack();

            other.transform.position = new Vector3(snapPoint.position.x, snapPoint.position.y + (Vector3.Distance(groundPoint.position, other.transform.position)), snapPoint.position.z);
            //other.transform.rotation = new Quaternion {x = 0, y= 0, z=0 };
            Rigidbody rbParent = other.GetComponent<Rigidbody>();
            if (rbParent != null)
                rbParent.isKinematic = true;

            BurnerValve bv = other.transform.Find("BurnerValve").GetComponent<BurnerValve>();
            if (bv != null)
                bv.setGasFlow(true);
        }
    }
}
