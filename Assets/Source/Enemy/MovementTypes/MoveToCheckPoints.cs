using System.Collections.Generic;
using Source.Interfaces;
using UnityEngine;

namespace Source.Enemy.MovementTypes
{
    public class MoveToCheckPoints : IMove
    {
        private readonly Transform _transform;
        private List<Transform> _checkPoints;
        public float _speed { get; set; }
        private Vector3 _inputDirection;
        private int _currentCheckPoint;

        public MoveToCheckPoints(float speed, IEnumerable<Transform> checkPoints, Transform transform)
        {
            _speed = speed;
            _checkPoints = new List<Transform>(checkPoints);
            _transform = transform;
        }

        public Vector2 Move()
        {
            if (((Vector2) _checkPoints[_currentCheckPoint].position - (Vector2) _transform.position).magnitude > 0.1f)
            {
                _inputDirection = (_checkPoints[_currentCheckPoint].position -
                                   _transform.position).normalized * _speed * Time.deltaTime;
                _inputDirection.z = 0;
                _transform.position += _inputDirection;
            }
            else
            {
                if (_currentCheckPoint < _checkPoints.Count - 1)
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

        public void AddCheckPoint(Transform checkPoint)
        {
            _checkPoints.Add(checkPoint);
        }

        public void RemoveCheckPoint(Transform checkPoint)
        {
            _checkPoints.Remove(checkPoint);
        }
        
        public void RemoveCheckPoint(int checkPointId)
        {
            _checkPoints.Remove(_checkPoints[checkPointId]);
        }
        
    }
}
