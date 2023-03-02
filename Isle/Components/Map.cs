using Nez;
using Microsoft.Xna.Framework;

namespace Isle
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

    public float WorldWidth { get { return worldWidth.HasValue ? worldWidth.Value : (worldWidth = (float?)((float)Width * (float)TileSize * Entity.Scale.X)).Value; } }
    public float WorldHeight { get { return worldHeight.HasValue ? worldHeight.Value : (worldHeight = (float?)((float)Height * (float)TileSize * Entity.Scale.Y)).Value; } }
    float? worldWidth;
    float? worldHeight;

    public Tile[] Tiles { get; }

    public int TileSize { get; } = 32;

    public Point HighestPoint { get { return highestPoint; } }
    private Point highestPoint = new Point();

    public float[,] RawValues { get; set; }
    public int[,] TileValues { get; set; }

    public int Octaves { get; set; } = 10;
    public int Seed { get; set; }

    private FastNoiseLite _noise = new FastNoiseLite();

    public override void OnAddedToEntity()
    {
      Seed = Random.NextInt(1000000);
    }

    public void Generate()
    {
      float highestPointValue = -1f;
      _noise.SetNoiseType(FastNoiseLite.NoiseType.Perlin);
      // _noise.SetFrequency(0.1f);
      _noise.SetSeed(Seed);
      float sqrt2 = Mathf.Sqrt(2);
      RawValues = new float[Width, Height];
      TileValues = new int[Width, Height];
      for (int y = 0; y < Height; y++)
      {
        for (int x = 0; x < Width; x++)
        {
          float nx = 2f * (float)x / (float)Width - 1f;
          float ny = 2f * (float)y / (float)Height - 1f;
          float dist = MathHelper.Min(1f, (nx * nx + ny * ny) / sqrt2);
          float value = 0;
          float multiplierSum = 0;
          for (int i = 1; i <= Octaves; i++)
          {
            float multiplier = 1f / (float)i;
            multiplierSum += multiplier;
            _noise.SetSeed(Seed + i);
            value += multiplier * ((_noise.GetNoise((float)x * (float)i, (float)y * (float)i)) + 1) / 2f;
          }
          value /= multiplierSum;
          value = MathHelper.Max(0f, (value - (-Mathf.Cos(dist * MathHelper.TwoPi) + 1) / 2));
          RawValues[x, y] = value;
          for (int i = 0; i < Tiles.Length; i++)
          {
            if (value >= Tiles[i].StartRange && value < Tiles[i].EndRange)
            {
              TileValues[x, y] = i;
            }
          }
          if (value > highestPointValue)
          {
            highestPointValue = value;
            highestPoint.X = x;
            highestPoint.Y = y;
          }
        }
      }
    }

    public Tile GetTile(int x, int y)
    {
      if (x < 0 || x >= Width || y < 0 || y >= Height)
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
    public Vector2 TileToWorldPosition(Point p) => new Vector2(p.X * TileSize * Entity.Scale.X, p.Y * TileSize * Entity.Scale.Y);
    public Tile GetTileAtWorldPos(float x, float y) => GetTile(WorldToTilePositionX(x), WorldToTilePositionY(y));
  }
}