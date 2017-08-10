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
    private bool _onZPosition;
    private bool _jumpOnZ;
    private bool _inJump = true;
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
        //if(_speed < 8f) _speed += 0.1f * Time.deltaTime;
        Move();  
        Sprint();
    }

    private void Gameover()
    {
        _inGame = false;
        GameEvents.Send(OnGameOver);
        _body.constraints = RigidbodyConstraints.None;
    }

    private void PositionOverHandle()
    {
        if (transform.position.y < 0.5)
        {
            Gameover();
        }
        if (transform.position.y < -15)
        {
            SceneManager.LoadScene(0);
        }
    }

    private void Move()
    {
        if (_onZPosition)
        {
            _body.velocity = new Vector3(0, _body.velocity.y, _speed);
        }
        else
        {
            _body.velocity = new Vector3(_speed, _body.velocity.y, 0);
        }
    }

    private void Sprint()                                                                                                                                                                
    {       
        if (_sprint)
        {
            if(_speed < _lastSpeed + 2f) _speed += 0.025f * _speed;
        }
        if(InputController.IsTouchOnScreen(TouchPhase.Began))
        {
            _lastSpeed = _speed;
        }
        
        if (InputController.IsTouchOnScreen(TouchPhase.Stationary) || 
            InputController.IsTouchOnScreen(TouchPhase.Moved)      || 
            InputController.IsTouchOnScreen(TouchPhase.Began))                                                                 
        {
            _sprint = true;
        }
        if(InputController.IsTouchOnScreen(TouchPhase.Ended))
        {
            if (!_inJump)
            {
                Jump();
                if (_inJump)
                {
                    Gameover();
                }
            }
            _sprint = false;
            _speed = _lastSpeed;
        }
    }
    
    private void Jump()
    {
        Vector3 jumpPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        if (_path.Count > 0)
        {
            if (_jumpOnZ)
            {
                _onZPosition = true;
                float posZ = _path[_path.Count - 1].transform.position.z + _path.Count + 1;
                jumpPosition = new Vector3(transform.position.x, transform.position.y, posZ);
            }
            else
            {
                _onZPosition = false;
                float posX = _path[_path.Count - 1].transform.position.x + _path.Count + 1;
                jumpPosition = new Vector3(posX, transform.position.y, transform.position.z);
            }
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
                _body.constraints = RigidbodyConstraints.None;
                _path.Clear();
                _inJump = true;
            });
    }

    private void OnCollisionEnter(Collision other)
    {
        Platform currentPlatform = other.gameObject.GetComponent<Platform>();
        if(!other.gameObject.CompareTag("Platform")) return;
        _inJump = false;
        _body.constraints = RigidbodyConstraints.FreezePositionY;
        if (currentPlatform != null && currentPlatform.IsRotatorLeft)
        {
            _jumpOnZ = true;
        }
        else if (currentPlatform != null && currentPlatform.IsRotatorRight)
        {
            _jumpOnZ = false;
        }
        HandlePlatform(other.gameObject);
    }

    private void OnCollisionExit(Collision other)
    {
        Platform currentPlatform = other.gameObject.GetComponent<Platform>();
        if (currentPlatform != null && currentPlatform.IsLast && !_inJump)
        {
            print("wtf");
            Gameover();
            _body.velocity = Vector3.zero;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!other.CompareTag("Platform")) return;
        _inJump = false;
        transform.position = 
            new Vector3(Mathf.FloorToInt(other.transform.position.x), 
                        transform.position.y, 
                        Mathf.FloorToInt(other.transform.position.z));
        if (!_inJump && other.GetComponent<Platform>() != null && other.GetComponent<Platform>().IsRotatorLeft)
        {
            _onZPosition = true;
        }
        else if (other.GetComponent<Platform>() != null && other.GetComponent<Platform>().IsRotatorRight)
        {
            _onZPosition = false;
        }
    }
    
    private void OnCollisionStay(Collision other)
    {   
        if(!other.gameObject.CompareTag("Platform")) return;
        HandlePlatform(other.gameObject);
    }

    private void HandlePlatform(GameObject go)
    {
        if (_sprint && !_path.Contains(go) && _inGame)
        {
            _path.Add(go);
            if(go.GetComponentInChildren<Renderer>() != null)
                go.GetComponentInChildren<Renderer>().material.color = Color.green;
        }
    }
}