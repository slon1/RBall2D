using UnityEngine;

public interface IGroundChecker {
	bool IsGrounded(Vector2 position);
	void CheckRadius(float radius);
}
