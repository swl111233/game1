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

    // ����һ���෢�ӵ�
    private bool isUsingSkill = false;
    //���ܳ���ʱ��
    public float skillDuration = 5.0f;
    private float skillTime;
    public Slider timerSlider;
    //�ӵ�����
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
            Instantiate(shot, shotSpawn.position, shotSpawn.rotation);//�������ӵ�
            GetComponent<AudioSource>().Play();
        }

        //�෢�ӵ�����״̬�ж�
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
        //��ȡ������������ֵ
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

    //ʶ���Ƿ���Triple Stone
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TripleStone"))
        {
            isUsingSkill = true;
            Destroy(other.gameObject);
        }
    }

    //�෢�ӵ�����ʵ��
    private void TripleSkill()
    {
        int median = bulletNum / 2;
        for (int i = 0; i < bulletNum; i++)
        {
            // ʵ�����ӵ����������ٶ�
            GameObject newBullet = Instantiate(shot, shotSpawn.position, Quaternion.identity);
            newBullet.transform.Rotate(Vector3.up, bulletAngle * (i - median));
        }
    }

    //�෢�ӵ����ܽ���
    private void EndTripleSkill()
    {
        isUsingSkill = false;
        skillTime = skillDuration;
        timerSlider.gameObject.SetActive(false);
    }
}
