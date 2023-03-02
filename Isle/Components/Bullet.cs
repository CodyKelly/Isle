using Nez;
using Microsoft.Xna.Framework;

namespace Isle
{
  class Bullet : Component, IUpdatable
  {
    public float Speed { get; set; } = 3000f;

    SubpixelVector2 _subpixelV2 = new SubpixelVector2();

    float liveTime = 50f;
    float spawnTime = 0;

    float damage = 50f;

    float lastRotation;

    Vector2 moveDir;

    public override void OnAddedToEntity()
    {
      spawnTime = Time.TotalTime;
      lastRotation = Transform.Rotation;
      moveDir = GetMoveDir();
    }

    public void Update()
    {
      if (Transform.Rotation != lastRotation)
      {
        lastRotation = Transform.Rotation;
        moveDir = GetMoveDir();
      }

      Vector2 movement = moveDir * Speed * Time.DeltaTime;
      _subpixelV2.Update(ref movement);

      var pos = Entity.Position;
      var result = Physics.Linecast(pos, pos + movement);
      if (result.Collider != null)
      {
        if (result.Collider.Entity.HasComponent<Health>())
        {
          var health = result.Collider.Entity.GetComponent<Health>();
          health.GiveDamage(damage);
        }
        Entity.DetachFromScene();
      }
      else
      {
        Entity.SetPosition(pos + movement);
      }

      float currentTime = Time.TotalTime;
      if (currentTime - spawnTime > liveTime)
      {
        Entity.DetachFromScene();
      }
    }

    Vector2 GetMoveDir()
    {
      return new Vector2(Mathf.Cos(Transform.Rotation), Mathf.Sin(Transform.Rotation));
    }

    public override void OnDisabled()
    {
      base.OnDisabled();
      Debug.Log(Entity.Name + " has been disabled.");
    }
  }
}