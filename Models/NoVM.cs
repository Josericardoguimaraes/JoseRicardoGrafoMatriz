using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoseRicardoGrafoMatriz.Models
{

    public class NoVM
    {

        public int valor { get; set; }
        public NoVM vizinhoSuperior { get; set; }
        public NoVM vizinhoInferior { get; set; }
        public NoVM vizinhoEsquerdo { get; set; }
        public NoVM vizinhoDireito { get; set; }
        public int posicaoLinha { get; set; }
        public int posicaoColuna { get; set; }
       
    }

    public class Percursos
    {
        public Percursos()
        {
            percursoValido = new List<NoVM>();
        }

       public List<NoVM> percursoValido;



    }

   

}
