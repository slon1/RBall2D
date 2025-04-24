using UnityEngine;

public interface IGroundChecker {
	public void SetSlopeThreshold(float slopeThreshold);
	bool IsGrounded(Vector2 position, out bool canJump);
	bool IsGrounded(Vector2 position);
	void CheckRadius(float radius);
	void SetOffset(float offset);
}
