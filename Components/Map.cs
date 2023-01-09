using Nez;
using Microsoft.Xna.Framework;

namespace NewProject
{
  class Map : Component
  {
    public Map(int width, int height, Tile[] tiles)
    {
      Width = width;
      Height = height;
      Tiles = tiles;
    }

    public int Width { get; }
    public int Height { get; }

    public Tile[] Tiles { get; }

    public int TileSize { get; } = 32;

    public float[,] RawValues { get; set; }

    public float Scale { get; set; } = 1f;

    private FastNoiseLite _noise = new FastNoiseLite();

    public void Generate()
    {
      _noise.SetNoiseType(FastNoiseLite.NoiseType.Perlin);
      // _noise.SetFrequency(0.1f);
      _noise.SetSeed(Random.NextInt(1000000));
      RawValues = new float[Width, Height];
      for (int y = 0; y < Height; y++)
      {
        for (int x = 0; x < Width; x++)
        {
          RawValues[x, y] = _noise.GetNoise(x * Scale, y * Scale);
        }
      }
    }

    public Tile GetTile(int x, int y)
    {
      if (x < 0 || x >= Width || y < 0 || y >= Width)
      {
        return null;
      }
      float value = RawValues[x, y];
      foreach (Tile tile in Tiles)
      {
        if (value >= tile.StartRange && value < tile.EndRange)
        {
          return tile;
        }
      }
      return null;
    }

    public int WorldToTilePositionX(float x) => (int)(x / TileSize / Entity.Scale.X);
    public int WorldToTilePositionY(float y) => (int)(y / TileSize / Entity.Scale.Y);
    public Tile GetTileAtWorldPos(float x, float y) => GetTile(WorldToTilePositionX(x), WorldToTilePositionY(y));
  }
}