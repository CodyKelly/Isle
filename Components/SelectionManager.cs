using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using Nez;

namespace NewProject
{
  class SelectionManager : RenderableComponent, IUpdatable
  {
    bool isSelecting = false;
    Vector2 firstPoint;
    VirtualButton _selectInput;

    public override RectangleF Bounds
    {
      get
      {
        if (_areBoundsDirty)
        {
          _bounds = new RectangleF(0, 0, Screen.Width, Screen.Height);
          _areBoundsDirty = false;
        }

        return _bounds;
      }
    }

    public void Update()
    {
      if (_selectInput.IsPressed)
      {
        isSelecting = true;
        firstPoint = Input.MousePosition;
      }
      else if (_selectInput.IsReleased)
      {
        isSelecting = false;
      }

      if (isSelecting)
      {

      }
    }

    public override void OnAddedToEntity()
    {
      _bounds = new RectangleF(0, 0, 0, 0);
      SetupInput();
    }

    void SetupInput()
    {
      // setup input for shooting a fireball. we will allow z on the keyboard or a on the gamepad
      _selectInput = new VirtualButton();
      _selectInput.Nodes.Add(new VirtualButton.MouseLeftButton());
    }

    public override void Render(Batcher batcher, Camera camera)
    {
      if (isSelecting)
      {
        var mousePos = Input.MousePosition;
        Vector2 topRight = new Vector2(mousePos.X, firstPoint.Y);
        Vector2 bottomLeft = new Vector2(firstPoint.X, mousePos.Y);
        batcher.DrawLine(firstPoint, topRight, Color);
        batcher.DrawLine(topRight, mousePos, Color);
        batcher.DrawLine(mousePos, bottomLeft, Color);
        batcher.DrawLine(bottomLeft, firstPoint, Color);
      }
    }
  }
}