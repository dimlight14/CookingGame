using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public class CustomersMarkUp : MonoBehaviour
    {
        [SerializeField] private List<Transform> customerPositions;
        [SerializeField] private Transform leftPosition;
        [SerializeField] private Transform rightPosition;

        public Vector3 LeftPosition => leftPosition.position;
        public Vector3 RightPosition => rightPosition.position;

        public float GetPosition(int positionNumber)
        {
            return customerPositions[positionNumber].position.x;
        }
    }
}