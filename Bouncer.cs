using Microsoft.Xna.Framework;
using Nez;

class Bouncer : Component, IUpdatable
{
  Mover mover;
  float speed = 200f;
  float angle;
  Vector2 velocity;

  public override void OnAddedToEntity()
  {
    Entity.AddComponent(new BoxCollider());
    mover = Entity.AddComponent(new Mover());
    angle = Random.NextAngle();
    velocity = new Vector2(speed * Mathf.Cos(angle), speed * Mathf.Sin(angle));
  }

  public void Update()
  {
    Vector2 movement = velocity * Time.DeltaTime;
    if (mover.CalculateMovement(ref movement, out var res))
    {
      var result = res.Normal;
      result.Round();

      if (result.X != 0)
      {
        velocity = new Vector2(velocity.X * -1f, velocity.Y);
      }

      if (result.Y != 0)
      {
        velocity = new Vector2(velocity.X, velocity.Y * -1f);
      }

      movement = velocity * Time.DeltaTime;
      mover.ApplyMovement(movement);
    }
    else
    {
      mover.ApplyMovement(movement);
    }

  }
}