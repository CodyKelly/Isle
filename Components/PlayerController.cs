using Nez;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nez.Sprites;
using Nez.Textures;
using Nez.ImGuiTools;

namespace NewProject
{
  class PlayerController : Component, IUpdatable
  {

    VirtualButton _fireInput;
    VirtualAxis _xAxisInput;
    VirtualAxis _yAxisInput;
    EntityMover _mover;
    Map _map;

    float fireRate = .25f;
    float lastFire = 0;
    bool firing = false;
    float fireSpawnDistance = 80f;

    public PlayerController(Map map) => _map = map;

    public void Update()
    {
      float currentTime = Time.TotalTime;
      Vector2 moveDir = new Vector2(_xAxisInput.Value, _yAxisInput.Value);

      _mover.MoveDir = moveDir;

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
      var downAtlas = Entity.Scene.Content.LoadTexture(Nez.Content.Textures.Character.Armorlancer.Armorlancerdownpng);
      var sideAtlas = Entity.Scene.Content.LoadTexture(Nez.Content.Textures.Character.Armorlancer.Armorlancersidepng);
      var upAtlas = Entity.Scene.Content.LoadTexture(Nez.Content.Textures.Character.Armorlancer.Armorlanceruppng);
      _mover = Entity.AddComponent(new EntityMover(_map, upAtlas, downAtlas, sideAtlas));
      SetupInput();
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