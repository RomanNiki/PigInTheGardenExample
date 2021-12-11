using UnityEngine;

namespace Source
{
    public class CameraScript : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _field;

        private void Start()
        {
            var screenRatio = Screen.width / (float) Screen.height;
            var targetRatio = _field.size.x / _field.size.y;

            if (screenRatio >= targetRatio)
            {
                Camera.main.orthographicSize = _field.size.y / 2;
            }
            else
            {
                var differenceInSize = targetRatio / screenRatio; 
                Camera.main.orthographicSize = _field.size.y / 2 * differenceInSize;
            }
        }
    }
}
