using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nez;

namespace Isle
{
  public class LightingSpinner : Component, IUpdatable
  {
    private const float RotationAcceleration = 0.5f;

    private float _rotationVelocity = 0f;
    private float _maxRotationVelocity = 5f;

    VirtualAxis _rotationAxis;

    MapRenderer _mapRenderer;

    public override void OnAddedToEntity()
    {
      _rotationAxis = new VirtualAxis();
      _rotationAxis.Nodes.Add(new VirtualAxis.KeyboardKeys(VirtualInput.OverlapBehavior.TakeNewer, Keys.Q, Keys.E));
      _mapRenderer = Entity.Scene.FindComponentOfType<MapRenderer>();
    }

    public void Update()
    {
      if (_rotationAxis.Value != 0)
      {
        _rotationVelocity += _rotationAxis.Value * RotationAcceleration;
        _rotationVelocity = Mathf.Clamp(_rotationVelocity, -_maxRotationVelocity, _maxRotationVelocity);
      }

      if (_rotationVelocity != 0)
      {
        _rotationVelocity = Mathf.Lerp(_rotationVelocity, 0, 0.1f);

        // Update the light source angle
        _mapRenderer.LightDirection = Mathf.RotateAroundRadians(_mapRenderer.LightDirection, Vector2.Zero, _rotationVelocity * Time.DeltaTime);
      }
    }
  }
}