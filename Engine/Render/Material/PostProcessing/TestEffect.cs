using System;
using System.Drawing;
using Engine.Output;
using Engine.Render;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace Engine.Material {
    public class TestEffect : PostProcessingMaterial{
        public int Program;
        public int fbo_texture;
        public int rbo_depth;
        public int fbo;
        public float[] fbo_vertices = {
            -1, -1,
            1, -1,
            -1,  1,
            1,  1
        };
        public int vbo_fbo_vertices;
        public int attribute_v_coord_postproc;
        public int uniform_fbo_texture;
        public int uniform_offset;
        
        public TestEffect() {
            Program = ShaderLoader.LoadShader("Render/Material/PostProcessing/test_effect_VS.glsl",
                "Render/Material/PostProcessing/test_effect_FS.glsl");
            
            GL.BindAttribLocation(Program, 0, "v_coord");
            attribute_v_coord_postproc = GL.GetAttribLocation(Program, "v_coord");
            GL.LinkProgram(Program);

            uniform_fbo_texture = GL.GetUniformLocation(Program, "fbo_texture");
            uniform_offset = GL.GetUniformLocation(Program, "offset");

            //Texture buffer
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.GenTextures(1,out fbo_texture );
            GL.BindTexture(TextureTarget.Texture2D, fbo_texture);
            GL.TexParameter(TextureTarget.Texture2D,TextureParameterName.TextureMagFilter, (int) TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D,TextureParameterName.TextureMinFilter, (int) TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D,TextureParameterName.TextureWrapS, (int) TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D,TextureParameterName.TextureWrapT, (int) TextureWrapMode.ClampToEdge);
            GL.TexImage2D(TextureTarget.Texture2D,0,PixelInternalFormat.Rgba,Display.DisplayWidth,Display.DisplayHeight,0,PixelFormat.Rgba,PixelType.UnsignedByte,IntPtr.Zero);
            GL.BindTexture(TextureTarget.Texture2D,0);
			
            //Depth buffer
            GL.GenRenderbuffers(1,out rbo_depth);
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer,rbo_depth);
            GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer,RenderbufferStorage.DepthComponent16,Display.DisplayWidth,Display.DisplayHeight);
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer,0);
			
            //framebuffer
            GL.GenFramebuffers(1, out fbo);
            GL.BindFramebuffer(FramebufferTarget.Framebuffer,fbo);
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer,FramebufferAttachment.ColorAttachment0,TextureTarget.Texture2D,fbo_texture,0);
            GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer,FramebufferAttachment.DepthAttachment,RenderbufferTarget.Renderbuffer,rbo_depth);
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);

            //vertices
            
            GL.GenBuffers(1,out vbo_fbo_vertices);
            GL.BindBuffer(BufferTarget.ArrayBuffer,vbo_fbo_vertices);
            GL.BufferData(BufferTarget.ArrayBuffer,fbo_vertices.Length,fbo_vertices,BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ArrayBuffer,0);
        }
        
        public void PreDraw() {
            
            
            
        }

        public override void Draw() {
            
            
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
            GL.ClearColor(new Color4(0,0,0,1));
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            
            GL.UseProgram(Program);
            GL.BindTexture(TextureTarget.Texture2D,fbo_texture);
            GL.Uniform1(uniform_fbo_texture, 0);
            GL.Uniform1(uniform_offset,(float)Time.TotalTime);
            GL.EnableVertexAttribArray(attribute_v_coord_postproc);
            GL.BindBuffer(BufferTarget.ArrayBuffer,vbo_fbo_vertices);
            GL.VertexAttribPointer(attribute_v_coord_postproc,2,VertexAttribPointerType.Float,false,0,0);
            GL.DrawArrays(PrimitiveType.TriangleStrip,0,4);
            GL.DisableVertexAttribArray(attribute_v_coord_postproc);
        }

        public void PostDraw() {
            
        }
    }
}