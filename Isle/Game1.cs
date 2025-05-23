using Nez;
using Nez.ImGuiTools;

namespace Isle
{
  class Game1 : Nez.Core
  {
    public Game1() : base()
    {
      // uncomment this line for scaled pixel art games
      System.Environment.SetEnvironmentVariable("FNA_OPENGL_BACKBUFFER_SCALE_NEAREST", "1");
    }

    override protected void Initialize()
    {
      base.Initialize();

      Screen.PreferredBackBufferWidth = Screen.MonitorWidth;
      Screen.PreferredBackBufferHeight = Screen.MonitorHeight;
      Screen.IsFullscreen = true;
      Screen.ApplyChanges();

      Physics.SpatialHashCellSize = 60;

      Scene = new BasicScene();

#if DEBUG
      System.Diagnostics.Debug.Listeners.Add(new System.Diagnostics.TextWriterTraceListener(System.Console.Out));

      // optionally render Nez in an ImGui window
      var imGuiManager = new ImGuiManager();
      Core.RegisterGlobalManager(imGuiManager);
      imGuiManager.Enabled = false;

      // optionally load up ImGui DLL if not using the above setup so that its command gets loaded in the DebugConsole
      //System.Reflection.Assembly.Load("Nez.ImGui")
#endif
    }
  }
}
