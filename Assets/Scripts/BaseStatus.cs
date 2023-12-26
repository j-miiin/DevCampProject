
// 모든 능력치 객체의 기본적인 행동 정의 / 몬스터에도 쓰일 예정
public class BaseStatus
{
    public int baseAttack; // 공격력
    public int baseHealth; // 체력
    public int baseDefense; // 방어력
    public int baseAtkSpeed; // 공격 속

    // 치명타 확률과 치명타 데미지는 일반적으로 큰 수치가 아니므로, float 사용
    public float baseCritChance;// 크리티컬 확률
    public int baseCritDamage;// 크리티컬 데미지
}
