﻿/**
  Autor: Dalton Solano dos Reis
**/

#define CG_Gizmo
// #define CG_Privado

using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;
using OpenTK.Input;
using CG_Biblioteca;

namespace gcgcg
{
    class Mundo : GameWindow
    {
        private static Mundo instanciaMundo = null;

        private Mundo(int width, int height) : base(width, height) { }

        public static Mundo GetInstance(int width, int height)
        {
            if (instanciaMundo == null)
                instanciaMundo = new Mundo(width, height);
            return instanciaMundo;
        }

        protected List<Objeto> objetosLista = new List<Objeto>();
        private ObjetoGeometria objetoSelecionado = null;
        private bool bBoxDesenhar = false;
        int mouseX, mouseY;   //TODO: achar método MouseDown para não ter variável Global
        private bool mouseMoverPto = false;
        private Retangulo obj_Retangulo;
        private Ponto4D ponto4DBase = new Ponto4D();

        private int size = 100;
        private int rotationGrados = 45;
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Console.WriteLine(" --- Ajuda / Teclas: ");
            Console.WriteLine(" [  H     ] mostra teclas usadas. ");
            GL.ClearColor(Color.Gray);
            this.ponto4DBase.X = 0;
            this.ponto4DBase.Y = 0;
        }
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(-200, 200, -200, 200, -1, 1);
        }
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();

            //Eixo X
            GL.Color3(Color.Red);
            GL.LineWidth(3);
            GL.Begin(PrimitiveType.Lines);
            GL.Vertex2(0, 0);
            GL.Vertex2(150, 0);
            GL.End();

            //Eixo Y
            GL.Color3(Color.Green);
            GL.Begin(PrimitiveType.Lines);
            GL.Vertex2(0, 0);
            GL.Vertex2(0, 150);
            GL.End();

            //Sr. palito
            SegReta.dynamicLine(this.ponto4DBase, Color.Black, this.size, this.rotationGrados);

            this.SwapBuffers();
        }

        protected override void OnKeyDown(OpenTK.Input.KeyboardKeyEventArgs e)
        {
            if (e.Key == Key.H)
                Utilitario.AjudaTeclado();
            else if (e.Key == Key.Escape)
                Exit();
            else if (e.Key == Key.T)
            {
                Console.WriteLine("--- Objetos / Pontos: ");
                for (var i = 0; i < objetosLista.Count; i++)
                {
                    Console.WriteLine(objetosLista[i]);
                }
            }
            else if (e.Key == Key.W) //Direita
            {
                this.ponto4DBase.X++;
            }
            else if (e.Key == Key.Q) //Esquerda
            {
                this.ponto4DBase.X--;
            }
            else if (e.Key == Key.S) //Aumentar
            {
                this.size++;
            }
            else if (e.Key == Key.A) //Diminuir
            {
                this.size--;

            }
            else if (e.Key == Key.Z) //Girar Esquerda
            {
                this.rotationGrados++;
            }
            else if (e.Key == Key.X) //Girar Direita
            {
                this.rotationGrados--;
            }
            else
                Console.WriteLine(" __ Tecla não implementada.");
        }

        //TODO: não está considerando o NDC
        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            mouseX = e.Position.X; mouseY = 600 - e.Position.Y; // Inverti eixo Y
            if (mouseMoverPto && (objetoSelecionado != null))
            {
                objetoSelecionado.PontosUltimo().X = mouseX;
                objetoSelecionado.PontosUltimo().Y = mouseY;
            }
        }

#if CG_Gizmo
        private void Sru3D()
        {
            GL.LineWidth(1);
            GL.Begin(PrimitiveType.Lines);
            GL.Color3(Color.Red);
            GL.Vertex3(0, 0, 0); GL.Vertex3(200, 0, 0);
            GL.Color3(Color.Green);
            GL.Vertex3(0, 0, 0); GL.Vertex3(0, 200, 0);
            GL.Color3(Color.Blue);
            GL.Vertex3(0, 0, 0); GL.Vertex3(0, 0, 200);
            GL.End();
        }
#endif
    }
    class Program
    {
        static void Main(string[] args)
        {
            Mundo window = Mundo.GetInstance(600, 600);
            window.Title = "CG-N2";
            window.Run(1.0 / 60.0);
        }
    }
}
