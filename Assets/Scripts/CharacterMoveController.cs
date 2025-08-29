using UnityEngine;

/// <summary>
/// キャラ操作の管理
/// </summary>
public class CharacterMoveController : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    
    public float _speed = 5f; // 移動速度

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        // 入力取得
        float moveHorizontal = Input.GetAxis("Horizontal"); // A/D or ←/→
        float moveVertical = Input.GetAxis("Vertical");     // W/S or ↑/↓

        // 移動ベクトル
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        // Rigidbodyで移動
        _rigidbody.MovePosition(transform.position + movement * _speed * Time.fixedDeltaTime);
    }
}
