using UnityEngine;
using TMPro;

public class BattleSystem : MonoBehaviour
{
    [Header("전투 기본값")]
    public int playerAttack = 30;
    public int maxEnemyHP = 300;
    public int currentEnemyHP = 300;
    public float critChance = 0.3f; // 30%

    [Header("치명타 통계")]
    public int totalAttackCount = 0;
    public int totalCritCount = 0;

    [Header("UI 연결")]
    public TextMeshProUGUI enemyHPText;
    public TextMeshProUGUI critText;
    public TextMeshProUGUI lootText;
    public TextMeshProUGUI rateText;

    [Header("드랍 확률")]
    public float normalRate = 50f;
    public float advancedRate = 30f;
    public float rareRate = 15f;
    public float legendaryRate = 5f;

    void Start()
    {
        currentEnemyHP = maxEnemyHP;
        UpdateUI();
    }

    public void Attack()
    {
        bool isCritical = RollCritical();
        int damage = playerAttack;

        if (isCritical)
        {
            damage *= 2;
            critText.text = "공격 결과 : 치명타! 데미지 " + damage;
        }
        else
        {
            critText.text = "공격 결과 : 일반 공격 데미지 " + damage;
        }

        currentEnemyHP -= damage;

        if (currentEnemyHP <= 0)
        {
            currentEnemyHP = 0;
            DropItem();
            SpawnNewEnemy();
        }

        UpdateUI();
    }

    bool RollCritical()
    {
        totalAttackCount++;

        float currentCritRate = 0f;
        if (totalAttackCount > 0)
        {
            currentCritRate = (float)totalCritCount / totalAttackCount;
        }

        if (currentCritRate < critChance)
        {
            if (((float)(totalCritCount + 1) / totalAttackCount) <= critChance)
            {
                totalCritCount++;
                return true;
            }
        }

        if (currentCritRate > critChance)
        {
            if (((float)totalCritCount / totalAttackCount) >= critChance)
            {
                return false;
            }
        }

        if (Random.value < critChance)
        {
            totalCritCount++;
            return true;
        }

        return false;
    }

    void DropItem()
    {
        float randomValue = Random.Range(0f, 100f);

        if (randomValue < legendaryRate)
        {
            lootText.text = "획득 아이템 : 전설";
            ResetDropRates();
        }
        else if (randomValue < legendaryRate + rareRate)
        {
            lootText.text = "획득 아이템 : 희귀";
            IncreaseLegendaryRate();
        }
        else if (randomValue < legendaryRate + rareRate + advancedRate)
        {
            lootText.text = "획득 아이템 : 고급";
            IncreaseLegendaryRate();
        }
        else
        {
            lootText.text = "획득 아이템 : 일반";
            IncreaseLegendaryRate();
        }
    }

    void IncreaseLegendaryRate()
    {
        legendaryRate += 1.5f;
        normalRate -= 0.5f;
        advancedRate -= 0.5f;
        rareRate -= 0.5f;

        if (normalRate < 0f) normalRate = 0f;
        if (advancedRate < 0f) advancedRate = 0f;
        if (rareRate < 0f) rareRate = 0f;
    }

    void ResetDropRates()
    {
        normalRate = 50f;
        advancedRate = 30f;
        rareRate = 15f;
        legendaryRate = 5f;
    }

    void SpawnNewEnemy()
    {
        currentEnemyHP = maxEnemyHP;
    }

    void UpdateUI()
    {
        enemyHPText.text = "적 체력 : " + currentEnemyHP + " / " + maxEnemyHP;

        rateText.text =
            "현재 확률\n" +
            "일반 : " + normalRate.ToString("F1") + "%\n" +
            "고급 : " + advancedRate.ToString("F1") + "%\n" +
            "희귀 : " + rareRate.ToString("F1") + "%\n" +
            "전설 : " + legendaryRate.ToString("F1") + "%";
    }
}