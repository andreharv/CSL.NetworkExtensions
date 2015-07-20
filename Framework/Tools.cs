﻿using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Reflection;
using ColossalFramework;
using UnityEngine;

namespace NetworkExtensions.Framework
{
    public static class Tools
    {
        public static T ShallowCopy<T>(this T source)
            where T : new()
        {
            var clone = new T();

            foreach (FieldInfo f in typeof(T).GetAllFields())
            {
                f.SetValue(clone, f.GetValue(source));
            }

            return clone;
        }

        public static T CopyMembersFrom<T>(this T destination, T source, params string[] omitMembers)
            where T : new()
        {
            foreach (FieldInfo f in destination.GetType().GetAllFields(true))
            {
                if (omitMembers.Contains(f.Name))
                {
                    continue;
                }

                f.SetValue(destination, f.GetValue(source));
            }

            return destination;
        }

        public static void RemoveNull<T>(ref T[] array)
        {
            int count = 0;
            for (int i = 0; i < array.Length; ++i)
            {
                if (array[i] != null) ++count;
            }
            T[] nu = new T[count];
            count = 0;
            for (int i = 0; i < array.Length; ++i)
            {
                if (array[i] != null)
                {
                    nu[count] = array[i];
                    ++count;
                }
            }
            array = nu;
        }

        public static void CloneArray<T>(ref T[] source)
            where T : new()
        {
            var newArray = new T[source.Length];
            for (int i = 0; i < newArray.Length; ++i)
            {
                T original = source[i];
                T copy = new T();
                foreach (FieldInfo fi in typeof(T).GetAllFields())
                {
                    fi.SetValue(copy, fi.GetValue(original));
                }
                newArray[i] = copy;
            }
            source = newArray;
        }
    }
}