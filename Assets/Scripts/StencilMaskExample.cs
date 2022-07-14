using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace mj.gist.postprocessing {
    public class StencilMaskExample : MonoBehaviour {
        [SerializeField] private MaskKind maskKind;
        [SerializeField] private Shader shader;

        private Material material;

        void Start() {
            material = new Material(shader);
        }
        private void OnRenderImage(RenderTexture source, RenderTexture destination) {
            Graphics.Blit(source, destination, material, (int)maskKind);
        }
    }
    public enum MaskKind { None = 0, Capsule = 1, Cube = 2, Sphere = 3 }
}