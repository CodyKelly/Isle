using Nez;
using Nez.Sprites;
using Nez.Textures;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NewProject
{
  class AnimationController : Component, IUpdatable
  {
    SpriteAnimator _animator;
    Texture2D _sideAtlas, _downAtlas, _upAtlas;
    float _framerate;
    Controller _controller;

    public AnimationController(ref Texture2D sideAtlas, ref Texture2D downAtlas, ref Texture2D upAtlas, float framerate)
    {
      _sideAtlas = sideAtlas;
      _downAtlas = downAtlas;
      _upAtlas = upAtlas;
      _framerate = framerate;
    }

    public void Update()
    {
      var moveDir = _controller.MoveDir;

      if (moveDir != Vector2.Zero)
      {
        _animator.FlipX = false;

        var animation = "down";

        if (moveDir.X < 0)
        {
          animation = "right";
          _animator.FlipX = true;
        }
        else if (moveDir.X > 0)
          animation = "right";

        if (moveDir.Y < 0)
          animation = "up";
        else if (moveDir.Y > 0)
          animation = "down";

        if (!_animator.IsAnimationActive(animation))
          _animator.Play(animation);
        else
          _animator.UnPause();
      }
      else
      {
        _animator.Pause();
      }
    }

    public override void OnAddedToEntity()
    {
      _controller = Entity.GetComponent<Controller>();

      _animator = Entity.AddComponent<SpriteAnimator>();
      _animator.AddAnimation("right", new SpriteAnimation(Sprite.SpritesFromAtlas(_sideAtlas, 16, 16).ToArray(), _framerate));
      _animator.AddAnimation("down", new SpriteAnimation(Sprite.SpritesFromAtlas(_downAtlas, 16, 16).ToArray(), _framerate));
      _animator.AddAnimation("up", new SpriteAnimation(Sprite.SpritesFromAtlas(_upAtlas, 16, 16).ToArray(), _framerate));
      _animator.Play("down");
    }
  }
}