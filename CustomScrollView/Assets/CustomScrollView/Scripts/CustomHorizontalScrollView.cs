
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CustomHorizontalScrollView : MonoBehaviour, IEndDragHandler, IDragHandler, IBeginDragHandler
{
    public Item Item;
    //用户数据
    private List<string> data = new List<string>();
    //父物体
    public RectTransform content;

    //itemList
    public List<Item> itemList = new List<Item>();

    private Vector3 lastPosition;
    private float offset;

    public int showNum;//显示的个数

    float size;

    private int startIndex = 0;
    private int endIndex = 0;

    public bool isMoveItem;//标记当前是否滑动UI
    private bool sliderRight;//标记当前的滑动方向

    private float index0Y;//记录第一个的位置
    private Vector3 dir = Vector3.right;
    private void Awake()
    {
        for (int i = 0; i < 100; i++)
        {
            data.Add(i.ToString());
        }

        startIndex = 0;
        endIndex = showNum - 1;

        for (int i = 0; i < showNum + 1; i++)
        {
            Item item = Instantiate(Item, content);
            Text tex = item.GetComponentInChildren<Text>();
            tex.text = data[i];
            if (i == 0)
            {
                size = item.rectTransform.rect.size.x;//得到每一个Item的高
                index0Y = item.transform.localPosition.x;
            }
            else
            {
                item.transform.localPosition = itemList[i - 1].transform.localPosition + dir * size;
            }
            item.gameObject.SetActive(true);
            item.GetComponent<Image>().color = new Color(Random.Range(0, 1.0f), Random.Range(0, 1.0f), Random.Range(0, 1.0f), 1);
            itemList.Add(item);

        }


    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        lastPosition = eventData.position;
    }
    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        offset = eventData.position.x - lastPosition.x;//得到滑动的偏移量
       
        offset = Mathf.Clamp(offset, -size / 2, size / 2);
        lastPosition = eventData.position;

        for (int i = 0; i < itemList.Count; i++)
        {
            itemList[i].transform.localPosition += dir * offset;
        }
        if (offset < 0)
        {
            if (isMoveItem && !sliderRight)
            {
                isMoveItem = false;
                startIndex--;
            }
            sliderRight = true;
            //往上滑

            if (itemList[0].transform.localPosition.x <= index0Y && isMoveItem == false)
            {
                //添加一个到末尾,但是这样会一直添加，要避免一直添加问题
                isMoveItem = true;
                // Debug.LogError("添加一个到末尾");
                itemList[itemList.Count - 1].transform.localPosition = itemList[itemList.Count - 2].transform.localPosition + dir * size;
                endIndex++;

                if (endIndex > data.Count - 1)
                {
                    endIndex = 0;
                }
                itemList[itemList.Count - 1].GetComponentInChildren<Text>().text = data[endIndex];
            }
            else
            {
                if (itemList[0].transform.localPosition.x <= index0Y-size)
                {
                    //第一个完全滑出区域外了
                    startIndex++;

                    if (startIndex > data.Count - 1)
                    {
                        startIndex = 0;
                    }


                    isMoveItem = false;
                    //循环到末尾
                    Item tmp = itemList[0];
                    tmp.transform.localPosition = itemList[itemList.Count - 1].transform.localPosition + dir * size;

                    for (int i = 1; i < itemList.Count; i++)
                    {

                        itemList[i - 1] = itemList[i];
                    }
                    itemList[itemList.Count - 1] = tmp;

                }
            }


        }
        else
        {
            if (offset > 0)
            {
                if (isMoveItem && sliderRight)
                {
                    endIndex++;
                    isMoveItem = false;
                }
                sliderRight = false;
                //往下滑

                if (itemList[0].transform.localPosition.x >= index0Y && isMoveItem == false)
                {
                    //添加一个到末尾,但是这样会一直添加，要避免一直添加问题
                    isMoveItem = true;
                    Debug.LogError("添加一个到开头");
                    itemList[itemList.Count - 1].transform.localPosition = itemList[0].transform.localPosition - dir * size;

                    startIndex--;

                    if (startIndex < 0)
                    {
                        startIndex = data.Count - 1;
                    }
                    itemList[itemList.Count - 1].GetComponentInChildren<Text>().text = data[startIndex];
                }
                else
                {
                    if (itemList[0].transform.localPosition.x >= size-index0Y)
                    {


                        endIndex--;

                        if (endIndex < 0)
                        {
                            endIndex = data.Count - 1;
                        }
                        isMoveItem = false;

                        //循环到末尾
                        Item tmp = itemList[itemList.Count - 1];
                        tmp.transform.localPosition = itemList[0].transform.localPosition - dir * size;
                        for (int i = itemList.Count - 1; i >= 1; i--)
                        {
                            itemList[i] = itemList[i - 1];
                        }
                        itemList[0] = tmp;

                    }
                }


            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //offset = index0Y - itemList[0].transform.localPosition.y;

        //if (sliderUp)
        //{
        //    offset = -offset;
        //}

        //for (int i = 0; i < itemList.Count; i++)
        //{
        //    itemList[i].DOLocalMove(itemList[i].localPosition + Vector3.up * offset, 0.1F);

        //}

    }



}
