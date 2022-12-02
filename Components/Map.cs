using Nez;
using Microsoft.Xna.Framework;

namespace NewProject
{
  class Map
  {
    public Map(int width, int height)
    {
      Width = width;
      Height = height;
    }

    public int Width { get; }
    public int Height { get; }

    public int TileSize { get; } = 32;

    public float[,] Tiles { get; set; }

    private FastNoiseLite _noise = new FastNoiseLite();

    public void Generate()
    {
      _noise.SetNoiseType(FastNoiseLite.NoiseType.Perlin);
      _noise.SetSeed(Random.NextInt(1000000));
      Tiles = new float[Width, Height];
      for (int y = 0; y < Height; y++)
      {
        for (int x = 0; x < Width; x++)
        {
          Tiles[x, y] = _noise.GetNoise(x, y);
        }
      }
    }

    public int WorldToTilePositionX(float x) => (int)(x / TileSize);
    public int WorldToTilePositionY(float y) => (int)(y / TileSize);
  }
}