using Microsoft.Xna.Framework;
using NewProject;
using Nez;

namespace NewProject
{
  class Map : Component
  {
    public int Width { get; set; } = 100;
    public int Height { get; set; } = 100;

    int[,] mapData;

    public override void OnAddedToEntity()
    {
      mapData = new int[Width, Height];
      GenerateMap();
    }

    private void GenerateMap()
    {
      for (int y = 0; y < Height; y++)
      {
        for (int x = 0; x < Width; x++)
        {
          mapData[x, y] = 1;
        }
      }
    }

    public int GetRenderLayerFromYCoordinate(int yCoordinate)
    {
      return 1;
    }
  }
}