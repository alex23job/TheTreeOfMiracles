using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PullControl : MonoBehaviour
{
    [SerializeField] private float speedPower = 2f;
    [SerializeField] private float turnSpeed = 5f;
    [SerializeField] private Transform target;
    [SerializeField] private Transform targetCircle;
    [SerializeField] private Transform support;
    [SerializeField] private Rigidbody stone;
    [SerializeField] private Transform snowSpawnPoint;
    [SerializeField] private GameObject[] snowRockPrefabs;
    [SerializeField] private Transform pullWheel;

    private Vector3 startPos;
    private Rigidbody rb;
    private bool isPower = false;
    private float deltaPower = 0f;
    private float coeffScale = 24000f;

    private InputActionAsset inputActions;

    private void OnEnable()
    {
        // Подписываемся на события перетаскивания        
        print($"inputActions=<{inputActions}>");
        inputActions.FindActionMap("Player").FindAction("Aim").performed += Aim;
        inputActions.FindActionMap("Player").FindAction("ChargeShot").performed += BeginShot;
        //inputActions.FindActionMap("Player").FindAction("ChargeShot").performed += ContinueShot;
        //inputActions.FindActionMap("Player").FindAction("ChargeShot").started += ContinueShot;
        inputActions.FindActionMap("Player").FindAction("ChargeShot").canceled += EndShot;
    }

    private void Awake()
    {
        inputActions = Resources.Load<InputActionAsset>("InputSystem_Actions");
        rb = GetComponent<Rigidbody>();
        transform.parent = support;
        startPos = transform.localPosition;
        stone.mass = 1f;
        rb.isKinematic = true;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //startPos = transform.position;
        //Invoke("SetStartPos", 3f);
        print(startPos);
        SpawnSnowRock();
    }

    // Update is called once per frame
    void Update()
    {
        if (isPower && inputActions.FindActionMap("Player").FindAction("ChargeShot").inProgress)
        {
            PowerShot();
        }

        /*if (isPower == false)
        {
            if (Input.GetMouseButtonDown(0))
            {
                isPower = true;
            }
        }
        else
        {
            if (Input.GetMouseButton(0))
            {
                if (deltaPower < 2f)
                {
                    deltaPower += Time.deltaTime * speedPower;
                    Vector3 pos = startPos;
                    pos.y -= deltaPower;
                    transform.position = pos;
                }
            }
            if (Input.GetMouseButtonUp(0))
            {
                isPower = false;
                rb.isKinematic = false;
                Invoke("SetStartPos", 3f);
            }
        }*/
    }

    void SetStartPos()
    {
        rb.isKinematic = true;
        stone.mass = 1;
        //print(transform.position);
        transform.localPosition = startPos;
        //print(transform.position);
        Invoke("SpawnSnowRock", 1f);
        //ChangeScale();
    }

    private void ChangeScale()
    {
        Vector3 scale = new Vector3(200f + coeffScale * deltaPower, 200f + coeffScale * deltaPower, 100f);
        target.localScale = scale;
        targetCircle.localScale = scale;
    }

    private void SpawnSnowRock()
    {
        int num = Random.Range(0,snowRockPrefabs.Length);
        Instantiate(snowRockPrefabs[num], snowSpawnPoint.position, Quaternion.identity);
    }

    private void Aim(InputAction.CallbackContext context)
    {
        Vector2 mousePosition = context.ReadValue<Vector2>();
        Vector3 cursorPosition = new Vector3(mousePosition.x, mousePosition.y, 1f);
        Vector3 pointerScreen = Camera.main.WorldToScreenPoint(target.position);
        Vector3 direction = cursorPosition - pointerScreen;
        direction = direction.normalized;
        // Получаем угол в радианах
        float angleRadians = Mathf.Atan2(direction.x, direction.y);

        // Преобразуем в градусы
        float angleDegrees = angleRadians * (180 / Mathf.PI);

        //print($"mp=<{mousePosition}>    dir=<{direction}>  angle=<{angleDegrees}>");

        // Плавно поворачиваем объект
        Quaternion targetRotation = Quaternion.Euler(90f, 0, -angleDegrees);
        target.rotation = Quaternion.RotateTowards(target.rotation, targetRotation, turnSpeed * Time.deltaTime);
        targetCircle.rotation = Quaternion.RotateTowards(targetCircle.rotation, targetRotation, turnSpeed * Time.deltaTime);
        Quaternion supportRotation = Quaternion.Euler(0, angleDegrees, 0);
        support.rotation = Quaternion.RotateTowards(support.rotation, supportRotation, turnSpeed * Time.deltaTime);
    }

    private void BeginShot(InputAction.CallbackContext context)
    {
        if (isPower == false)
        {
            print("BeginShot");
            isPower = true;
        }
    }
    private void ContinueShot(InputAction.CallbackContext context)
    {
        if (isPower)
        {
            print("ContinueShot");
        }
    }

    private void PowerShot()
    {
        print($"PowerShot  delta={deltaPower}");
        if (deltaPower < 0.012f)
        {
            deltaPower += Time.deltaTime * speedPower;
            Vector3 pos = startPos;
            pos.y -= deltaPower;
            transform.localPosition = pos;
            ChangeScale();
            Vector3 wheelRot = pullWheel.rotation.eulerAngles;
            wheelRot.z += deltaPower * 50;
            pullWheel.rotation = Quaternion.Euler(wheelRot);
        }
    }

    private void EndShot(InputAction.CallbackContext context)
    {
        print("EndShot");
        isPower = false;
        stone.mass = 50f;
        deltaPower = 0;
        rb.isKinematic = false;
        Invoke("SetStartPos", 2f);
        Invoke("ChangeScale", 3f);
    }
}
