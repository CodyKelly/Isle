using System;
using Nez;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NewProject
{
  class BatEntity : Entity, IPoolable
  {
    private Health _health;
    static Texture2D downAtlas, sideAtlas, upAtlas;

    public override void OnAddedToScene()
    {
      base.OnAddedToScene();

      if (downAtlas == null)
      {
        downAtlas = Scene.Content.LoadTexture(Nez.Content.Textures.Character.Bat.Batpng);
        sideAtlas = Scene.Content.LoadTexture(Nez.Content.Textures.Character.Bat.Batpng);
        upAtlas = Scene.Content.LoadTexture(Nez.Content.Textures.Character.Bat.Batpng);
      }

      AddComponent(new BatController());
      AddComponent(new AnimationController(ref sideAtlas, ref downAtlas, ref upAtlas, 8f));
      AddComponent(new BoxCollider());
      _health = AddComponent(new Health());

      Transform.SetScale(3f);

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