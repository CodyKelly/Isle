using Nez;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nez.Sprites;
using Nez.Textures;
using Nez.ImGuiTools;

namespace NewProject
{
  class BatController : AnimatedController
  {
    float turnTimer = 0f;
    float lastTurn = 0f;
    float maxTurnTime = 10;

    public override void Update()
    {
      base.Update();

      var currentTime = Time.TotalTime;

      var pos = Entity.Position;
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
    }

    public override void OnAddedToEntity()
    {
      _collider = Entity.AddComponent<BoxCollider>();
      base.OnAddedToEntity();
      MaxSpeed = 850;
      _framerate = 8f;
      turnTimer = Random.NextFloat(maxTurnTime);
      MoveDir = Random.NextUnitVector();
      _map = ((BasicScene)Entity.Scene).Map;
    }

    protected override void LoadTextures()
    {
      if (_upAtlas == null)
      {
        _downAtlas = Entity.Scene.Content.LoadTexture(Nez.Content.Textures.Character.Bat.Batpng);
        _sideAtlas = Entity.Scene.Content.LoadTexture(Nez.Content.Textures.Character.Bat.Batpng);
        _upAtlas = Entity.Scene.Content.LoadTexture(Nez.Content.Textures.Character.Bat.Batpng);
      }
    }
  }
}