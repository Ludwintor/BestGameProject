using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Back : MonoBehaviour
{
	public GameObject AllSettings;
	public void onClick()
	{
		gameObject.SetActive(false);
		AllSettings.SetActive(true);
	}
}
