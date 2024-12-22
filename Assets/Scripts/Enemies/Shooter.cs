using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour, IEnemy
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletMoveSpeed;
    [SerializeField] private int brustCount;
    [SerializeField] private int projectilePerBrust;
    [SerializeField][Range(0, 359)] private float angleSpread;
    [SerializeField] private float startingDistance = 0.1f;
    [SerializeField] private float timeBetweenBrusts;
    [SerializeField] private float restTime = 1f;
    [SerializeField] private bool stagger;
    [Tooltip("Stagger must be enable for oscillate to function properly.")]
    [SerializeField] private bool oscillate;

    private bool isShooting = false;

    private void OnValidate()
    {
        if(oscillate) { stagger = true; }
        if(!oscillate) { stagger = false; }
        if(projectilePerBrust < 1) { projectilePerBrust = 1; }
        if(brustCount < 1) { brustCount = 1; }
        if(timeBetweenBrusts < 0.1f) { timeBetweenBrusts = 0.1f; }
        if(restTime < 0.1f) { restTime = 0.1f; }
        if(startingDistance < 0.1f) { startingDistance = 0.1f; }
        if(angleSpread == 0) { projectilePerBrust = 1; }
        if( bulletMoveSpeed <= 0 ) { bulletMoveSpeed = 0.1f; }
    }

    public void Attack()
    {
        if(!isShooting)
        {
            StartCoroutine(ShootRoutine());
        }
        
    }

    //private IEnumerator ShootRoutine()
    //{
    //    isShooting = true;

    //    for (int i = 0; i < brustCount; i++)
    //    {
    //        Vector2 tartgetDirection = PlayerController.Instance.transform.position - transform.position;
             
    //        GameObject newBullet = Instantiate(bulletPrefab,transform.position, Quaternion.identity);
    //        newBullet.transform.right = tartgetDirection;

    //        if(newBullet.TryGetComponent(out Projectile projectile))
    //        {
    //            projectile.UpdateMoveSpeed(bulletMoveSpeed);
    //        }

    //        yield return new WaitForSeconds(timeBetweenBrusts);
    //    }

    //    yield return new WaitForSeconds(restTime);
    //    isShooting = false;
    //}

    private IEnumerator ShootRoutine()
    {
        isShooting = true;
        float startAngle, currentAngle, angleStep,endAngle;
        float timeBetweenProjectiles = 0f;

        TargetConeOfInfluence(out startAngle, out currentAngle, out angleStep, out endAngle);

        if(stagger) { timeBetweenProjectiles = timeBetweenBrusts / projectilePerBrust; }

        for (int i = 0; i < brustCount; i++)
        {
            if (!oscillate)
            {
                TargetConeOfInfluence(out startAngle, out currentAngle, out angleStep, out endAngle);
            }

            if(oscillate && i % 2 != 1)
            {
                TargetConeOfInfluence(out startAngle, out currentAngle, out angleStep, out endAngle);
            }
            else if(oscillate)
            {
                currentAngle = endAngle;
                endAngle = startAngle;
                startAngle = currentAngle;
                angleStep *= -1;
            }

            for (int j = 0; j < projectilePerBrust; j++)
            {
                Vector2 pos = FindBulletSpawmPos(currentAngle);

                GameObject newBulllet = Instantiate(bulletPrefab, pos, Quaternion.identity);
                newBulllet.transform.right = newBulllet.transform.position - transform.position;

                if(newBulllet.TryGetComponent(out Projectile projectile))
                {
                    projectile.UpdateMoveSpeed(bulletMoveSpeed);
                }

                currentAngle += angleStep;

                if(stagger) { yield return new WaitForSeconds(timeBetweenProjectiles); }
            }

            currentAngle = startAngle;

            if(!stagger) { yield return new WaitForSeconds(timeBetweenBrusts); }
            
        }

        yield return new WaitForSeconds(restTime);
        isShooting = false;
    }

    private void TargetConeOfInfluence(out float startAngle, out float currentAngle, out float angleStep, out float endAngle)
    {
        Vector2 targetDirection = PlayerController.Instance.transform.position - transform.position;
         float tartgetAngle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
        startAngle = tartgetAngle;

        endAngle = tartgetAngle;
        currentAngle = tartgetAngle;

        float halfAngleSpread = 0f;
        angleStep = 0;
        if(angleSpread != 0)
        {
            angleStep = angleSpread / (projectilePerBrust - 1);
            halfAngleSpread = angleSpread / 2f;
            startAngle = tartgetAngle - halfAngleSpread;
            endAngle = tartgetAngle + halfAngleSpread;
            currentAngle = startAngle;
        }
    }

    private Vector2 FindBulletSpawmPos(float currentAngle)
    {
        float x = transform.position.x + startingDistance * Mathf.Cos(currentAngle * Mathf.Deg2Rad);
        float y = transform.position.y + startingDistance * Mathf.Sin(currentAngle * Mathf.Deg2Rad);
        
        Vector2 pos = new Vector2(x, y);

        return pos;
    }
}
