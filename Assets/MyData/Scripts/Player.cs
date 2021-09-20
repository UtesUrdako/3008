using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour, IStorage
{
    [SerializeField] private GameObject _leg;
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Transform _bulletSpawn;
    [SerializeField] private Transform _target;
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private PlayerMovement _pm;
    [SerializeField] private Animator _anim;
    [SerializeField] private float _speed;
    [SerializeField] private float _speedRotate = 50f;
    [SerializeField] private float _forceBullet = 50f;
    [SerializeField] private Text _ammoText;

    private float _cfSpeed = 1f;
    private static int _id = 0;
    private Vector3 _direction;
    [SerializeField] private bool _isFire;
    private bool _isFire1;
    private bool _isFire2;
    private bool _isSprint;
    private float _rechargeTime = 2f;
    private List<string> _storage;
    private int counterFire = 0;

    private void Awake()
    {
        _isFire = true;
        _isFire1 = false;
        _isFire2 = false;
        name = "Player " + _id++;
        _direction = Vector3.zero;

        _rb = GetComponent<Rigidbody>();
        _anim = GetComponent<Animator>();
        _pm = new PlayerMovement();
        _storage = new List<string>();

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
            if (Input.GetMouseButtonDown(0))
            _isFire1 = true;
            if (Input.GetMouseButtonDown(1))
            _isFire2 = true;
        }

        _isSprint = Input.GetButton("Sprint");
    }

    private void FixedUpdate()
    {
        _direction.x = Input.GetAxis("Horizontal");
        _direction.z = Input.GetAxis("Vertical");

        if (_direction == Vector3.zero)
            _anim.SetBool("IsMove", false);
        else
            _anim.SetBool("IsMove", true);

        float sprint = (_isSprint) ? 2f : 1f;

        _rb.MovePosition(transform.position + _direction.normalized * _speed * sprint * _cfSpeed * Time.fixedDeltaTime);

        Vector3 rotate = new Vector3(0f, Input.GetAxis("Mouse X") * _speedRotate * Time.fixedDeltaTime, 0f);
        //_rb.MoveRotation(_rb.rotation * Quaternion.Euler(rotate));

        //transform.Translate(_direction.normalized * _speed * sprint * _cfSpeed * Time.fixedDeltaTime);
        transform.Rotate(Vector3.up, Input.GetAxis("Mouse X") * _speedRotate * Time.fixedDeltaTime);

        if (_isFire1)
        {
            _isFire1 = false;
            _anim.SetTrigger("Shoot");
            _anim.SetInteger("NumShoot", 0);
        }
        if (_isFire2)
        {
            _isFire2 = false;
            _anim.SetTrigger("Shoot");
            _anim.SetInteger("NumShoot", 1);
        }

        Show();

        //_pm.FixedUpdate();
    }

    private void Fire1()
    {
        Fire(true);
    }

    private void Fire2()
    {
        Fire(false);
    }

    private void Fire(bool selfHoming)
    {
        _isFire = false;

        GameObject bullet = Instantiate(_bulletPrefab, _bulletSpawn.position, _bulletSpawn.rotation);

        bullet.GetComponent<Bullet>().Initialization(15f, _target, selfHoming);
        counterFire++;
        _ammoText.text = $"count fire: {counterFire}";

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

    public bool IsItem(string name)
    {
        foreach (string item in _storage)
            if (item.Equals(name))
                return true;

        return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Finish"))
        {
            _storage.Add("KeyFinish");
            Destroy(other);
            Destroy(_leg);
        }
    }
}
