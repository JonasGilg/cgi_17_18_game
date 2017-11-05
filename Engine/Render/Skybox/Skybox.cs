using System;
using System.Drawing;
using System.Drawing.Imaging;
using Engine.Model;
using Engine.Texture;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using GL = OpenTK.Graphics.OpenGL.GL;
using PixelFormat = OpenTK.Graphics.OpenGL.PixelFormat;

namespace Engine.Render.Skybox {
    public class Skybox {
        private TransformComponent _transformComponent;
        private RenderComponent _renderComponent;
        private int Programm;
        private readonly int _uProjectionMatrix;
        private readonly int _uWorldToCameraMatrix;
        private readonly int _mvp;
        private int _vao;
        int textCube;
        private int[] textureIds;
        private Model3D skybox;

        public Skybox() {
            _transformComponent = new TransformComponent(Matrix4.Identity);

            skybox = ModelLoaderObject3D.Load("data/objects/Skybox.obj");
            Programm = ShaderLoader.LoadShader("Render/Skybox/Skybox_VS.glsl", "Render/Skybox/Skybox_FS.glsl");

            createCubeMap();
        }

        private void createCubeMap() {
            GL.ActiveTexture(TextureUnit.Texture0);

            textCube = GL.GenTexture();


            loadCubeMapSide("data/textures/skybox/skybox_right1.png", TextureTarget.TextureCubeMapPositiveX);
            loadCubeMapSide("data/textures/skybox/skybox_left2.png", TextureTarget.TextureCubeMapNegativeX);
            loadCubeMapSide("data/textures/skybox/skybox_top3.png", TextureTarget.TextureCubeMapPositiveY);
            loadCubeMapSide("data/textures/skybox/skybox_bottom4.png", TextureTarget.TextureCubeMapNegativeY);
            loadCubeMapSide("data/textures/skybox/skybox_front5.png", TextureTarget.TextureCubeMapPositiveZ);
            loadCubeMapSide("data/textures/skybox/skybox_back6.png", TextureTarget.TextureCubeMapNegativeZ);

            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMagFilter,
                (float) TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMinFilter,
                (float) TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapR,
                (float) TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapS,
                (float) TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapT,
                (float) TextureWrapMode.ClampToEdge);
        }

        private bool loadCubeMapSide(string path, TextureTarget sideTarget) {
            GL.BindTexture(TextureTarget.TextureCubeMap, textCube);

            var bmp = new Bitmap(path);

            var bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly,
                System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            GL.TexImage2D(sideTarget, 0, PixelInternalFormat.Rgba, bmpData.Width, bmpData.Height, 0, PixelFormat.Bgra,
                PixelType.UnsignedByte, bmpData.Scan0);


            return true;
        }

        public void Draw() {
            GL.DepthMask(false);

            GL.UseProgram(Programm);
            GL.BindVertexArray(skybox.Vao);
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.TextureCubeMap, textCube);
            GL.BindAttribLocation(Programm, 0, "in_position");

            var uWorldToCameraMatrix = DisplayCamera.Transformation;
            var uProjectionMatrix = DisplayCamera.PerspectiveProjection;
            var mvp = DisplayCamera.Transformation * DisplayCamera.PerspectiveProjection;

            /*
            GL.UniformMatrix4(_uWorldToCameraMatrix,false,ref uWorldToCameraMatrix);
            GL.UniformMatrix4(_uProjectionMatrix,false,ref uProjectionMatrix);
            */
            
            GL.UniformMatrix4(_mvp, false, ref mvp);

            GL.DepthFunc(DepthFunction.Lequal);
            
            GL.DrawElements(PrimitiveType.Triangles, skybox.Indices.Count, DrawElementsType.UnsignedInt, IntPtr.Zero);

            GL.DepthFunc(DepthFunction.Less);

            GL.DepthMask(true);
        }

        public void Update() {
            _transformComponent.Position = DisplayCamera.Position;
        }
    }
}