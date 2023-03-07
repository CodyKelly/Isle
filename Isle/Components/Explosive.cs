using Nez;
using Microsoft.Xna.Framework;

namespace Isle
{
  class Explosive : Component
  {
    public void Explode(Vector2 position, float radius, float power, float depth, float damage)
    {
      Collider[] colliders = new Collider[1000];
      int numCollisions = Physics.OverlapCircleAll(position, radius, colliders);
      for (int i = 0; i < numCollisions; i++)
      {
        Collider currentCollider = colliders[i];
        if (currentCollider.Entity is GameEntity)
        {
          Vector2 thatPos = currentCollider.Entity.Position;

          float distance = Vector2.Distance(position, thatPos);
          float angle = Mathf.AngleBetweenVectors(position, thatPos);
          float percent = 1 - distance / radius;
          if (percent > 0)
          {
            Vector2 vel = Mathf.AngleToVector(angle, percent * power);
            ((GameEntity)currentCollider.Entity).Velocity += vel;

            if (currentCollider.Entity.HasComponent<Health>())
            {
              Health health = currentCollider.Entity.GetComponent<Health>();
              health.GiveDamage(percent * damage);
            }
          }
        }
      }

      if (depth > 0)
      {
        var map = ((BasicScene)Entity.Scene).Map;
        int explosionRadiusInTiles = (int)(radius / map.TileSize);
        Vector2 posOnMap = new Vector2(
          map.WorldToTilePositionX(position.X),
          map.WorldToTilePositionY(position.Y));
        int right = (int)posOnMap.X + explosionRadiusInTiles;
        int top = (int)posOnMap.Y - explosionRadiusInTiles;
        int left = (int)posOnMap.X - explosionRadiusInTiles;
        int bottom = (int)posOnMap.Y + explosionRadiusInTiles;
        right = Mathf.Clamp(right, 0, map.Width - 1);
        top = Mathf.Clamp(top, 0, map.Width - 1);
        left = Mathf.Clamp(left, 0, map.Width - 1);
        bottom = Mathf.Clamp(bottom, 0, map.Width - 1);
        for (int y = top; y < bottom; y++)
        {
          for (int x = left; x < right; x++)
          {
            float dist = Vector2.Distance(new Vector2(x, y), posOnMap);
            float percent = 1 - dist / explosionRadiusInTiles;
            if (percent > 0)
            {
              map.SetValue(x, y, map.RawValues[x, y] - percent * depth);
            }
          }
        }
      }
    }
  }
}