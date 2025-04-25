using System;
using UnityEngine;

public class UserInput : MonoBehaviour {
	[SerializeField] private InputBtn left;
	[SerializeField] private InputBtn right;
	[SerializeField] private InputBtn up;


	public event Action<bool> OnLeft;
	public event Action<bool> OnRight;
	public event Action<bool> OnJump;


	private void Awake() {

		left.OnPressed += HandleLeft;
		right.OnPressed += HandleRight;
		up.OnPressed += HandleJump;

	}

	private void OnDestroy() {
		left.OnPressed -= HandleLeft;
		right.OnPressed -= HandleRight;
		up.OnPressed -= HandleJump;
	}

	private void HandleLeft(bool state) => OnLeft?.Invoke(state);

	private void HandleRight(bool state) => OnRight?.Invoke(state);

	private void HandleJump(bool state) => OnJump?.Invoke(state);

}