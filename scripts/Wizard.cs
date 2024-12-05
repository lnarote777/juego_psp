using Godot;

public partial class Wizard : CharacterBody2D
{
	[Export] public float Speed = 100f;
	private float _maxHealth = 10f;
	private float _currentHealth;
	public const float JumpVelocity = -500.0f;
	private float _acceleration = .25f;
	private float _friction = .1f;
	private int _gravity = 4000;
	private int _jumpHeight = 300;
	private int _dashSpeed = 1000;


	AnimatedSprite2D _animatedSprite;
	CollisionShape2D _collision;

    public override void _Ready()
    {
        
        // Inicializa el AnimatedSprite2D obteniéndolo del nodo
        _animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		_collision = GetNode<CollisionShape2D>("CollisionShape2D");
		_currentHealth = _maxHealth;
    }

    public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = new Vector2();
		int direction = 0;

		if (Input.IsActionPressed("move_left")){
			direction -= 1;
		}
		if (Input.IsActionPressed("move_right")){
			direction += 1;
		}
		if (direction != 0){
			velocity.X = Mathf.Lerp(velocity.X, direction * Speed, _acceleration);
		}else{
			velocity.X = Mathf.Lerp(velocity.X, 0, _friction);
		}
		if (Input.IsActionJustPressed("jump")){

			if (IsOnFloor()){
				_animatedSprite.Play("jump");
				velocity.Y -= _jumpHeight;
			}
		}

		if (Input.IsActionJustPressed("dash")){
			if (Input.IsActionPressed("move_left")){
				velocity.X = _dashSpeed;
			}
			if (Input.IsActionPressed("move_right")){
				velocity.X = _dashSpeed;
			}
		}
		
		velocity.Y += (float)(_gravity * delta);
 		
		MoveAndSlide();
	}
}
