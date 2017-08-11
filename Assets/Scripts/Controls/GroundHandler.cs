using UnityEngine;

public class GroundHandler : MonoBehaviour {

    private void OnTriggerExit(Collider other)
    {
        Platform currentPlatform = other.GetComponent<Platform>();
        if (currentPlatform != null && currentPlatform.IsLast && !transform.parent.GetComponent<Controller>().InJump)
        {    
            transform.parent.GetComponent<Controller>().Gameover();
        }
    }
}
