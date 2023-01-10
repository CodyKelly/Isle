using Nez;
using Nez.Sprites;
using Microsoft.Xna.Framework;

namespace NewProject
{
  class Bullet : Component, IUpdatable
  {
    public Vector2 Vector { get; }
    public float Speed { get; set; } = 1200f;

    Mover _mover;
    SubpixelVector2 _subpixelV2 = new SubpixelVector2();
    CircleCollider _collider;

    float liveTime = 20f;
    float spawnTime = 0;

    public Bullet(Vector2 vector)
    {
      Vector = vector;
    }

    public override void OnAddedToEntity()
    {
      Entity.SetScale(2f);
      Entity.AddComponent(new SpriteRenderer(Entity.Scene.Content.LoadTexture(Content.Textures.Bulletpng)));
      _collider = Entity.AddComponent(new CircleCollider());
      _mover = Entity.AddComponent(new Mover());
      spawnTime = Time.TotalTime;
    }

    public void Update()
    {
      Vector2 movement = Vector * Speed * Time.DeltaTime;
      _subpixelV2.Update(ref movement);
      _mover.ApplyMovement(movement);

      if (_collider.CollidesWithAny(out CollisionResult result) && result.Collider.Entity.Name != "bullet")
      {
        result.Collider.Entity.Destroy();
        Entity.Destroy();
      }

      float currentTime = Time.TotalTime;
      if (currentTime - spawnTime > liveTime)
      {
        Entity.Destroy();
      }
    }
  }
}