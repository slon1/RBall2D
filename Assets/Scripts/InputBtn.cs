using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputBtn : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
	public event Action<bool> OnPressed;
	public void OnPointerDown(PointerEventData eventData) {
		OnPressed?.Invoke(true);
	}

	public void OnPointerUp(PointerEventData eventData) {
		OnPressed?.Invoke(false);
	}	
}
