using Nez;
using System.Collections.Generic;
using System.Threading.Tasks;
using Nez.Textures;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Isle
{
  class MapRenderer : RenderableComponent
  {
    public Map Map { get; }

    public override float Width => (Map.Width - 1) * Map.TileSize;
    public override float Height => (Map.Height - 1) * Map.TileSize;

    private int extra = 5;

    float scaleX, scaleY;

    public MapRenderer(Map map)
    {
      Map = map;
    }

    public override void Initialize()
    {
      base.Initialize();
      scaleX = Map.TileSize * Transform.Scale.X;
      scaleY = Map.TileSize * Transform.Scale.Y;
    }

    private (int left, int right, int top, int bottom) GetVisibleTileRange(Camera camera)
    {
      int mapLeft = Map.WorldToTilePositionX(camera.Bounds.Left);
      int mapRight = Map.WorldToTilePositionX(camera.Bounds.Right);
      int mapTop = Map.WorldToTilePositionY(camera.Bounds.Top);
      int mapBottom = Map.WorldToTilePositionY(camera.Bounds.Bottom);

      int left = Math.Max(mapLeft - extra, 0);
      int right = Math.Min(mapRight + extra, Map.Width - 1);
      int top = Math.Max(mapTop - extra, 0);
      int bottom = Math.Min(mapBottom + extra, Map.Height - 1);

      return (left, right, top, bottom);
    }

    public override void Render(Batcher batcher, Camera camera)
    {
      (int left, int right, int top, int bottom) = GetVisibleTileRange(camera);

      for (int y = top; y < bottom; y++)
      {
        for (int x = left; x < right; x++)
        {
          batcher.Draw(Map.Tiles[Map.TileValues[x, y]].Sprite, new Vector2(x * scaleX, y * scaleY), Color.White, 0f, Vector2.Zero, Transform.Scale, SpriteEffects.None, LayerDepth);
        }
      }
    }

    private Sprite getSprite(Tile[] tileBlock)
    {
      return null;
    }
  }
}