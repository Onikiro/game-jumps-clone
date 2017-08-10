using UnityEngine;

public class PathSplicer : MonoBehaviour
{	
	public PathSplicer PreviousPart;

	public bool IsFirst;
	public bool IsCantFirstBlock;
	public Transform StartPositionPlatform;
	public Transform EndPositionPlatform;
		
	private void Update()
	{
		if (IsFirst) return;
			
		float posX, posZ;
		{			
			posX = transform.position.x + (PreviousPart.EndPositionPlatform.position.x - StartPositionPlatform.position.x + 1);
			posZ = transform.position.z + (PreviousPart.EndPositionPlatform.position.z - StartPositionPlatform.position.z);
		}
		transform.position = new Vector3(posX, 0, posZ);
	}
}