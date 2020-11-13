using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Aoe.asset", menuName = "Attack/AOE")]
public class Aoe : AttackDefinition 
{
    public float Radius;
    public float TimeInScene;
    public GameObject AoePrefab;

    public void Fire(GameObject Caster, Vector3 Position, int Layer)
    {
        // Instantiate and destroy our aoe prefab
        var aoe = Instantiate(AoePrefab, Position, Quaternion.identity);
        Destroy(aoe, TimeInScene);

        // get objects inside our aoe radius
        var collidedObjects = Physics.OverlapSphere(Position, Radius);

        // loop through all collided objects
        foreach(var collsion in collidedObjects)
        {
            var collisionGo = collsion.gameObject;

            // check if we are ignoring the collision's layer, if so move on to the next object
            if (Physics.GetIgnoreLayerCollision(Layer, collisionGo.layer))
                continue;

            // create attack and send it to the attackable behaviors of our collision
            var casterStats = Caster.GetComponent<CharacterStats>();
            var collisionStats = collisionGo.GetComponent<CharacterStats>();

            var attack = CreateAttack(casterStats, collisionStats);

            var attackables = collisionGo.GetComponentsInChildren(typeof(IAttackable));
            foreach(IAttackable a in attackables)
            {
                a.OnAttack(Caster, attack);
            }
        }
    }
}
