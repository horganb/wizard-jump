namespace Singletons
{
    public class LavaAudioSource : SingletonMonoBehaviour<LavaAudioSource>
    {
        // Update is called once per frame
        private void Update()
        {
            var tf = transform;
            var pos = tf.position;
            pos.x = Player.Instance.transform.position.x;
            tf.position = pos;
        }
    }
}