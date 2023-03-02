using Nez;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Isle
{
  class PlayerController : Controller, IUpdatable
  {

    VirtualButton _fireInput;
    VirtualAxis _xAxisInput;
    VirtualAxis _yAxisInput;
    Map _map;
    Mover _mover;

    float fireRate = .05f;
    float lastFire = 0;
    bool firing = false;
    float fireSpawnDistance = 80f;

    public void Update()
    {
      Speed = MaxSpeed;
      Tile currentTile = _map.GetTileAtWorldPos(Entity.Position.X, Entity.Position.Y);
      bool inWater = currentTile == null ? false : currentTile.Name == "water";
      if (inWater)
      {
        Speed /= 2f;
      }

      MoveDir = new Vector2(_xAxisInput.Value, _yAxisInput.Value) * Speed * Time.DeltaTime;
      _mover.Move(MoveDir, out var res);

      float currentTime = Time.TotalTime;
      if (Input.LeftMouseButtonPressed) { firing = true; }
      else if (Input.LeftMouseButtonReleased) firing = false;

      if (firing && currentTime - lastFire > fireRate)
      {
        lastFire = currentTime;
        var newBullet = Pool<BulletEntity>.Obtain();
        var pos = Entity.Position;
        var mousePos = Entity.Scene.Camera.MouseToWorldPoint();
        var vector = Vector2.Normalize(mousePos - pos);
        newBullet.SetPosition(new Vector2(pos.X + (vector.X * fireSpawnDistance), pos.Y + (vector.Y * fireSpawnDistance)));
        newBullet.SetRotation(Mathf.AngleBetweenVectors(pos, mousePos));
        newBullet.SetEnabled(true);
        Entity.Scene.AddEntity(newBullet);
      }
    }

    public override void OnAddedToEntity()
    {
      Entity.Scale = Vector2.One * 3f;
      MaxSpeed = 500f;
      _map = ((BasicScene)Entity.Scene).Map;
      _mover = Entity.AddComponent<Mover>();
      SetupInput();
      base.OnAddedToEntity();
    }

    void SetupInput()
    {
      _fireInput = new VirtualButton();
      _fireInput.Nodes.Add(new VirtualButton.KeyboardKey(Keys.Z));
      _fireInput.Nodes.Add(new VirtualButton.GamePadButton(0, Buttons.A));

      _xAxisInput = new VirtualAxis();
      _xAxisInput.Nodes.Add(new VirtualAxis.GamePadDpadLeftRight());
      _xAxisInput.Nodes.Add(new VirtualAxis.GamePadLeftStickX());
      _xAxisInput.Nodes.Add(new VirtualAxis.KeyboardKeys(VirtualInput.OverlapBehavior.TakeNewer, Keys.Left, Keys.Right));
      _xAxisInput.Nodes.Add(new VirtualAxis.KeyboardKeys(VirtualInput.OverlapBehavior.TakeNewer, Keys.A, Keys.D));

      _yAxisInput = new VirtualAxis();
      _yAxisInput.Nodes.Add(new VirtualAxis.GamePadDpadUpDown());
      _yAxisInput.Nodes.Add(new VirtualAxis.GamePadLeftStickY());
      _yAxisInput.Nodes.Add(new VirtualAxis.KeyboardKeys(VirtualInput.OverlapBehavior.TakeNewer, Keys.Up, Keys.Down));
      _yAxisInput.Nodes.Add(new VirtualAxis.KeyboardKeys(VirtualInput.OverlapBehavior.TakeNewer, Keys.W, Keys.S));
    }
  }
}