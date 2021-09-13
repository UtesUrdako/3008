using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Transform _bulletSpawn;
    [SerializeField] private Transform _target;
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private PlayerMovement _pm;
    [SerializeField] private float _speed;
    [SerializeField] private float _speedRotate = 50f;
    [SerializeField] private float _forceBullet = 50f;
    private float _cfSpeed = 1f;
    private static int _id = 0;
    private Vector3 _direction;
    [SerializeField] private bool _isFire;
    private bool _isFire1;
    private bool _isFire2;
    private bool _isSprint;
    private float _rechargeTime = 2f;


    private void Awake()
    {
        _isFire = true;
        _isFire1 = false;
        _isFire2 = false;
        name = "Player " + _id++;
        _direction = Vector3.zero;

        _rb = GetComponent<Rigidbody>();

        _pm = new PlayerMovement();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Start()
    {
        FindObjectOfType<Player>();

        float distance = Vector3.Distance(transform.position, _target.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (_isFire)
        {
            _isFire1 = Input.GetMouseButtonDown(0);
            _isFire2 = Input.GetMouseButtonDown(1);
        }

        _isSprint = Input.GetButton("Sprint");
    }

    private void FixedUpdate()
    {
        _direction.x = Input.GetAxis("Horizontal");
        _direction.z = Input.GetAxis("Vertical");

        float sprint = (_isSprint) ? 2f : 1f;

        _rb.MovePosition(transform.position + _direction.normalized * _speed * sprint * _cfSpeed * Time.fixedDeltaTime);

        Vector3 rotate = new Vector3(0f, Input.GetAxis("Mouse X") * _speedRotate * Time.fixedDeltaTime, 0f);
        _rb.MoveRotation(_rb.rotation * Quaternion.Euler(rotate));

        //transform.Translate(_direction.normalized * _speed * sprint * _cfSpeed * Time.fixedDeltaTime);
        //transform.Rotate(Vector3.up, Input.GetAxis("Mouse X") * _speedRotate * Time.fixedDeltaTime);

        if (_isFire1)
            Fire(true);
        if (_isFire2)
            Fire(false);

        Show();

        //_pm.FixedUpdate();
    }

    private void Fire(bool selfHoming)
    {
        _isFire = false;

        if (selfHoming)
            _isFire1 = false;
        else
            _isFire2 = false;

        GameObject bullet = Instantiate(_bulletPrefab, _bulletSpawn.position, _bulletSpawn.rotation);

        bullet.GetComponent<Bullet>().Initialization(15f, _target, selfHoming);

        Rigidbody rbBullet = bullet.GetComponent<Rigidbody>();
        rbBullet.AddForce(bullet.transform.forward * _forceBullet, ForceMode.Impulse);

        StartCoroutine(Recharge(_rechargeTime));
    }

    public void SetBoostSpeed(float cfSpeed)
    {
        _cfSpeed = cfSpeed;
    }

    private IEnumerator Recharge(float time)
    {
        yield return new WaitForSeconds(time);

        _isFire = true;
    }

    private void Show()
    {
        if (Physics.Raycast(_bulletSpawn.position, transform.forward, out RaycastHit hit))
        {
            Debug.DrawRay(hit.point, hit.normal, Color.red);
        }

    }
}
