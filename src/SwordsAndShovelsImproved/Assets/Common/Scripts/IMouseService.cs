using System;
using UnityEngine;

public interface IMouseService
{
  event Action<Vector3> OnEnvironmentClick;
  event Action<Vector3> OnEnvironmentRightClick;
  event Action<GameObject> OnAttackableClick;
}