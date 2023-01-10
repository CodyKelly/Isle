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
    [Inspectable]
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
        var ent = Entity.Scene.AddEntity(new Entity("bullet"));
        var vector = GetFaceVector(Entity.Scene.Camera.MouseToWorldPoint());
        var pos = Entity.Position;
        ent.SetPosition(new Vector2(pos.X + (vector.X * fireSpawnDistance), pos.Y + (vector.Y * fireSpawnDistance)));
        ent.AddComponent(new Bullet(vector));
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
      // setup input for shooting a fireball. we will allow z on the keyboard or a on the gamepad
      _fireInput = new VirtualButton();
      _fireInput.Nodes.Add(new VirtualButton.KeyboardKey(Keys.Z));
      _fireInput.Nodes.Add(new VirtualButton.GamePadButton(0, Buttons.A));

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

    private Vector2 GetFaceVector(Vector2 other)
    {
      Vector2 pos = Entity.Position;
      Vector2 diff = new Vector2(other.X - pos.X, other.Y - pos.Y);
      float dist = Mathf.Sqrt(diff.X * diff.X + diff.Y * diff.Y);
      return diff / dist;
    }
  }
}