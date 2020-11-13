using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttackable
{
    void OnAttack(GameObject attacker, Attack attack);
}
