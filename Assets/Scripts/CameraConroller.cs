using UnityEngine;

public class CameraConroller : MonoBehaviour
{
    [Header("追従設定")]
    // 追従対象（プレイヤー）
    [SerializeField] private Transform _target;
    // 初期オフセット
    [SerializeField] private Vector3 _offset = new Vector3(0, 2, -5);
    // カメラ補間速度
    [SerializeField] private float _smoothSpeed = 10f;

    [Header("回転設定")] 
    [SerializeField] private float _mouseSensitivity = 2f;
    [SerializeField] private float _minYAngle = -20f;
    [SerializeField] private float _maxYAngle = 60f;

    [Header("障害物回避")]
    // 障害物レイヤー
    [SerializeField] private LayerMask _obstacleMask;

    // カメラの当たり判定用半径
    [SerializeField] private float _cameraRadius = 0.3f;

    private float _currentX = 0f;
    private float _currentY = 20f;

    void LateUpdate()
    {
        if (_target == null) return;

        // マウス入力で回転
        _currentX += Input.GetAxis("Mouse X") * _mouseSensitivity;
        _currentY -= Input.GetAxis("Mouse Y") * _mouseSensitivity;
        _currentY = Mathf.Clamp(_currentY, _minYAngle, _maxYAngle);

        // 回転クォータニオン作成
        Quaternion rotation = Quaternion.Euler(_currentY, _currentX, 0);

        // カメラ希望位置 = 回転 * オフセット + プレイヤー位置
        Vector3 desiredPosition = _target.position + rotation * _offset;

        // 障害物回避（SphereCast）
        Vector3 direction = desiredPosition - _target.position;
        if (Physics.SphereCast(_target.position, _cameraRadius, direction.normalized, out RaycastHit hit,
                direction.magnitude, _obstacleMask))
        {
            desiredPosition = hit.point - direction.normalized * _cameraRadius; // 少し手前に移動
        }

        // 滑らかに補間
        transform.position = Vector3.Lerp(transform.position, desiredPosition, _smoothSpeed * Time.deltaTime);

        // プレイヤーを注視
        transform.LookAt(_target.position + Vector3.up * 1.5f);
    }
}