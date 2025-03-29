using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class EllyController : PlayerControllerBase
{
	private bool _canMakeSecondJump;
	private bool _isTouchingWall;
	private float _defaultGravityScale;

	private readonly Func<Vector2, float, int, bool> _isOverlappingWithWall = (Vector2 position, float radius, int layerMask)
		=> Physics2D.OverlapCircle(position, radius, layerMask) != null;

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
		_inputSystemActions.Elly.Jump.started += context => TryDoJump((_isGrounded || _canMakeSecondJump) && !_isTouchingWall);
	}

	protected override void Update()
	{
		_isTouchingWall = _isOverlappingWithWall(_leftWallCheck.position, _wallCheckRadius, _wallLayer)
			|| _isOverlappingWithWall(_rightWallCheck.position, _wallCheckRadius, _wallLayer);
		_rb.gravityScale = _isTouchingWall ? _wallSlidingSpeedDown : _defaultGravityScale;
		base.Update();
	}

	protected override void FixedUpdate()
	{
		var movement = _inputSystemActions.Elly.Move.ReadValue<Vector2>();
		var isJumpPressed = _inputSystemActions.Elly.Jump.IsPressed();
		DoMove(movement.x);
		TryDoWallClimbing(isJumpPressed);
		base.FixedUpdate();
	}

	protected override bool TryDoJump(bool jumpingCondition)
	{
		var hasJumped = base.TryDoJump(jumpingCondition);
		if (hasJumped)
		{
			StartCoroutine(nameof(JumpCoroutine));
		}
		return hasJumped;
	}

	private bool TryDoWallClimbing(bool isJumpPressed)
	{
		if (isJumpPressed && _isTouchingWall)
		{
			_rb.linearVelocity = Vector2.up * _wallSlidingSpeedUp;
			return true;
		}
		return false;
	}

	private IEnumerator JumpCoroutine()
	{
		_canMakeSecondJump = true;
		yield return new WaitForSeconds(0.5f);
		_canMakeSecondJump = false;
	}
}