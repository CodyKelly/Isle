using System;
using Nez;
using Microsoft.Xna.Framework;

namespace NewProject
{
  class BatEntity : Entity, IPoolable
  {
    private Health _health;

    public override void OnAddedToScene()
    {
      base.OnAddedToScene();
      var map = ((BasicScene)Scene).Map;

      AddComponent(new BatController(((BasicScene)Scene).Map));
      _health = AddComponent(new Health());

      Reset();
    }

    public override void OnRemovedFromScene()
    {
      base.OnRemovedFromScene();
      Pool<BatEntity>.Free(this);
      SetEnabled(false);
    }

    public void Reset()
    {
      var map = ((BasicScene)Scene).Map;
      Transform.SetPosition(new Vector2(Nez.Random.NextFloat() * map.WorldWidth, Nez.Random.NextFloat() * map.WorldHeight));
      _health.Value = 100f;
    }
  }
}