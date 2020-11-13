using UnityEngine;

namespace Common.Scripts
{
  public class EnemyMarker : MonoBehaviour
  {
    public EnemyType EnemyType;

    private void OnDrawGizmos()
    {
      Gizmos.color = Color.red;
      Gizmos.DrawSphere(transform.position, 1);
      Gizmos.color = Color.white;
    }
  }
}