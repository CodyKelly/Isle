using Nez;
using Nez.Sprites;
using Microsoft.Xna.Framework.Graphics;

namespace NewProject
{
  class BulletEntity : Entity
  {
    static Texture2D _texture;

    public override void OnAddedToScene()
    {
      base.OnAddedToScene();
      Transform.SetScale(2f);
      if (_texture == null) _texture = Scene.Content.LoadTexture(Content.Textures.Bulletpng);
      AddComponent(new SpriteRenderer(_texture));
      AddComponent(new Bullet());
    }

    public override void OnRemovedFromScene()
    {
      base.OnRemovedFromScene();
      Pool<BulletEntity>.Free(this);
      SetEnabled(false);
    }
  }
}