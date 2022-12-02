using Nez;

namespace NewProject
{
  class GenerateNewMap : Component, IUpdatable
  {
    private Map _map;
    public GenerateNewMap(Map map) => _map = map;
    public void Update()
    {
      if (Input.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.G))
      {
        _map.Generate();
      }
    }
  }
}