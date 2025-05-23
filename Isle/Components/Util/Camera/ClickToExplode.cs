using Nez;
using Microsoft.Xna.Framework;

namespace Isle
{
    class ClickToExplode : Component, IUpdatable
    {
        float radius = 300;
        float power = 50;
        float depth = .1f;
        float damage = 2;
        float explodeRate = .05f;
        float lastExplode = 0;
        bool makeItRain = false;
        Explosive _explosive;

        public void Update()
        {
            if (Input.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.X))//Input.LeftMouseButtonDown)
            {
                Vector2 mousePos = Entity.Scene.Camera.MouseToWorldPoint();
                _explosive.Explode(mousePos, radius, power, depth, damage);
            }
            if (Input.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.K))
            {
                makeItRain = !makeItRain;
            }

            if (makeItRain)
            {
                if (Time.TotalTime > lastExplode + explodeRate)
                {
                    lastExplode = Time.TotalTime;
                    float x = Random.NextFloat() * ((BasicScene)Entity.Scene).Map.WorldWidth;
                    float y = Random.NextFloat() * ((BasicScene)Entity.Scene).Map.WorldHeight;
                    _explosive.Explode(new Vector2(x, y), radius * 2, power * 1.5f, depth, damage);
                }
            }
        }

        public override void OnAddedToEntity()
        {
            base.OnAddedToEntity();
            _explosive = Entity.AddComponent(new Explosive());
        }
    }
}