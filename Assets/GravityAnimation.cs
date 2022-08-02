using UnityEngine;
using System.Collections;

public class GravityAnimation : MonoBehaviour
{
    //方向是否反向移动
    public bool isReverseX = false;
    public bool isReverseY = false;

    //贴图移动极限值
    public float maxX;
    public float minX;
    public float maxY;
    public float minY;

    //范围（防止画面抖动）
    public float range = 0.01f;

    //区间（陀螺仪的值改变多少区间 完成整个移动）
    public float section = 0.3f;







    //上一帧陀螺仪的值
    Vector3 lastAttitude = Vector3.zero;

    //移动平均值
    float meanX;
    float meanY;






    void Start()
    {
        Input.gyro.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform == null)
        {
            return;
        }


        //获取陀螺仪的值
        Vector3 attitude = new Vector3(Input.gyro.attitude.x, Input.gyro.attitude.y, 0);


        //使安全范围内 - 1之1
        attitude.x = Tool(attitude.x, 1, -1);
        attitude.y = Tool(attitude.y, 1, -1);




        float x = transform.localPosition.x;
        float y = transform.localPosition.y;




        //如果这帧变化不大 则使用上帧的值   防止抖动;
        if (System.Math.Abs(lastAttitude.x - attitude.x) >= range)
        {
            //这帧的偏移量
            float direction = attitude.x - lastAttitude.x;

            meanX = (maxX - minX) / section;

            //偏移量对于的实际坐标位移
            float Position = direction * meanX;


            if (isReverseX)
            {
                x = Tool(transform.localPosition.x + Position, maxX, minX);
            }
            else
            {
                x = Tool(transform.localPosition.x - Position, maxX, minX);
            }
        }



        //如果这帧变化不大 则使用上帧的值   防止抖动;
        if (System.Math.Abs(lastAttitude.y - attitude.y) >= range)
        {
            //这帧的偏移量
            float direction = attitude.y - lastAttitude.y;


            meanY = (maxY - minY) / section;
            //偏移量对于的实际坐标位移
            float Position = direction * meanY;

            if (isReverseY)
            {
                y = Tool(transform.localPosition.y + Position, maxY, minY);
            }
            else
            {
                y = Tool(transform.localPosition.y - Position, maxY, minY);
            }
        }




        transform.localPosition = new Vector3(x, y, transform.localPosition.z);

        //保存值
        lastAttitude = attitude;
        Debug.Log("gravity test " + transform.localPosition+" "+ lastAttitude);
    }





    //使第origin的值在界限之内
    float Tool(float origin, float max, float min)
    {
        float retrun = origin;

        if (retrun >= max)
        {
            retrun = max;
        }
        else if (retrun <= min)
        {
            retrun = min;
        }

        return retrun;
    }


}