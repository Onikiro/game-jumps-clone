using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Controller : MonoBehaviour
{
    public static Action OnGameOver;

    private float _lastSpeed;
    [SerializeField] private float _speed;
    [SerializeField] private float _force;
    [SerializeField] private float _time;
    private List<GameObject> _path = new List<GameObject>();
    private Rigidbody _body;
    private bool _MoveonZDirection;
    private bool _jumpOnZDirection;
    public bool InJump = true;
    private bool _sprint;
    private bool _inGame;

    private void Start()
    {
        _inGame = true;
        _body = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        PositionOverHandle();
        if (!_inGame) return;
        Move();
        // SetConstraints();
        Sprint();
    }

    private void SetConstraints()
    {
        if (!InJump)
        {
            _body.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
        }
        else
        {
            _body.constraints = RigidbodyConstraints.None | RigidbodyConstraints.FreezeRotation;
        }
    }

    public void Gameover()
    {
        _inGame = false;
        GameEvents.Send(OnGameOver);
        _body.constraints = RigidbodyConstraints.None | RigidbodyConstraints.FreezeRotation;
        Invoke("Restart", 5f);
    }

    void Restart()
    {
        SceneManager.LoadScene(0);
    }

    private void PositionOverHandle()
    {
        if (transform.position.y < 0)
        {
            Gameover();
        }
        if (transform.position.y < -15)
        {
            Restart();
        }
    }

    private void Move()
    {
        if (_MoveonZDirection)
        {
            _body.velocity = new Vector3(0, _body.velocity.y, _speed);
        }
        else
        {
            _body.velocity = new Vector3(_speed, _body.velocity.y, 0);
        }
        //if(_speed < 8f) _speed += 0.1f * Time.deltaTime;
    }

    private void Sprint()                                                                                                                                                                
    {
        if (_sprint)
        {
            //if (_speed < _lastSpeed + 2f) _speed += 0.025f * _speed;
        }
        if (InputController.IsTouchOnScreen(TouchPhase.Began))
        {
            _lastSpeed = _speed;
        }

        if (InputController.IsTouchOnScreen(TouchPhase.Stationary) ||
            InputController.IsTouchOnScreen(TouchPhase.Moved) ||
            InputController.IsTouchOnScreen(TouchPhase.Began))
        {
            _sprint = true;
        }
        if (InputController.IsTouchOnScreen(TouchPhase.Ended))
        {
            if (!InJump)
            {
                Jump();
                // if (InJump)
                // {
                //     Gameover();
                // }
            }
            _sprint = false;
            _speed = _lastSpeed;
        }
    }
    
    private void Jump()
    {
        if (_path.Count <= 0) return;
        Vector3 jumpPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        if (_jumpOnZDirection)
        {
            _MoveonZDirection = true;
            float posZ = _path[_path.Count - 1].transform.position.z + _path.Count + 1;
            jumpPosition = new Vector3(_path[_path.Count - 1].transform.position.x, jumpPosition.y, posZ);
        }
        else
        {
            _MoveonZDirection = false;
            float posX = _path[_path.Count - 1].transform.position.x + _path.Count + 1;
            jumpPosition = new Vector3(posX, jumpPosition.y, _path[_path.Count - 1].transform.position.z);
        }
        float newTime = _time;
        if (_path.Count < 2)
        {
            newTime = 0.35f;
        }

        transform.DOJump(jumpPosition, _force, 1, newTime)
            .SetEase(Ease.Linear)
            .OnStart(() =>
            {
                _path.Clear();
                InJump = true;
            })
            .OnComplete(() =>
            {
                _body.velocity = new Vector3(_body.velocity.x + _force  * 3f, _body.velocity.y - _force * 3f, _body.velocity.z);
            });
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!other.gameObject.CompareTag("Platform") || !_inGame) return;
        Platform currentPlatform = other.gameObject.GetComponent<Platform>();
        InJump = false;
        SetJumpDirection(currentPlatform);
        HandlePlatform(other.gameObject);
    }

    void SetJumpDirection(Platform currentPlatform)
    {
        if (currentPlatform == null || InJump) return;
        if (currentPlatform.IsRotatorLeft)
        {
            _jumpOnZDirection = true;
        }
        else if (currentPlatform.IsRotatorRight)
        {
            _jumpOnZDirection = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!other.CompareTag("Platform") || !_inGame || other.GetComponent<Platform>() == null) return;
        
        if (other.GetComponent<Platform>().IsRotatorLeft)
        {
            CorrectPosition(other.gameObject);
            _MoveonZDirection = true;
        }
        else if (other.GetComponent<Platform>().IsRotatorRight)
        {
            CorrectPosition(other.gameObject);
            _MoveonZDirection = false;
        }
    }

    private void CorrectPosition(GameObject go)
    {
        transform.position = new Vector3(Mathf.Floor(go.transform.position.x), transform.position.y, Mathf.Floor(go.transform.position.z));
    }

    private void OnCollisionStay(Collision other)
    {   
        if(!other.gameObject.CompareTag("Platform") || !_inGame) return;
        HandlePlatform(other.gameObject);
    }

/// <summary>
/// Полуает значение после запятой
/// </summary>
/// <param name="pos">Значение числа типа float</param>
/// <returns>Возвращает значение типа float < 0</returns>
    float GetValueAfterDot(float pos)
    {
        return pos - Mathf.Floor(pos);
    }

    private void HandlePlatform(GameObject go)
    {
        if (_sprint && !_path.Contains(go))
        {
            if(transform.position.x - go.transform.position.x > 0.25) return;
            if(transform.position.z - go.transform.position.z > 0.25) return;

            _path.Add(go);
            if(go.GetComponentInChildren<Renderer>() != null)
                go.GetComponentInChildren<Renderer>().material.color = Color.green;
        }
    }
}