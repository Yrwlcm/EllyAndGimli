using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class EllyController : PlayerControllerBase
{
	private bool _canMakeSecondJump;
	private bool _isTouchingLeftWall;
	private bool _isTouchingRightWall;
	private float _defaultGravityScale;

	private readonly Func<Vector2, float, int, bool> _isOverlappingWithWall = (Vector2 position, float radius, int layerMask)
		=> Physics2D.OverlapCircle(position, radius, layerMask) != null;

	[Header("Опции двойного прыжка")]
	[SerializeField] private float _timeToActivateDoubleJump = 0.5f;

	[Header("Связанное с коллизиями со стенами")]
	[SerializeField] private Transform _leftWallCheck;
	[SerializeField] private Transform _rightWallCheck;
	[SerializeField] private float _wallCheckRadius = 0.2f;
	[SerializeField] private LayerMask _wallLayer;
	[SerializeField] private float _wallSlidingSpeedDown = 0.1f;
	[SerializeField] private float _wallSlidingSpeedUp = 0.5f;


	protected override void Start()
	{
		base.Start();
		_defaultGravityScale = _rb.gravityScale;
		_inputSystemActions.Elly.Jump.started += context => TryDoJump(_isGrounded || _canMakeSecondJump);
	}

	protected override void Update()
	{
		_isTouchingLeftWall = _isOverlappingWithWall(_leftWallCheck.position, _wallCheckRadius, _wallLayer);
		_isTouchingRightWall = _isOverlappingWithWall(_rightWallCheck.position, _wallCheckRadius, _wallLayer);
		_rb.gravityScale = _isTouchingLeftWall || _isTouchingRightWall ? _wallSlidingSpeedDown : _defaultGravityScale;
		base.Update();
	}

	protected override void FixedUpdate()
	{
		var movement = _inputSystemActions.Elly.Move.ReadValue<Vector2>();
		var isJumpPressed = _inputSystemActions.Elly.Jump.IsPressed();
		TryDoWallClimbing(isJumpPressed);
		TryJumpingFromWall(movement.x, isJumpPressed);
		DoMove(movement.x);
		base.FixedUpdate();
	}

	protected override bool TryDoJump(bool jumpingCondition)
	{
		var hasJumped = base.TryDoJump(jumpingCondition);
		if (hasJumped)
		{
			print(1);
			StartCoroutine(nameof(JumpCoroutine));
		}
		return hasJumped;
	}

	private bool TryDoWallClimbing(bool isJumpPressed)
	{
		if (isJumpPressed && (_isTouchingLeftWall || _isTouchingRightWall))
		{
			_rb.linearVelocity = new Vector2(_rb.linearVelocity.x, (Vector2.up * _wallSlidingSpeedUp).y);
			return true;
		}
		return false;
	}

	private bool TryJumpingFromWall(float xMovement, bool isJumpPressed)
	{
		var isTouchingWalls = _isTouchingLeftWall || _isTouchingRightWall;
		if (xMovement == 0 || !isTouchingWalls || !isJumpPressed) return false;
		if (xMovement > 0 && _isTouchingLeftWall) return TryDoJump(true);
		else if (xMovement < 0 && _isTouchingRightWall) return TryDoJump(true);
		return false;
	}

	private IEnumerator JumpCoroutine()
	{
		_canMakeSecondJump = true;
		yield return new WaitForSeconds(_timeToActivateDoubleJump);
		_canMakeSecondJump = false;
	}
}