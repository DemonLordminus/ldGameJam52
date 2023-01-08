using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace playerController
{
    public class Player : Character
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
        [SerializeField]
        private bool isOnGround;
        [Header("移动")]
        public float moveSpeedUp;//加速度
        public float moveSpeedMax;//最大速度
        public float moveSpeedDecay;//减速度
        [Header("冲刺")]
        [Tooltip("最大冲刺次数")]
        public int dashCountMax;
        private int dashCount;
        private Vector2 dashDirection;//之后改为private
        private Vector2 dashCurrentSpeed;
        private Vector2 playerCurrentSpeed;
        private Vector2 moveCurrentSpeed;
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
        private Rigidbody2D rb2d;
        [Header("检测距离")]
        public float distance;

        [SerializeField]
        private bool isJumpPress;
        //动画部分
        private Animator animator;

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
            animator = GetComponent<Animator>();
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
            if (!isPause)
                HitCheck();
        }
        private void FixedUpdate()
        {
            OnGroundCheck();
            HandleGravity();
            DashMoveHandle();
            //InputSystem.Update();
            JumpHandle();
            Move();
            MoveDecay();
            MoveHandle();
            AnimChange();
        }
        #region 跳跃
        // 跳跃设计为向上冲刺
        public void Jump(InputAction.CallbackContext callback)
        {
            if(callback.phase == InputActionPhase.Started)
            {
                isJumpPress = true;
            } 
        }
        private void JumpHandle()
        { 
            if(isJumpPress)
            {
                Dash(Vector2.up);
                isJumpPress = false;
            }
        }
        #endregion
        #region 移动
        public void MoveDirGet(InputAction.CallbackContext value)
        {
            moveDir = value.ReadValue<float>();
        }

        private float moveDir;
        private void Move()
        {
            moveCurrentSpeed.x += moveDir * moveSpeedUp;
            moveCurrentSpeed.x = Mathf.Clamp(moveCurrentSpeed.x, -moveSpeedMax, moveSpeedMax);
        }
        private void MoveDecay()
        {
            if (moveCurrentSpeed.x != 0 && moveDir == 0)
            {
                int sign = moveCurrentSpeed.x > 0 ? 1 : -1;
                moveCurrentSpeed.x -= sign * moveSpeedDecay;
                if (moveCurrentSpeed.x * sign < 0)
                {
                    moveCurrentSpeed.x = 0;
                }
            }
        }
        #endregion
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
        private void DashSpeedDecay()//空气阻力 施加运动方向相反的力
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
        #region 位置处理
        //处理速度
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
        #region 冲刺次数相关
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
        #region 地面检测
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
        #region 动画处理
        private static readonly int AnimIdle = Animator.StringToHash("PlayerIDLE");
        private static readonly int AnimMove = Animator.StringToHash("PlayerMove");
        private static readonly int AnimJumpUp = Animator.StringToHash("PlayerJumpUp");
        private static readonly int AnimJumpStay = Animator.StringToHash("PlayerJumpStay");
        private static readonly int AnimJumpDown = Animator.StringToHash("PlayerJumpDown");

        private int currentState;
        private float lockTill;
        private int GetState()
        {
            if (Time.time < lockTill)
            { 
                return currentState;
            }
            if(dashStateNow == dashState.speedUp)
            {
                return AnimJumpUp;
            }
            if (dashStateNow == dashState.staySpeed)
            {
                return AnimJumpStay;
            }
            if(dashStateNow == dashState.speedDecay)
            {
                return LockState(AnimJumpDown,0.2f);
            }
            if(isOnGround==false)
            {
                return AnimJumpStay;
            }
            if(Mathf.Abs(moveCurrentSpeed.x)>0)
            {
                return AnimMove;
            }
            return AnimIdle;

            int LockState(int state,float timeLock)
            {
                lockTill = Time.time + timeLock;
                return state;
            }
        }
        private void AnimChange()
        {
            var state = GetState();
            if (state != currentState) 
            {
                animator.CrossFade(state, 0, 0);
                currentState = state;
            }
            if (transform.localScale.x * moveCurrentSpeed.x < 0)
            {
                transform.localScale = new Vector3(transform.localScale.x*-1,transform.localScale.y,transform.localScale.z);
            }
        }
        #endregion
        public override void ResetCharacter()
        {

        }

        public override IEnumerator DamageCharacter(int damage, float interval)
        {
            yield return null;
        }

        [Header("物体判定")]
        public ItemName currentItem;
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

        private void OnEnable()
        {
            EventHandler.GameStateChangedEvent += OnGameStateChangedEvent;
        }

        private void OnDisable()
        {
            EventHandler.GameStateChangedEvent -= OnGameStateChangedEvent;
        }

        private bool isPause;
        private void OnGameStateChangedEvent(GameState gameState)
        {
            isPause = gameState switch
            {
                GameState.GamePlay => false,
                GameState.Pause => true,
                _ => false,
            };
            if (isPause)
                StartCoroutine(delay(0.5f));
        }

        IEnumerator delay(float time)
        {
            yield return new WaitForSeconds(time);
            rb2d.velocity = Vector2.zero;
        }


        public void HitCheck()
        {
            //int layerMask = 7;
            //layerMask = ~layerMask;
            if (Input.GetKey(KeyCode.D))
                direction = new Vector2(1, 0);
            else if(Input.GetKey(KeyCode.A))
                direction = new Vector2(-1,0);
            
            if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Return))
            {
                RaycastHit2D hit = Physics2D.Raycast(rb2d.position, direction, distance, LayerMask.GetMask("Default"));
                if (hit.collider != null)
                {
                    switch (hit.collider.gameObject.tag )
                    {
                        case "Item":
                            var item = hit.collider.gameObject.GetComponent<Item>();
                            item?.ItemClick();
                            break;
                        case "Interactive":
                            var interactive = hit.collider.gameObject.GetComponent<Interactive>();
                            if (holdItem)
                                interactive?.CheckItem(currentItem);
                            else
                                interactive?.EmptyAction();
                            break;
                    }
                }
            }
        }
    }

}