using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace W3Labs.Utils
{
    public class Rotator : MonoBehaviour
    {
        [SerializeField] private Vector3 axis;
        [SerializeField] private float speed;
        static Vector3 CurrentRotation = new Vector3(0, 0, 0);

        [System.Obsolete]
        private void Start()
        {
            transform.eulerAngles = CurrentRotation;
        }

        [System.Obsolete]
        private void Update()
        {
            transform.Rotate(speed * Time.deltaTime * axis);
            CurrentRotation = transform.rotation.eulerAngles;
        }
    }
}
