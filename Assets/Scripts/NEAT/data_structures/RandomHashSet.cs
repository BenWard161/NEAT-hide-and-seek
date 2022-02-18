using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomHashSet<Gene>
{
    private HashSet<Gene> set;
    private ArrayList data;

    public RandomHashSet()
    {
        set = new HashSet<Gene>();
        data = new ArrayList();
    }

//    public bool contains(Gene elem)
//    {
//        return set.Contains(elem);
//    }

//    public Gene random_element()
//    {
//        if(set.Count > 0)
//        {
//            System.Random random = new System.Random();
//            return (Gene)data[(int)(random.NextDouble() * size())];
//        }
//        return default(Gene);
//    }

//    public int size()
//    {
//        return data.Count;
//    }

//    public void add(Gene elem)
//    {
//        if(!set.Contains(elem))
//        {
//            set.Add(elem);
//            data.Add(elem);
//        }
//    }

//    public void add_sorted(Gene elem)
//    {
//        for(int i = 0; i < this.size(); i++)
//        {
//            int innovation = ((Gene)data[i]).getInnovation_number();
//            if(elem.getInnovation_number() < innovation)
//            {
//                data.Insert(i, elem);
//                set.Add(elem);
//                return;
//            }
//        }
//        data.Add(elem);
//        set.Add(elem);
//    }

//    public void clear()
//    {
//        set.Clear();
//        data.Clear();
//    }

//    public T get(int index)
//    {
//        if(index < 0 || index >= size())
//        {
//            return default(T);
//        }
//        return (T)data[index];
//    }

//    public void remove(int index)
//    {
//        if(index < 0 || index >= size())
//        {
//            return;
//        }
//        set.Remove((Gene)data[index]);
//        data.Remove(index);
//    }

//    public void remove(Gene elem)
//    {
//        set.Remove(elem);
//        data.Remove(elem);
//    }

//    public ArrayList getData()
//    {
//        return data;
//    }
}
