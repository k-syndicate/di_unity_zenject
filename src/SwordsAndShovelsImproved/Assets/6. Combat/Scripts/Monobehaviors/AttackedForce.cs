using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackedForce : MonoBehaviour, IAttackable
{
    public float ForceToAdd;
    private Rigidbody rBody;

	public void OnAttack(GameObject attacker, Attack attack)
    {
        var forceDirection = transform.position - attacker.transform.position;
        forceDirection.y += 0.5f;
        forceDirection.Normalize();

        rBody.AddForce(forceDirection * ForceToAdd);
    }

    private void Awake()
	{
        rBody = GetComponent<Rigidbody>();
	}
}
