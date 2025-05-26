using Nez;
using Microsoft.Xna.Framework;
using System.Threading.Tasks;

namespace Isle
{
  class Map : Component
  {
    public Map(int width, int height, Tile[] tiles, int seed = 0)
    {
      Width = width;
      Height = height;
      Seed = seed;
      Tiles = tiles;
    }

    public int Width { get; }
    public int Height { get; }

    public float WorldWidth { get { return worldWidth ?? (worldWidth = Width * TileSize * Entity.Scale.X).Value; } }
    public float WorldHeight { get { return worldHeight ?? (worldHeight = Height * TileSize * Entity.Scale.Y).Value; } }
    protected float? worldWidth;
    protected float? worldHeight;

    public Tile[] Tiles { get; }
    public int TileSize { get; } = 32;

    public Point HighestPoint { get { return highestPoint; } }
    protected Point highestPoint = new Point();

    public float[,] RawValues { get; protected set; }
    public int[,] TileValues { get; protected set; }

    public int Octaves { get; set; } = 7;
    public int Seed { get; set; }

    protected FastNoiseLite _noise = new FastNoiseLite();

    public override void OnAddedToEntity()
    {
      Seed = Nez.Random.NextInt(1000000);
    }

    public Tile GetTile(int x, int y)
    {
      return Tiles[TileValues[x, y]];
    }

    virtual public void Generate()
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
          float dist = 0;//MathHelper.Min(1f, (nx * nx + ny * ny) / sqrt2);
          float value = 0;
          float multiplierSum = 0;
          for (int i = 1; i <= Octaves; i++)
          {
            float multiplier = 1f / (float)i;
            multiplierSum += multiplier;
            // _noise.SetSeed(Seed + i);
            value += multiplier * ((_noise.GetNoise((float)x * (float)i, (float)y * (float)i)) + 1) / 2f;
          }
          value /= multiplierSum;
          // value = MathHelper.Max(0f, (value - (-Mathf.Cos(dist * MathHelper.TwoPi) + 1) / 2));
          RawValues[x, y] = value;
          // var value = 1f;
          // RawValues[x, y] = value;
          TileValues[x, y] = TileValueFromRawValue(value);
          if (value > highestPointValue)
          {
            highestPointValue = value;
            highestPoint.X = x;
            highestPoint.Y = y;
          }
        }
      }
    }

    protected int TileValueFromRawValue(float rawValue)
    {
      for (int i = 0; i < Tiles.Length; i++)
      {
        if (rawValue >= Tiles[i].StartRange && rawValue <= Tiles[i].EndRange)
        {
          return i;
        }
      }
      throw new System.ArgumentOutOfRangeException("Can't convert raw value to tile");
    }

    public void SetValue(int x, int y, float value)
    {
      if (x < 0 || x >= Width || y < 0 || y >= Height)
      {
        throw new System.ArgumentOutOfRangeException("Pos is outside of map");
      }

      value = Mathf.Clamp01(value);
      RawValues[x, y] = value;
      TileValues[x, y] = TileValueFromRawValue(value);
    }

    public int WorldToTilePositionX(float x) => (int)(x / TileSize / Entity.Scale.X);
    public int WorldToTilePositionY(float y) => (int)(y / TileSize / Entity.Scale.Y);
    public Vector2 TileToWorldPosition(Point p) => new Vector2(p.X * TileSize * Entity.Scale.X, p.Y * TileSize * Entity.Scale.Y);
    public Tile GetTileAtWorldPos(float x, float y) => GetTile(WorldToTilePositionX(x), WorldToTilePositionY(y));
  }
}