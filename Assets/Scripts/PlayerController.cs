using System.Collections.Generic;
using UnityEngine;


namespace playerController
{
    public class PlayerController : MonoBehaviour
    {
        [Header("重力")]
        public bool isEnableGravity;
        private Vector2 GravityCurrentSpeed;
        [Range(0f, 1f)]
        public float PlayerGravity;
        [Range(0f, 1f)]
        public float PlayerGravitySpeedMax;
        [Header("碰撞体")]
        [Tooltip("玩家图层index")]
        public int playerMask;
        public Vector2 groundCheckPos;
        public Vector2 groundCheckSize;
        private Vector3 colliderSize, colliderPosition;
        private Collider2D[] colliders;
        public bool isOnGround;
        [Header("冲刺")]
        [Tooltip("最大冲刺次数")]
        public int dashCountMax;
        private int dashCount;
        private Vector2 dashDirection;//之后改为private
        private Vector2 DashCurrentSpeed;
        private Vector2 playerCurrentSpeed;
        private dashState dashStateNow;
        private int dashNowFrame;
        [Header("冲刺过程运动调整")]
        [Header("冲刺加速")]
        [Tooltip("冲刺加速的帧率")]
        public int dashSpeedUpFrame;//冲刺加速的帧率
        [Tooltip("每帧加速的速度")]
        public float dashSpeed;//每帧加速的速度
        [Header("速度保持状态")]
        public int dashSpeedStayFrame;
        [Header("空气阻力")]
        public int dashSpeedDecayFrame;
        public float dashSpeedDecay;
        [Header("是否防止卡墙里")]
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


        #region 重力
        //处理重力
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
        #region 冲刺处理
        //重点:冲刺
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
        private void DashSpeedDecay()//空气阻力 施加运动方向相反的力
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