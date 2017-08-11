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
	}
}
