using Nez;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nez.Sprites;
using Nez.Textures;
using Nez.ImGuiTools;

namespace NewProject
{
  class PlayerController : Component, IUpdatable
  {

    VirtualButton _fireInput;
    [Inspectable]
    VirtualAxis _xAxisInput;
    VirtualAxis _yAxisInput;
    SubpixelVector2 _subpixelV2 = new SubpixelVector2();

    [Inspectable]
    float speed = 300f;

    Mover _mover;
    SpriteAnimator _animator;
    BoxCollider _boxCollider;

    public void Update()
    {
      var moveDir = new Vector2(_xAxisInput.Value, _yAxisInput.Value);

      var mouseWheelDelta = Input.MouseWheelDelta;
      // float speed = -(float)Math.Log10(_camera.RawZoom) * ZoomSpeed;
      if (mouseWheelDelta != 0)
      {

      }

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

        var movement = moveDir * speed * Time.DeltaTime;
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
      SetupInput();
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

    void SetupInput()
    {
      // setup input for shooting a fireball. we will allow z on the keyboard or a on the gamepad
      _fireInput = new VirtualButton();
      _fireInput.Nodes.Add(new VirtualButton.KeyboardKey(Keys.Z));
      _fireInput.Nodes.Add(new VirtualButton.GamePadButton(0, Buttons.A));

      // horizontal input from dpad, left stick or keyboard left/right
      _xAxisInput = new VirtualAxis();
      _xAxisInput.Nodes.Add(new VirtualAxis.GamePadDpadLeftRight());
      _xAxisInput.Nodes.Add(new VirtualAxis.GamePadLeftStickX());
      _xAxisInput.Nodes.Add(new VirtualAxis.KeyboardKeys(VirtualInput.OverlapBehavior.TakeNewer, Keys.Left, Keys.Right));

      // vertical input from dpad, left stick or keyboard up/down
      _yAxisInput = new VirtualAxis();
      _yAxisInput.Nodes.Add(new VirtualAxis.GamePadDpadUpDown());
      _yAxisInput.Nodes.Add(new VirtualAxis.GamePadLeftStickY());
      _yAxisInput.Nodes.Add(new VirtualAxis.KeyboardKeys(VirtualInput.OverlapBehavior.TakeNewer, Keys.Up, Keys.Down));
    }
  }
}