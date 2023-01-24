using Nez;
using Microsoft.Xna.Framework;

namespace NewProject
{
  abstract class Controller : Component, IUpdatable
  {
    Mover _mover;
    SubpixelVector2 _subpixelV2;
    protected Map _map;
    protected Collider _collider;

    public Vector2 MoveDir { get; set; } = Vector2.Zero;

    public float MaxSpeed { get; set; } = 750f;
    public float Speed { get; set; }

    public virtual void Update()
    {
      var movement = MoveDir * Speed * Time.DeltaTime;
      var bounds = _collider.Bounds;
      _subpixelV2.Update(ref movement);
      _mover.Move(movement, out _);
    }

    public override void OnAddedToEntity()
    {
      base.OnAddedToEntity();
      _subpixelV2 = new SubpixelVector2();
      _map = ((BasicScene)Entity.Scene).Map;
      _mover = Entity.AddComponent<Mover>();

      Speed = MaxSpeed;
    }
  }
}