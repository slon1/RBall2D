using UnityEngine;

public class GroundChecker : IGroundChecker {
	private readonly float offset;
	private float checkRadius;
	private readonly LayerMask groundMask;

	public GroundChecker(float offset, LayerMask groundMask) {
		this.offset = offset;		
		this.groundMask = groundMask;
	}
	public void CheckRadius(float radius) {
		checkRadius = radius;
	}

	public bool IsGrounded(Vector2 position) {
		Vector2 origin = position + Vector2.down * offset;
		return Physics2D.OverlapCircle(origin, checkRadius, groundMask);
	}
}
