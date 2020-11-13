using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructedRagdoll : MonoBehaviour, IDestructible
{
    public Ragdoll RagdollObject;
    public float Force;
    public float Lift;

    public void OnDestruction(GameObject destroyer)
    {
        var ragdoll = Instantiate(RagdollObject, transform.position, transform.rotation);

        var vectorFromDestroyer = transform.position - destroyer.transform.position;
        vectorFromDestroyer.Normalize();
        vectorFromDestroyer.y += Lift;

        ragdoll.ApplyForce(vectorFromDestroyer * Force);
    }
}
