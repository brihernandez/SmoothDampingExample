using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    public List<GameObject> Prefabs;
    public int Count = 50;
    public float Range = 100;
    public float HeightDivisor = 4;
    public Vector2 MinMaxScale = new(2, 4);

    private void Start()
    {
        for (int i = 0; i < Count; i++)
        {
            var randomPos = Random.insideUnitSphere * Range;
            randomPos.y /= HeightDivisor;
            var randomRotation = Random.rotation;
            var asteroid = Instantiate(Prefabs[Random.Range(0, Prefabs.Count)], randomPos, randomRotation);
            asteroid.transform.SetParent(transform, worldPositionStays: true);
            asteroid.transform.localScale = Random.Range(MinMaxScale.x, MinMaxScale.y) * Vector3.one;
        }
    }
}
