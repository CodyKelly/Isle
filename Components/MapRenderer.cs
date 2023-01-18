using Nez;
using System.Collections.Generic;
using Nez.Textures;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace NewProject
{
  class MapRenderer : RenderableComponent
  {
    public Map Map { get; }

    public override float Width => (Map.Width - 1) * 32;
    public override float Height => (Map.Height - 1) * 32;

    private int extra = 5;

    public MapRenderer(Map map)
    {
      Map = map;
    }

    public override void Render(Batcher batcher, Camera camera)
    {
      int mapLeft = (int)(Map.WorldToTilePositionX(camera.Bounds.Left));
      int mapRight = (int)(Map.WorldToTilePositionX(camera.Bounds.Right));
      int mapTop = (int)(Map.WorldToTilePositionY(camera.Bounds.Top));
      int mapBottom = (int)(Map.WorldToTilePositionY(camera.Bounds.Bottom));

      int left = mapLeft - extra >= 0 ? mapLeft - extra : 0;
      int right = mapRight + extra < Map.Width ? mapRight + extra : Map.Width - 1;
      int top = mapTop - extra >= 0 ? mapTop - extra : 0;
      int bottom = mapBottom + extra < Map.Height ? mapBottom + extra : Map.Height - 1;

      for (int y = top; y < bottom; y++)
      {
        for (int x = left; x < right; x++)
        {
          // Tile[] tileBlock = new Tile[9];
          // int count = 0;
          // for (int neighborY = y - 1; neighborY < y + 1; neighborY++)
          // {
          //   for (int neighborX = x - 1; neighborX < x + 1; neighborX++)
          //   {
          //     tileBlock[count] = Map.GetTile(x, y);
          //     count++;
          //   }
          // }
          Tile tile = Map.Tiles[Map.TileValues[x, y]];
          float range = tile.EndRange - tile.StartRange;
          float tileValue = Map.RawValues[x, y] - tile.StartRange + 0.5f;
          batcher.Draw(tile.Sprite, new Vector2(x * 32 * Transform.Scale.X, y * 32 * Transform.Scale.Y), Color.White * tileValue, 0f, Vector2.Zero, Transform.Scale, SpriteEffects.None, LayerDepth);
          // batcher.DrawLine(new Vector2(x * Map.TileSize * Transform.Scale.X, 0), new Vector2(x * Map.TileSize * Transform.Scale.X, Map.Height * Map.TileSize * Transform.Scale.Y), Color.Yellow, 5f);
        }
        // batcher.DrawLine(new Vector2(0, y * Map.TileSize * Transform.Scale.Y), new Vector2(Map.Width * Map.TileSize * Transform.Scale.X, y * Map.TileSize * Transform.Scale.Y), Color.Yellow, 5f);
      }
    }

    private Sprite getSprite(Tile[] tileBlock)
    {
      return null;
    }
  }
}