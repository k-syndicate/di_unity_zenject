using Zenject;

namespace Common.Infrastructure
{
  public class BootstrapInstaller  : MonoInstaller
  {
    public MouseManager MouseManagerPrefab;
    
    public override void InstallBindings()
    {
      BindMouseService();
    }

    private void BindMouseService()
    {
      Container
        .Bind<IMouseService>()
        .FromComponentInNewPrefab(MouseManagerPrefab)
        .UnderTransform(transform)
        .AsSingle();
    }
  }
}