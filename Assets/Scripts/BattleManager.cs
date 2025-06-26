using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Drawing;
public class BattleManager : MonoBehaviour
{
    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private Image _image;
    private int _numberOfFighters = 2;
    [SerializeField]
    private UnityEvent _onFigthersReady;
    [SerializeField]
    private UnityEvent _onBattleFinished;
    [SerializeField]
    private UnityEvent _onBattleStarted;
    private List<Fighter> _fighters = new List<Fighter>();
    private Coroutine _battleCoroutine;
    private DamageTarget _damageTarget = new DamageTarget();
    public void AddFighter(Fighter fighter)
    {
        _fighters.Add(fighter);
        CheckFighters();
    }
    public void RemoveFighter(Fighter fighter)
    {
        _fighters.Remove(fighter);
        if (_battleCoroutine != null)
        {
            StopCoroutine(_battleCoroutine);
            _battleCoroutine = null;
        }
        _onBattleFinished?.Invoke();
    }
    private void CheckFighters()
    {
        if (_fighters.Count < _numberOfFighters)
        {
            return;
        }
        _onBattleStarted?.Invoke();
    }
    public void StartBattle()
    {
        _battleCoroutine = StartCoroutine(BattleCoroutine());
    }
    private IEnumerator BattleCoroutine()
    {
        _onBattleStarted?.Invoke();
        while (_fighters.Count > 1)
        {
            Fighter attacker = _fighters[Random.Range(0, _fighters.Count)];
            Fighter defender = attacker;
            while (defender == attacker)
            {
                defender = _fighters[Random.Range(0, _fighters.Count)];
            }
            attacker.transform.LookAt(defender.transform);
            defender.transform.LookAt(attacker.transform);
            Attack attack = attacker.Attacks.GetRandomAttack();
            SoundManager.instance.Play(attack.soundName);
            attacker.CharacterAnimator.Play(attack.animationName);
            GameObject attackParticles = Instantiate(attack.particlesPrefab, attacker.transform.position, Quaternion.identity);
            attackParticles.transform.SetParent(attacker.transform);
            yield return new WaitForSeconds(attack.attackTime);
            float damage = Random.Range(attack.minDamage, attack.maxDamage);
            GameObject defendParticles = Instantiate(attack.hitParticlesPrefab, defender.transform.position, Quaternion.identity);
            defendParticles.transform.SetParent(defender.transform);
            _damageTarget.SetDamageTarget(damage, defender.transform);
            defender.health.TakeDamage(_damageTarget);
            if (defender.health.CurrentHealth <= 0)
            {
                RemoveFighter(defender);
            }   
            yield return new WaitForSeconds(1f);
        }
        _onBattleFinished?.Invoke();
    }
}
