﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewsRoll : MonoBehaviour
{
    public Queue<String> newsList = new Queue<String>();
    public int spacing;
    public News newsPrefab;
    public float newsRollSpeed;

    private float _starter;
    private List<News> _newsesList = new List<News>();

    private void Start()
    {
        _starter = UnityEngine.Screen.width;
    }

    private void GenerateNewsQueue()
    {
        while (newsList.Count > 0)
        {
            News newsObj = Instantiate(newsPrefab, this.transform);
            _newsesList.Add(newsObj);
            newsObj.Starter = _starter;
            newsObj.content = newsList.Dequeue() + new String(' ',spacing);
            newsObj.Speed = newsRollSpeed;
            
        }

    }

    private int _curRolling = 0;
    private bool _isRollFinished = false;
    private void SetNewsesRoll()
    {
        if (_newsesList.Count > 0)
        {
            //走完了一轮且末尾也已走完
            if (_isRollFinished && _newsesList[_newsesList.Count-1].IsEnd)
            {
                _newsesList[_curRolling].gameObject.SetActive(true);
                _isRollFinished = false;
            }
            else if (_newsesList[_curRolling].IsEnd) //如果当前走完
            {
                _curRolling++;
                if (_curRolling + 1 > _newsesList.Count)
                {
                    _curRolling = 0;
                    _isRollFinished = true;
                    
                }
                
            }
            else if(!_newsesList[_curRolling].gameObject.activeSelf)
            {
                _newsesList[_curRolling].gameObject.SetActive(true);
            }
        }
    }

    public void AddNews(String content) => newsList.Enqueue(content);
    

    public void RemoveNews(string content)
    {
        int index = _newsesList.FindIndex(t => t.content == content);
        News curRollingNews = _newsesList[_curRolling];
        if (index != _curRolling)
        {
            _newsesList.RemoveAt(index);
            _curRolling = _newsesList.FindIndex(t => t.content == curRollingNews.content);
        }
        else
        {
            _newsesList.RemoveAt(index);
        }

    }

    private void FixedUpdate()
    {
        GenerateNewsQueue();
        SetNewsesRoll();
    }
}
