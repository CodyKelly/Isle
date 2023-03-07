using Nez;
using Microsoft.Xna.Framework;

namespace Isle
{
  class Explosive : Component
  {
    public void Explode(Vector2 position, float radius, float power, float damage)
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
          Vector2 vel = Mathf.AngleToVector(angle, radius / distance * power);
          ((GameEntity)currentCollider.Entity).Velocity += vel;

          if (currentCollider.Entity.HasComponent<Health>())
          {
            Health health = currentCollider.Entity.GetComponent<Health>();
            health.GiveDamage(damage);
          }
        }
      }
    }
  }
}