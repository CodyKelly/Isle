using Nez;
using Nez.UI;
using Nez.Sprites;
using Nez.Textures;
using Nez.BitmapFonts;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Diagnostics;
using System;

namespace Isle
{
  class BasicScene : Scene
  {
    public Map Map { get; private set; }
    public Entity Player { get; private set; }
    public override void Initialize()
    {
      base.Initialize();

      AddRenderer(new RenderLayerExcludeRenderer(0, new int[] { RenderLayers.SCREEN_SPACE_LAYER }));
      AddRenderer(new ScreenSpaceRenderer(1, new int[] { RenderLayers.SCREEN_SPACE_LAYER }));
      // Screen.SetSize(1024, 720);

      Dictionary<int, Sprite> tileAtlas = new Dictionary<int, Sprite>();
      var atlas = Content.LoadTexture(Nez.Content.Textures.Terrain_atlaspng);
      tileAtlas.Add(0, new Sprite(atlas, new Rectangle(320, 401, 32, 32), Vector2.Zero));
      tileAtlas.Add(1, new Sprite(atlas, new Rectangle(32 * 21, 32 * 5, 32, 32), Vector2.Zero));
      tileAtlas.Add(2, new Sprite(atlas, new Rectangle(32 * 22, 32 * 5, 32, 32), Vector2.Zero));
      tileAtlas.Add(3, new Sprite(atlas, new Rectangle(32 * 23, 32 * 5, 32, 32), Vector2.Zero));
      tileAtlas.Add(4, new Sprite(atlas, new Rectangle(32 * 21, 32 * 11, 32, 32), Vector2.Zero));
      tileAtlas.Add(5, new Sprite(atlas, new Rectangle(32 * 22, 32 * 11, 32, 32), Vector2.Zero));
      tileAtlas.Add(6, new Sprite(atlas, new Rectangle(32 * 23, 32 * 11, 32, 32), Vector2.Zero));
      tileAtlas.Add(7, new Sprite(atlas, new Rectangle(32 * 7, 32 * 12, 32, 32), Vector2.Zero));

      TileTransition grassToWater = new TileTransition(
        new Sprite[] { new Sprite(atlas, new Rectangle(32 * 22, 32 * 3, 32, 32), Vector2.Zero) },
        new Sprite[] { new Sprite(atlas, new Rectangle(32 * 22, 32 * 3, 32, 32), Vector2.Zero) },
        new Sprite[] { new Sprite(atlas, new Rectangle(32 * 22, 32 * 3, 32, 32), Vector2.Zero) }
      );

      Tile grassTile = new Tile(
        "grass",
        0,
        .55f,
        1f,
        new Sprite(atlas, new Rectangle(32 * 22, 32 * 3, 32, 32), Vector2.Zero),
        null
        );

      Tile waterTile = new Tile(
        "water",
        1,
        0f,
        .45f,
        new Sprite(atlas, new Rectangle(32 * 7, 32 * 12, 32, 32), Vector2.Zero),
        null
      );

      Tile sandTile = new Tile(
        "sand",
        1,
        .45f,
        .55f,
        new Sprite(atlas, new Rectangle(128, 384, 32, 32), Vector2.Zero),
        null
      );


      var mapEntity = CreateEntity("Map");
      Map = mapEntity.AddComponent(new Map(1000, 1000, new Tile[] { waterTile, grassTile, sandTile }));
      Stopwatch stopwatch = new Stopwatch();

      stopwatch.Start();

      Map.Generate();

      stopwatch.Stop();

      Console.WriteLine("Time elapsed: {0}", stopwatch.Elapsed);
      mapEntity.Scale = Vector2.One * 2f;
      var mapRenderer = mapEntity.AddComponent(new MapRenderer(Map));
      mapRenderer.AddComponent(new LightingSpinner());
      mapEntity.AddComponent(new CameraBounds(mapRenderer.Bounds));
      mapRenderer.RenderLayer = int.MaxValue;

      ClearColor = Color.Black;
      List<Sprite> decorations = new List<Sprite>
      {
        new Sprite(atlas, new Rectangle(1024 - 32 * 3, 1024 - 32 * 4, 32 * 3, 32 * 4), Vector2.Zero),
        new Sprite(atlas, new Rectangle(32 * 15, 32 * 24, 32 * 1, 32 * 2), Vector2.Zero),
        new Sprite(atlas, new Rectangle(32 * 19, 32 * 15, 32 * 1, 32 * 3), Vector2.Zero),
        new Sprite(atlas, new Rectangle(32 * 11, 32 * 7, 32 * 1, 32 * 1), Vector2.Zero),
        new Sprite(atlas, new Rectangle(32 * 1, 1024 - 32, 32 * 1, 32 * 1), Vector2.Zero)
      };
      // decorations.Add(new Sprite(atlas, new Rectangle(1024 - 32 * 3, 1024 - 32 * 4, 32 * 3, 32 * 4), Vector2.Zero));
      // decorations.Add(new Sprite(atlas, new Rectangle(1024 - 32 * 3, 1024 - 32 * 4, 32 * 3, 32 * 4), Vector2.Zero));

      // CreateTrees(decorations.ToArray());

      var player = AddEntity(new PlayerEntity());
      var followCamera = new FollowCamera(player);
      Camera.Entity.AddComponent(followCamera);
      Player = player;

      var building = AddEntity(new BuildingEntity());
      building.SetPosition(Map.WorldWidth / 2, Map.WorldHeight / 2);

      // Camera.Entity.AddComponent(new CameraController(800f, 20000f));

      Camera.Entity.SetPosition(Map.WorldWidth / 2f, Map.WorldHeight / 2f);
      Camera.Entity.AddComponent(new ScrollZoom(Camera));
      Camera.Entity.AddComponent(new ClickToExplode());
      Camera.Entity.AddComponent(new GenerateNewMap(Map));
      Camera.Entity.UpdateOrder = int.MaxValue;
      Camera.MinimumZoom = .05f;
      Camera.MaximumZoom = 1.5f;
      Camera.Zoom = -1f;
      Camera.AddComponent(new SelectionManager().SetRenderLayer(RenderLayers.SCREEN_SPACE_LAYER));

      // CreateMobs(Map);
    }

    private void CreateMobs(Map map)
    {
      int mobsWidth = 20;
      int mobsHeight = 20;
      int spacing = 1;
      int width = 16 * 3;
      Vector2 initialPos = new Vector2(Map.WorldWidth / 2 - (mobsWidth * 16 * 3 + mobsWidth * spacing) / 2, Map.WorldHeight / 2 - (mobsHeight * 16 * 3 + mobsHeight * spacing) / 2);//0);//
      for (int y = 0; y < mobsHeight; y++)
      {
        for (int x = 0; x < mobsWidth; x++)
        {
          var newEnemy = AddEntity(Pool<GruntEntity>.Obtain());
          newEnemy.SetPosition(new Vector2(initialPos.X + x * width + spacing * x, initialPos.Y + y * width + spacing * y));
          newEnemy.SetEnabled(true);
        }
      }
    }

    private void CreateTrees(Sprite[] sprites)
    {
      int numTrees = 1000;
      float maxRange = 200;
      float multiplier = 64f;
      float spawnWidth = maxRange * multiplier;

      for (int i = 0; i < numTrees; i++)
      {
        float x = (int)(Nez.Random.NextFloat() * maxRange) * multiplier;
        float y = (int)(Nez.Random.NextFloat() * maxRange) * multiplier;
        var position = new Vector2(x, y);
        var newTree = CreateEntity("tree", position);
        newTree.Scale = Vector2.One * 2f;
        var newSpriteRenderer = new SpriteRenderer(sprites[Nez.Random.Range(0, sprites.Length)]);
        newTree.AddComponent(newSpriteRenderer);
        var renderOrder = -(int)newSpriteRenderer.Bounds.Bottom;
        newSpriteRenderer.RenderLayer = renderOrder;
      }
    }
  }
}
