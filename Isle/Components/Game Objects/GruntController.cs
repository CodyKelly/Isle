using Nez;
using Nez.AI.FSM;
using Microsoft.Xna.Framework;

namespace Isle
{
  class GruntController : Controller, IUpdatable
  {
    float turnTimer = 0f;
    float lastTurn = 0f;
    float maxTurnTime = 30;

    Transform transform;

    Map _map;

    public void Update()
    {
      Speed = MaxSpeed;
      Tile currentTile = _map.GetTileAtWorldPos(Entity.Position.X, Entity.Position.Y);
      bool inWater = currentTile == null ? false : currentTile.Name == "water";
      if (inWater)
      {
        Speed /= 2f;
      }

      ((GameEntity)Entity).Velocity += Speed * new Vector2(0, 0);
    }

    public override void OnAddedToEntity()
    {
      MaxSpeed = 1.5f;
      turnTimer = maxTurnTime;//Random.NextFloat(maxTurnTime);
      transform = Transform;
      _map = ((BasicScene)Entity.Scene).Map;

      base.OnAddedToEntity();
    }

    public void Explode()
    {

    }
  }
}