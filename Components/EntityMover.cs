using Nez;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nez.Sprites;
using Nez.Textures;
using Nez.ImGuiTools;

namespace NewProject
{
  class EntityMover : Component, IUpdatable
  {
    [Inspectable]
    float speed = 750f;

    Mover _mover;
    SpriteAnimator _animator;
    BoxCollider _boxCollider;
    SubpixelVector2 _subpixelV2 = new SubpixelVector2();
    Map _map;

    public EntityMover(Map map) => _map = map;

    public Vector2 MoveDir { get; set; } = Vector2.Zero;

    public void Update()
    {
      float calcSpeed = speed;
      Tile currentTile = _map.GetTileAtWorldPos(Entity.Position.X, Entity.Position.Y);
      bool inWater = currentTile == null ? false : currentTile.Name == "water";
      if (inWater)
      {
        calcSpeed /= 3f;
      }

      if (MoveDir != Vector2.Zero)
      {
        _animator.FlipX = false;

        var animation = "down";

        if (MoveDir.X < 0)
        {
          animation = "right";
          _animator.FlipX = true;
        }
        else if (MoveDir.X > 0)
          animation = "right";

        if (MoveDir.Y < 0)
          animation = "up";
        else if (MoveDir.Y > 0)
          animation = "down";

        if (!_animator.IsAnimationActive(animation))
          _animator.Play(animation);
        else
          _animator.UnPause();

        var movement = MoveDir * calcSpeed * Time.DeltaTime;
        var bounds = _boxCollider.Bounds;
        _animator.SetRenderLayer(-(int)bounds.Bottom - (int)(bounds.Height / 2f));
        _subpixelV2.Update(ref movement);
        _mover.ApplyMovement(movement);
      }
      else
      {
        _animator.Pause();
      }
    }

    public override void OnAddedToEntity()
    {
      Entity.Scale = Vector2.One * 3f;
      _animator = Entity.AddComponent<SpriteAnimator>();
      _mover = Entity.AddComponent(new Mover());
      _boxCollider = Entity.AddComponent<BoxCollider>();
      SetupAnimations();
    }

    void SetupAnimations()
    {
      float animFramerate = 4f;
      var downAtlas = Entity.Scene.Content.LoadTexture(Nez.Content.Textures.Character.Armorlancer.Armorlancerdownpng);
      var sideAtlas = Entity.Scene.Content.LoadTexture(Nez.Content.Textures.Character.Armorlancer.Armorlancersidepng);
      var upAtlas = Entity.Scene.Content.LoadTexture(Nez.Content.Textures.Character.Armorlancer.Armorlanceruppng);
      _animator.AddAnimation("right", new SpriteAnimation(Sprite.SpritesFromAtlas(sideAtlas, 16, 16).ToArray(), animFramerate));
      _animator.AddAnimation("down", new SpriteAnimation(Sprite.SpritesFromAtlas(downAtlas, 16, 16).ToArray(), animFramerate));
      _animator.AddAnimation("up", new SpriteAnimation(Sprite.SpritesFromAtlas(upAtlas, 16, 16).ToArray(), animFramerate));
      _animator.Play("down");
    }
  }
}