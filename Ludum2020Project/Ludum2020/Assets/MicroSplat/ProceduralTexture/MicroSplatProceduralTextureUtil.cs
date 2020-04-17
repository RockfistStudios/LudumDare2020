using UnityEngine;
using System.Collections;

// for sampling texturing from the CPU. If your using jobs, there's a jobified version
// included in a zip file next to this version.

namespace JBooth.MicroSplat
{
   public class MicroSplatProceduralTextureUtil
   {


      public enum NoiseUVMode
      {
         UV = 0,
         World,
         Triplanar
      }

      static float PCFilter(int index, float height, float slope, float cavity, float flow, Vector3 worldPos, Vector2 uv,
          Color bMask, out int texIndex, Vector3 pN,
          Texture2D procTexCurves, Texture2D procTexParams, Texture2D procTexNoise, NoiseUVMode noiseMode)
      {
         // params0 are rgba (noise scale, min, max, offset)
         // params1 are rg (weight, index)
         Vector2 noiseUV = uv;

         float offset = 1.0f / 32.0f;
         float halfOff = 1.0f / 64.0f;
         float y = (index * offset) + halfOff;

         float h0 = procTexCurves.GetPixelBilinear(height, y).r;
         float s0 = procTexCurves.GetPixelBilinear(slope, y).g;
         float c0 = procTexCurves.GetPixelBilinear(cavity, y).b;
         float f0 = procTexCurves.GetPixelBilinear(flow, y).a;

         Color params0 = procTexParams.GetPixelBilinear(0.166666f, y);
         Color params1 = procTexParams.GetPixelBilinear(0.5f, y);
         Color params2 = procTexParams.GetPixelBilinear(0.833333f, y);


         Color noise = new Color(0, 0, 0, 0);
         if (noiseMode == NoiseUVMode.Triplanar)
         {
            Vector2 nUV0 = new Vector2(worldPos.z, worldPos.y) * 0.002f * params0.r + new Vector2(params0.a, params0.a);
            Vector2 nUV1 = new Vector2(worldPos.x, worldPos.z) * 0.002f * params0.r + new Vector2(params0.a + 0.31f, params0.a + 0.31f);
            Vector2 nUV2 = new Vector2(worldPos.x, worldPos.y) * 0.002f * params0.r + new Vector2(params0.a + 0.71f, params0.a + 0.71f);

            Color noise0 = procTexNoise.GetPixelBilinear(nUV0.x, nUV0.y);
            Color noise1 = procTexNoise.GetPixelBilinear(nUV1.x, nUV1.y);
            Color noise2 = procTexNoise.GetPixelBilinear(nUV2.x, nUV2.y);
            noise = noise0 * pN.x + noise1 * pN.y + noise2 * pN.z;
         }
         else if (noiseMode == NoiseUVMode.World)
         {
            noise = procTexNoise.GetPixelBilinear(noiseUV.x * params0.r + params0.a, noiseUV.y * params0.r + params0.a);
         }
         else if (noiseMode == NoiseUVMode.UV)
         {
            noise *= procTexNoise.GetPixelBilinear(worldPos.x * 0.002f * params0.r + params0.a, worldPos.z * 0.002f * params0.r + params0.a);
         }

         // unpack
         noise.r = noise.r * 2 - 1;
         noise.g = noise.g * 2 - 1;

         h0 *= 1.0f + Mathf.Lerp(params0.g, params0.b, noise.r);
         s0 *= 1.0f + Mathf.Lerp(params0.g, params0.b, noise.g);
         c0 *= 1.0f + Mathf.Lerp(params0.g, params0.b, noise.b);
         f0 *= 1.0f + Mathf.Lerp(params0.g, params0.b, noise.a);
         float bWeight = 1;
         bMask *= params2;
         // handle 16 mode
         bWeight = Mathf.Max(Mathf.Max(Mathf.Max(bMask.r, bMask.g), bMask.b), bMask.a);

         texIndex = (int)params1.g;
         return Mathf.Clamp01(h0 * s0 * c0 * f0 * params1.r * bWeight);
      }

      public struct Int4
      {
         public int x;
         public int y;
         public int z;
         public int w;
      }

      static void PCProcessLayer(ref Vector4 weights, ref Int4 indexes, ref float totalWeight,
          int curIdx, float height, float slope, float cavity, float flow, Vector3 worldPos, Vector2 uv,
          Color biomeMask, Vector3 pN,
          Texture2D procTexCurves, Texture2D procTexParams, Texture2D procTexNoise, NoiseUVMode noiseMode)
      {
         int texIndex = 0;
         float w = PCFilter(curIdx, height, slope, cavity, flow, worldPos, uv, biomeMask, out texIndex, pN, procTexCurves, procTexParams, procTexNoise, noiseMode);
         w = Mathf.Min(totalWeight, w);
         totalWeight -= w;

         // sort
         if (w > weights.x)
         {
            weights.w = weights.z;
            weights.z = weights.y;
            weights.y = weights.x;
            indexes.w = indexes.z;
            indexes.z = indexes.y;
            indexes.y = indexes.x;
            weights.x = w;
            indexes.x = texIndex;
         }
         else if (w > weights.y)
         {
            weights.w = weights.z;
            weights.z = weights.y;
            indexes.w = indexes.z;
            indexes.z = indexes.y;
            weights.y = w;
            indexes.y = texIndex;
         }
         else if (w > weights.z)
         {
            weights.w = weights.z;
            indexes.w = indexes.z;
            weights.z = w;
            indexes.z = texIndex;
         }
         else if (w > weights.w)
         {
            weights.w = w;
            indexes.w = texIndex;
         }
      }


      public static void Sample(Vector2 uv, Vector3 worldPos, int layerCount, float worldHeight, Vector2 worldRange, Vector3 worldNormal, Vector3 up,
      Texture2D curves, Texture2D properties, Texture2D cavityMap, Texture2D noise, Texture2D mask, bool biom16mode, NoiseUVMode noiseUVMode, out Vector4 weights, out Int4 indexes)
      {

         weights = new Vector4(0, 0, 0, 0);

         float height = Mathf.Clamp01((worldHeight - worldRange.x) / Mathf.Max(0.1f, (worldRange.y - worldRange.x)));
         float slope = 1.0f - Mathf.Clamp01(Vector3.Dot(worldNormal, up) * 0.5f + 0.49f);
         float cavity = 0.5f;
         float flow = 0.5f;
         if (cavityMap != null)
         {
            var p = cavityMap.GetPixelBilinear(uv.x, uv.y);
            cavity = p.g;
            flow = p.a;
         }
         // find 4 highest weights and indexes
         indexes = new Int4();
         indexes.x = 0;
         indexes.y = 1;
         indexes.z = 2;
         indexes.w = 3;

         float totalWeight = 1.0f;

         Color biomeMask = new Color(1, 1, 1, 1);
         if (mask != null)
         {
            biomeMask = mask.GetPixelBilinear(uv.x, uv.y);
         }

         Vector3 pN = new Vector3(0, 0, 0);
         if (noiseUVMode == NoiseUVMode.Triplanar)
         {
            Vector3 absWN = worldNormal;
            absWN.x = Mathf.Abs(absWN.x);
            absWN.y = Mathf.Abs(absWN.y);
            absWN.z = Mathf.Abs(absWN.z);
            pN.x = Mathf.Pow(absWN.x, 4);
            pN.y = Mathf.Pow(absWN.y, 4);
            pN.z = Mathf.Pow(absWN.z, 4);
            float ttl = pN.x + pN.y + pN.z;
            pN.x /= ttl;
            pN.y /= ttl;
            pN.z /= ttl;
         }


         for (int i = 0; i < layerCount; ++i)
         {
            PCProcessLayer(ref weights, ref indexes, ref totalWeight, i, height, slope, cavity, flow, worldPos, uv, biomeMask, pN, curves, properties, noise, noiseUVMode);
         }

      }
   }
}

