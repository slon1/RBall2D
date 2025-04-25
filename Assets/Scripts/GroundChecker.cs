using UnityEngine;

public class GroundChecker : IGroundChecker {
	private float offset;
	private float checkRadius;
	private readonly LayerMask groundMask;
	private float slopeDotThreshold = 0.7f; 

	public GroundChecker(LayerMask groundMask) {
		this.groundMask = groundMask;
	}

	public void SetSlopeThreshold(float slopeThreshold) {
		this.slopeDotThreshold = slopeThreshold;
	}
	public void CheckRadius(float radius) {
		checkRadius = radius;
	}

	public void SetOffset(float offset) {
		this.offset = offset;
	}

	public bool IsGrounded(Vector2 position, out bool canJump) {
		Vector2 origin = position + Vector2.down * offset;
		canJump = false;

		Collider2D hit = Physics2D.OverlapCircle(origin, checkRadius, groundMask);
		if (hit) {
			Vector2 closestPoint = hit.ClosestPoint(origin);
			Vector2 directionToCenter = (closestPoint - origin).normalized; 
			float dot = Vector2.Dot(Vector2.down, directionToCenter);
			canJump = dot > slopeDotThreshold;	

			return true;
		}
		return false;
	}
	
}