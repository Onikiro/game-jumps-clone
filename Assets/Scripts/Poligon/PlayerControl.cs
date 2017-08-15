using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerControl : MonoBehaviour {
	[SerializeField] private List<GameObject> _sprintPath = new List<GameObject>();
	[SerializeField] private float _duration;
	[SerializeField] private float _timeStep;
	[SerializeField] private float _force;
	[SerializeField] private float _jumpTime;
	private float _timer;
	private bool _MoveonXDirection = true;
	private bool _inSprint;
	private bool _inGame;
	private bool _inGround;
	private bool _inJump;

	void Start()
	{
		_inGame = true;
	}

	void Update()
	{
		if(!_inGame) return;
		GroundCheck();
		if(_inGround)
		{
			Sprint();
			_timer += Time.deltaTime;
			if(_timer > _timeStep)
			{
				Move();
				_timer = 0;
			}
		}
		else _timer = 0;
	}

	void GroundCheck()
	{
		if(transform.position.y < 0.9)
		{
			_inGround = false;
		}
		else
		{
			_inGround = true;
		}
	}

	void Move()
	{
		if(_inJump) return;
		Vector3 endPosition = Vector3.zero;
		if(_MoveonXDirection)
		{
			endPosition = new Vector3(transform.position.x + 1f, transform.position.y, transform.position.z);
		}
		else
		{
			endPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z + 1f);
		}

		transform.DOMove(endPosition, _duration);
	}

	void Sprint()
	{
		if(InputController.IsTouchOnScreen(TouchPhase.Began) || InputController.IsTouchOnScreen(TouchPhase.Stationary) || InputController.IsTouchOnScreen(TouchPhase.Moved))
		{
			_inSprint = true;
		}

		if(InputController.IsTouchOnScreen(TouchPhase.Ended))
		{
			if(!_inJump)
			{
				Jump();
			}
			_inSprint = false;
			_sprintPath.Clear();
		}
	}

private void Jump()
    {
        if (_sprintPath.Count == 0) return;
		GameObject lastPlatform = _sprintPath[_sprintPath.Count - 1];
        Vector3 jumpPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        if (_MoveonXDirection)
        {
            float posX = lastPlatform.transform.position.x + _sprintPath.Count + 1;
            jumpPosition = new Vector3(posX, jumpPosition.y, lastPlatform.transform.position.z);
        }
        else
        {
			float posZ = lastPlatform.transform.position.z + _sprintPath.Count + 1;
            jumpPosition = new Vector3(lastPlatform.transform.position.x, jumpPosition.y, posZ);
        }
		DOTween.KillAll();
        transform.DOJump(jumpPosition, _force, 1, _jumpTime)
            .SetEase(Ease.Linear)
            .OnStart(() =>
            {
				_inJump = true;
            })
			.OnComplete(() => 
			{
				_inJump = false;
			});
    }

	void OnCollisionEnter(Collision other)
	{
		HandePlatform(other.gameObject);
		if(other.gameObject.GetComponent<Platform>() == null) return;
		if(other.gameObject.GetComponent<Platform>().IsRotatorLeft)
		{
			_MoveonXDirection = false;
		}
		if(other.gameObject.GetComponent<Platform>().IsRotatorRight)
		{
			_MoveonXDirection = true;
		}
	}

	void OnCollisionStay(Collision other)
	{
		HandePlatform(other.gameObject);
	}

	void HandePlatform(GameObject go)
	{
		if(!go.CompareTag("Platform")) return;
		if(_inSprint && !_sprintPath.Contains(go))
		{
			_sprintPath.Add(go);
			if(go.GetComponentInChildren<Renderer>() != null)
                go.GetComponentInChildren<Renderer>().material.color = Color.green;
		}
	}
}
