using UnityEngine;

/// <summary>
/// キャラ操作の管理
/// </summary>
public class CharacterMoveController : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    
    [Header("移動設定")]
    // 地上・空中移動速度
    [SerializeField] private float _speed = 5f;
    // 回転速度
    [SerializeField] private float _rotationSpeed = 10f;
    
    [Header("ジャンプ設定")]
    // ジャンプ力
    [SerializeField] private float _jumpForce = 7f;
    // 地面判定用
    [SerializeField] private LayerMask _groundMask;

    private bool isGrounded;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        // 回転はスクリプトで制御
        _rigidbody.freezeRotation = true;
    }

    void Update()
    {
        // ジャンプ
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            _rigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
        }
    }

    void FixedUpdate()
    {
        // 入力
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // カメラ方向取得
        Transform cam = Camera.main.transform;
        Vector3 camForward = Vector3.Scale(cam.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 camRight = cam.right;

        // 移動方向
        Vector3 moveDir = camForward * vertical + camRight * horizontal;
        moveDir.Normalize();

        if (moveDir.magnitude > 0.1f)
        {
            // 回転
            Quaternion targetRot = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, _rotationSpeed * Time.fixedDeltaTime);

            // 移動（水平速度のみ）
            Vector3 targetVelocity = moveDir * _speed;
            Vector3 velocityChange =
                targetVelocity - new Vector3(_rigidbody.linearVelocity.x, 0, _rigidbody.linearVelocity.z);
            _rigidbody.AddForce(velocityChange, ForceMode.VelocityChange);
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