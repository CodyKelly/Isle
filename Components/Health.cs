using Nez;

namespace NewProject
{
  class Health : Component
  {
    public float Value { get; set; } = 100f;

    public void GiveDamage(float amount)
    {
      Value -= amount;
      if (Value <= 0f)
      {
        Entity.DetachFromScene();
      }
    }
  }
}