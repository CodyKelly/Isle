using Nez;
using Microsoft.Xna.Framework;
using Nez.Sprites;

namespace Isle
{
  class BuildingEntity : GameEntity
  {
    private Health _health;
    float hp = 10f;
    static Microsoft.Xna.Framework.Graphics.Texture2D texture;

    public override void OnAddedToScene()
    {
      base.OnAddedToScene();
      Name = "Building";

      if (texture == null)
      {
        texture = Scene.Content.LoadTexture(Nez.Content.Textures.Overworldpng);
      }

      AddComponent(new SpriteRenderer(new Nez.Textures.Sprite(texture, new Rectangle(96, 0, 80, 80), Vector2.Zero)));
      AddComponent(new BoxCollider(-16, 16, 32, 32));
      AddComponent(new SetRenderLayerByPos());
      _health = AddComponent(new Health(hp));

      Transform.SetScale(2f);
    }

    public override void OnRemovedFromScene()
    {
      base.OnRemovedFromScene();
    }
  }
}