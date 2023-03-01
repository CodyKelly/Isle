using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using Nez.ImGuiTools;
using Nez;
namespace NewProject;

public class Game1 : Nez.Core
{
  protected override void Initialize()
  {
    base.Initialize();

    Physics.SpatialHashCellSize = 1000;

    Scene = new BasicScene();

    var imGuiManager = new ImGuiManager();
    RegisterGlobalManager(imGuiManager);
    imGuiManager.Enabled = false;

    Window.AllowUserResizing = true;
    // Window.IsBorderless = true;
  }
}
