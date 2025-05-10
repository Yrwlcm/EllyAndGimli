using Assets.EllyAndGimli.Constants;
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
		_inputSystemActions.Elly.Jump.started += context => TryDoJump((_isGrounded || _canMakeSecondJump) && !(_isTouchingLeftWall || _isTouchingRightWall));
	}

	protected override void Update()
	{
		_isTouchingLeftWall = _isOverlapping(_leftWallCheck.position, _wallCheckRadius, _wallLayer);
		_isTouchingRightWall = _isOverlapping(_rightWallCheck.position, _wallCheckRadius, _wallLayer);
		_rb.gravityScale = _isTouchingLeftWall || _isTouchingRightWall ? _wallSlidingSpeedDown : _defaultGravityScale;
		base.Update();
		_animator.SetBool(AnimationsConst.IsFalling, !_isGrounded && !_isTouchingLeftWall && !_isTouchingRightWall && _rb.linearVelocityY < 0);
		_animator.SetBool(AnimationsConst.IsWallHanging, _isGrounded && ((_isTouchingLeftWall && !_spriteRenderer.flipX) || (_isTouchingRightWall && _spriteRenderer.flipX)));
	}

	protected override void FixedUpdate()
	{
		var movement = _inputSystemActions.Elly.Move.ReadValue<Vector2>();
		var isJumpPressed = _inputSystemActions.Elly.Jump.IsPressed();
		DoMove(movement.x);
		var isClimbing = TryDoWallClimbing(isJumpPressed);
		_animator.SetBool(AnimationsConst.IsSliding, !isClimbing && !_isGrounded && (_isTouchingLeftWall || _isTouchingRightWall));
		TryJumpingFromWall(movement.x, isJumpPressed);
		DoMove(movement.x);
		base.FixedUpdate();
	}

	protected override bool TryDoJump(bool jumpingCondition, bool isDouble = false)
	{
		var hasJumped = base.TryDoJump(jumpingCondition, isDouble = _canMakeSecondJump);
		if (hasJumped)
		{
			StartCoroutine(nameof(JumpCoroutine));
		}
		return hasJumped;
	}

	private bool TryDoWallClimbing(bool isJumpPressed)
	{
		var climbCondition = isJumpPressed && (_isTouchingLeftWall || _isTouchingRightWall);
		_animator.SetBool(AnimationsConst.IsClimbing, climbCondition);
		if (climbCondition)
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