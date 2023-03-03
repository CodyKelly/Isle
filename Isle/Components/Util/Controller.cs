using Nez;
using Microsoft.Xna.Framework;

namespace Isle
{
  abstract class Controller : Component
  {
    public Vector2 MoveDir { get; set; } = Vector2.Zero;

    public float MaxSpeed { get; set; } = 750f;
    public float Speed { get; set; }

    public override void OnAddedToEntity()
    {
      base.OnAddedToEntity();

      Speed = MaxSpeed;
    }
  }
}