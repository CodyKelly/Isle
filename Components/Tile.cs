using System.Collections.Generic;
using Nez.Textures;

namespace NewProject
{
  class Tile
  {
    int _layer;

    Dictionary<string, TileTransition> _transitions;

    public string Name { get; }
    public Sprite Sprite { get; }
    public float StartRange { get; }
    public float EndRange { get; }

    public Tile(string name, int layer, float startRange, float endRange, Sprite sprite, Dictionary<string, TileTransition> transitions)
    {
      Name = name;
      _layer = layer;
      Sprite = sprite;
      _transitions = transitions;
      StartRange = startRange;
      EndRange = endRange;
    }
  }
}