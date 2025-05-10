using Assets.EllyAndGimli.Constants;
using UnityEngine;

public class GimliController : PlayerControllerBase
{
	[SerializeField] private float _sideCheckRadius = 0.01f;
	[SerializeField] private Transform _leftSideCheck;
	[SerializeField] private Transform _rightSideCheck;
	[SerializeField] private LayerMask _moveableObjectsLayer;

	protected override void Update()
	{
		base.Update();
		_animator.SetBool(AnimationsConst.IsFalling, !_isGrounded && _rb.linearVelocityY < 0);
		var isLeftPushing = _isOverlapping(_leftSideCheck.position, _sideCheckRadius, _moveableObjectsLayer) && !_spriteRenderer.flipX;
		var isRightPushing = _isOverlapping(_rightSideCheck.position, _sideCheckRadius, _moveableObjectsLayer) && _spriteRenderer.flipX;
		_animator.SetBool(AnimationsConst.IsPushing, (isLeftPushing || isRightPushing) && _rb.linearVelocityX != 0);
	}

	protected override void FixedUpdate()
	{
		var movement = _inputSystemActions.Gimli.Move.ReadValue<Vector2>();
		var isJumpPressed = _inputSystemActions.Gimli.Jump.IsPressed();
		DoMove(movement.x);
		TryDoJump(isJumpPressed && _isGrounded);
		base.FixedUpdate();
	}
}
