using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructedRestartGame : MonoBehaviour, IDestructible 
{
    public float TimeToWaitUnitlGameRestart;

    public void OnDestruction(GameObject destroyer)
    {
        StartCoroutine(RestartGame());
    }

    private IEnumerator RestartGame()
    {
        yield return new WaitForSeconds(TimeToWaitUnitlGameRestart);
        GameManager.Instance.RestartGame();
    }
}
