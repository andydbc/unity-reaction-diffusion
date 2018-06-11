using UnityEngine;
using UnityEngine.Rendering;

namespace ReactionDiffusion
{
    public class RDUpdate : MonoBehaviour
    {
        [SerializeField]
        RDSettings _settings = null;
        public RDSettings settings {
            get { return _settings; }
        }

        [SerializeField]
        int _resolution = 512;

        [SerializeField]
        ComputeShader _compute;

        private RenderTexture[] _textures = new RenderTexture[2];

        private RenderTexture readBuffer {
            get { return _textures[0];  } 
            set { _textures[0] = value; }
        }

        private RenderTexture writeBuffer {
            get { return _textures[1]; }
            set { _textures[1] = value; }
        }

        void Start()
        {
            readBuffer = CreateTexture();
            writeBuffer = CreateTexture();

            Initialize();
        }

        private RenderTexture CreateTexture()
        {
            RenderTexture texture = new RenderTexture(_resolution, _resolution, 16, RenderTextureFormat.RGInt);

            texture.name = "Output";
            texture.enableRandomWrite = true;
            texture.dimension = TextureDimension.Tex2D;
            texture.volumeDepth = _resolution;
            texture.filterMode = FilterMode.Bilinear;
            texture.wrapMode = TextureWrapMode.Repeat;
            texture.Create();

            return texture;
        }

        void Initialize()
        {
            int kernel = _compute.FindKernel("Init");

            _compute.SetTexture(kernel, "Write", readBuffer);
            _compute.Dispatch(kernel, _resolution / 8, _resolution / 8, _resolution / 8);
        }

        void Update()
        {
            int kernel = _compute.FindKernel("Update");

            _compute.SetVector("_Diffusion", new Vector4(_settings.du, _settings.dv, 0f, 0f));
            _compute.SetFloat("_Feed", _settings.feed);
            _compute.SetFloat("_Kill", _settings.kill);

            _compute.SetTexture(kernel, "Read", readBuffer);
            _compute.SetTexture(kernel, "Write", writeBuffer);

            _compute.Dispatch(kernel, _resolution / 8, _resolution / 8, 1);

            Swap();
        }

        private void Swap()
        {
            RenderTexture tmp = _textures[0];
            _textures[0] = _textures[1];
            _textures[1] = tmp;
        }
    }
}
