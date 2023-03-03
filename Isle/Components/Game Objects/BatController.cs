using Nez;
using Microsoft.Xna.Framework;

namespace Isle
{
  class BatController : Controller, IUpdatable
  {
    float turnTimer = 0f;
    float lastTurn = 0f;
    float maxTurnTime = 30;

    Transform transform;

    Map _map;

    SubpixelVector2 _movementSubpixel;

    Vector2 movement;
    Mover mover;

    public void Update()
    {
      var currentTime = Time.TotalTime;

      var pos = transform.Position;
      if (pos.X < 0 || pos.X > _map.WorldWidth)
      {
        MoveDir = new Vector2(-MoveDir.X, MoveDir.Y);
      }
      if (pos.Y < 0 || pos.Y > _map.WorldHeight)
      {
        MoveDir = new Vector2(MoveDir.X, -MoveDir.Y);
      }
      if (currentTime - lastTurn > turnTimer)
      {
        lastTurn = currentTime;
        turnTimer = Random.NextFloat(maxTurnTime);
        MoveDir = Random.NextUnitVector();
      }

      movement = MoveDir * Speed * Time.DeltaTime;

      if (!mover.CalculateMovement(ref movement, out _))
      {
        _movementSubpixel.Update(ref movement);
        mover.ApplyMovement(movement);
      }
    }

    public override void OnAddedToEntity()
    {
      MaxSpeed = 850;
      turnTimer = maxTurnTime;
      MoveDir = Random.NextUnitVector();
      mover = Entity.AddComponent(new Mover());
      transform = Transform;
      _map = ((BasicScene)Entity.Scene).Map;

      base.OnAddedToEntity();
    }
  }
}