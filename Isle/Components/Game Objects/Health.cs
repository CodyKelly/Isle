using Nez;

namespace Isle
{
  class Health : Component
  {
    public Health(float value) => Value = value;
    public Health() : this(10f) { }

    public float Value { get; set; }

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