
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;

using OpenTK;
using OpenTK.Graphics.OpenGL;

#pragma warning disable CS0618


namespace Tomogramm
{
    class View
    {
        Bitmap textureImage;
        Vector3 cameraPosition = new Vector3(2 , 3, 4);
        Vector3 cameraDirecton = new Vector3(0, 0, 0);
        Vector3 cameraUp = new Vector3(0, 0, 1);

        int VBOtexture;

        public int min = 0, width = 2000;

        //void SetCamera()
        //{
        //    GL.Viewport(0, 0, width, height);
        //    /* Set camera position */
        //    GL.MatrixMode(MatrixMode.Modelview);
        //    GL.LoadIdentity();
        //    Matrix4.LookAt(m_vEye[0], m_vEye[1], m_vEye[2],
        //        m_vRef[0], m_vRef[1], m_vRef[2],
        //         m_vUp[0], m_vUp[1], m_vUp[2]);
        //    /* Set projection frustrum */
        //    GL.MatrixMode(MatrixMode.Projection);
        //    GL.LoadIdentity();
        //    Matrix4.Perspective(m_fYFOV, width / height, m_fNear, m_fFar);
        //}



        public void SetupView(int width, int height)
        {
            // было

            if (width < height) height = width;
            if (height < width) width = height;


            /* ортогональная 
             GL.Viewport(0, 0, width, height);
             GL.ShadeModel(ShadingModel.Smooth);
             GL.MatrixMode(MatrixMode.Projection);
             GL.LoadIdentity();
            //GL.Ortho(-100, Bin.X, 0, Bin.Y, -1, 1);
       */


            // относительно камеры
            GL.Viewport(0, 0, width, height);
            GL.ShadeModel(ShadingModel.Smooth);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            Matrix4 viewMat = Matrix4.LookAt(cameraPosition, cameraDirecton, cameraUp);
            GL.LoadMatrix(ref viewMat);



            //GL.Viewport(0, 0, width, height);
            //GL.ShadeModel(ShadingModel.Smooth);
            //GL.MatrixMode(MatrixMode.Projection);
            //Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, width / (float)height, 0.1f, 64.0f);
            //GL.LoadIdentity();
            //GL.LoadMatrix(ref projection);
            //Matrix4.Perspective(15, width / (float)height, 1, 100);
            //GL.LoadMatrix(ref projection);

            //GL.Viewport(0, 0, width, height);
            //GL.ShadeModel(ShadingModel.Smooth);
            //GL.Enable(EnableCap.DepthTest);
            //Matrix4 viewMat = Matrix4.LookAt(cameraPosition, cameraDirecton, cameraUp);
            ////Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, width / (float)height, 0.1f, 64.0f);
            //GL.MatrixMode(MatrixMode.Projection);
            //GL.LoadIdentity();
            //GL.LoadMatrix(ref viewMat);
            //GL.Frustum(0, Bin.X, 0, Bin.Y, 0.1, 100);

            //Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, width / (float)height, 0.1f, 100.0f);
            //GL.LoadMatrix(ref projection);
            //GL.LoadIdentity();
            ////GL.Ortho(0, Bin.X, 0, Bin.Y, -1000, 1000);
            // Matrix4 ortho = Matrix4.CreateOrthographic(width, height, -1, 1);


        }

        public int Clamp(int value, int min, int max)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }

        Color TransferFunction(short value)
        {
            
            int max = min + width;
            int newVal = Clamp((value - min) * 255 / (max - min), 0, 255);
            return Color.FromArgb(255, newVal, newVal, newVal);
        }

        //public void DrawQuads(int layerNumber)
        //{
        //    GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        //    GL.Begin(BeginMode.Quads);

        //    for (int x_coord = 0; x_coord < Bin.X - 1; x_coord++)
        //    {
        //        for (int y_coord = 0; y_coord < Bin.Y - 1; y_coord++)
        //        {
        //            short value;

        //            // 1 вершина 
        //            value = Bin.array[x_coord + y_coord * Bin.X + layerNumber * Bin.X * Bin.Y];
        //            GL.Color3(TransferFunction(value));
        //            GL.Vertex2(x_coord, y_coord);

        //            //2 вершина
        //            value = Bin.array[x_coord + (y_coord + 1) * Bin.X + layerNumber * Bin.X * Bin.Y];
        //            GL.Color3(TransferFunction(value));
        //            GL.Vertex2(x_coord, y_coord + 1);

        //            //3 вершина
        //            value = Bin.array[x_coord + 1 + (y_coord + 1) * Bin.X + layerNumber * Bin.X * Bin.Y];
        //            GL.Color3(TransferFunction(value));
        //            GL.Vertex2(x_coord + 1, y_coord + 1);

        //            //4 вершина
        //            value = Bin.array[x_coord + 1 + y_coord * Bin.X + layerNumber * Bin.X * Bin.Y];
        //            GL.Color3(TransferFunction(value));
        //            GL.Vertex2(x_coord + 1, y_coord);
        //        }
        //    }

        //    GL.End();
        //}

        public void DrawQuads(int layerNumber)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Begin(BeginMode.Quads);
            int z_coord = 1; 
            for (int x_coord = 0; x_coord < Bin.X - 1; x_coord++)
            {
                for (int y_coord = 0; y_coord < Bin.Y - 1; y_coord++)
                {
                    short value;

                    // 1 вершина 
                    value = Bin.array[x_coord + y_coord * Bin.X + layerNumber * Bin.X * Bin.Y];
                    GL.Color3(TransferFunction(value));
                    GL.Vertex3(x_coord, y_coord,z_coord);

                    //2 вершина
                    value = Bin.array[x_coord + (y_coord + 1) * Bin.X + layerNumber * Bin.X * Bin.Y];
                    GL.Color3(TransferFunction(value));
                    GL.Vertex3(x_coord, y_coord + 1, z_coord);

                    //3 вершина
                    value = Bin.array[x_coord + 1 + (y_coord + 1) * Bin.X + layerNumber * Bin.X * Bin.Y];
                    GL.Color3(TransferFunction(value));
                    GL.Vertex3(x_coord + 1, y_coord + 1, z_coord);

                    //4 вершина
                    value = Bin.array[x_coord + 1 + y_coord * Bin.X + layerNumber * Bin.X * Bin.Y];
                    GL.Color3(TransferFunction(value));
                    GL.Vertex3(x_coord + 1, y_coord, z_coord);
                }
            }

            GL.End();
        }


        public void DrawQuadsStrip(int layerNumber)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            for (int x_coord = 0; x_coord < Bin.X - 1; x_coord++)
            {
                GL.Begin(BeginMode.QuadStrip);
                for (int y_coord = 0; y_coord < Bin.Y - 1; y_coord++)
                {
                    short value;
                    
                    value = Bin.array[x_coord + y_coord  * Bin.X + layerNumber * Bin.X * Bin.Y];
                    GL.Color3(TransferFunction(value));
                    GL.Vertex2(x_coord, y_coord);

                    
                    value = Bin.array[x_coord + 1 + y_coord * Bin.X + layerNumber * Bin.X * Bin.Y];
                    GL.Color3(TransferFunction(value));
                    GL.Vertex2(x_coord + 1, y_coord);
                }
            }

            GL.End();
        }




        public void Load2DTexture()
        {
            GL.BindTexture(TextureTarget.Texture2D, VBOtexture);
            BitmapData data = textureImage.LockBits(
                new Rectangle(0, 0, textureImage.Width, textureImage.Height),
                ImageLockMode.ReadOnly,
                System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba,
                data.Width, data.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra,
                PixelType.UnsignedByte, data.Scan0);

            textureImage.UnlockBits(data);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter,
                (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter,
                (int)TextureMinFilter.Linear);

            ErrorCode Er = GL.GetError();
            string str = Er.ToString();
        }

        public void generateTextureImage(int layerNumber)
        {
            textureImage = new Bitmap(Bin.X, Bin.Y);

            for (int i = 0; i < Bin.X; i++)
                for (int j = 0; j < Bin.Y; j++)
                {
                    int pixelNumber = i + j * Bin.X + layerNumber * Bin.X * Bin.Y;
                    textureImage.SetPixel(i, j, TransferFunction(Bin.array[pixelNumber]));
                }
        }

        public void DrawTexture()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, VBOtexture);

            GL.Begin(BeginMode.Quads);
            GL.Color3(Color.White);
            GL.TexCoord2(0f, 0f);
            GL.Vertex2(0, 0);
            GL.TexCoord2(0f, 1f);
            GL.Vertex2(0, Bin.Y);
            GL.TexCoord2(1f, 1f);
            GL.Vertex2(Bin.X, Bin.Y);
            GL.TexCoord2(1f, 0f);
            GL.Vertex2(Bin.X, 0);
            GL.End();

            GL.Disable(EnableCap.Texture2D);
        }

    }
}



