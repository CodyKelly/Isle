using Nez;
using Nez.Sprites;
using Nez.Textures;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NewProject
{
  abstract class AnimatedController : Controller
  {
    protected SpriteAnimator _animator;
    static protected Texture2D _sideAtlas, _downAtlas, _upAtlas;
    protected float _framerate;

    protected abstract void LoadTextures();

    public override void Update()
    {
      base.Update();
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

        var bounds = _collider.Bounds;
        _animator.SetRenderLayer(-(int)bounds.Bottom - (int)(bounds.Height / 2f));
      }
      else
      {
        _animator.Pause();
      }
    }

    public override void OnAddedToEntity()
    {
      base.OnAddedToEntity();

      LoadTextures();

      Entity.Scale = Vector2.One * 3f;

      _animator = Entity.AddComponent<SpriteAnimator>();
      _animator.AddAnimation("right", new SpriteAnimation(Sprite.SpritesFromAtlas(_sideAtlas, 16, 16).ToArray(), _framerate));
      _animator.AddAnimation("down", new SpriteAnimation(Sprite.SpritesFromAtlas(_downAtlas, 16, 16).ToArray(), _framerate));
      _animator.AddAnimation("up", new SpriteAnimation(Sprite.SpritesFromAtlas(_upAtlas, 16, 16).ToArray(), _framerate));
      _animator.Play("down");
    }
  }
}