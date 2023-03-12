using System;
using Nez;
using Microsoft.Xna.Framework;

namespace Isle
{
  class PlayerEntity : GameEntity
  {
    private Health _health;

    public override void OnAddedToScene()
    {
      base.OnAddedToScene();
      var map = ((BasicScene)Scene).Map;

      var downAtlas = Scene.Content.LoadTexture(Nez.Content.Textures.Character.Armorlancer.Armorlancerdownpng);
      var sideAtlas = Scene.Content.LoadTexture(Nez.Content.Textures.Character.Armorlancer.Armorlancersidepng);
      var upAtlas = Scene.Content.LoadTexture(Nez.Content.Textures.Character.Armorlancer.Armorlanceruppng);

      AddComponent(new AnimationController(ref sideAtlas, ref downAtlas, ref upAtlas, 5f));
      AddComponent(new BoxCollider(-8, 4, 16, 4));
      AddComponent(new PlayerController());
      AddComponent(new SetRenderLayerByPos());
      AddComponent(new Health());

      Transform.SetPosition(map.WorldWidth / 2, map.WorldHeight / 4);
    }

    public override void OnRemovedFromScene()
    {
      base.OnRemovedFromScene();
    }
  }
}