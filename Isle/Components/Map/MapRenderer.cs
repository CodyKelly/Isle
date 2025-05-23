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

    public Vector2 LightDirection { get; set; } = new Vector2(0, 1);

    private int extra = 5;

    float scaleX, scaleY;
    Tile[] tileCache;

    public MapRenderer(Map map)
    {
      Map = map;
    }

    public override void Initialize()
    {
      base.Initialize();
      scaleX = Map.TileSize * Transform.Scale.X;
      scaleY = Map.TileSize * Transform.Scale.Y;
      tileCache = new Tile[Map.Width * Map.Height];
      for (int i = 0; i < tileCache.Length; i++)
      {
        tileCache[i] = Map.Tiles[Map.TileValues[i % Map.Width, i / Map.Width]];
      }
    }

    private (int left, int right, int top, int bottom) GetVisibleTileRange(Camera camera)
    {
      int mapLeft = (int)Map.WorldToTilePositionX(camera.Bounds.Left);
      int mapRight = (int)Map.WorldToTilePositionX(camera.Bounds.Right);
      int mapTop = (int)Map.WorldToTilePositionY(camera.Bounds.Top);
      int mapBottom = (int)Map.WorldToTilePositionY(camera.Bounds.Bottom);

      int left = Math.Max(mapLeft - extra, 0);
      int right = Math.Min(mapRight + extra, Map.Width - 1);
      int top = Math.Max(mapTop - extra, 0);
      int bottom = Math.Min(mapBottom + extra, Map.Height - 1);

      return (left, right, top, bottom);
    }


    public void OldRender(Batcher batcher, Camera camera)
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
          batcher.Draw(tile.Sprite, new Vector2(x * Map.TileSize * Transform.Scale.X, y * Map.TileSize * Transform.Scale.Y), Color.White * tileValue, 0f, Vector2.Zero, Transform.Scale, SpriteEffects.None, LayerDepth);
          // batcher.DrawLine(new Vector2(x * Map.TileSize * Transform.Scale.X, 0), new Vector2(x * Map.TileSize * Transform.Scale.X, Map.Height * Map.TileSize * Transform.Scale.Y), Color.Yellow, 5f);
        }
        // batcher.DrawLine(new Vector2(0, y * Map.TileSize * Transform.Scale.Y), new Vector2(Map.Width * Map.TileSize * Transform.Scale.X, y * Map.TileSize * Transform.Scale.Y), Color.Yellow, 5f);
      }
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
          Tile tile = Map.Tiles[Map.TileValues[x, y]];
          float range = tile.EndRange - tile.StartRange;
          float tileValue;

          // Calculate the gradient of the tile
          Vector2 gradient;

          if (x > 0 && x < Map.Width - 1 && y > 0 && y < Map.Height - 1)
          {
            gradient = new Vector2(
                Map.RawValues[x + 1, y] - Map.RawValues[x - 1, y],
                Map.RawValues[x, y + 1] - Map.RawValues[x, y - 1]
            );
          }
          else
          {
            gradient = Vector2.Zero; // Use a default value for the gradient if x or y is on the edge of the map
          }


          // Normalize the gradient
          gradient = Vector2.Normalize(gradient);

          // Calculate the dot product of the gradient and the light direction
          float dotProduct = Vector2.Dot(gradient, LightDirection);

          // Calculate the brightness of the tile
          tileValue = 0.5f - dotProduct * 0.075f;

          if (tile.Name == "water")
          {
            tileValue /= 4;
            tileValue += (Map.RawValues[x, y] - tile.StartRange) / (tile.EndRange - tile.StartRange) - 0.15f;
          }

          //float tileValue = Map.RawValues[x, y] - tile.StartRange + 0.5f;
          batcher.Draw(tile.Sprite, new Vector2(x * scaleX, y * scaleY), Color.White * tileValue, 0f, Vector2.Zero, Transform.Scale, SpriteEffects.None, LayerDepth);

        }
      }
    }

    private Sprite getSprite(Tile[] tileBlock)
    {
      return null;
    }
  }
}