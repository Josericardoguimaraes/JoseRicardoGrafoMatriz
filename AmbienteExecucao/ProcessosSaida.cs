using JoseRicardoGrafoMatriz.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoseRicardoGrafoMatriz.AmbienteExecucao
{
    public class ProcessosSaida
    {
        private List<NoVM> matriz;
        List<NoVM> caminhoAtual;
        List<Percursos> percursoSucesso = new List<Percursos>();
        List<Percursos> todoosPercursos = new List<Percursos>();
        int linF, colF;


        public ProcessosSaida(List<NoVM> m, int LinhaChegada, int ColunaChegada)
        {
            this.matriz = m;
            linF = LinhaChegada;
            colF = ColunaChegada;
            caminhoAtual = new List<NoVM>();

        }


        public void setPontoDePartida(int x, int y)
        {
            NoVM ponto = matriz.FirstOrDefault(ex => ex.posicaoLinha == x && ex.posicaoColuna == y);
            caminhoAtual.Add(ponto);

        }

        public List<Percursos> getP() { return percursoSucesso; }
        public List<Percursos> getTodosPerc() { return todoosPercursos; }


        public List<NoVM> calculaPercurso(List<NoVM> caminhoAt, NoVM noz)
        {
            //Valida a primeira chamada.
            if(caminhoAt == null)
            {
                caminhoAt = this.caminhoAtual;
                noz = new NoVM()
                {
                    posicaoColuna = caminhoAtual[0].posicaoColuna,
                    posicaoLinha = caminhoAtual[0].posicaoLinha,
                    valor = caminhoAtual[0].valor,
                    vizinhoDireito = caminhoAtual[0].vizinhoDireito,
                    vizinhoEsquerdo = caminhoAtual[0].vizinhoEsquerdo,
                    vizinhoInferior = caminhoAtual[0].vizinhoInferior,
                    vizinhoSuperior = caminhoAtual[0].vizinhoSuperior

                }; 
           
            }
            else
            {
                caminhoAt.Add(noz);
            }

            //Todos Os Percursos
            Percursos perc = new Percursos();
            foreach (var itemNo in caminhoAt)
            {
                perc.percursoValido.Add(new NoVM()
                {
                    posicaoColuna = itemNo.posicaoColuna,
                    posicaoLinha = itemNo.posicaoLinha,
                    valor = itemNo.valor,
                    vizinhoDireito = itemNo.vizinhoDireito ,
                    vizinhoEsquerdo = itemNo.vizinhoEsquerdo,
                    vizinhoInferior = itemNo.vizinhoInferior,
                    vizinhoSuperior = itemNo.vizinhoSuperior
                });
            }


            todoosPercursos.Add(perc); //ADD Todos os Percursos percorridos




            if (noz.posicaoLinha == linF && noz.posicaoColuna == colF)//se coluna e linha do nó for igual ao desejado.
            {
                caminhoAtual = caminhoAt;
                Percursos per = new Percursos();
                  foreach(var itemNo in caminhoAt){
                        per.percursoValido.Add(new NoVM(){
                            posicaoColuna = itemNo.posicaoColuna,
                            posicaoLinha = itemNo.posicaoLinha,
                            valor = itemNo.valor,
                            vizinhoDireito = itemNo.vizinhoDireito,
                            vizinhoEsquerdo = itemNo.vizinhoEsquerdo,
                            vizinhoInferior = itemNo.vizinhoInferior,
                            vizinhoSuperior = itemNo.vizinhoSuperior
                        });
                }


                percursoSucesso.Add(per); //ADD PERCURSOS VALIDOS
                caminhoAt.Remove(noz);
                return caminhoAt;
   
            }

            if(noz.vizinhoDireito != null && noz.vizinhoDireito.valor == 1  && !(caminhoAt.Exists(el => el == noz.vizinhoDireito))) // Busca Vizinho Direita
            {
                caminhoAt = calculaPercurso(caminhoAt, noz.vizinhoDireito);


            }
            if (noz.vizinhoEsquerdo != null && noz.vizinhoEsquerdo.valor == 1  && !(caminhoAt.Exists(el => el == noz.vizinhoEsquerdo))) // Busca Vizinho Esquedo
            {
                caminhoAt = calculaPercurso(caminhoAt, noz.vizinhoEsquerdo);


            }
            if (noz.vizinhoSuperior != null && noz.vizinhoSuperior.valor == 1 && !(caminhoAt.Exists(el => el == noz.vizinhoSuperior))) // Busca Vizinho Superior
            {
                caminhoAt = calculaPercurso(caminhoAt, noz.vizinhoSuperior);


            }
            if (noz.vizinhoInferior != null && noz.vizinhoInferior.valor == 1  && !(caminhoAt.Exists(el => el == noz.vizinhoInferior))) // Busca Vizinho Inferior
            {
                caminhoAt = calculaPercurso(caminhoAt, noz.vizinhoInferior);


            }

            caminhoAt.Remove(noz);
            return caminhoAt;





        }





    }
}
