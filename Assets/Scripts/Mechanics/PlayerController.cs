using System.Collections;
using UnityEngine;
using Platformer.Gameplay;
using static Platformer.Core.Simulation;
using Platformer.Model;
using Platformer.Core;
using TMPro;


namespace Platformer.Mechanics
{
    /// <summary>
    /// This is the main class used to implement control of the player.
    /// It is a superset of the AnimationController class, but is inlined to allow for any kind of customisation.
    /// </summary>
    public class PlayerController : KinematicObject
    {
        public AudioClip jumpAudio;
        public AudioClip respawnAudio;
        public AudioClip ouchAudio;

        /// <summary>
        /// Max horizontal speed of the player.
        /// </summary>
        public float maxSpeed = 7;
        /// <summary>
        /// Initial jump velocity at the start of a jump.
        /// </summary>
        public float jumpTakeOffSpeed = 7;

        public JumpState jumpState = JumpState.Grounded;
        private bool stopJump;
        /*internal new*/ public Collider2D collider2d;
        /*internal new*/ public AudioSource audioSource;
        public Health health;
        public bool controlEnabled = true;

        bool jump;
        Vector2 move;
        SpriteRenderer spriteRenderer;
        internal Animator animator;
        readonly PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        public TMP_Text scoreText;  // 分數的 UI 文本
        public TMP_Text healthText; // 生命值的 UI 文本
        public TMP_Text bearText;
        public int score = 0;
        public int life = 3;
        public int bear = 0;
        public GameObject victoryPanel; // Reference to the VictoryPanel
        public GameObject[] solidStars; // References to the solid stars (should be set to the first 3 stars)
        public GameObject[] hollowStars; // References to the hollow stars (should be set to the last 3 stars)
        public GameObject legendaryPanel; // Reference to the VictoryPanel
        public GameObject[] solidStars2; // References to the solid stars (should be set to the first 3 stars)
        public GameObject[] hollowStars2; // References to the hollow stars (should be set to the last 3 stars)
        public bool isDying = false;
        public Bounds Bounds => collider2d.bounds;

        void Awake()
        {
            health = GetComponent<Health>();
            audioSource = GetComponent<AudioSource>();
            collider2d = GetComponent<Collider2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();
            controlEnabled = false;
            score = 0;
            bear = 0;
            scoreText.text = "0";
            bearText.text = "0";
            healthText.text = "3";
            //transform.position = new Vector3(0, -0.44f, 1);
            //UnityEngine.Debug.Log($"Awake() : {transform.position}");
        }
        public void ShowVictoryPanel()
        {
            if (bear <= 3)
            {
                if (score >= 500)
                {
                    SetStars(true, true, true);
                }
                else if (score >= 300)
                {
                    SetStars(true, true, false);
                }
                else if (score >= 100)
                {
                    SetStars(true, false, false);
                }
                else
                {
                    SetStars(false, false, false);
                }

                victoryPanel.SetActive(true);
            }
            else
            {
                if (score >= 500)
                {
                    SetStars2(true, true, true);
                }
                else if (score >= 300)
                {
                    SetStars2(true, true, false);
                }
                else if (score >= 100)
                {
                    SetStars2(true, false, false);
                }
                else
                {
                    SetStars2(false, false, false);
                }

                legendaryPanel.SetActive(true);
            }
        }

        void SetStars(bool star1, bool star2, bool star3)
        {
            solidStars[0].SetActive(star1);
            solidStars[1].SetActive(star2);
            solidStars[2].SetActive(star3);

            hollowStars[0].SetActive(!star1);
            hollowStars[1].SetActive(!star2);
            hollowStars[2].SetActive(!star3);
        }
        void SetStars2(bool star1, bool star2, bool star3)
        {
            solidStars2[0].SetActive(star1);
            solidStars2[1].SetActive(star2);
            solidStars2[2].SetActive(star3);

            hollowStars2[0].SetActive(!star1);
            hollowStars2[1].SetActive(!star2);
            hollowStars2[2].SetActive(!star3);
        }

        public void UpdateScoreText()
        {
            scoreText.text = score.ToString();
        }
        public void UpdateLifeText()
        {
            healthText.text = life.ToString();
        }
        public void UpdateBearText()
        {
            bearText.text = bear.ToString();
        }

        protected override void Update()
        {
            
            if (controlEnabled)
            {
                
                move.x = Input.GetAxis("Horizontal");
                if (jumpState == JumpState.Grounded && Input.GetButtonDown("Jump"))
                    jumpState = JumpState.PrepareToJump;
                else if (Input.GetButtonUp("Jump"))
                {
                    stopJump = true;
                    Schedule<PlayerStopJump>().player = this;
                }
            }
            else
            {
                move.x = 0;
            }
            UpdateJumpState();
            base.Update();

            if (life <= 0)
            {
               
                GameOver();
            }
        }
        IEnumerator WaitForGameOverSequence()
        {
            controlEnabled = false;
            yield return new WaitForSeconds(1f);  // Wait for 2 seconds
            MenuManager.Instance.ShowGameOverPanel();
        }
        void GameOver()
        {
            StartCoroutine(WaitForGameOverSequence());
        }
    



        void UpdateJumpState()
        {
            jump = false;
            switch (jumpState)
            {
                case JumpState.PrepareToJump:
                    jumpState = JumpState.Jumping;
                    jump = true;
                    stopJump = false;
                    break;
                case JumpState.Jumping:
                    if (!IsGrounded)
                    {
                        Schedule<PlayerJumped>().player = this;
                        jumpState = JumpState.InFlight;
                    }
                    break;
                case JumpState.InFlight:
                    if (IsGrounded)
                    {
                        Schedule<PlayerLanded>().player = this;
                        jumpState = JumpState.Landed;
                    }
                    break;
                case JumpState.Landed:
                    jumpState = JumpState.Grounded;
                    break;
            }
        }

        protected override void ComputeVelocity()
        {
            if (jump && IsGrounded)
            {
                velocity.y = jumpTakeOffSpeed * model.jumpModifier;
                jump = false;
            }
            else if (stopJump)
            {
                stopJump = false;
                if (velocity.y > 0)
                {
                    velocity.y = velocity.y * model.jumpDeceleration;
                }
            }

            if (move.x > 0.01f)
                spriteRenderer.flipX = false;
            else if (move.x < -0.01f)
                spriteRenderer.flipX = true;

            animator.SetBool("grounded", IsGrounded);
            animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);

            targetVelocity = move * maxSpeed;
        }

        public enum JumpState
        {
            Grounded,
            PrepareToJump,
            Jumping,
            InFlight,
            Landed
        }
 
    
    
    
    
    
   
}
}