using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace playerController
{
    public class Player : Character
    {
        [Header("����")]
        public bool isEnableGravity;
        private Vector2 GravityCurrentSpeed;
        [Range(0f, 1f)]
        public float PlayerGravity;
        [Range(0f, 1f)]
        public float PlayerGravitySpeedMax;
        [Header("��ײ��")]
        [Tooltip("���ͼ��index")]
        public int playerMask;
        public Vector2 groundCheckPos;
        public Vector2 groundCheckSize;
        private Vector3 colliderSize, colliderPosition;
        private Collider2D[] colliders;
        [SerializeField]
        private bool isOnGround;
        [Header("�ƶ�")]
        public float moveSpeedUp;//���ٶ�
        public float moveSpeedMax;//����ٶ�
        public float moveSpeedDecay;//���ٶ�
        [Header("���")]
        [Tooltip("����̴���")]
        public int dashCountMax;
        private int dashCount;
        private Vector2 dashDirection;//֮���Ϊprivate
        private Vector2 dashCurrentSpeed;
        private Vector2 playerCurrentSpeed;
        private Vector2 moveCurrentSpeed;
        private dashState dashStateNow;
        private int dashNowFrame;
        [Header("��̹����˶�����")]
        [Header("��̼���")]
        [Tooltip("��̼��ٵ�֡��")]
        public int dashSpeedUpFrame;//��̼��ٵ�֡��
        [Tooltip("ÿ֡���ٵ��ٶ�")]
        public float dashSpeed;//ÿ֡���ٵ��ٶ�
        [Header("�ٶȱ���״̬")]
        public int dashSpeedStayFrame;
        [Header("��������")]
        public int dashSpeedDecayFrame;
        public float dashSpeedDecay;
        private Rigidbody2D rb2d;



        enum dashState
        {
            noDash = 0,
            speedUp = 1,
            staySpeed = 2,
            speedDecay = 3
        }
        private void Awake()
        {
            rb2d = GetComponent<Rigidbody2D>();
        }
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            OnGroundCheck();
            ResetDashCountOnGround();
        }
        private void FixedUpdate()
        {
            OnGroundCheck();
            HandleGravity();
            DashMoveHandle();
            InputSystem.Update();
            Move();
            MoveDecay();
            MoveHandle();
        }
        #region ��Ծ
        // ��Ծ���Ϊ���ϳ��
        public void Jump()
        {
            Dash(Vector2.up);
        }
        #endregion
        #region �ƶ�
        public void MoveDirGet(InputAction.CallbackContext value)
        {
             moveDir = value.ReadValue<float>();
        }

        private float moveDir; 
       private void Move()
        {
            moveCurrentSpeed.x += moveDir * moveSpeedUp;
            moveCurrentSpeed.x = Mathf.Clamp(moveCurrentSpeed.x, -moveSpeedMax,moveSpeedMax);
        }
        private void MoveDecay()
        { 
           if(moveCurrentSpeed.x!=0 && moveDir==0)
            {
                int sign = moveCurrentSpeed.x>0 ? 1 : -1;
                moveCurrentSpeed.x -= sign * moveSpeedDecay;
                if(moveCurrentSpeed.x * sign < 0)
                {
                    moveCurrentSpeed.x = 0;
                }
            }
        }
        #endregion
        #region ����
        //��������
        private void HandleGravity()
        {
            if (isEnableGravity && !isOnGround)
            {
                float _speed = -Mathf.Clamp(-GravityCurrentSpeed.y + PlayerGravity, 0, PlayerGravitySpeedMax);
                GravityCurrentSpeed = new Vector2(0, _speed);
            }
            else
            {
                GravityCurrentSpeed = new Vector2(0, 0);
            }
        }
        #endregion
        #region ��̴���
        //�ص�:���
        [EditorButton]
        public void Dash(Vector2 _dashDireciton)
        {
            if (--dashCount >= 0)
            {
                dashDirection = _dashDireciton;
                dashNowFrame = 0;
                dashStateNow = dashState.speedUp;
                dashCurrentSpeed = Vector2.zero;
            }
        }
        private void DashMoveHandle()
        {
            switch (dashStateNow)
            {
                case dashState.noDash: DashSpeedStop(); break;
                case dashState.speedUp: DashSpeedup(); break;
                case dashState.staySpeed: DashSpeedStay(); break;
                case dashState.speedDecay: DashSpeedDecay(); break;
            }
        }
        private void DashSpeedup()
        {
            dashCurrentSpeed += dashDirection.normalized * dashSpeed;
            if (++dashNowFrame > dashSpeedUpFrame)
            {
                dashStateNow = dashState.staySpeed;
                dashNowFrame = 0;
            }
        }

        private void DashSpeedStay()
        {
            if (++dashNowFrame > dashSpeedStayFrame)
            {
                dashStateNow = dashState.speedDecay;
                dashNowFrame = 0;
            }
        }
        private void DashSpeedDecay()//�������� ʩ���˶������෴����
        {
            dashCurrentSpeed -= dashCurrentSpeed.normalized * dashSpeedDecay;
            if (++dashNowFrame > dashSpeedDecayFrame)
            {
                dashStateNow = dashState.noDash;
                dashNowFrame = 0;
            }
        }
        private void DashSpeedStop()
        {
            dashCurrentSpeed = new Vector2(0, 0);
        }
        #endregion
        #region λ�ô���
        //�����ٶ�
        private void MoveHandle()
        {
            //Debug.Log(playerCurrentSpeed);
            playerCurrentSpeed = dashCurrentSpeed + GravityCurrentSpeed + moveCurrentSpeed;
            Vector2 nowPos = transform.position;
            rb2d.MovePosition(playerCurrentSpeed + nowPos);
            //rb2d.velocity = playerCurrentSpeed;
            Vector2 pos = transform.position;
            Debug.DrawLine(transform.position, pos + dashCurrentSpeed * 10, Color.red);
        }
        #endregion
        #region ��̴������
        private bool ResetDashCountOnGround()
        {
            if (isOnGround && dashStateNow != dashState.speedUp)
            {
                ResetDashCount();
                return true;
            }
            return false;
        }
        public int ResetDashCount()
        {
            dashCount = dashCountMax;
            return dashCount;
        }
        #endregion
        #region ������
        bool OnGroundCheck()
        {
            Vector2 pos = transform.position;
            colliderPosition = groundCheckPos + pos;
            colliderSize = groundCheckSize;
            LayerMask ignoreMask = ~(1 << playerMask);
            colliders = Physics2D.OverlapBoxAll(colliderPosition, colliderSize, 0, ignoreMask);
            if (colliders.Length != 0)
            {
                isOnGround = true;
                return true;
            }
            else
            {
                isOnGround = false;
                return false;
            }
        }
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            //Vector2 showSize = new Vector2(colliderSize.x * 2, colliderSize.y);
            Gizmos.DrawWireCube(colliderPosition, colliderSize);
        }
#endif

        #endregion

        public override void ResetCharacter()
        {

        }

        public override IEnumerator DamageCharacter(int damage, float interval)
        {
            yield return null;  
        }

        public override void TriggerEvent(Collider2D collsion)
        {
            switch (collsion.gameObject.tag)
            {
                case "Teleport":
                    var teleport = collsion.gameObject.GetComponent<Teleport>();
                    teleport?.TeleportToScene();
                    break;
            }
        }
    }

}