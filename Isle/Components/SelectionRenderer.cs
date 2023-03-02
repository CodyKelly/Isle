using Nez;
using Microsoft.Xna.Framework;

namespace Isle
{
  class SelectionRenderer : ScreenSpaceRenderer
  {
    public SelectionRenderer(int renderOrder, params int[] renderLayers) : base(renderOrder, renderLayers)
    {
    }
    //   public override void Render(Batcher batcher, Camera camera)
    //   {
    //     if (isSelecting)
    //     {
    //       var mousePos = Input.MousePosition;
    //       Vector2 topRight = new Vector2(mousePos.X, firstPoint.Y);
    //       Vector2 bottomLeft = new Vector2(firstPoint.X, mousePos.Y);
    //       batcher.DrawLine(firstPoint, topRight, Color);
    //       batcher.DrawLine(topRight, mousePos, Color);
    //       batcher.DrawLine(mousePos, bottomLeft, Color);
    //       batcher.DrawLine(bottomLeft, firstPoint, Color);
    //     }
    //   }
    // }
    public override void Render(Scene scene)
    {

    }
  }
}