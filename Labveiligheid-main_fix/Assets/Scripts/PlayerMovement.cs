using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Valve.VR.InteractionSystem
{
    public class PlayerMovement : MonoBehaviour
    {
        // Start is called before the first frame update

        public SteamVR_Action_Vector2 input;
        public float movementSpeed;
        public Transform playersPosition;
        private Rigidbody playersBody;

        private float deadzone = 0.1f;

        void Start()
        {
            playersBody = GetComponent<Rigidbody>();
        }

        // Update is called once per frame
        void Update()
        {
            playersBody = GetComponent<Rigidbody>();

            Vector3 moveDirection = Quaternion.AngleAxis(Angle(input.axis) + playersPosition.localRotation.eulerAngles.y, Vector3.up) * Vector3.forward * input.axis.magnitude;

            if (input.axis.magnitude > deadzone)
            {
                Debug.Log("Moving...");
                playersBody.AddForce(moveDirection.x * movementSpeed-playersBody.velocity.x, 0, moveDirection.z * movementSpeed- playersBody.velocity.z, ForceMode.VelocityChange);
            }
            //playersBody.AddForce(movementSpeed * Time.fixedDeltaTime * direction, ForceMode.VelocityChange);
            
            
            //-----old movement-----
            //Vector3 direction = Player.instance.hmdTransform.TransformDirection(new Vector3(input.axis.x, 0, input.axis.y));
            //playersPosition.position += movementSpeed * Time.deltaTime * Vector3.ProjectOnPlane(direction, Vector3.up);

        }

        public static float Angle(Vector2 p_vector2)
        {
            if (p_vector2.x < 0)
            {
                return 360 - (Mathf.Atan2(p_vector2.x, p_vector2.y) * Mathf.Rad2Deg * -1);
            }
            else
            {
                return Mathf.Atan2(p_vector2.x, p_vector2.y) * Mathf.Rad2Deg;
            }
        }
    }
}

