# Unity_StencilMaskGenerator

## StencilMaskGenerator.cs

**Create command buffer**
```
buffer = new CommandBuffer { name = "StencilGenerator" };
```
**Create temp texture and save current render result to it**
```
if (cameraRenderingTextureID == 0)
    cameraRenderingTextureID = Shader.PropertyToID("_temp");

// store current render result to tmp
buffer.GetTemporaryRT(cameraRenderingTextureID, -1, -1, 0);
buffer.Blit(BuiltinRenderTextureType.CameraTarget, cameraRenderingTextureID);
```

**Create each stencil and save them to global textures.**
```
foreach (var mask in masks) {
    var name = mask.name;
    var stencil = mask.stencil;

    var mat = new Material(shader);

    mat.SetInt("_Stencil", stencil);
    var outputId = Shader.PropertyToID($"_output_{name}");
    buffer.GetTemporaryRT(outputId, -1, -1, 0);
    buffer.Blit(cameraRenderingTextureID, BuiltinRenderTextureType.CameraTarget, mat);
    buffer.Blit(BuiltinRenderTextureType.CameraTarget, outputId);
    buffer.SetGlobalTexture(name, outputId);
}

// set original rendering back
buffer.Blit(cameraRenderingTextureID, BuiltinRenderTextureType.CameraTarget);
```

## StencilMask.shader (Two passes)
**Pass1 : set other stencils to black**
```
Pass
{
  Stencil
  {
    Ref[_Stencil]
    Comp NotEqual
  }
  ...
  fixed4 frag(v2f_img i) : SV_Target
  {
    return 0;
  }
  ENDCG
}
```
**Pass2 : set target stencil to white**
```
Pass
{
  Stencil
  {
    Ref[_Stencil]
    Comp Equal
  }
  ...
  fixed4 frag(v2f_img i) : SV_Target
  {
    return 1;
  }
  ENDCG
}
```

## Shaders for each objects
**Give them specific stencil values**
```
Stencil
{
    Ref[_Stencil]
    Comp Always
    Pass Replace
}
```
