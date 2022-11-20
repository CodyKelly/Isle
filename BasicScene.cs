using Nez;
using Nez.UI;
using Nez.Sprites;
using Nez.Textures;
using Nez.BitmapFonts;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace NewProject
{
  class BasicScene : Scene
  {
    public override void Initialize()
    {
      base.Initialize();

      AddRenderer(new DefaultRenderer());
      Screen.SetSize(1024, 720);

      ClearColor = new Color(0x2f8136);
      var atlas = Content.LoadTexture(Nez.Content.Textures.Terrain_atlaspng);
      List<Sprite> decorations = new List<Sprite>();
      decorations.Add(new Sprite(atlas, new Rectangle(1024 - 32 * 3, 1024 - 32 * 4, 32 * 3, 32 * 4), Vector2.Zero));
      decorations.Add(new Sprite(atlas, new Rectangle(32 * 15, 32 * 24, 32 * 1, 32 * 2), Vector2.Zero));
      decorations.Add(new Sprite(atlas, new Rectangle(32 * 19, 32 * 15, 32 * 1, 32 * 3), Vector2.Zero));
      decorations.Add(new Sprite(atlas, new Rectangle(32 * 11, 32 * 7, 32 * 1, 32 * 1), Vector2.Zero));
      decorations.Add(new Sprite(atlas, new Rectangle(32 * 1, 1024 - 32, 32 * 1, 32 * 1), Vector2.Zero));
      // decorations.Add(new Sprite(atlas, new Rectangle(1024 - 32 * 3, 1024 - 32 * 4, 32 * 3, 32 * 4), Vector2.Zero));
      // decorations.Add(new Sprite(atlas, new Rectangle(1024 - 32 * 3, 1024 - 32 * 4, 32 * 3, 32 * 4), Vector2.Zero));
      var player = CreateEntity("player", new Vector2(Screen.Width / 2, Screen.Height / 2));
      player.AddComponent(new PlayerController());

      var followCamera = new FollowCamera(player);
      followCamera.FollowLerp = 0.4f;
      Camera.Entity.AddComponent(followCamera);

      CreateTrees(decorations.ToArray());
    }

    private void CreateTrees(Sprite[] sprites)
    {
      int numTrees = 10000;
      float maxRange = 100;
      float multiplier = 32f;

      for (int i = 0; i < numTrees; i++)
      {
        float x = (int)(Random.MinusOneToOne() * maxRange) * multiplier;
        float y = (int)(Random.MinusOneToOne() * maxRange) * multiplier;
        var position = new Vector2(x, y);
        var newTree = CreateEntity("tree", position);
        var newSpriteRenderer = new SpriteRenderer(sprites[Random.Range(0, sprites.Length)]);
        newTree.AddComponent(newSpriteRenderer);
        newSpriteRenderer.RenderLayer = -(int)newSpriteRenderer.Bounds.Bottom;
      }
    }
  }
}
