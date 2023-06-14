
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CustomHorizontalScrollView : MonoBehaviour, IEndDragHandler, IDragHandler, IBeginDragHandler
{
    public Item Item;
    //�û�����
    private List<string> data = new List<string>();
    //������
    public RectTransform content;

    //itemList
    public List<Item> itemList = new List<Item>();

    private Vector3 lastPosition;
    private float offset;

    public int showNum;//��ʾ�ĸ���

    float size;

    private int startIndex = 0;
    private int endIndex = 0;

    public bool isMoveItem;//��ǵ�ǰ�Ƿ񻬶�UI
    private bool sliderRight;//��ǵ�ǰ�Ļ�������

    private float index0Y;//��¼��һ����λ��
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
                size = item.rectTransform.rect.size.x;//�õ�ÿһ��Item�ĸ�
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
        offset = eventData.position.x - lastPosition.x;//�õ�������ƫ����
       
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
            //���ϻ�

            if (itemList[0].transform.localPosition.x <= index0Y && isMoveItem == false)
            {
                //���һ����ĩβ,����������һֱ��ӣ�Ҫ����һֱ�������
                isMoveItem = true;
                // Debug.LogError("���һ����ĩβ");
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
                    //��һ����ȫ������������
                    startIndex++;

                    if (startIndex > data.Count - 1)
                    {
                        startIndex = 0;
                    }


                    isMoveItem = false;
                    //ѭ����ĩβ
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
                //���»�

                if (itemList[0].transform.localPosition.x >= index0Y && isMoveItem == false)
                {
                    //���һ����ĩβ,����������һֱ��ӣ�Ҫ����һֱ�������
                    isMoveItem = true;
                    Debug.LogError("���һ������ͷ");
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

                        //ѭ����ĩβ
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
