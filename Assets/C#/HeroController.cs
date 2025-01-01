// =============================================================================
// 文件名称: HeroController.cs
// 作者: 刘垚
// 创建日期: 2024.11.18
// 更新日期：2024.11.24
// 使用的设计模式：
// 备注：
// =============================================================================
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class HeroController : MonoBehaviour
{
    public static int TEAMID_PLAYER = 0;
    public static int TEAMID_AI = 1;

    public GameObject levelupEffectPrefab;
    public GameObject projectileStart;

    [HideInInspector]
    public int gridType = 0;
    [HideInInspector]
    public int gridPositionX = 0;
    [HideInInspector]
    public int gridPositionZ = 0;

    //英雄所属阵营，0是玩家，1是AI
    [HideInInspector]
    public int teamID = 0;

    [HideInInspector]
    public IHero hero;

    [HideInInspector]
    public float maxHealth = 0;

    [HideInInspector]
    public float currentHealth = 0;

    [HideInInspector]
    public float currentDamage = 0;

    [HideInInspector]
    public int lvl = 1;

    private MyMap map;
    private AIOpponent aIOpponent;
    private HeroAnimation heroAnimation;
    private GameObject target;
    private WorldCanvasController worldCanvasController;


    //NavMeshAgent 是 Unity 中用于实现智能寻路和导航的一个组件
    private NavMeshAgent navMeshAgent;

    private Vector3 gridTargetPosition;

    //该英雄是否在被拖拽状态
    private bool _isDragged = false;
    public bool IsDragged
    {
        get { return _isDragged; }
        set { _isDragged = value; }
    }

    [HideInInspector]
    public bool isAttacking = false;

    //表示死亡状态
    [HideInInspector]
    public bool isDead = false;


    private bool isInCombat = false;
    private float combatTimer = 0;

    //表示被眩晕状态
    private bool isStuned = false;
    private float stunTimer = 0;

    private List<Effect> effects;

    private List<string> equipments;

    private IAttackBehavior attackEffect;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //英雄拖动过程中的处理
        if (_isDragged)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            float enter = 100.0f;
            if (map.m_Plane.Raycast(ray, out enter))
            {
                Vector3 hitPoint = ray.GetPoint(enter);

                Vector3 p = new Vector3(hitPoint.x, 1.0f, hitPoint.z);

                this.transform.position = Vector3.Lerp(this.transform.position, p, 0.1f);
            }
        }

        //这一段没看懂在干啥
        else
        {
            if (GameManager.Instance.currentGameStage == GameStage.Preparation)
            {
                //calc distance
                float distance = Vector3.Distance(gridTargetPosition, this.transform.position);

                if (distance > 0.25f)
                {
                    this.transform.position = Vector3.Lerp(this.transform.position, gridTargetPosition, 0.1f);
                }
                else
                {
                    this.transform.position = gridTargetPosition;
                }
            }
        }

        //战斗逻辑
        if (isInCombat && isStuned == false)
        {
            //确保英雄不宕机
            if (target == null)
            {
                combatTimer += Time.deltaTime;
                if (combatTimer > 0.5f)
                {
                    combatTimer = 0;

                    TryAttackNewTarget();
                }
            }

            //战斗！！！
            if (target != null)
            {
                //旋转朝向目标
                this.transform.LookAt(target.transform, Vector3.up);

                if (target.GetComponent<HeroController>().isDead == true) //如果目标死了
                {
                    //移除目标，停止自动寻路 
                    target = null;
                    navMeshAgent.isStopped = true;
                }
                else
                {
                    //如果当前没有攻击
                    if (isAttacking == false)
                    {
                        //计算距离
                        float distance = Vector3.Distance(this.transform.position, target.transform.position);

                        if (distance < hero.AttackRange)
                        {
                            DoAttack();
                        }
                        else
                        {
                            navMeshAgent.destination = target.transform.position;
                        }
                    }
                }
            }
        }

        if (isStuned)
        {
            stunTimer -= Time.deltaTime;

            if (stunTimer < 0)
            {
                isStuned = false;

                heroAnimation.IsAnimated(true);

                if (target != null)
                {
                    navMeshAgent.destination = target.transform.position;

                    navMeshAgent.isStopped = false;
                }
            }
        }
    }

    //初始化函数
    public void Init(int heroIndex, int _teamID)
    {
        hero = GameManager.Instance.gameHeroData.herosArray[heroIndex];
        teamID = _teamID;

        //store scripts
        map = GameObject.Find("Scripts").GetComponent<MyMap>();
        aIOpponent = GameObject.Find("Scripts").GetComponent<AIOpponent>();
        worldCanvasController = GameObject.Find("Scripts").GetComponent<WorldCanvasController>();
        navMeshAgent = this.GetComponent<NavMeshAgent>();
        heroAnimation = this.GetComponent<HeroAnimation>();

        //disable agent
        navMeshAgent.enabled = false;

        //set stats
        maxHealth = hero.Health;
        currentHealth = hero.Health;
        currentDamage = hero.Damage;

        worldCanvasController.AddHealthBar(this.gameObject);

        effects = new List<Effect>();

        equipments = new List<string>();
        //随机生成装备
        if (Random.Range(0, 10) < 5)
        {
            equipments.Add("Vampire");
        }
        if (Random.Range(0, 10) < 5)
        {
            equipments.Add("Killer");
        }

        //用装饰器修饰攻击效果
        attackEffect = new AttackEffect();
        foreach (string equipment in equipments)
        {
            switch (equipment)
            {
                case "Vampire":
                    attackEffect = new Vampire(attackEffect);
                    break;
                case "Killer":
                    attackEffect = new Killer(attackEffect);
                    break;
            }
        }
    }

    //在战斗结束后对英雄进行重置
    public void Reset()
    {
        this.gameObject.SetActive(true);

        //重设状态
        maxHealth = hero.Health * lvl;
        currentHealth = hero.Health * lvl;
        isDead = false;
        isInCombat = false;
        target = null;
        isAttacking = false;

        //重设位置
        SetWorldPosition();
        SetWorldRotation();

        //去除所有影响
        foreach (Effect e in effects)
        {
            e.Remove();
        }

        effects = new List<Effect>();
    }

    //设置英雄的网格坐标
    public void SetGridPosition(int _gridType, int _gridPositionX, int _gridPositionZ)
    {
        gridType = _gridType;
        gridPositionX = _gridPositionX;
        gridPositionZ = _gridPositionZ;

        gridTargetPosition = GetWorldPosition();
    }

    //将网格坐标转换为世界坐标
    public Vector3 GetWorldPosition()
    {
        Vector3 worldPosition = Vector3.zero;

        if (gridType == MyMap.GRIDTYPE_OWN_INVENTORY)
        {
            worldPosition = map.ownInventoryGridPositions[gridPositionX];
        }
        else if (gridType == MyMap.GRIDTYPE_HEXA_MAP)
        {
            worldPosition = map.mapGridPositions[gridPositionX, gridPositionZ];
        }

        return worldPosition;
    }

    //将英雄位置设置为世界坐标
    public void SetWorldPosition()
    {
        navMeshAgent.enabled = false;

        //get world position
        Vector3 worldPosition = GetWorldPosition();

        this.transform.position = worldPosition;

        gridTargetPosition = worldPosition;
    }

    //设置英雄的旋转
    public void SetWorldRotation()
    {
        Vector3 rotation = Vector3.zero;

        if (teamID == 0)
        {
            rotation = new Vector3(0, 200, 0);
        }
        else if (teamID == 1)
        {
            rotation = new Vector3(0, 20, 0);
        }

        this.transform.rotation = Quaternion.Euler(rotation);
    }

    //英雄升级函数
    public void UpgradeLevel()
    {
        lvl++;

        float newSize = 1;
        maxHealth = hero.Health;
        currentHealth = hero.Health;

        if (lvl == 2)
        {
            newSize = 1.5f;
            maxHealth = hero.Health * 2;
            currentHealth = hero.Health * 2;
            currentDamage = hero.Damage * 2;

        }

        if (lvl == 3)
        {
            newSize = 2f;
            maxHealth = hero.Health * 3;
            currentHealth = hero.Health * 3;
            currentDamage = hero.Damage * 3;
        }

        //设置大小
        this.transform.localScale = new Vector3(newSize, newSize, newSize);

        //实例化
        GameObject levelupEffect = Instantiate(levelupEffectPrefab);

        //设置位置
        levelupEffect.transform.position = this.transform.position;

        //删除
        Destroy(levelupEffect, 1.0f);
    }

    //寻敌函数
    private GameObject FindTarget()
    {
        GameObject closestEnemy = null;
        float bestDistance = 1000;

        if (teamID == TEAMID_PLAYER)
        {
            for (int x = 0; x < MyMap.hexMapSizeX; x++)
            {
                for (int z = 0; z < MyMap.hexMapSizeZ / 2; z++)
                {
                    if (aIOpponent.gridHerosArray[x, z] != null)
                    {
                        HeroController championController = aIOpponent.gridHerosArray[x, z].GetComponent<HeroController>();

                        if (championController.isDead == false)
                        {
                            //计算距离
                            float distance = Vector3.Distance(this.transform.position, aIOpponent.gridHerosArray[x, z].transform.position);

                            //更新最近距离与最近敌人
                            if (distance < bestDistance)
                            {
                                bestDistance = distance;
                                closestEnemy = aIOpponent.gridHerosArray[x, z];
                            }
                        }
                    }
                }
            }
        }
        else if (teamID == TEAMID_AI)
        {
            for (int x = 0; x < MyMap.hexMapSizeX; x++)
            {
                for (int z = 0; z < MyMap.hexMapSizeZ / 2; z++)
                {
                    if (GameManager.Instance.gridHerosArray[x, z] != null)
                    {
                        HeroController championController = GameManager.Instance.gridHerosArray[x, z].GetComponent<HeroController>();

                        if (championController.isDead == false)
                        {
                            float distance = Vector3.Distance(this.transform.position, GameManager.Instance.gridHerosArray[x, z].transform.position);

                            if (distance < bestDistance)
                            {
                                bestDistance = distance;
                                closestEnemy = GameManager.Instance.gridHerosArray[x, z];
                            }
                        }
                    }
                }
            }
        }
        return closestEnemy;
    }

    //尝试攻击函数
    private void TryAttackNewTarget()
    {
        //寻敌
        target = FindTarget();

        if (target != null)
        {
            //设置寻路目标位置
            navMeshAgent.destination = target.transform.position;
            //激活寻路
            navMeshAgent.isStopped = false;
        }
    }

    //战斗阶段开始时调用
    public void OnCombatStart()
    {
        IsDragged = false;

        this.transform.position = gridTargetPosition;

        if (gridType == MyMap.GRIDTYPE_HEXA_MAP)
        {
            isInCombat = true;

            navMeshAgent.enabled = true;

            TryAttackNewTarget();
        }
    }

    //攻击函数
    private void DoAttack()
    {
        isAttacking = true;

        //停止自动寻路
        navMeshAgent.isStopped = true;

        heroAnimation.DoAttack(true);
    }

    //在攻击动作结束后调用
    public void OnAttackAnimationFinished()
    {
        isAttacking = false;

        if (target != null)
        {
            //得到敌人的英雄控制器
            HeroController targetHero = target.GetComponent<HeroController>();

            List<HeroBonus> activeBonuses = null;

            if (teamID == TEAMID_PLAYER)
                activeBonuses = GameManager.Instance.activeBonusList;
            else if (teamID == TEAMID_AI)
                activeBonuses = aIOpponent.activeBonusList;


            float d = 0;
            foreach (HeroBonus b in activeBonuses)
            {
                d += b.ApplyOnAttack(this, targetHero);
            }

            //伤害效果
            bool isTargetDead = attackEffect.Attack(targetHero, this, d + currentDamage);

            //如果敌人死了就尝试攻击下一个目标
            if (isTargetDead)
                TryAttackNewTarget();

            //如果有投射物则建立投射物
            if (hero.AttackProjectile != null && projectileStart != null)
            {
                GameObject projectile = Instantiate(hero.AttackProjectile);
                projectile.transform.position = projectileStart.transform.position;
                projectile.GetComponent<Projectile>().Init(target);
            }
        }
    }

    //受到攻击函数
    public bool OnGotHit(float damage)
    {
        List<HeroBonus> activeBonuses = null;

        if (teamID == TEAMID_PLAYER)
            activeBonuses = GameManager.Instance.activeBonusList;
        else if (teamID == TEAMID_AI)
            activeBonuses = aIOpponent.activeBonusList;

        foreach (HeroBonus b in activeBonuses)
        {
            damage = b.ApplyOnGotHit(this, damage);
        }

        currentHealth -= damage;

        //死亡
        if (currentHealth <= 0)
        {
            this.gameObject.SetActive(false);
            isDead = true;

            //每有一个英雄阵亡都要判断是否有一方所有英雄均死亡
            aIOpponent.OnHeroDeath();
            GameManager.Instance.OnHeroDeath();
        }

        worldCanvasController.AddDamageText(this.transform.position + new Vector3(0, 2.5f, 0), damage);

        return isDead;
    }

    //当英雄被眩晕时调用
    public void OnGotStun(float duration)
    {
        isStuned = true;
        stunTimer = duration;

        heroAnimation.IsAnimated(false);

        navMeshAgent.isStopped = true;
    }

    //当英雄被治疗后调用
    public void OnGotHeal(float f)
    {
        worldCanvasController.AddHealText(this.transform.position + new Vector3(0, 2.5f, 0), f);
        currentHealth += f;
    }

    //给英雄添加影响
    public void AddEffect(GameObject effectPrefab, float duration)
    {
        if (effectPrefab == null)
            return;

        //look for effect
        bool foundEffect = false;
        foreach (Effect e in effects)
        {
            if (effectPrefab == e.effectPrefab)
            {
                e.duration = duration;
                foundEffect = true;
            }
        }

        //not found effect
        if (foundEffect == false)
        {
            Effect effect = this.gameObject.AddComponent<Effect>();
            effect.Init(effectPrefab, this.gameObject, duration);
            effects.Add(effect);
        }

    }

    //去除英雄所有影响
    public void RemoveEffect(Effect effect)
    {
        effects.Remove(effect);
        effect.Remove();
    }

    public void Skill()
    {
        hero.Skill();
    }
}
