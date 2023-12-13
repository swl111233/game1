using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class PlayerControl : MonoBehaviour
{
    [System.Serializable]
    public class Boundary
    {
        public float xMin, xMax, zMin, zMax;
    }

    public float speed = 5.0f;
    public Boundary boundary;
    public float tilt = 4.0f;

    public GameObject shot;
    public Transform shotSpawn;
    public float fireRate = 0.5f;
    private float nextFire;

    // 技能一：多发子弹
    private bool isUsingSkill = false;
    //技能持续时间
    public float skillDuration = 5.0f;
    private float skillTime;
    public Slider timerSlider;
    //子弹数量
    public int bulletNum = 3;
    public float bulletAngle = 15;


    private void Start()
    {
        skillTime = skillDuration;
        timerSlider.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetButton("Fire1") && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            Instantiate(shot, shotSpawn.position, shotSpawn.rotation);//创建出子弹
            GetComponent<AudioSource>().Play();
        }

        //多发子弹技能状态判断
        if (isUsingSkill)
        {
            timerSlider.gameObject.SetActive(true);
            skillTime -= Time.deltaTime;
            if (skillTime <= 0)
            {
                Debug.Log("Countdown finished!");
                EndTripleSkill();
            }
            else
            {
                timerSlider.value = skillTime / skillDuration;
                TripleSkill();
            }
        }
    }

    void FixedUpdate()
    {
        //获取横向纵向输入值
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        GetComponent<Rigidbody>().velocity = movement * speed;
        GetComponent<Rigidbody>().position = new Vector3
            (
            Mathf.Clamp(GetComponent<Rigidbody>().position.x, boundary.xMin, boundary.xMax),
            0.0f,
            Mathf.Clamp(GetComponent<Rigidbody>().position.z, boundary.zMin, boundary.zMax)
            );

        GetComponent<Rigidbody>().rotation =
            Quaternion.Euler(0.0f, 0.0f, GetComponent<Rigidbody>().velocity.x * -tilt);
    }

    //识别是否是Triple Stone
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TripleStone"))
        {
            isUsingSkill = true;
            Destroy(other.gameObject);
        }
    }

    //多发子弹技能实现
    private void TripleSkill()
    {
        int median = bulletNum / 2;
        for (int i = 0; i < bulletNum; i++)
        {
            // 实例化子弹对象并设置速度
            GameObject newBullet = Instantiate(shot, shotSpawn.position, Quaternion.identity);
            newBullet.transform.Rotate(Vector3.up, bulletAngle * (i - median));
        }
    }

    //多发子弹技能结束
    private void EndTripleSkill()
    {
        isUsingSkill = false;
        skillTime = skillDuration;
        timerSlider.gameObject.SetActive(false);
    }
}
