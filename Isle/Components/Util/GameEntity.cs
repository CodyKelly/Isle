using Nez;
using Microsoft.Xna.Framework;

namespace Isle
{
  class GameEntity : Entity, IUpdatable
  {
    public Vector2 Velocity { get; set; }

    Vector2 bounds;

    public float friction = 0.25f;
    float velocityThreshold = .01f;
    Mover mover;

    public override void Update()
    {
      base.Update();

      Position += Velocity;
      Velocity *= 1 - friction;

      bool xInThreshold = InThreshold(Velocity.X);
      bool yInThreshold = InThreshold(Velocity.Y);
      if (xInThreshold || yInThreshold)
      {
        Velocity = new Vector2(
          xInThreshold ? 0f : Velocity.X,
          yInThreshold ? 0f : Velocity.Y
        );
      }

      var movement = Velocity * Time.DeltaTime;
      if (mover.CalculateMovement(ref movement, out _))
      {
        mover.ApplyMovement(movement);
      }
      Position = Vector2.Clamp(Position, Vector2.Zero, bounds);
    }

    public override void OnAddedToScene()
    {
      base.OnAddedToScene();

      mover = AddComponent(new Mover());

      var map = ((BasicScene)Scene).Map;
      bounds = new Vector2(map.WorldWidth - 64, map.WorldHeight - 64);
    }

    bool InThreshold(float velocity)
    {
      return velocity <= velocityThreshold && velocity > 0 ||
      velocity >= -velocityThreshold && velocity < 0;
    }
  }
}