using Common.Scripts;
using UnityEngine;

namespace Common.Infrastructure
{
  public interface IEnemyFactory
  {
    void Create(EnemyType type, Vector3 at);
    void Load();
  }
}