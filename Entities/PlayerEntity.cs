using System;
using Nez;
using Microsoft.Xna.Framework;

namespace NewProject
{
  class PlayerEntity : Entity
  {
    private Health _health;

    public override void OnAddedToScene()
    {
      base.OnAddedToScene();
      var map = ((BasicScene)Scene).Map;

      AddComponent(new PlayerController(map));
      AddComponent(new Health());

      Transform.SetPosition(map.TileToWorldPosition(map.HighestPoint));
    }

    public override void OnRemovedFromScene()
    {
      base.OnRemovedFromScene();
    }
  }
}