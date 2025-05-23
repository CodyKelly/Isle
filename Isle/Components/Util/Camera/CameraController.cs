using Nez;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Isle
{
  class CameraController : Component, IUpdatable
  {
    public CameraController(float acceleration = 400f, float maxSpeed = 5000f)
    {
      Acceleration = acceleration;
      MaxSpeed = maxSpeed;
    }

    VirtualAxis _xAxisInput;
    VirtualAxis _yAxisInput;
    SubpixelVector2 _subpixelV2 = new SubpixelVector2();

    [Inspectable]
    Vector2 speed = Vector2.Zero;
    float Acceleration { get; set; }
    float MaxSpeed { get; set; }

    Mover _mover;

    public void Update()
    {
      var moveDir = new Vector2(_xAxisInput.Value, _yAxisInput.Value);

      speed = new Vector2(
        moveDir.X != 0f ?
          Mathf.Clamp(speed.X + moveDir.X * Acceleration, -MaxSpeed, MaxSpeed) :
        speed.X < 0 ?
          Mathf.Clamp(speed.X + Acceleration, -MaxSpeed, 0) :
        speed.X > 0 ?
          Mathf.Clamp(speed.X - Acceleration, 0, MaxSpeed) :
        0f,
        moveDir.Y != 0f ?
          Mathf.Clamp(speed.Y + moveDir.Y * Acceleration, -MaxSpeed, MaxSpeed) :
        speed.Y < 0 ?
          Mathf.Clamp(speed.Y + Acceleration, -MaxSpeed, 0) :
        speed.Y > 0 ?
          Mathf.Clamp(speed.Y - Acceleration, 0, MaxSpeed) :
        0f
      );

      if (speed != Vector2.Zero)
      {
        var movement = new Vector2(
          speed.X * Time.DeltaTime,
          speed.Y * Time.DeltaTime
          );
        _subpixelV2.Update(ref movement);
        _mover.ApplyMovement(movement);
      }
    }

    public override void OnAddedToEntity()
    {
      _mover = Entity.AddComponent(new Mover());
      SetupInput();
    }

    void SetupInput()
    {
      // horizontal input from dpad, left stick or keyboard left/right
      _xAxisInput = new VirtualAxis();
      _xAxisInput.Nodes.Add(new VirtualAxis.GamePadDpadLeftRight());
      _xAxisInput.Nodes.Add(new VirtualAxis.GamePadLeftStickX());
      _xAxisInput.Nodes.Add(new VirtualAxis.KeyboardKeys(VirtualInput.OverlapBehavior.TakeNewer, Keys.Left, Keys.Right));
      _xAxisInput.Nodes.Add(new VirtualAxis.KeyboardKeys(VirtualInput.OverlapBehavior.TakeNewer, Keys.A, Keys.D));

      // vertical input from dpad, left stick or keyboard up/down
      _yAxisInput = new VirtualAxis();
      _yAxisInput.Nodes.Add(new VirtualAxis.GamePadDpadUpDown());
      _yAxisInput.Nodes.Add(new VirtualAxis.GamePadLeftStickY());
      _yAxisInput.Nodes.Add(new VirtualAxis.KeyboardKeys(VirtualInput.OverlapBehavior.TakeNewer, Keys.Up, Keys.Down));
      _yAxisInput.Nodes.Add(new VirtualAxis.KeyboardKeys(VirtualInput.OverlapBehavior.TakeNewer, Keys.W, Keys.S));
    }
  }
}