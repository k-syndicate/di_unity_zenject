using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterStats))]
public class AttackedTakeDamage : MonoBehaviour, IAttackable
{
    private CharacterStats stats;

	private void Awake()
	{
        stats = GetComponent<CharacterStats>();
	}

	public void OnAttack(GameObject attacker, Attack attack)
    {
        stats.TakeDamage(attack.Damage);

        if(stats.GetHealth() <= 0)
        {
            // Destroy Object
            var destructibles = GetComponents(typeof(IDestructible));
            foreach (IDestructible d in destructibles)
                d.OnDestruction(attacker);
        }
    }
}
