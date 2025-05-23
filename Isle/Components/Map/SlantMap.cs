using Nez;
using Microsoft.Xna.Framework;

namespace Isle
{
  class SlantMap : Map
  {
    public SlantMap(int width, int height, Tile[] tiles) : base(width, height, tiles) { }

    public override void Generate()
    {
      RawValues = new float[Width, Height];
      TileValues = new int[Width, Height];

      for (int y = 0; y < Height; y++)
      {
        for (int x = 0; x < Width; x++)
        {
          // Set the raw value to decrease by one per tile across the x-axis
          float value = 1f - (float)x / (float)Width;
          RawValues[x, y] = value;
          TileValues[x, y] = TileValueFromRawValue(value);
        }
      }
    }
  }
}