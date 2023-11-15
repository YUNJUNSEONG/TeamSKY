using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// PlayerController는 플레이어 캐릭터로서 Player 게임 오브젝트를 제어한다.
public class PlayerController : MonoBehaviour {
    public float jumpForce = 250f; // 점프 힘

    private int jumpCount = 0; // 누적 점프 횟수
    private bool isGrounded = false; // 바닥에 닿았는지 나타냄
    private bool isDead = false; // 사망 상태
    private bool isDoubluJump = false; // 더블점프 상태

    private Rigidbody2D playerRigidbody; // 사용할 리지드바디 컴포넌트
    private Animator animator; // 사용할 애니메이터 컴포넌트

    private void Start() {
        // 게임 오브젝트로부터 사용할 컴포넌트들을 가져와 변수에 할당
        playerRigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update() {
        if (isDead) {
            // 사망시 처리를 더 이상 진행하지 않고 종료
            return;
        }

        // 키보드 위 화살표를 눌렀으며 && 최대 점프 횟수(2)에 도달하지 않았다면
        if (Input.GetKeyDown(KeyCode.UpArrow) && jumpCount < 2) {
            // 점프 횟수 증가
            jumpCount++;
            // 점프 직전에 속도를 순간적으로 제로(0, 0)로 변경
            playerRigidbody.velocity = Vector2.zero;
            // 리지드바디에 위쪽으로 힘을 주기
            playerRigidbody.AddForce(new Vector2(0, jumpForce));
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow) && playerRigidbody.velocity.y > 0) {
            // 키보드 위 화살표에서 손을 떼는 순간 && 속도의 y 값이 양수라면 (위로 상승 중)
            // 현재 속도를 절반으로 변경
            playerRigidbody.velocity = playerRigidbody.velocity * 0.5f;
        }

        // 애니메이터의 Grounded, DoubleJump 파라미터를 갱신
        animator.SetBool("Grounded", isGrounded);
        animator.SetBool("DoubleJump", isDoubluJump);

        if (jumpCount == 2) {
            // 더블점프를 해서 누적 점프 횟수가 2가 되면 isDoubluJump를 true로 바꿈
            isDoubluJump = true;
        }
    }

    private void Die() {
        // 애니메이터의 Die 트리거 파라미터를 셋
        animator.SetTrigger("Die");

        // 속도를 제로(0, 0)로 변경
        playerRigidbody.velocity = Vector2.zero;
        // 사망 상태를 true로 변경
        isDead = true;
    }


    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Obstacle" && !isDead) {
            // 충돌한 상대방의 태그가 Obstacle이며 아직 사망하지 않았다면 Die() 실행
            Die();
        }

        /* if (other.tag == "Coin") {
            // 충돌한 상대방의 태그가 Coin
            
        } */
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        // 어떤 콜라이더와 닿았으며, 충돌 표면이 위쪽을 보고 있으면
        if (collision.contacts[0].normal.y > 0.7f)
        {
            // isGrounded를 true로, isDoubluJump를 false로 변경 후 누적 점프 횟수를 0으로 리셋
            isGrounded = true;
            jumpCount = 0;
            isDoubluJump = false;
        }
    }

    private void OnCollisionExit2D(Collision2D collision) {
        // 어떤 콜라이더에서 떼어진 경우 isGrounded를 false로 변경
        isGrounded = false;
    }
}