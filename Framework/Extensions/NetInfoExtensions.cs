﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using NetworkExtensions.Framework.Extensions;
using UnityEngine;
using Object = UnityEngine.Object;
#if DEBUG
using Debug = NetworkExtensions.Framework.Debug;
#endif

namespace NetworkExtensions.Framework
{
    public static class NetInfoExtensions
    {
        public static NetInfo Clone(this NetInfo originalNetInfo, string newName)
        {
            Debug.Log(String.Format("NExt: Cloning {0} -> {1}", originalNetInfo.name, newName));

            var gameObject = Object.Instantiate(originalNetInfo.gameObject);
            gameObject.transform.parent = originalNetInfo.gameObject.transform; // N.B. This line is evil and removing it is killoing the game's performances
            gameObject.name = newName;

            var info = gameObject.GetComponent<NetInfo>();
            info.m_prefabInitialized = false;

            Debug.Log(String.Format("NExt: Cloning completed {0} -> {1}", originalNetInfo.name, newName));

            return info;
        }

        public static void DisplayLaneProps(this NetInfo info)
        {
            foreach (var lane in info.m_lanes)
            {
                if (lane.m_laneProps != null)
                {
                    Debug.Log(string.Format("NExt: Lane name {0}", lane.m_laneProps.name));

                    if (lane.m_laneProps.m_props != null)
                    {
                        foreach (var prop in lane.m_laneProps.m_props)
                        {
                            if (prop.m_prop != null)
                            {
                                Debug.Log(string.Format("NExt:     Prop name {0}", prop.m_prop.name));
                            }
                        }
                    }
                }
            }
        }

        public static NetInfo SetUICategory(this NetInfo info, string category)
        {
            typeof(NetInfo).GetField("m_UICategory", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(info, category);

            return info;
        }

        public static NetInfo SetSegmentsTexture(this NetInfo info, string mainTexPath, string xysMapPath = null, string aprMapPath = null)
        {
            var mainTex = TextureManager.Instance.GetTexture(mainTexPath);
            var xysMap = TextureManager.Instance.GetTexture(xysMapPath);
            var aprMap = TextureManager.Instance.GetTexture(aprMapPath);

            SetSegmentsTexture(info, mainTex, xysMap, aprMap);

            return info;
        }

        public static NetInfo SetSegmentsTexture(this NetInfo info, Texture mainTex, Texture xysMap, Texture aprMap)
        {
            for (int i = 0; i < info.m_segments.Length; i++)
            {
                var material = new Material(info.m_segments[i].m_material);

                material.SetTexture("_MainTex", mainTex);

                if (xysMap != null)
                {
                    material.SetTexture("_XYSMap", xysMap);
                }

                if (aprMap != null)
                {
                    material.SetTexture("_APRMap", aprMap);
                }

                info.m_segments[i].m_material = material;
            }

            return info;
        }

        public static NetInfo SetSegmentsTexture(this NetInfo info, TexturesSet newTextures, TexturesSet newLODTextures = null)
        {
            foreach (var segment in info.m_segments)
            {
                segment.m_material = segment.m_material.Clone(newTextures);

                if (newLODTextures != null)
                {
                    segment.m_lodMaterial = segment.m_lodMaterial.Clone(newLODTextures);
                }
            }

            return info;
        }

        public static NetInfo SetNodesTexture(this NetInfo info, TexturesSet newTextures, TexturesSet newLODTextures = null)
        {
            foreach (var node in info.m_nodes)
            {
                node.m_material = node.m_material.Clone(newTextures);

                if (newLODTextures != null)
                {
                    node.m_lodMaterial = node.m_lodMaterial.Clone(newLODTextures);
                }
            }

            return info;
        }
    }
}
