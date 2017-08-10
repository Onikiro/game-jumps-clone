using UnityEngine;

public class Platform : MonoBehaviour
{
	public bool IsRotator;
	public bool IsLast;
	
	private void Start()
	{
		foreach (var col in gameObject.GetComponents<Collider>())
		{
			if (col.isTrigger)
			{
				IsRotator = true;
				break;
			}
		}
	}
}
