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

      ClearColor = Color.LightBlue;

      var player = CreateEntity("player", new Vector2(Screen.Width / 2, Screen.Height / 2));

      var playerTex = Content.LoadTexture(Nez.Content.Textures.DVD_logo);
      player.AddComponent(new SpriteRenderer(playerTex));
      player.SetScale(new Vector2(.2f, .2f));
      player.AddComponent(new Bouncer());

      CreateWalls();
    }

    private void CreateWalls()
    {
      float halfWallWidth = 5.0f;
      Entity[] walls = new Entity[4];
      walls[0] = CreateEntity("wall0", new Vector2(Screen.Width + halfWallWidth, Screen.Height / 2f));
      walls[0].AddComponent(new BoxCollider(halfWallWidth * 2f, Screen.Height));

      walls[1] = CreateEntity("wall1", new Vector2(Screen.Width / 2f, -halfWallWidth));
      walls[1].AddComponent(new BoxCollider(Screen.Width, halfWallWidth * 2f));

      walls[2] = CreateEntity("wall2", new Vector2(-halfWallWidth, Screen.Height / 2f));
      walls[2].AddComponent(new BoxCollider(halfWallWidth * 2f, Screen.Height));

      walls[3] = CreateEntity("wall3", new Vector2(Screen.Width / 2f, Screen.Height + halfWallWidth));
      walls[3].AddComponent(new BoxCollider(Screen.Width, halfWallWidth * 2f));
    }
  }
}
