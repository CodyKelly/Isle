using Nez;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nez.Sprites;
using Nez.Textures;
using Nez.ImGuiTools;

namespace NewProject
{
  class BatController : Component, IUpdatable
  {
    EntityMover _mover;
    Map _map;

    float turnTimer = 0f;
    float lastTurn = 0f;
    float maxTurnTime = 10;

    public BatController(Map map) => _map = map;

    public void Update()
    {
      var currentTime = Time.TotalTime;


      if (currentTime - lastTurn > turnTimer)
      {
        lastTurn = currentTime;
        turnTimer = Random.NextFloat(maxTurnTime);
        _mover.MoveDir = Random.NextUnitVector();
      }
    }

    public override void OnAddedToEntity()
    {
      Entity.Scale = Vector2.One * 3f;
      var downAtlas = Entity.Scene.Content.LoadTexture(Nez.Content.Textures.Character.Bat.Batpng);
      var sideAtlas = Entity.Scene.Content.LoadTexture(Nez.Content.Textures.Character.Bat.Batpng);
      var upAtlas = Entity.Scene.Content.LoadTexture(Nez.Content.Textures.Character.Bat.Batpng);
      _mover = Entity.AddComponent(new EntityMover(_map, upAtlas, downAtlas, sideAtlas));
      _mover.Speed = 850;
      _mover.AnimFramerate = 8f;
      turnTimer = Random.NextFloat(maxTurnTime);
      _mover.MoveDir = Random.NextUnitVector();
    }
  }
}