using UnityEngine;

public class Platform : MonoBehaviour
{
	public bool IsRotatorLeft;
	public bool IsRotatorRight;
	public bool IsLast;
	
	private void Start()
	{
		foreach (var col in gameObject.GetComponents<Collider>())
		{
			if (col.isTrigger)
			{
				if (Mathf.Abs(transform.localRotation.y) > 0)
				{
					IsRotatorRight = true;
					IsRotatorLeft = false;
					break;
				}
				IsRotatorRight = false;
				IsRotatorLeft = true;
				break;
			}
		}
//
//		if (IsLast)
//		{
//			BoxCollider trigger = gameObject.AddComponent<BoxCollider>();
//			trigger.center = new Vector3(0, 1, 0);
//			trigger.size = new Vector3(0.75f, 0.75f, 0.75f);
//		}
	}
}
