using Nez;
using Nez.Sprites;
using Microsoft.Xna.Framework;

namespace NewProject
{
  class BasicScene : Scene
  {
    public override void Initialize()
    {
      base.Initialize();
      var player = CreateEntity("player", new Vector2(Screen.Width / 2, Screen.Height / 2));
      var playerTex = Content.LoadTexture(Nez.Content.Icon);
      player.AddComponent(new SpriteRenderer(playerTex));
      player.AddComponent(new PlayerController());
    }
  }
}
