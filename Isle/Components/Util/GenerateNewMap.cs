using Nez;

namespace Isle
{
  class GenerateNewMap : Component, IUpdatable
  {
    private Map _map;
    public GenerateNewMap(Map map) => _map = map;
    public void Update()
    {
      if (Input.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.G))
      {
        _map.Seed = Random.NextInt(10000000);
        _map.Generate();
      }
    }
  }
}