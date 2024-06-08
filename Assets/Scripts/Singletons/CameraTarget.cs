namespace Singletons
{
    public class CameraTarget : SingletonMonoBehaviour<CameraTarget>
    {
        private void Update()
        {
            var transform1 = transform;
            var pos = transform1.position;
            pos.y = Player.Instance.transform.position.y;
            transform1.position = pos;
        }
    }
}