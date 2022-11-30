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

      AddRenderer(new RenderLayerExcludeRenderer(0, new int[] { RenderLayers.SCREEN_SPACE_LAYER }));
      AddRenderer(new ScreenSpaceRenderer(1, new int[] { RenderLayers.SCREEN_SPACE_LAYER }));
      // SetDefaultDesignResolution(1024, 1024, SceneResolutionPolicy.BestFit);
      Screen.SetSize(1024, 720);

      var map = new Map(200, 200);
      map.Generate();
      var mapEntity = CreateEntity("Map");
      mapEntity.Scale = Vector2.One * 2f;
      Dictionary<int, Sprite> tileAtlas = new Dictionary<int, Sprite>();
      var atlas = Content.LoadTexture(Nez.Content.Textures.Terrain_atlaspng);
      tileAtlas.Add(0, new Sprite(atlas, new Rectangle(32 * 22, 32 * 3, 32, 32), Vector2.Zero));
      tileAtlas.Add(1, new Sprite(atlas, new Rectangle(32 * 21, 32 * 5, 32, 32), Vector2.Zero));
      tileAtlas.Add(2, new Sprite(atlas, new Rectangle(32 * 22, 32 * 5, 32, 32), Vector2.Zero));
      tileAtlas.Add(3, new Sprite(atlas, new Rectangle(32 * 23, 32 * 5, 32, 32), Vector2.Zero));
      tileAtlas.Add(4, new Sprite(atlas, new Rectangle(32 * 21, 32 * 11, 32, 32), Vector2.Zero));
      tileAtlas.Add(5, new Sprite(atlas, new Rectangle(32 * 22, 32 * 11, 32, 32), Vector2.Zero));
      tileAtlas.Add(6, new Sprite(atlas, new Rectangle(32 * 23, 32 * 11, 32, 32), Vector2.Zero));
      var mapRenderer = mapEntity.AddComponent(new MapRenderer(map, tileAtlas));
      mapEntity.AddComponent(new CameraBounds(mapRenderer.Bounds));
      mapRenderer.RenderLayer = int.MaxValue;

      ClearColor = Color.Black;
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

      CreateTrees(decorations.ToArray());

      var followCamera = new FollowCamera(player);
      followCamera.FollowLerp = 0.08f;
      Camera.Entity.AddComponent(followCamera);
      Camera.Entity.AddComponent(new ScrollZoom(Camera));
      Camera.Entity.UpdateOrder = int.MaxValue;
      Camera.MinimumZoom = .25f;
      Camera.MaximumZoom = 2f;

      Camera.AddComponent(new SelectionManager().SetRenderLayer(RenderLayers.SCREEN_SPACE_LAYER));
    }

    private void CreateTrees(Sprite[] sprites)
    {
      int numTrees = 1000;
      float maxRange = 200;
      float multiplier = 64f;
      float spawnWidth = maxRange * multiplier;

      for (int i = 0; i < numTrees; i++)
      {
        float x = (int)(Random.NextFloat() * maxRange) * multiplier;
        float y = (int)(Random.NextFloat() * maxRange) * multiplier;
        var position = new Vector2(x, y);
        var newTree = CreateEntity("tree", position);
        newTree.Scale = Vector2.One * 2f;
        var newSpriteRenderer = new SpriteRenderer(sprites[Random.Range(0, sprites.Length)]);
        newTree.AddComponent(newSpriteRenderer);
        var renderOrder = -(int)newSpriteRenderer.Bounds.Bottom;
        newSpriteRenderer.RenderLayer = renderOrder;
      }
    }
  }
}
