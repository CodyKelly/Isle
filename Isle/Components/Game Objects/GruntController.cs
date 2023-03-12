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

    Entity _player;

    Transform transform;

    Map _map;

    public void Update()
    {
      if (_player != null)
      {
        var posDiff = _player.Position - Entity.Position;
        posDiff.Normalize();
        var movement = posDiff * Speed;
        ((GameEntity)Entity).Velocity += movement;
      }
    }

    public override void OnAddedToEntity()
    {
      MaxSpeed = 1.5f;
      Speed = MaxSpeed;
      turnTimer = maxTurnTime;//Random.NextFloat(maxTurnTime);
      transform = Transform;
      _map = ((BasicScene)Entity.Scene).Map;
      _player = ((BasicScene)Entity.Scene).Player;

      base.OnAddedToEntity();
    }
  }
}