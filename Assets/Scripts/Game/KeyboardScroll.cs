using UnityEngine;
using System.Collections;

public class KeyboardScroll : MonoBehaviour {
	
	//We need the Scrollbars for keyboard scroll
	UIScrollBar hScrollbar;
	UIScrollBar vScrollbar;
	public float keyboardSensitivity = 1;

	public static KeyboardScroll instance;
	public Vector2 autoScroll = Vector2.zero;
	
	void Awake()
	{
		instance = this;
		//Assign both scrollbars on Awake
		hScrollbar = GetComponent<UIDraggablePanel>().horizontalScrollBar;
		vScrollbar = GetComponent<UIDraggablePanel>().verticalScrollBar;
	}

	void Start()
	{
		hScrollbar.alpha = 0.4f;
		vScrollbar.alpha = 0.4f;
	}
	
	void Update()
	{
		//Get keyboard input axes values
		Vector2 keyDelta = Vector2.zero;
		//autoScroll = Vector2.zero;

		keyDelta.Set(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
		keyDelta += autoScroll;

		hScrollbar.alpha = 0.4f;
		vScrollbar.alpha = 0.4f;

		//If no keyboard arrow is pressed, don't go further
		if(keyDelta == Vector2.zero) return;
		//Make it framerate independent and multiply by sensitivity
		keyDelta *= Time.deltaTime * keyboardSensitivity;
		//Scroll by adjusting scrollbars' values
		hScrollbar.value += keyDelta.x;
		vScrollbar.value -= keyDelta.y;

		if(keyDelta.x != 0) hScrollbar.alpha = 0.8f;
		if(keyDelta.y != 0) vScrollbar.alpha = 0.8f;
	

	}
}
