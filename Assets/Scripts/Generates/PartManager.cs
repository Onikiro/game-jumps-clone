using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

namespace Generates
{
    public class PartManager : MonoBehaviour
    {
        [SerializeField] private GameObject _startPart;
        [SerializeField] private List<GameObject> _partPrefabs;
        [SerializeField] private List<GameObject> _path;
        [SerializeField] private int _maxCount;
        private bool _isRemoved;

        private void Update()
        {
            if (_path.Count == 0)
            {
                GeneratePart(true);
                _startPart.GetComponent<PathSplicer>().IsFirst = true;
                _startPart.GetComponent<PathSplicer>().PreviousPart = _startPart.GetComponent<PathSplicer>();
            }
            if (_path.Count < _maxCount)
            {
                GeneratePart();
                _path[_path.Count - 1].GetComponent<PathSplicer>().PreviousPart = _path[_path.Count - 2].GetComponent<PathSplicer>();
            }
            if (!_isRemoved)
            {
                _isRemoved = true;
                // Invoke("DestroyOldPart", 10f);
            }
        }

        private void GeneratePart(bool isFirst = false)
        {
            if (isFirst)
            {
                _path.Add(Instantiate(_startPart, new Vector3(0, 0, 0), Quaternion.identity));
            }
            else
            {
                int randomIndex = Random.Range(0, _partPrefabs.Count);
                string name = _path[_path.Count - 1].gameObject.name;
                name = name.Substring(0, 6);
                while(_partPrefabs[randomIndex].gameObject.name == name)
                {
                    randomIndex = randomIndex > 1 ? Random.Range(0, randomIndex) : Random.Range(randomIndex + 1, _partPrefabs.Count);
                }
                _path.Add(Instantiate(_partPrefabs[randomIndex], new Vector3(-35, 0, -35), Quaternion.identity)); 
            }
        }

        void DestroyOldPart()
        {
            _isRemoved = false;
            //Destroy(_path[0]);
            //_path.Remove(_path[0]);
        }
    }
}