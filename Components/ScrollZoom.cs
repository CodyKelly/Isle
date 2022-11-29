using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using Nez;

namespace NewProject
{
  class ScrollZoom : Component, IUpdatable
  {
    public float ZoomSpeed { get; set; } = .2f;

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
        Debug.Log(zoomAmount);
        _camera.Zoom += zoomAmount;
        Debug.Log(_camera.Zoom);
        Debug.Log("\n");
      }
    }

    public override void OnAddedToEntity()
    {
      _camera = Entity.GetComponent<Camera>();
    }
  }
}