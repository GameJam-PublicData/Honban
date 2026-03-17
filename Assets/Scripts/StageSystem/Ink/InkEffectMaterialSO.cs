using System.Collections.Generic;
using UnityEngine;

namespace StageSystem.Ink
{
[CreateAssetMenu(menuName = "ScriptableObject/InkEffectMaterial")]
public class InkEffectMaterialSO : ScriptableObject
{
   [SerializeField] List<Material> materials = new();
   
   public Material GetMaterial(string materialName)
   {
         return materials.Find(m => m.name == materialName);
   }
}
}