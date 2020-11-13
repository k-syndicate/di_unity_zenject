  using Common.Scripts;
using UnityEngine;
using Zenject;

namespace Common.Infrastructure
{
  public class EnemyFactory : IEnemyFactory
  {
    private const string MeleeEnemyPrefabPath = "BatGoblin";
    private const string RangedEnemyPrefabPath = "BatGoblinRanged";

    private GameObject _meleeEnemyPrefab;
    private GameObject _rangedEnemyPrefab;

    private readonly DiContainer _diContainer;

    public EnemyFactory(DiContainer diContainer)
    {
      _diContainer = diContainer;
    }

    public void Create(EnemyType type, Vector3 at)
    {
      switch (type)
      {
        case EnemyType.Melee:
          _diContainer.InstantiatePrefab(_meleeEnemyPrefab, at, Quaternion.identity, null);
          break;
        case EnemyType.Ranged:
          _diContainer.InstantiatePrefab(_rangedEnemyPrefab, at, Quaternion.identity, null);
          break;
      }
    }

    public void Load()
    {
      _meleeEnemyPrefab = Resources.Load(MeleeEnemyPrefabPath) as GameObject;
      _rangedEnemyPrefab = Resources.Load(RangedEnemyPrefabPath) as GameObject;
    }
  }
}