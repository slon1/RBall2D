using UnityEngine;

public interface IGroundChecker {
	public void SetSlopeThreshold(float slopeThreshold);
	bool IsGrounded(Vector2 position, out bool canJump);	
	void CheckRadius(float radius);
	void SetOffset(float offset);
}
