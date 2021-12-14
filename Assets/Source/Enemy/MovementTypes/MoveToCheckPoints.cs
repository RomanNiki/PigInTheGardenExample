using Source.Interfaces;
using UnityEngine;

namespace Source.Enemy.MovementTypes
{
    public class MoveToCheckPoints : IMove
    {
        private readonly Transform _transform;
        public Transform[] CheckPoints { get; set; }
        public float _speed { get; set; }
        private Vector3 _inputDirection;
        private int _currentCheckPoint;

        public MoveToCheckPoints(float speed, Transform[] checkPoints, Transform transform)
        {
            _speed = speed;
            CheckPoints = checkPoints;
            _transform = transform;
        }

        public Vector2 Move()
        {
            if (((Vector2) CheckPoints[_currentCheckPoint].position - (Vector2) _transform.position).magnitude > 0.1f)
            {
                _inputDirection = (CheckPoints[_currentCheckPoint].position -
                                   _transform.position).normalized * _speed * Time.deltaTime;
                _inputDirection.z = 0;
                _transform.position += _inputDirection;
            }
            else
            {
                if (_currentCheckPoint < CheckPoints.Length - 1)
                {
                    _currentCheckPoint++;
                }
                else
                {
                    _currentCheckPoint = 0;
                }
            }

            return _inputDirection;
        }
        
    }
}
