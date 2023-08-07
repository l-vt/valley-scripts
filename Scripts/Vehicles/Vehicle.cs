using Camera;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;

namespace Vehicles
{
    public class Vehicle : MonoBehaviour
    {
        [SerializeField] private Transform[] m_wheels;
        [SerializeField] private Rigidbody m_motorSphere;
        [SerializeField] private Transform m_visuals;

        [SerializeField] private float m_forwardSpeed;
        [SerializeField] private float m_reverseSpeed;
        [SerializeField] private float m_turnSpeed;
        [SerializeField] private float m_currentSpeed;


        [SerializeField] private LayerMask m_groundLayer;
        [SerializeField] private float m_groundedCheckLength = 0.85f;

        [Header("Driver")] [SerializeField] private bool m_isOccupied;

        [SerializeField] private Transform m_driverDoor;

        [Header("Events")] public UnityEvent OnEnterCar = new();

        public UnityEvent OnLeaveCar = new();
        public UnityEvent OnStartBackwards = new();
        public UnityEvent OnStopBackwards = new();
        public UnityEvent OnStartGrounded = new();
        public UnityEvent OnStopGrounded = new();
        private float m_forwardSpeedAcceleration;
        private bool m_isGrounded = true;

        private float m_moveInput;

        private bool m_movesForward;
        private float m_movingAngle;
        private Entities.Player.Player m_occupant;
        private float m_turnInput;
        public bool IsOccupied => m_isOccupied;

        private void Update()
        {
            HandlePlayerInput();

            var realSpeed = m_motorSphere.velocity.magnitude * 0.05f;
            m_movesForward = Vector3.Angle(transform.forward, m_motorSphere.velocity.normalized) < 90;
            var rotationSpeed = Mathf.Atan(realSpeed * 2);


            CalculateCurrentSpeed();

            // rotation
            var newRotation = m_turnInput * m_turnSpeed * Time.deltaTime * rotationSpeed;
            transform.Rotate(0, newRotation, 0, Space.World);

            m_movingAngle = (Vector3.Angle(transform.right, m_motorSphere.velocity.normalized) - 90f) * realSpeed *
                            0.2f;

            // visuals: car rotation
            m_visuals.rotation = transform.rotation * Quaternion.AngleAxis(m_movingAngle, Vector3.forward);

            // visuals: wheels rotation
            foreach (var wheel in m_wheels)
                wheel.transform.localRotation =
                    Quaternion.AngleAxis(m_turnInput * 30f * (m_movesForward ? 1 : -1), Vector3.up);

            // visuals/whole: car position
            transform.position = m_motorSphere.transform.position;

            var wasGrounded = m_isGrounded;
            RaycastHit hit;
            m_isGrounded = Physics.Raycast(transform.position, -transform.up, out hit, m_groundedCheckLength,
                m_groundLayer);

            if (wasGrounded && !m_isGrounded)
                // no longer grounded
                OnStopGrounded.Invoke();
            if (!wasGrounded && m_isGrounded)
                // is now grounded
                OnStartGrounded.Invoke();

            if (m_isGrounded)
                transform.rotation = Quaternion.Lerp(transform.rotation,
                    Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation, 15f * Time.deltaTime);
        }

        private void FixedUpdate()
        {
            if (m_isGrounded)
            {
                m_motorSphere.drag = 3f;
                m_motorSphere.AddForce(transform.forward * m_currentSpeed, ForceMode.Acceleration);
            }
            else
            {
                m_motorSphere.drag = 0.2f;
                m_motorSphere.AddForce(Vector3.up * -(9.81f + 9.81f * m_motorSphere.drag), ForceMode.Acceleration);
            }
        }

        private void OnEnable()
        {
            m_motorSphere.transform.parent = null;
        }

        private void OnDrawGizmos()
        {
            // visual forward direction
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + transform.forward);

            // motor sphere forward direction
            Gizmos.color = Color.green;
            Gizmos.DrawLine(m_motorSphere.transform.position,
                m_motorSphere.transform.position + m_motorSphere.velocity.normalized);

            // raycast length for grounded check
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, transform.position - transform.up * m_groundedCheckLength);

            Gizmos.color = Color.white;
            Gizmos.DrawLine(transform.position, transform.position - transform.up * m_forwardSpeedAcceleration * 0.3f);
        }

        public void SetOccupant([CanBeNull] Entities.Player.Player player)
        {
            m_occupant = player;
            m_isOccupied = player != null;
        }

        public void EnterCar(Entities.Player.Player player)
        {
            OnEnterCar.Invoke();

            SetOccupant(player);
            m_occupant.gameObject.SetActive(false);
            GameManager.Instance.GameCamera.AddTarget(transform, GameCamera.ZoomLevel.Wide);
            GameManager.Instance.SetGameMode(GameManager.GameMode.Driving);
        }

        public void LeaveCar()
        {
            OnLeaveCar.Invoke();

            m_occupant.transform.position = m_driverDoor ? m_driverDoor.position : transform.position;
            m_occupant.gameObject.SetActive(true);

            SetOccupant(null);

            GameManager.Instance.GameCamera.RemoveTarget(transform);
            GameManager.Instance.SetGameMode(GameManager.GameMode.Walking);
        }

        private void HandlePlayerInput()
        {
            if (!m_isGrounded) return;

            m_moveInput = IsOccupied ? Input.GetAxisRaw("Vertical") : 0;
            m_turnInput = (IsOccupied ? Input.GetAxisRaw("Horizontal") : 0) * (m_movesForward ? 1 : -1);

            if (!IsOccupied) return;

            var leaveOccupied = Input.GetKeyDown(KeyCode.Space);
            if (leaveOccupied) LeaveCar();
        }

        private void CalculateCurrentSpeed()
        {
            // fake drag for acceleration
            var fSpeed = m_forwardSpeedAcceleration;
            if (m_moveInput > 0)
            {
                if (fSpeed < m_forwardSpeed)
                    // drive forward
                    m_forwardSpeedAcceleration += m_forwardSpeed * 0.5f * Time.deltaTime;
                if (fSpeed < 0)
                    // drive backward and accelerate
                    m_forwardSpeedAcceleration += m_forwardSpeed * 0.9f * Time.deltaTime;
            }
            else if (m_moveInput < 0)
            {
                if (fSpeed > -m_reverseSpeed)
                    // drive backward
                    m_forwardSpeedAcceleration -= m_forwardSpeed * 0.5f * Time.deltaTime;
                if (fSpeed > 0)
                    // drive forward and brake
                    m_forwardSpeedAcceleration -= m_forwardSpeed * 0.8f * Time.deltaTime;
            }
            else
            {
                if (fSpeed > 0)
                    // driving forward, not accelerating
                    m_forwardSpeedAcceleration -= m_forwardSpeed * 0.6f * Time.deltaTime;
                if (fSpeed < 0)
                    // driving backward, not accelerating
                    m_forwardSpeedAcceleration += m_forwardSpeed * 0.6f * Time.deltaTime;
            }

            m_currentSpeed = m_forwardSpeedAcceleration;
        }
    }
}