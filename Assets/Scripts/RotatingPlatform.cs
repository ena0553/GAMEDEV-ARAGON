using System.Threading;
using UnityEngine;

public class RotatingPlatform : MonoBehaviour
{

    [SerializeField] Vector3 rotationSpeed = new Vector3(0, 40, 0);

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(rotationSpeed.x * Time.deltaTime, rotationSpeed.y * Time.deltaTime, rotationSpeed.z * Time.deltaTime);
    }
}
