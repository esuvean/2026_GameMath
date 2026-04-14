using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class ProbabilitySimulationUI : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI resultText;
    public TextMeshProUGUI itemText;

    [Header("Battle Settings")]
    public int enemyHP = 100;

    [Header("Poisson Distribution")]
    public float lambda = 2f; // 턴마다 등장 적 수

    [Header("Binomial Distribution")]
    public int maxAttackTrials = 5;   // 최대 공격 시도 수
    public float hitChance = 0.6f;    // 명중 확률

    [Header("Normal Distribution")]
    public float meanDamage = 20f;    // 평균 데미지
    public float stdDevDamage = 5f;   // 표준편차

    [Header("Bernoulli Distribution")]
    public float criticalChance = 0.2f;   // 치명타 확률
    public float criticalMultiplier = 2f; // 치명타 배수

    [Header("Rare Item Chance")]
    public float startRareChance = 0.2f;      // 시작 레어 확률 20%
    public float rareChanceIncreasePerTurn = 0.05f; // 턴마다 5% 상승

    int totalTurns;
    int totalEnemyCount;
    int totalKillCount;
    int totalHitCount;
    int totalCritCount;
    float totalDamage;

    Dictionary<string, int> itemCounts = new Dictionary<string, int>();

    public void RunSimulation()
    {
        ResetData();

        StringBuilder consoleLog = new StringBuilder();
        consoleLog.AppendLine("===== 시뮬레이션 시작 =====");

        bool rareAcquired = false;
        float currentRareChance = startRareChance;

        while (!rareAcquired)
        {
            totalTurns++;

            int enemyCount = SamplePoisson(lambda);
            if (enemyCount <= 0)
                enemyCount = 1; // 적이 0명 나오면 너무 허전하니까 최소 1명 처리

            totalEnemyCount += enemyCount;

            consoleLog.AppendLine($"\n[Turn {totalTurns}] 적 등장 수: {enemyCount}, 레어 확률: {currentRareChance * 100f:0}%");

            for (int i = 0; i < enemyCount; i++)
            {
                int currentHP = enemyHP;
                int enemyHits = SampleBinomial(maxAttackTrials, hitChance);
                float enemyTotalDamage = 0f;
                int enemyCrits = 0;

                for (int h = 0; h < enemyHits; h++)
                {
                    float damage = SampleNormal(meanDamage, stdDevDamage);

                    // 음수 데미지 방지
                    if (damage < 0f)
                        damage = 0f;

                    bool isCritical = SampleBernoulli(criticalChance);
                    if (isCritical)
                    {
                        damage *= criticalMultiplier;
                        enemyCrits++;
                        totalCritCount++;
                    }

                    enemyTotalDamage += damage;
                    totalDamage += damage;
                    totalHitCount++;

                    currentHP -= Mathf.RoundToInt(damage);
                }

                consoleLog.AppendLine(
                    $"- 적 {i + 1}: 명중 {enemyHits}회 / 치명타 {enemyCrits}회 / 총 데미지 {enemyTotalDamage:0.0} / 남은 HP {Mathf.Max(currentHP, 0)}"
                );

                if (currentHP <= 0)
                {
                    totalKillCount++;

                    string reward = SampleReward();
                    AddItem(reward);

                    consoleLog.AppendLine($"  > 처치 성공! 보상: {reward}");

                    if (reward == "Weapon" || reward == "Armor")
                    {
                        bool rare = SampleBernoulli(currentRareChance);

                        if (rare)
                        {
                            string rareName = reward == "Weapon" ? "Rare Weapon" : "Rare Armor";
                            AddItem(rareName);

                            consoleLog.AppendLine($"  > 레어 아이템 획득! {rareName}");
                            rareAcquired = true;
                            break;
                        }
                    }
                }
                else
                {
                    consoleLog.AppendLine("  > 적 처치 실패");
                }
            }

            currentRareChance += rareChanceIncreasePerTurn;
            if (currentRareChance > 1f)
                currentRareChance = 1f;
        }

        consoleLog.AppendLine("\n===== 시뮬레이션 종료 =====");
        Debug.Log(consoleLog.ToString());

        UpdateUI();
    }

    void ResetData()
    {
        totalTurns = 0;
        totalEnemyCount = 0;
        totalKillCount = 0;
        totalHitCount = 0;
        totalCritCount = 0;
        totalDamage = 0f;

        itemCounts.Clear();
        itemCounts["Gold"] = 0;
        itemCounts["Weapon"] = 0;
        itemCounts["Armor"] = 0;
        itemCounts["Potion"] = 0;
        itemCounts["Rare Weapon"] = 0;
        itemCounts["Rare Armor"] = 0;
    }

    void UpdateUI()
    {
        if (resultText != null)
        {
            resultText.text =
                $"총 턴 수 : {totalTurns}\n" +
                $"총 등장 적 수 : {totalEnemyCount}\n" +
                $"총 처치 수 : {totalKillCount}\n" +
                $"총 명중 수 : {totalHitCount}\n" +
                $"총 치명타 수 : {totalCritCount}\n" +
                $"총 누적 데미지 : {totalDamage:0.0}\n" +
                $"평균 데미지 : {(totalHitCount > 0 ? totalDamage / totalHitCount : 0f):0.0}";
        }

        if (itemText != null)
        {
            itemText.text =
                "획득한 아이템\n\n" +
                $"Gold : {itemCounts["Gold"]}\n" +
                $"Weapon : {itemCounts["Weapon"]}\n" +
                $"Armor : {itemCounts["Armor"]}\n" +
                $"Potion : {itemCounts["Potion"]}\n" +
                $"Rare Weapon : {itemCounts["Rare Weapon"]}\n" +
                $"Rare Armor : {itemCounts["Rare Armor"]}";
        }
    }

    void AddItem(string itemName)
    {
        if (!itemCounts.ContainsKey(itemName))
            itemCounts[itemName] = 0;

        itemCounts[itemName]++;
    }

    string SampleReward()
    {
        int value = Random.Range(0, 4); // 균등 분포
        switch (value)
        {
            case 0: return "Gold";
            case 1: return "Weapon";
            case 2: return "Armor";
            default: return "Potion";
        }
    }

    bool SampleBernoulli(float p)
    {
        return Random.value < p;
    }

    int SampleBinomial(int trials, float chance)
    {
        int success = 0;

        for (int i = 0; i < trials; i++)
        {
            if (Random.value < chance)
                success++;
        }

        return success;
    }

    int SamplePoisson(float lambdaValue)
    {
        int k = 0;
        float p = 1f;
        float L = Mathf.Exp(-lambdaValue);

        while (p > L)
        {
            k++;
            p *= Random.value;
        }

        return k - 1;
    }

    float SampleNormal(float mean, float stdDev)
    {
        float u1 = Random.value;
        float u2 = Random.value;

        float randStdNormal =
            Mathf.Sqrt(-2.0f * Mathf.Log(u1)) *
            Mathf.Sin(2.0f * Mathf.PI * u2);

        return mean + stdDev * randStdNormal;
    }
}