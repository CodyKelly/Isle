using Nez;
using Nez.Sprites;
using Microsoft.Xna.Framework;

namespace NewProject
{
  class Bullet : Component, IUpdatable
  {
    public Vector2 Vector { get; }
    public float Speed { get; set; } = 3000f;

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

      var pos = Entity.Position;
      var result = Physics.Linecast(pos, pos + movement);
      if (result.Collider != null)
      {
        result.Collider.Entity.Destroy();
        Entity.Destroy();
      }
      else
      {
        Entity.SetPosition(pos + movement);
      }

      // if (_mover.Move(movement, out CollisionResult result) && result.Collider.Entity.Name != "bullet")
      // {

      // }

      float currentTime = Time.TotalTime;
      if (currentTime - spawnTime > liveTime)
      {
        Entity.Destroy();
      }
    }
  }
}