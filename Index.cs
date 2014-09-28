using UnityEngine;
using System.Collections;

public class Index : MonoBehaviour 
{
	public UIButton LocalButton;
	public UIButton CloudButton;

	void Start()
	{
		EventDelegate.Add (LocalButton.onClick, ClickLocal);
		EventDelegate.Add (CloudButton.onClick, ClickCloud);
	}


	private void ClickLocal()
	{
		Application.LoadLevel("Local");
	}

	private void ClickCloud()
	{
		Application.LoadLevel("CloudReco");
	}
}

