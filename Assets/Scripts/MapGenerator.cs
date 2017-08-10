using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] private GameObject[] _prefabs;

    [SerializeField] public List<GameObject> Objects = new List<GameObject>();

    [SerializeField] private int _maxCount;
    
    private bool _inZPosition;
    private bool _startGenerate;
    
    private void OnGameOver()
    {
        Clear();
    }
    
    private void OnEnable()
    {
        OnGameOver();  
    }
    
    private void Update()
    {
        if (Objects.Count >= _maxCount) return;
        
        if (Objects.Count < 2)
        {
            GenerateCube(_prefabs[0]);
        }
        else Generate();
    }

    private void Generate()
    {    
        bool platform = true;

        for (int i = 0; i < _maxCount; i++)
        {
            _inZPosition = Random.value > 0.5;
            int length = platform ? Random.Range(0, 3) : Random.Range(2, 5);
            for (int j = 0; j <= length; j++)
            {
                if (j == 0)
                {
                    GenerateCube(_prefabs[0]);
                    continue;
                }
                if (j == length)
                {
                    GenerateCube(_prefabs[0]);
                    continue;
                }
                GenerateCube(platform ? _prefabs[0] : _prefabs[1]);
            }
            platform = !platform;
        } 
    }
    
    private void Clear()
    {
        foreach (var cube in Objects)
        {
            Destroy(cube);
        }
        Objects.Clear();
    }

    private void GenerateCube(GameObject prefab)
    {
        Objects.Add(Instantiate(prefab));
        if (Objects.Count > 1)
        {
            float positionX = GetPositionX();
            float positionY = GetPositionY();
            float positionZ = GetPositionZ();

            if (_inZPosition)
            {
                Objects[Objects.Count - 1].GetComponent<Transform>().Rotate(0,-90,0);
            }
            
            Objects[Objects.Count - 1].GetComponent<Transform>().position = new Vector3(positionX, positionY, positionZ);
        }
    }
    
    private float GetPositionX()
    {
        if (_inZPosition)
        {
            return Objects[Objects.Count - 2].transform.position.x;
        }
        return Objects[Objects.Count - 2].transform.position.x + 1;
    }

    private float GetPositionY()
    {
        return Objects[Objects.Count - 1].transform.position.y;
    }

    private float GetPositionZ()
    {
        if (_inZPosition)
        {
            return Objects[Objects.Count - 2].transform.position.z + 1;
        }
        return Objects[Objects.Count - 2].transform.position.z;
    }
}