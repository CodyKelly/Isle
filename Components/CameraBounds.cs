using Microsoft.Xna.Framework;
using Nez;

namespace NewProject
{
  public class CameraBounds : Component, IUpdatable
  {
    public RectangleF Rect;


    public CameraBounds()
    {
      // make sure we run last so the camera is already moved before we evaluate its position
      SetUpdateOrder(int.MaxValue);
    }


    public CameraBounds(RectangleF rect) : this()
    {
      Rect = rect;
    }


    public override void OnAddedToEntity()
    {
      Entity.UpdateOrder = int.MaxValue;
    }


    void IUpdatable.Update()
    {
      var cameraBounds = Entity.Scene.Camera.Bounds;

      if (cameraBounds.Top < Rect.Top)
        Entity.Scene.Camera.Position += new Vector2(0, Rect.Top - cameraBounds.Top);

      if (cameraBounds.Left < Rect.Left)
        Entity.Scene.Camera.Position += new Vector2(Rect.Left - cameraBounds.Left, 0);

      if (cameraBounds.Bottom > Rect.Bottom)
        Entity.Scene.Camera.Position += new Vector2(0, Rect.Bottom - cameraBounds.Bottom);

      if (cameraBounds.Right > Rect.Right)
        Entity.Scene.Camera.Position += new Vector2(Rect.Right - cameraBounds.Right, 0);
    }
  }
}