using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtinguishFire : MonoBehaviour
{
    [SerializeField] public LayerMask targetLayer;
    [SerializeField] public ParticleSystem foamPS;
    public float raycastRadius;
    public float raycastDistance;
    public float extinguishValue;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (foamPS.isPlaying)
        {
            
            if (Physics.SphereCast(transform.position, raycastRadius, -transform.right, out RaycastHit hit, raycastDistance, targetLayer))
            {
                Debug.Log("HITTING THE SPHERECAST!");
                if (hit.transform.transform.gameObject.GetComponentInChildren<Fire>())
                    hit.transform.transform.gameObject.GetComponentInChildren<Fire>().TryExtinguish(extinguishValue);

            }
        }
        
    }


}
