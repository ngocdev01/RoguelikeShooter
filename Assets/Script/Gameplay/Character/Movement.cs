using NgocDev.Core.Addressable;
using NgocDev.Core.Input;
using NgocDev.Core.ServiceLocator;
using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;
using UnityEngine.Search;







[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Rigidbody))]
public class Movement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float acceleration = 20f;


    [Header("Collision")]
    [SerializeField] private float minimumCollideDistance = 0.01f;
    [SerializeField] private LayerMask collisionMask = ~0;

    [Header("Input")]
    [SerializeField] private InputActionReference moveAction;
    [SerializeField] private InputActionReference aimAction;

    private float angularVelocityY;
    private Vector3 currentVelocity;
    private Vector2 moveInput;
    private Vector2 aimInput;
    private BoxCollider boxCollider;
    private Rigidbody rb;
    public Vector3 Velocity => currentVelocity;
    public Vector2 AimDirection => aimInput;




    private void Awake()
    {
        var ass = AppDomain.CurrentDomain.GetAssemblies();
 

        boxCollider = GetComponent<BoxCollider>();
        rb = GetComponent<Rigidbody>();
      
    }

    private void Update()
    {
        ReadInput();
    }

    private void FixedUpdate()
    {
        ComputeVelocity();
        ComputeRotation();
        Move();

    }

    private void ReadInput()
    {
        moveInput = moveAction.action.ReadValue<Vector2>();



        if (aimAction != null && aimAction.action != null)
        {
            aimInput = aimAction.action.ReadValue<Vector2>();
            aimInput = Vector2.ClampMagnitude(aimInput, 1f);
        }
    }

    private void Move()
    {
        Quaternion nextRot = Quaternion.Euler(0f, rb.rotation.eulerAngles.y + angularVelocityY * Time.fixedDeltaTime, 0f);


        Vector3 nextPos = transform.position + currentVelocity * Time.fixedDeltaTime;
        rb.MovePosition(nextPos);
        rb.MoveRotation(nextRot);



    }


    private void ComputeVelocity()
    {
        Vector3 targetVelocity = moveSpeed * transform.forward * moveInput.y;
        var direction = targetVelocity - currentVelocity;
        currentVelocity += direction.sqrMagnitude > 0.01f ? direction * acceleration * Time.fixedDeltaTime : Vector3.zero;
    }

    private void ComputeRotation()
    {
        float targetAngularVelocityY = moveInput.x * 180f;
        var direction = targetAngularVelocityY - angularVelocityY;

        angularVelocityY += Mathf.Abs(direction) > 0.01f ? direction * acceleration * Time.fixedDeltaTime : 0f;
    }
}
