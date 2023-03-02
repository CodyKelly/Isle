using Nez;

namespace Isle
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