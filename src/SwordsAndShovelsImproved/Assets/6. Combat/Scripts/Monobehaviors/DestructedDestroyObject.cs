using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructedDestroyObject : MonoBehaviour, IDestructible
{
    public void OnDestruction(GameObject destroyer)
    {
        Destroy(gameObject);
    }
}
