using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MicroSplatProceduralTextureConfig : ScriptableObject
{
   public enum TableSize
   {
      k64 = 64,
      k128 = 128,
      k256 = 256,
      k512 = 512,
      k1024 = 1024,
      k2048 = 2048,
      k4096 = 4096
   }

   public TableSize proceduralCurveTextureSize = TableSize.k256;

   [System.Serializable]
   public class Layer
   {
      public float weight = 1;
      public int textureIndex = 0;
      public bool noiseActive;
      public float noiseFrequency = 1;
      public float noiseOffset = 0;
      public Vector2 noiseRange = new Vector2(0, 1);

      public Vector4 biomeWeights = new Vector4(1, 1, 1, 1);
      public Vector4 biomeWeights2 = new Vector4 (1, 1, 1, 1);

      public bool heightActive;
      public AnimationCurve heightCurve = AnimationCurve.Linear(0, 1, 1, 1);
      public bool slopeActive;
      public AnimationCurve slopeCurve = AnimationCurve.Linear(0, 1, 1, 1);
      public bool erosionMapActive;
      public AnimationCurve erosionMapCurve = AnimationCurve.Linear(0, 1, 1, 1);
      public bool cavityMapActive;
      public AnimationCurve cavityMapCurve = AnimationCurve.Linear(0, 1, 1, 1);

      

      public Layer Copy()
      {
         Layer l = new Layer ();
         l.weight = weight;
         l.textureIndex = textureIndex;
         l.noiseActive = noiseActive;
         l.noiseFrequency = noiseFrequency;
         l.noiseOffset = noiseOffset;
         l.noiseRange = noiseRange;
         l.biomeWeights = biomeWeights;
         l.biomeWeights2 = biomeWeights2;
         l.heightActive = heightActive;
         l.slopeActive = slopeActive;
         l.erosionMapActive = erosionMapActive;
         l.cavityMapActive = cavityMapActive;
         l.heightCurve = new AnimationCurve (heightCurve.keys);
         l.slopeCurve = new AnimationCurve (slopeCurve.keys);
         l.erosionMapCurve = new AnimationCurve (erosionMapCurve.keys);
         l.cavityMapCurve = new AnimationCurve (cavityMapCurve.keys);
         return l;
      }
   }

   [System.Serializable]
   public class HSVCurve
   {
      public AnimationCurve H = AnimationCurve.Linear(0, 0.5f, 1, 0.5f);
      public AnimationCurve S = AnimationCurve.Linear(0, 0.0f, 1, 0.0f);
      public AnimationCurve V = AnimationCurve.Linear(0, 0.0f, 1, 0.0f);
   }


   public List<Gradient> heightGradients = new List<Gradient>();
   public List<HSVCurve> heightHSV = new List<HSVCurve>();
   public List<Gradient> slopeGradients = new List<Gradient> ();
   public List<HSVCurve> slopeHSV = new List<HSVCurve> ();


   [HideInInspector]
   public List<Layer> layers = new List<Layer>();


   // cached textures, generated on demand when Syncing
   Texture2D curveTex;
   Texture2D paramTex;
   Texture2D heightGradientTex;
   Texture2D heightHSVTex;
   Texture2D slopeGradientTex;
   Texture2D slopeHSVTex;


   // 32, 128 LUT for curves. R = Height, G = Slope
   public Texture2D GetHeightGradientTexture()
   {
      int height = 32;  // max layers
      int width = 128;
      if (heightGradientTex == null)
      {
         heightGradientTex = new Texture2D(width, height, TextureFormat.RGBA32, false);
         heightGradientTex.hideFlags = HideFlags.HideAndDontSave;
      }

      Color grey = Color.grey;
      for (int i = 0; i < heightGradients.Count; ++i)
      {
         for (int x = 0; x < width; ++x)
         {
            Color c = grey;
            float v = (float)x / width;
            c = heightGradients[i].Evaluate(v);
            heightGradientTex.SetPixel(x, i, c);
         }
      }
      for (int i = heightGradients.Count; i < 32; ++i)
      {
         for (int x = 0; x < width; ++x)
         {
            heightGradientTex.SetPixel(x, i, grey);
         }
      }


      heightGradientTex.Apply(false, false);
      return heightGradientTex;
   }


   // 32, 128 LUT for HSV curves
   public Texture2D GetHeightHSVTexture()
   {
      int height = 32;  // max layers
      int width = 128;
      if (heightHSVTex == null)
      {
         heightHSVTex = new Texture2D(width, height, TextureFormat.RGBA32, false);
         heightHSVTex.hideFlags = HideFlags.HideAndDontSave;
      }

      Color grey = Color.grey;
      for (int i = 0; i < heightHSV.Count; ++i)
      {
         for (int x = 0; x < width; ++x)
         {
            Color c = grey;
            float v = (float)x / width;
            c.r = heightHSV[i].H.Evaluate(v) * 0.5f + 0.5f;
            c.g = heightHSV[i].S.Evaluate(v) * 0.5f + 0.5f;
            c.b = heightHSV[i].V.Evaluate(v) * 0.5f + 0.5f;
            heightHSVTex.SetPixel(x, i, c);
         }
      }
      for (int i = heightHSV.Count; i < 32; ++i)
      {
         for (int x = 0; x < width; ++x)
         {
            heightHSVTex.SetPixel(x, i, grey);
         }
      }


      heightHSVTex.Apply(false, false);
      return heightHSVTex;
   }

   // 32, 128 LUT for curves. R = Height, G = Slope
   public Texture2D GetSlopeGradientTexture ()
   {
      int height = 32;  // max layers
      int width = 128;
      if (slopeGradientTex == null)
      {
         slopeGradientTex = new Texture2D (width, height, TextureFormat.RGBA32, false);
         slopeGradientTex.hideFlags = HideFlags.HideAndDontSave;
      }

      Color grey = Color.grey;
      for (int i = 0; i < slopeGradients.Count; ++i)
      {
         for (int x = 0; x < width; ++x)
         {
            Color c = grey;
            float v = (float)x / width;
            c = slopeGradients [i].Evaluate (v);
            slopeGradientTex.SetPixel (x, i, c);
         }
      }
      for (int i = slopeGradients.Count; i < 32; ++i)
      {
         for (int x = 0; x < width; ++x)
         {
            slopeGradientTex.SetPixel (x, i, grey);
         }
      }


      slopeGradientTex.Apply (false, false);
      return slopeGradientTex;
   }


   // 32, 128 LUT for HSV curves
   public Texture2D GetSlopeHSVTexture ()
   {
      int height = 32;  // max layers
      int width = 128;
      if (slopeHSVTex == null)
      {
         slopeHSVTex = new Texture2D (width, height, TextureFormat.RGBA32, false);
         slopeHSVTex.hideFlags = HideFlags.HideAndDontSave;
      }

      Color grey = Color.grey;
      for (int i = 0; i < slopeHSV.Count; ++i)
      {
         for (int x = 0; x < width; ++x)
         {
            Color c = grey;
            float v = (float)x / width;
            c.r = slopeHSV [i].H.Evaluate (v) * 0.5f + 0.5f;
            c.g = slopeHSV [i].S.Evaluate (v) * 0.5f + 0.5f;
            c.b = slopeHSV [i].V.Evaluate (v) * 0.5f + 0.5f;
            slopeHSVTex.SetPixel (x, i, c);
         }
      }
      for (int i = slopeHSV.Count; i < 32; ++i)
      {
         for (int x = 0; x < width; ++x)
         {
            slopeHSVTex.SetPixel (x, i, grey);
         }
      }


      slopeHSVTex.Apply (false, false);
      return slopeHSVTex;
   }

   // 32, tableSize LUT for curves. R = Height, G = Slope, B = Cavity, A = Erosion
   public Texture2D GetCurveTexture()
   {
      int height = 32;  // max layers
      int width = (int)proceduralCurveTextureSize;
      if (curveTex != null && curveTex.width != width)
      {
         DestroyImmediate (curveTex);
         curveTex = null;
      }
      if (curveTex == null)
      {
         curveTex = new Texture2D(width, height, TextureFormat.RGBA32, false, true);
         curveTex.hideFlags = HideFlags.HideAndDontSave;
      }
      

      Color white = Color.white;
      for (int i = 0; i < layers.Count; ++i)
      {
         for (int x = 0; x < width; ++x)
         {
            Color c = white;
            float v = (float)x / width;
            if (layers[i].heightActive)
            {
               c.r = layers[i].heightCurve.Evaluate(v);
            }
            if (layers[i].slopeActive)
            {
               c.g = layers[i].slopeCurve.Evaluate(v);
            }
            if (layers[i].cavityMapActive)
            {
               c.b = layers[i].cavityMapCurve.Evaluate(v);
            }
            if (layers[i].erosionMapActive)
            {
               c.a = layers[i].erosionMapCurve.Evaluate(v);
            }
            curveTex.SetPixel(x, i, c);
         }
      }


      curveTex.Apply(false, false);
      return curveTex;
   }



   // 4x32 LUT, noise aprams in x0 (RGBA), weights in x1.r
   public Texture2D GetParamTexture()
   {
      int height = 32;  // max textures
      int width = 4;
      if (paramTex == null || paramTex.format != TextureFormat.RGBAHalf || paramTex.width != width)
      {
         paramTex = new Texture2D(width, height, TextureFormat.RGBAHalf, false, true);
         paramTex.hideFlags = HideFlags.HideAndDontSave;
      }


      Color black = new Color(0, 0, 0, 0);
      for (int i = 0; i < layers.Count; ++i)
      {
         Color c0 = black;
         Color c1 = black;
         if (layers[i].noiseActive)
         {
            c0.r = layers[i].noiseFrequency;
            c0.g = layers[i].noiseRange.x;
            c0.b = layers[i].noiseRange.y;
            c0.a = layers[i].noiseOffset;
         }
         c1.r = layers[i].weight;
         c1.g = layers[i].textureIndex;
         paramTex.SetPixel(0, i, c0);
         paramTex.SetPixel(1, i, c1);
         Vector4 bw = layers[i].biomeWeights;
         paramTex.SetPixel(2, i, new Color(bw.x, bw.y, bw.z, bw.w));
         Vector4 bw2 = layers [i].biomeWeights2;
         paramTex.SetPixel (3, i, new Color (bw2.x, bw2.y, bw2.z, bw2.w));
      }

      paramTex.Apply(false, false);
      return paramTex;
   }

}

