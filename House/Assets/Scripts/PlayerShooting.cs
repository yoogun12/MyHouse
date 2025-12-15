using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{

    public GameObject projetilePrefabFire;      //Projectile 프리팹

    public GameObject projetilePrefabWater;      //Projectile 프리팹

    public Transform firePoint;             //발사 위치 (총구)

    Camera cam;

    public bool FireAttack = true;


    void Start()
    {
        cam = Camera.main; //메인 카메라 가져오기   
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            FireAttack = !FireAttack;
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (FireAttack)
            {
                ShootFire();
            }
            else
            {
                ShootWater();
            }
        }
    }

    void ShootFire()
    {
        
            // 화면에서 마우스 -> 광선(Ray) Thrl
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            Vector3 targetPoint;
            targetPoint = ray.GetPoint(50f);
            Vector3 direction = (targetPoint - firePoint.position).normalized; //방향 벡터

            // Projectile 생성
            GameObject proj = Instantiate(projetilePrefabFire, firePoint.position, Quaternion.LookRotation(direction));
      
    }

    void ShootWater()
    {
      
            // 화면에서 마우스 -> 광선(Ray) Thrl
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            Vector3 targetPoint;
            targetPoint = ray.GetPoint(50f);
            Vector3 direction = (targetPoint - firePoint.position).normalized; //방향 벡터

            // Projectile 생성
            GameObject proj = Instantiate(projetilePrefabWater, firePoint.position, Quaternion.LookRotation(direction));
       
    }
}
