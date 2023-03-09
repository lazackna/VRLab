using UnityEngine;
using Valve.VR.InteractionSystem;
using System.Collections.Generic;
using UnityEngine.Events;


/*
 * Based off of the ComplexThrowable from SteamVR 
 */
public class InteractableModel : MonoBehaviour
{
	public float attachForce = 800.0f;
	public float attachForceDamper = 25.0f;
	public UnityEvent onPickUp;
	public UnityEvent onDetachFromHand;
	protected Vector3 attachPosition;
	protected Quaternion attachRotation;
	protected bool attached = false;
	protected float attachTime;

	[EnumFlags]
	public Hand.AttachmentFlags attachmentFlags = 0;

	private List<Hand> holdingHands = new List<Hand>();
	private List<Rigidbody> holdingBodies = new List<Rigidbody>();
	private List<Vector3> holdingPoints = new List<Vector3>();

	private List<Rigidbody> rigidBodies = new List<Rigidbody>();


	void Awake()
	{
		GetComponentsInChildren<Rigidbody>(rigidBodies);
	}

	void Update()
	{
		for (int i = 0; i < holdingHands.Count; i++)
		{
			if (holdingHands[i].IsGrabEnding(this.gameObject))
			{
				PhysicsDetach(holdingHands[i]);
			}

            if (this.gameObject.layer == 8 && holdingHands[i].currentAttachedObject == this.gameObject)//layer 8 is throwable
            {
				this.gameObject.transform.rotation = holdingHands[i].transform.rotation;
            }
		}

		//if -> layer... && hand.. 
	}

	private void OnHandHoverBegin(Hand hand)
	{
		//Debug.Log("hover over object");
		if (holdingHands.IndexOf(hand) == -1)
		{
			if (hand.isActive)
			{
				hand.TriggerHapticPulse(800);
			}
		}
	}

	private void OnHandHoverEnd(Hand hand)
	{
		//Debug.Log("hover exit");
		if (holdingHands.IndexOf(hand) == -1)
		{
			if (hand.isActive)
			{
				hand.TriggerHapticPulse(500);
			}
		}
	}


	private void HandHoverUpdate(Hand hand)
	{
		GrabTypes startingGrabType = hand.GetGrabStarting();

		if (startingGrabType != GrabTypes.None)
		{
			PhysicsAttach(hand, startingGrabType);
		}
	}


	//-------------------------------------------------
	private void PhysicsAttach(Hand hand, GrabTypes startingGrabType)
	{
		attached = true;
		PhysicsDetach(hand);

		onPickUp.Invoke();

		Rigidbody holdingBody = null;
		Vector3 holdingPoint = Vector3.zero;

		attachPosition = transform.position;
		attachRotation = transform.rotation;
		attachTime = Time.time; 

		// The hand should grab onto the nearest rigid body
		float closestDistance = float.MaxValue;
		for (int i = 0; i < rigidBodies.Count; i++)
		{
			float distance = Vector3.Distance(rigidBodies[i].worldCenterOfMass, hand.transform.position);
			if (distance < closestDistance)
			{
				holdingBody = rigidBodies[i];
				closestDistance = distance;
			}
		}

		// Couldn't grab onto a body
		if (holdingBody == null)
			return;

		// Don't let the hand interact with other things while it's holding us
		hand.HoverLock(null);

		// Affix this point
		Vector3 offset = hand.transform.position - holdingBody.worldCenterOfMass;
		offset = Mathf.Min(offset.magnitude, 1.0f) * offset.normalized;
		holdingPoint = holdingBody.transform.InverseTransformPoint(holdingBody.worldCenterOfMass + offset);

		hand.AttachObject(this.gameObject, startingGrabType, attachmentFlags);

		// Update holding list
		holdingHands.Add(hand);
		holdingBodies.Add(holdingBody);
		holdingPoints.Add(holdingPoint);

        foreach (Collider c in GetComponents<Collider>())
        {
			Physics.IgnoreCollision(GameObject.Find("Player 1(Clone)").transform.Find("SteamVRObjects").transform.Find("BodyCollider").GetComponent<CapsuleCollider>(), c, true);
        }

	}
	private bool PhysicsDetach(Hand hand)
	{
		attached = false;
		int i = holdingHands.IndexOf(hand);

		onDetachFromHand.Invoke();

		if (i != -1)
		{
			// Detach this object from the hand
			holdingHands[i].DetachObject(this.gameObject, false);

			// Allow the hand to do other things
			holdingHands[i].HoverUnlock(null);

			Util.FastRemove(holdingHands, i);
			Util.FastRemove(holdingBodies, i);
			Util.FastRemove(holdingPoints, i);

			bool stillHolding = false;
            for (int j = 0; j < holdingHands.Count; j++)
            {
                if (holdingHands[j].currentAttachedObject != null && holdingHands[j].currentAttachedObject == this.gameObject)
                {
					stillHolding = true;
                }
            }

            if (!stillHolding)
            {
				foreach (Collider c in GetComponents<Collider>())
				{
					Physics.IgnoreCollision(GameObject.Find("Player 1(Clone)").transform.Find("SteamVRObjects").transform.Find("BodyCollider").GetComponent<CapsuleCollider>(), c, false);
				}
			}

			return true;
		}

		return false;
	}


	//-------------------------------------------------
	void FixedUpdate()
	{
		
		for (int i = 0; i < holdingHands.Count; i++)
		{
			Vector3 targetPoint = holdingBodies[i].transform.TransformPoint(holdingPoints[i]);
			Vector3 vdisplacement = holdingHands[i].transform.position - targetPoint;

			holdingBodies[i].AddForceAtPosition(attachForce * vdisplacement, targetPoint, ForceMode.Acceleration);
			holdingBodies[i].AddForceAtPosition(-attachForceDamper * holdingBodies[i].GetPointVelocity(targetPoint), targetPoint, ForceMode.Acceleration);
		}
	}

	public void SetRotation()
    {
		transform.rotation = new Quaternion { x = 0, y = 90, z = 0 };
	}
}