using System.Collections.Generic;
using UnityEngine;

public class PartManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> _partPrefabs;
    [SerializeField] private List<GameObject> _path;
    [SerializeField] private int _maxCount;

    private void Update()
    {
        if (_path.Count == 0)
        {
            GeneratePart(true);
            _path[0].GetComponent<PathSplicer>().IsFirst = true;
            _path[0].GetComponent<PathSplicer>().PreviousPart = _path[0].GetComponent<PathSplicer>();
        }
        if (_path.Count < _maxCount)
        {
            GeneratePart();
            _path[_path.Count - 1].GetComponent<PathSplicer>().PreviousPart = _path[_path.Count - 2].GetComponent<PathSplicer>();
        }
    }

    private void GeneratePart(bool isFirst = false)
    {
        if (isFirst)
        {
            GameObject firstPart;
            do firstPart = _partPrefabs[Random.Range(0, _partPrefabs.Count)];
            while (firstPart.GetComponent<PathSplicer>().IsCantFirstBlock);
            _path.Add(Instantiate(firstPart, new Vector3(0, 0, 0), Quaternion.identity));
        }
        else
        {
            _path.Add(Instantiate(_partPrefabs[Random.Range(0, _partPrefabs.Count)], new Vector3(35, 0, 35), Quaternion.identity)); 
        }
    }
}