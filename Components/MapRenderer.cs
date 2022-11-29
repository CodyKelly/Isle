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

    public override float Width => Map.Width * 32;
    public override float Height => Map.Height * 32;

    private int extra = 5;

    public Dictionary<int, Sprite> TileAtlas { get; }

    public MapRenderer(Map map, Dictionary<int, Sprite> tileAtlas)
    {
      Map = map;
      TileAtlas = tileAtlas;
    }

    public override void Render(Batcher batcher, Camera camera)
    {
      int mapLeft = Map.WorldToTilePositionX(camera.Bounds.Left);
      int mapRight = Map.WorldToTilePositionX(camera.Bounds.Right);
      int mapTop = Map.WorldToTilePositionY(camera.Bounds.Top);
      int mapBottom = Map.WorldToTilePositionY(camera.Bounds.Bottom);

      int left = mapLeft - extra >= 0 ? mapLeft - extra : 0;
      int right = mapRight + extra < Map.Width ? mapRight + extra : Map.Width - 1;
      int top = mapTop - extra >= 0 ? mapTop - extra : 0;
      int bottom = mapBottom + extra < Map.Height ? mapBottom + extra : Map.Height - 1;

      float scale = Entity.LocalScale.X;

      for (int y = top; y < bottom; y++)
      {
        for (int x = left; x < right; x++)
        {
          batcher.Draw(TileAtlas[Map.Tiles[x, y]], new Vector2(x * 32 * scale, y * 32 * scale), Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, LayerDepth);
        }
      }
    }
  }
}