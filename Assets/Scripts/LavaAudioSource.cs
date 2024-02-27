using UnityEngine;

public class LavaAudioSource : MonoBehaviour
{
    private Player _player;

    // Start is called before the first frame update
    private void Start()
    {
        _player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    private void Update()
    {
        var tf = transform;
        var pos = tf.position;
        pos.x = _player.transform.position.x;
        tf.position = pos;
    }
}