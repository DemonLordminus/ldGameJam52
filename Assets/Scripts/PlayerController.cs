using System.Collections.Generic;
using UnityEngine;


namespace playerController
{
    public class PlayerController : MonoBehaviour
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
        public bool isOnGround;
        [Header("���")]
        [Tooltip("����̴���")]
        public int dashCountMax;
        private int dashCount;
        private Vector2 dashDirection;//֮���Ϊprivate
        private Vector2 DashCurrentSpeed;
        private Vector2 playerCurrentSpeed;
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
        [Header("�Ƿ��ֹ��ǽ��")]
        public bool isCloseToEdge;
        public bool isNoSpeedUpWhenTouchEdge;
        private CapsuleCollider2D capsuleCollider;
        [Range(0f, 1f)]
        public float edgeCheckSize;
        private Rigidbody2D rb2d;
        private Vector2 nextPosOffset;
        public bool isUseMovePostion;


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
            capsuleCollider = GetComponent<CapsuleCollider2D>();
        }
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }


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
                DashCurrentSpeed = Vector2.zero;
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
            DashCurrentSpeed += dashDirection.normalized * dashSpeed;
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
            DashCurrentSpeed -= DashCurrentSpeed.normalized * dashSpeedDecay;
            if (++dashNowFrame > dashSpeedDecayFrame)
            {
                dashStateNow = dashState.noDash;
                dashNowFrame = 0;
            }
        }
        private void DashSpeedStop()
        {
            DashCurrentSpeed = new Vector2(0, 0);
        }
        #endregion
    }
}