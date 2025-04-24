using DG.Tweening;
using UnityEngine;

public class BallSquashAnimator  {
	public float squashScaleY = 0.7f;
	public float squashTime = 0.1f;


	
	private Vector3 originalScale;

	

	public void PlayJumpSquash(Transform transform) {
		originalScale = transform.localScale;
		Sequence seq = DOTween.Sequence();
		seq.Append(transform.DOScale(new Vector3(1.2f, squashScaleY, 1f), squashTime))
		   .Append(transform.DOScale(originalScale, squashTime));
	}

	public void PlayLandSquash(Transform transform) {
		originalScale = transform.localScale;
		Sequence seq = DOTween.Sequence();
		seq.Append(transform.DOScale(new Vector3(0.8f, 1.3f, 1f), squashTime))
		   .Append(transform.DOScale(originalScale, squashTime));
	}
}