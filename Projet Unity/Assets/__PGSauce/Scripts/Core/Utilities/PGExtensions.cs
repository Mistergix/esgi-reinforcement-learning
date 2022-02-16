using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PGSauce.Core.PGDebugging;
using UnityEngine.UI;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace PGSauce.Core.Utilities
{
    public static class PGExtensions
    {

        /// <summary>
        /// Transforms "PGSauce.Core.FSM.Base.State" into "State"
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public static string RemoveNamespace(this string typeName)
        {
            var index = typeName.LastIndexOf(".", StringComparison.Ordinal);
            if (index >= 0)
            {
                typeName = typeName.Substring(index + 1);
            }

            return typeName;
        }
        
        /// <summary>
        /// Convert World Rotation to this transform's local Space
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="worldRotation"></param>
        /// <returns></returns>
        public static Quaternion GetLocalRotation(this Transform transform, Quaternion worldRotation)
        {
            var originalRotation = transform.rotation;
            transform.rotation = worldRotation;
            var localRotation = transform.localRotation;
            transform.rotation = originalRotation;

            return localRotation;
        }
        
        /// <summary>
        /// Convert this transform's local Space rotation to World Rotation
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="localRotation"></param>
        /// <returns></returns>
        public static Quaternion GetWorldRotation(this Transform transform, Quaternion localRotation)
        {
            var originalRotation = transform.rotation;
            transform.localRotation = localRotation;
            var worldRotation = transform.rotation;
            transform.rotation = originalRotation;

            return worldRotation;
        }

        public static Vector3 GetWorldPosition(this Transform transform, Vector3 localPosition)
        {
            return transform.TransformPoint(localPosition);
        }

        public static Vector3 GetLocalPosition(this Transform transform, Vector3 worldPosition)
        {
            return transform.InverseTransformPoint(worldPosition);
        }

        public static Vector3 GetWorldDirection(this Transform transform, Vector3 localDirection)
        {
            return transform.TransformDirection(localDirection);
        }

        public static Vector3 GetLocalDirection(this Transform transform, Vector3 worldDirection)
        {
            return transform.InverseTransformDirection(worldDirection);
        }

        public static bool IsBetween(this float f, float a, float b, bool strict = false)
        {
            if (strict)
            {
                return a < f && f < b;
            }

            return a <= f && f <= b;
        }
        
        public static bool IsBetween(this int f, float a, float b, bool strict = false)
        {
            return ((float) f).IsBetween(a, b, strict);
        }
        
        public static void Alpha(this Graphic graphics, float a)
        {
            graphics.color = graphics.color.Alpha(a);
        }

        public static Color Alpha(this Color color, float a)
        {
            var col = color;
            col.a = a;
            return col;
        }
        
        public static int GetRandomIndex<T>(this List<T> list)
        {
            return Random.Range(0, list.Count);
        }
        
        public static bool OutOfRange<T>(this IEnumerable<T> list, int index)
        {
            return !(index >= 0 && index < list.Count());
        }
        
        public static T GetRandomValue<T>(this List<T> list)
        {
            var result = list.Count == 0 ? default : list[Random.Range(0, list.Count)];
            return result;
        }
        
        public static void Shuffle<T>(this List<T> list)
        {
            if (list == null)
            {
                PGDebug.Message("Can't shuffle, the list is null").LogError();
                return;
            }

            for (var i = 0; i < list.Count; ++i)
            {
                list.Swap(i, Random.Range(0, list.Count));
            }
        }
        
        public static void Swap<T>(this List<T> list, int firstIndex, int secondIndex)
        {
            if (list == null)
            {
                PGDebug.Message("Can't swap indexes, the list is null").LogError();
                return;
            }
            if (list.OutOfRange(firstIndex) || list.OutOfRange(secondIndex))
            {
                PGDebug.Message($"Can't swap indexes, the list is out of range : {Mathf.Max(firstIndex, secondIndex)} > {list.Count}").LogError();
                return;
            }
            var element = list[firstIndex];
            list[firstIndex] = list[secondIndex];
            list[secondIndex] = element;
        }

        public static bool IsInLayerMask(this int layer, LayerMask layermask)
        {
            return layermask == (layermask | (1 << layer));
        }
        public static bool ContainsLayer(this LayerMask layerMask, int layer)
        {
            return layer.IsInLayerMask(layerMask);
        }
        
        public static string GetTransformPath(this Transform current) {
            if (current.parent == null)
                return "/" + current.name;
            return current.parent.GetTransformPath() + "/" + current.name;
        }
        
        public static float Remap(this float value, float inputA, float inputB, float outputA, float outputB)
        {
            return (value - inputA) / (inputB - inputA) * (outputB - outputA) + outputA;
        }

        public static float Remap(this int value, float inputA, float inputB, float outputA, float outputB)
        {
            return Remap((float)value, inputA, inputB, outputA, outputB);
        }

        /// 
        /// Is the object left, right, or in front ?
        /// 
        /// 
        /// 
        /// 
        /// -1 = left, 1 = right, 0 = in front (or behind)
        public static float RelativeOrientation(this Transform transform, Vector3 targetDir)
        {
            var up = transform.up;
            var forward = transform.forward;

            var perp = Vector3.Cross(forward, targetDir);
            var dir = Vector3.Dot(perp, up);
            if (dir > 0f)
            {
                return 1f;
            }
            else if (dir < 0f)
            {
                return -1f;
            }
            else
            {
                return 0f;
            }
        }

        /// <summary>
        /// positive = in front, negative = behind
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static float InFront(this Transform transform, Transform target)
        {
            var toTarget = (target.position - transform.position).normalized;
            return Vector3.Dot(toTarget, transform.forward);
        }

        /// <summary>
        /// Increment the int not passed the list count
        /// </summary>
        public static int IncrementClamped<T>(this int index, List<T> list)
        {
            return index.IncrementClamped(list.Count - 1);
        }

        /// <summary>
        /// Increment the int not passed the value
        /// </summary>
        public static int IncrementClamped(this int index, int threshold)
        {
            return Mathf.Clamp(index + 1, 0, threshold);
        }

        public static void X(this Transform transform, float x)
        {
            var pos = transform.position;
            pos.x = x;
            transform.position = pos;
        }

        public static void Y(this Transform transform, float y)
        {
            var pos = transform.position;
            pos.y = y;
            transform.position = pos;
        }

        public static void Z(this Transform transform, float z)
        {
            var pos = transform.position;
            pos.z = z;
            transform.position = pos;
        }

        public static void LocalX(this Transform transform, float x)
        {
            var pos = transform.localPosition;
            pos.x = x;
            transform.localPosition = pos;
        }

        public static void LocalY(this Transform transform, float y)
        {
            var pos = transform.localPosition;
            pos.y = y;
            transform.localPosition = pos;
        }

        public static void LocalZ(this Transform transform, float z)
        {
            var pos = transform.localPosition;
            pos.z = z;
            transform.localPosition = pos;
        }

        public static void LocalScale(this Transform t, float scale)
        {
            t.LocalScale(scale, scale, scale);
        }

        public static void LocalScale(this Transform t, float x, float y, float z)
        {
            t.localScale = new Vector3(x, y ,z);
        }

        public static void LocalScaleX(this Transform t, float x)
        {
            var localScale = t.localScale;
            localScale = new Vector3(x, localScale.y , localScale.z);
            t.localScale = localScale;
        }


        public static void LocalScaleY(this Transform t, float y)
        {
            var localScale = t.localScale;
            localScale = new Vector3(localScale.x, y , localScale.z);
            t.localScale = localScale;
        }


        public static void LocalScaleZ(this Transform t, float z)
        {
            var localScale = t.localScale;
            localScale = new Vector3(localScale.x, localScale.y , z);
            t.localScale = localScale;
        }

        public static void SetGlobalScale (this Transform transform, Vector3 globalScale)
        {
            transform.localScale = Vector3.one;
            var lossyScale = transform.lossyScale;
            transform.localScale = new Vector3 (globalScale.x/lossyScale.x, globalScale.y/lossyScale.y, globalScale.z/lossyScale.z);
        }
        
        public static T Clone<T>(this T scriptableObject) where T : ScriptableObject
        {
            if (scriptableObject == null)
            {
                Debug.LogError($"ScriptableObject was null. Returning default {typeof(T)} object.");
                return (T)ScriptableObject.CreateInstance(typeof(T));
            }
 
            var instance = Object.Instantiate(scriptableObject);
            instance.name = scriptableObject.name; // remove (Clone) from name
            return instance;
        }
    }
}
