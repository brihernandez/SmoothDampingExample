using UnityEngine;

public class Rotation : MonoBehaviour
{
    public float MaxSpeed = 30;

    private Vector3 _angularVelocity = Vector3.zero;

    private void Awake()
    {
        _angularVelocity = Random.insideUnitSphere * MaxSpeed;
    }

    private void Update()
    {
        transform.Rotate(_angularVelocity * Time.deltaTime);
    }
}