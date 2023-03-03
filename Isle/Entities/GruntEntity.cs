using Nez;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Isle
{
  class GruntEntity : Entity, IPoolable
  {
    private Health _health;
    float hp = 10f;
    static Texture2D downAtlas, sideAtlas, upAtlas;

    public override void OnAddedToScene()
    {
      base.OnAddedToScene();

      if (downAtlas == null)
      {
        downAtlas = Scene.Content.LoadTexture(Nez.Content.Textures.Character.Beasts.Beastdownpng);
        sideAtlas = Scene.Content.LoadTexture(Nez.Content.Textures.Character.Beasts.Beastsidepng);
        upAtlas = Scene.Content.LoadTexture(Nez.Content.Textures.Character.Beasts.Beastuppng);
      }

      AddComponent(new GruntController());
      AddComponent(new AnimationController(ref sideAtlas, ref downAtlas, ref upAtlas, 8f));
      AddComponent(new BoxCollider());
      _health = AddComponent(new Health(hp));

      Transform.SetScale(3f);

      Reset();
    }

    public override void OnRemovedFromScene()
    {
      base.OnRemovedFromScene();
      Pool<GruntEntity>.Free(this);
      SetEnabled(false);
    }

    public void Reset()
    {
      var map = ((BasicScene)Scene).Map;
      Transform.SetPosition(new Vector2(Nez.Random.NextFloat() * map.WorldWidth, Nez.Random.NextFloat() * map.WorldHeight));
      _health.Value = hp;
    }
  }
}