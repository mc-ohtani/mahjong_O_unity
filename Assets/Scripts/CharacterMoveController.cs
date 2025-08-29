using UnityEngine;

/// <summary>
/// キャラ操作の管理
/// </summary>
public class CharacterMoveController : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    [Header("移動設定")]
    // 地上・空中移動速度
    public float _speed = 5f;
    // 回転速度
    public float _rotationSpeed = 10f;
    [Header("ジャンプ設定")]
    // ジャンプ力
    public float _jumpForce = 7f;
    // 地面判定用
    public LayerMask _groundMask;

    private bool isGrounded;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        // 回転はスクリプトで制御
        _rigidbody.freezeRotation = true; 
    }

    private void FixedUpdate()
    {
        // 入力
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        Vector3 inputDir = new Vector3(moveX, 0, moveZ).normalized;

        // 回転
        if (inputDir.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(inputDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
        }

        // 移動（AddForceで加速）
        Vector3 horizontalVelocity = new Vector3(_rigidbody.linearVelocity.x, 0, _rigidbody.linearVelocity.z);
        Vector3 targetVelocity = inputDir * _speed;
        Vector3 velocityChange = targetVelocity - horizontalVelocity;
        _rigidbody.AddForce(velocityChange, ForceMode.VelocityChange);

        // ジャンプ
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            _rigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
        }
    }
    
    // BoxCollider 同士の接地判定
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}