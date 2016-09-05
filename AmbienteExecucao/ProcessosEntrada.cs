using JoseRicardoGrafoMatriz.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoseRicardoGrafoMatriz.MetodosExecucao
{
    public class ProcessosEntrada
    {
        private List<NoVM> matriz; // Buffer
        private Random rad = new Random();


        public ProcessosEntrada()
        {
            matriz = new List<NoVM>();
        }


        public void populandoMatriz(int x, int y)
        {
            matriz = new List<NoVM>();//Todo clique do botão cria uma nova Matriz

            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    NoVM noAux = new NoVM()
                    {
                        posicaoLinha = i,
                        posicaoColuna = j,
                        valor = gerarValorAleatorioBit(),
                    };


                   
                    //Vizinho Esquerdo
                    if ((j - 1) >= 0 && existeVizinho(i, (j - 1)))
                    {
                        noAux.vizinhoEsquerdo = matriz.FirstOrDefault(es => es.posicaoLinha == i && es.posicaoColuna == (j - 1));
                        matriz.FirstOrDefault(es => es.posicaoLinha == i && es.posicaoColuna == (j - 1)).vizinhoDireito = noAux;
                    }

                    //Vizinho Superior
                    if ((i - 1) >= 0 && existeVizinho((i - 1), j))
                    {
                        noAux.vizinhoSuperior = matriz.FirstOrDefault(es => es.posicaoLinha == (i - 1) && es.posicaoColuna == j);
                        matriz.FirstOrDefault(es => es.posicaoLinha == (i - 1) && es.posicaoColuna == j).vizinhoInferior = noAux;
                    }

                    

                    matriz.Add(noAux); //Add o Objeto Nó na lista de buffer.
                }

            }



        }

        public List<NoVM> getMatriz() { return matriz; }

        public void mostraMatriz()
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    Console.Write("["+matriz.FirstOrDefault(ex => ex.posicaoLinha == i && ex.posicaoColuna == j).valor +"]");
                }
                Console.Write("\n");
            }

        }



        #region Metodos Privados

        private int gerarValorAleatorioBit()
        {
            
            var valor = rad.Next(1, 11);
            if (valor < 7)
            {
                return 1;
            }
            else
            {
                return 0;
            }

        }
        private bool existeVizinho(int x, int y)
        {
            try
            {
                var noExistente = matriz.Exists(el => el.posicaoLinha == x && el.posicaoColuna == y);
                return noExistente;

            }catch( Exception es)
            {
                return false;
            }

        }


        //TESTE

        public void setvALORnAmATRIZ(int x, int y)
        {
          
          matriz.FirstOrDefault(el => el.posicaoLinha == x && el.posicaoColuna == y).valor = 1;
             

        }
        #endregion


    }
}
