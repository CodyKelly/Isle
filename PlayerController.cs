using Nez;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nez.ImGuiTools;

namespace NewProject
{
  class PlayerController : Component, IUpdatable
  {

    VirtualButton _fireInput;
    [Inspectable]
    VirtualIntegerAxis _xAxisInput;
    VirtualIntegerAxis _yAxisInput;
    SubpixelVector2 _subpixelV2 = new SubpixelVector2();

    [Inspectable]
    float speed = 1000f;

    Mover _mover;

    public void Update()
    {
      var moveDir = new Vector2(_xAxisInput.Value, _yAxisInput.Value);
      Debug.Log(moveDir);
      var movement = moveDir * speed * Time.DeltaTime;
      _mover.CalculateMovement(ref movement, out var res);
      _subpixelV2.Update(ref movement);
      _mover.ApplyMovement(movement);
    }

    public override void OnAddedToEntity()
    {
      _mover = Entity.AddComponent(new Mover());
      SetupInput();
    }

    void SetupInput()
    {
      // setup input for shooting a fireball. we will allow z on the keyboard or a on the gamepad
      _fireInput = new VirtualButton();
      _fireInput.Nodes.Add(new VirtualButton.KeyboardKey(Keys.Z));
      _fireInput.Nodes.Add(new VirtualButton.GamePadButton(0, Buttons.A));

      // horizontal input from dpad, left stick or keyboard left/right
      _xAxisInput = new VirtualIntegerAxis();
      _xAxisInput.Nodes.Add(new VirtualAxis.GamePadDpadLeftRight());
      _xAxisInput.Nodes.Add(new VirtualAxis.GamePadLeftStickX());
      _xAxisInput.Nodes.Add(new VirtualAxis.KeyboardKeys(VirtualInput.OverlapBehavior.TakeNewer, Keys.Left, Keys.Right));

      // vertical input from dpad, left stick or keyboard up/down
      _yAxisInput = new VirtualIntegerAxis();
      _yAxisInput.Nodes.Add(new VirtualAxis.GamePadDpadUpDown());
      _yAxisInput.Nodes.Add(new VirtualAxis.GamePadLeftStickY());
      _yAxisInput.Nodes.Add(new VirtualAxis.KeyboardKeys(VirtualInput.OverlapBehavior.TakeNewer, Keys.Up, Keys.Down));
    }
  }
}