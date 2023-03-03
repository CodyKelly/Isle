using Microsoft.Xna.Framework;
using Nez;

namespace Isle
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

      if (cameraBounds.Height > Rect.Height)
      {
        Entity.Scene.Camera.Position = new Vector2(Entity.Scene.Camera.Position.X, Rect.Center.Y);
      }
      else
      {
        if (cameraBounds.Top < Rect.Top)
          Entity.Scene.Camera.Position += new Vector2(0, Rect.Top - cameraBounds.Top);

        if (cameraBounds.Bottom > Rect.Bottom && !(cameraBounds.Height > Rect.Height))
          Entity.Scene.Camera.Position += new Vector2(0, Rect.Bottom - cameraBounds.Bottom);
      }

      if (cameraBounds.Width > Rect.Width)
      {
        Entity.Scene.Camera.Position = new Vector2(Rect.Center.X, Entity.Scene.Camera.Position.Y);
      }
      else
      {
        if (cameraBounds.Left < Rect.Left && !(cameraBounds.Width > Rect.Width))
          Entity.Scene.Camera.Position += new Vector2(Rect.Left - cameraBounds.Left, 0);

        if (cameraBounds.Right > Rect.Right && !(cameraBounds.Width > Rect.Width))
          Entity.Scene.Camera.Position += new Vector2(Rect.Right - cameraBounds.Right, 0);
      }
    }
  }
}