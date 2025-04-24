using System;
using System.Collections;
using UnityEngine;
public class Installer : MonoBehaviour {

	private static DIContainer container;


	void Awake() {
		container = new DIContainer();
		container.Register<IGroundChecker>(new GroundChecker(LayerMask.GetMask("Ground")));
		container.Register(new BallSquashAnimator());
	}

	public static T GetService<T>() => container.Resolve<T>();
	private void OnDestroy() {
		container.Dispose();
	}
}
