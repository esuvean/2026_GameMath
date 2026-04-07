using UnityEngine;
using TMPro;

public class DamageSimulator : MonoBehaviour
{
    public enum WeaponType
    {
        Dagger,
        LongSword,
        Axe
    }

    [Header("UI")]
    public TextMeshProUGUI damageText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI baseDamageText;
    public TextMeshProUGUI summaryText;   // 요약 표시용 추가

    [Header("Player")]
    public int level = 1;

    [Header("Weapon")]
    public WeaponType currentWeapon;

    float baseDamage;
    float stdDev;

    float critChance;
    float critMultiplier;

    float totalDamage;
    int attackCount;

    int weakCount;
    int missCount;
    int critCount;

    float maxDamage;

    void Start()
    {
        Initialize();
        ApplyWeapon(WeaponType.Dagger);
        UpdateUI();
    }

    void Initialize()
    {
        totalDamage = 0;
        attackCount = 0;
        weakCount = 0;
        missCount = 0;
        critCount = 0;
        maxDamage = 0;

        baseDamage = level * 20f;
    }

    public void LevelUp()
    {
        level++;
        Initialize();
        ApplyWeapon(currentWeapon);
        UpdateUI();
    }

    public void EquipDagger()
    {
        Initialize();
        ApplyWeapon(WeaponType.Dagger);
        UpdateUI();
    }

    public void EquipLongSword()
    {
        Initialize();
        ApplyWeapon(WeaponType.LongSword);
        UpdateUI();
    }

    public void EquipAxe()
    {
        Initialize();
        ApplyWeapon(WeaponType.Axe);
        UpdateUI();
    }

    void ApplyWeapon(WeaponType weapon)
    {
        currentWeapon = weapon;

        if (weapon == WeaponType.Dagger)
        {
            stdDev = baseDamage * 0.1f;
            critChance = 0.4f;
            critMultiplier = 1.5f;
        }
        else if (weapon == WeaponType.LongSword)
        {
            stdDev = baseDamage * 0.2f;
            critChance = 0.3f;
            critMultiplier = 2f;
        }
        else
        {
            stdDev = baseDamage * 0.3f;
            critChance = 0.2f;
            critMultiplier = 3f;
        }
    }

    public void Attack()
    {
        float normalDamage = GaussianRandom(baseDamage, stdDev);   // 일반 데미지
        float finalDamage = normalDamage;

        float weakThreshold = baseDamage + 2f * stdDev;
        float missThreshold = baseDamage - 2f * stdDev;

        bool isWeak = normalDamage > weakThreshold;
        bool isMiss = normalDamage < missThreshold;
        bool isCritical = Random.value < critChance;

        // 명중 실패 먼저 처리
        if (isMiss)
        {
            finalDamage = 0f;
            missCount++;
        }
        else
        {
            // 크리티컬
            if (isCritical)
            {
                finalDamage *= critMultiplier;
                critCount++;
            }

            // 약점 공격
            if (isWeak)
            {
                finalDamage *= 2f;
                weakCount++;
            }
        }

        totalDamage += finalDamage;
        attackCount++;

        if (finalDamage > maxDamage)
        {
            maxDamage = finalDamage;
        }

        string resultText = "";

        if (isMiss)
            resultText += "[MISS] ";
        else
        {
            if (isCritical) resultText += "[CRIT] ";
            if (isWeak) resultText += "[WEAK] ";
        }

        damageText.text = resultText + "Damage : " + finalDamage.ToString("F1");

        UpdateUI();

        Debug.Log(resultText + "Damage : " + finalDamage.ToString("F1"));
    }
    void ResetBattleStats()
    {
        totalDamage = 0;
        attackCount = 0;
        weakCount = 0;
        missCount = 0;
        critCount = 0;
        maxDamage = 0;
    }
    public void Attack1000()
    {
        // 1000회 공격용 통계 초기화
        totalDamage = 0;
        attackCount = 0;
        weakCount = 0;
        missCount = 0;
        critCount = 0;
        maxDamage = 0;

        for (int i = 0; i < 1000; i++)
        {
            Attack();
        }

        float avgDamage = attackCount > 0 ? totalDamage / attackCount : 0f;

        summaryText.text =
            "=== 1000회 공격 결과 ===\n" +
            "총 공격 횟수 : " + attackCount + "\n" +
            "약점 공격 횟수 : " + weakCount + "\n" +
            "명중 실패 횟수 : " + missCount + "\n" +
            "크리티컬 횟수 : " + critCount + "\n" +
            "최대 데미지 : " + maxDamage.ToString("F1") + "\n" +
            "평균 데미지 : " + avgDamage.ToString("F1");
    }

    float GaussianRandom(float mean, float stdDev)
    {
        float u1 = Random.Range(0.0001f, 1f);
        float u2 = Random.Range(0f, 1f);

        float z = Mathf.Sqrt(-2f * Mathf.Log(u1)) *
                  Mathf.Cos(2f * Mathf.PI * u2);

        return mean + z * stdDev;
    }

    void UpdateUI()
    {
        levelText.text = "Level : " + level;
        baseDamageText.text =
            "Base Damage : " + baseDamage.ToString("F1") +
            "\nStd Dev : " + stdDev.ToString("F1") +
            "\nCrit Chance : " + (critChance * 100f).ToString("F0") + "%" +
            "\nCrit Multiplier : x" + critMultiplier.ToString("F1");

        if (summaryText != null)
        {
            float avgDamage = attackCount > 0 ? totalDamage / attackCount : 0f;

            summaryText.text =
                "총 데미지 : " + totalDamage.ToString("F1") + "\n" +
                "공격 횟수 : " + attackCount + "\n" +
                "평균 데미지 : " + avgDamage.ToString("F1") + "\n" +
                "약점 공격 : " + weakCount + "\n" +
                "명중 실패 : " + missCount + "\n" +
                "크리티컬 : " + critCount + "\n" +
                "최대 데미지 : " + maxDamage.ToString("F1");
        }
    }
}