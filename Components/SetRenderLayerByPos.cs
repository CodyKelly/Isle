using Nez;
using Nez.Sprites;

namespace NewProject
{
  class SetRenderLayerByPos : Component, IUpdatable
  {
    RectangleF _bounds;
    SpriteRenderer _renderer;

    public override void OnAddedToEntity()
    {
      _bounds = Entity.GetComponent<Collider>().Bounds;
      _renderer.SetRenderLayer(-(int)_bounds.Bottom - (int)(_bounds.Height / 2f));
    }

    public void Update()
    {
      _renderer.SetRenderLayer(-(int)_bounds.Bottom - (int)(_bounds.Height / 2f));
    }
  }
}