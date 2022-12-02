using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using Nez;

namespace NewProject
{
  class ScrollZoom : Component, IUpdatable
  {
    public float ZoomSpeed { get; set; } = .02f;

    Camera _camera;

    public ScrollZoom(Camera camera) { _camera = camera; }
    public ScrollZoom() { }

    public void Update()
    {
      if (_camera == null) return;

      var delta = Input.MouseWheelDelta;
      // float speed = -(float)Math.Log10(_camera.RawZoom) * ZoomSpeed;
      if (delta != 0)
      {
        var zoomAmount = (float)delta * ZoomSpeed * Time.DeltaTime;
        _camera.Zoom += zoomAmount;
      }
    }

    public override void OnAddedToEntity()
    {
      _camera = Entity.GetComponent<Camera>();
    }
  }
}