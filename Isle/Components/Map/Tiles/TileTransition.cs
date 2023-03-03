using System.Collections.Generic;
using Nez.Textures;

namespace Isle
{
  struct TileTransition
  {
    Sprite[] SingleSprites { get; }
    Sprite[] InnerSprites { get; }
    Sprite[] OuterSprites { get; }

    public TileTransition(Sprite[] singleSprites, Sprite[] innerSprites, Sprite[] outerSprites)
    {
      SingleSprites = singleSprites;
      InnerSprites = innerSprites;
      OuterSprites = outerSprites;
    }
  }
}