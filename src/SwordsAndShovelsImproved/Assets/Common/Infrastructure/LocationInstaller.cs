using System.Collections.Generic;
using Common.Scripts;
using UnityEngine;
using Zenject;

namespace Common.Infrastructure
{
  public class LocationInstaller : MonoInstaller, IInitializable
  {
    public Transform StartPoint;
    public GameObject HeroPrefab;
    public InventoryDisplay InventoryDisplayHolder;
    public List<EnemyMarker> EnemyMarkers;

    public override void InstallBindings()
    {
      // Container.BindFactory<NPCController, MeleeEnemyFactory>().FromComponentInNewPrefab(MeleeEnemyPrefab);
      // Container.BindFactory<NPCController, RangedEnemyFactory>().FromComponentInNewPrefab(RangedEnemyPrefab);
      Container.BindInterfacesTo<LocationInstaller>().FromInstance(this);
      Container.BindInterfacesAndSelfTo<EnemyFactory>().AsSingle();
      BindInventoryDisplay();
      BindHero();
    }

    private void BindHero()
    {
      HeroController hero = Container.InstantiatePrefabForComponent<HeroController>(HeroPrefab, StartPoint);

      Container.Bind(typeof(HeroController))
        .FromInstance(hero)
        .AsSingle();
    }

    private void BindInventoryDisplay()
    {
      Container.Bind(typeof(InventoryDisplay))
        .FromInstance(InventoryDisplayHolder)
        .AsSingle();
    }

    public void Initialize()
    {
      var enemyFactory = Container.Resolve<IEnemyFactory>();
      enemyFactory.Load();
      
      foreach (EnemyMarker marker in EnemyMarkers) 
        enemyFactory.Create(marker.EnemyType, marker.transform.position);
    }
  }
}