using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon.asset", menuName = "Attack/Weapon")]
public class Weapon : AttackDefinition 
{
    public Rigidbody weaponPreb;

    public void ExecuteAttack(GameObject attacker, GameObject defender)
    {
        if (defender == null)
            return;

        // Check if defender is in range of the attacker
        if (Vector3.Distance(attacker.transform.position, defender.transform.position) > Range)
            return;

        // Check if defender is in front of the player
        if (!attacker.transform.IsFacingTarget(defender.transform))
            return;

        // at this point the attack will connect
        var attackerStats = attacker.GetComponent<CharacterStats>();
        var defenderStats = defender.GetComponent<CharacterStats>();

        var attack = CreateAttack(attackerStats, defenderStats);

        var attackables = defender.GetComponentsInChildren(typeof(IAttackable));
        foreach(IAttackable a in attackables)
        {
            a.OnAttack(attacker, attack);    
        }
    }
}
