using Nez;
using Nez.Sprites;
using Microsoft.Xna.Framework;

namespace Isle
{
  class SetRenderLayerByPos : Component, IUpdatable
  {
    SpriteRenderer _renderer;
    Vector2 lastPos;

    public override void OnAddedToEntity()
    {
      _renderer = Entity.GetComponent<SpriteRenderer>();
      _renderer.SetRenderLayer(-(int)_renderer.Bounds.Bottom);
      lastPos = Entity.Position;
    }

    public void Update()
    {
      if (Entity.Position != lastPos)
      {
        _renderer.SetRenderLayer(-(int)_renderer.Bounds.Bottom);
      }
    }
  }
}