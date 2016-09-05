using JoseRicardoGrafoMatriz.AmbienteExecucao;
using JoseRicardoGrafoMatriz.MetodosExecucao;
using JoseRicardoGrafoMatriz.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace JoseRicardoGrafoMatriz
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int[,] matrixFrontEnd; // Mostra Para Usuário
        List<string> PossiveisCaminhos = new List<string>();
        private string MenorPercurso;
        private int TamanhoMenorPercurso;
        private int TamanhoMaiorPercurso;
        private string MaiorPercurso;
        private string TotalPercorridoSuecsso;



        ProcessosEntrada proc = new ProcessosEntrada(); // Processos de Entrada
        ProcessosSaida saida; // Processos de Saida
        private int MatriX, MatriY; // Parametros da Primeira Matriz

        public MainWindow()
        {
            InitializeComponent();

        }


        private void populaMatrizFrontE(int x, int y, List<NoVM> nos)
        {
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    matrixFrontEnd[i, j] = nos.FirstOrDefault(el => el.posicaoLinha == i && el.posicaoColuna == j).valor;

                }

            }



        }

        private List<string> populaResultadoFrontE(List<Percursos> PercursosPossiveis)
        {
            PossiveisCaminhos = new List<string>();
            PercursosPossiveis = PercursosPossiveis.OrderBy(ex => ex.percursoValido.Count()).ToList();
            TotalPercorridoSuecsso = PercursosPossiveis.Count().ToString();
            for (int i = 0; i < PercursosPossiveis.Count(); i++)
            {

                string aux = "";
                int auxiNT = 0;
                foreach (var no in PercursosPossiveis[i].percursoValido)
                {
                    aux += "[" + (no.posicaoLinha+1) + "," + (no.posicaoColuna+1) + "] ";
                    auxiNT += 1;
                }
                PossiveisCaminhos.Add(aux);

                if (i == 0)
                {
                    MenorPercurso = PossiveisCaminhos[i];
                    TamanhoMenorPercurso = auxiNT;
                }
                if (i == (PercursosPossiveis.Count() - 1))
                {
                    MaiorPercurso = PossiveisCaminhos[i];
                    TamanhoMaiorPercurso = auxiNT;
                }



            }
            return PossiveisCaminhos;
        }


        private void btnGerarMatriz_Click(object sender, RoutedEventArgs e)
        {
            

            if (!int.TryParse(tamanhoX.Text, out MatriX) || MatriX <= 0)
            {
                MessageBox.Show("Digite um valor válido para o X da matriz","Alerta");
                return;
            }
            if (!int.TryParse(tamanhoY.Text, out MatriY) || MatriY <= 0)
            {
                MessageBox.Show("Digite um valor válido para o y da matriz","Alerta");
                return;
            }



            //==Back_end==
            proc.populandoMatriz(MatriX, MatriY); // Matriz Back-end (SEM ESTRUTURA PRIMARIA)


            //==Front-End==
            matrixFrontEnd = new int[MatriX, MatriY]; // Matriz Front-end 
            populaMatrizFrontE(MatriX, MatriY, proc.getMatriz());


            // POpula Matriz para usuário
            c_dataGrid.ItemsSource = GetBindable2DArray<int>(matrixFrontEnd);


            GridBusca.Visibility = Visibility.Visible;
            ScrolLista.Visibility = Visibility.Visible;
            GridResultado.Visibility = Visibility.Visible;
            lblValidacao.Visibility = Visibility.Collapsed;


        }


        public static DataView GetBindable2DArray<T>(T[,] array)
        {
            var table = new DataTable();
            for (var i = 0; i < array.GetLength(1); i++)
            {
                table.Columns.Add((i + 1).ToString(), typeof(int)).ExtendedProperties.Add("idx", i);
            }
            for (var i = 0; i < array.GetLength(0); i++)
            {
                
                table.Rows.Add(table.NewRow());
            }

            var view = new DataView(table);
            for (var ri = 0; ri < array.GetLength(0); ri++)
            {
                for (var ci = 0; ci < array.GetLength(1); ci++)
                {
                    view[ri][ci] = array[ri, ci];
                }
            }

           
            table.ColumnChanged += (s, e) =>
            {
                var ci = (int)e.Column.ExtendedProperties["idx"]; 
                var ri = e.Row.Table.Rows.IndexOf(e.Row);

                array[ri, ci] = (T)view[ri][ci];
            };
            
            return view;
        }

        private void btnGerarRotas_Click(object sender, RoutedEventArgs e)
        {
            int Partx, Party, Chegx, Chegy;

            #region Validações
            if (!int.TryParse(PartidaX.Text, out Partx) || Partx <= 0 || Partx > MatriX)
            {
                MessageBox.Show("Digite um valor válido para o X do ponto de partida", "Alerta");
                return;
            }
            if (!int.TryParse(PartidaY.Text, out Party) || Party <= 0 || Party > MatriY)
            {
                MessageBox.Show("Digite um valor válido para o Y do ponto de partida", "Alerta");
                return;
            }
            if (!int.TryParse(ChegadaX.Text, out Chegx) || Chegx <= 0 || Chegx > MatriX)
            {
                MessageBox.Show("Digite um valor válido para o X do ponto de chegada", "Alerta");
                return;
            }
            if (!int.TryParse(ChegadaY.Text, out Chegy) || Chegy <= 0 || Chegy > MatriY)
            {
                MessageBox.Show("Digite um valor válido para o Y do ponto de chegada", "Alerta");
                return;
            }

            //Valida se o Nó de partida e chegada existe, ou seja se é igual à [1]
            if (proc.getMatriz().FirstOrDefault(el => el.posicaoLinha == Partx-1 && el.posicaoColuna == Party-1).valor != 1)
            {
                MessageBox.Show("Atenção o Nó de Partida não existe, ou seja não contem [1] em sua casa", "Alerta");
                return;
            }
            if (proc.getMatriz().FirstOrDefault(el => el.posicaoLinha == Chegx-1 && el.posicaoColuna == Chegy-1).valor != 1)
            {
                MessageBox.Show("Atenção o Nó de Chegada não existe, ou seja não contem [1] em sua casa", "Alerta");
                return;
            }

            #endregion



            //==Back-end==
            saida = new ProcessosSaida(proc.getMatriz(), Chegx - 1, Chegy - 1);
            saida.setPontoDePartida(Partx - 1, Party - 1);
            var retorno = saida.calculaPercurso(null, null); // CALCULA PERCURSO  


            var TodosCaminhosPoss = saida.getP(); // Todos Percursos Possiveis

            if (TodosCaminhosPoss.Any())
            {
                var percursosFront = populaResultadoFrontE(TodosCaminhosPoss);
                listaPercursos.ItemsSource = percursosFront;
                MenorCaminho.Content = this.MenorPercurso;
                MaiorCaminho.Content = this.MaiorPercurso;
                TtotalCaminhosSucesso.Content = this.TotalPercorridoSuecsso;

                lblTamMaior.Content = "[" + this.TamanhoMaiorPercurso + "]";
                lblTamMenor.Content = "[" + this.TamanhoMenorPercurso + "]";

                // validação Ux
                lblValidacao.Visibility = Visibility.Collapsed;
                ScrolLista.Visibility = Visibility.Visible;
                GridResultado.Visibility = Visibility.Visible;

                lbltotal.Content = "Quantidade de Caminhos Possiveis:";
                lblValidacao.Visibility = Visibility.Visible;
                lblValidacao.Content = "Nó encontrado com sucesso";

            }
            else
            {
                var todosPercorridos = saida.getTodosPerc();
                var percursosFront = populaResultadoFrontE(todosPercorridos);
                listaPercursos.ItemsSource = percursosFront;
                MenorCaminho.Content = this.MenorPercurso;
                MaiorCaminho.Content = this.MaiorPercurso;
                lblTamMaior.Content = "[" + this.TamanhoMaiorPercurso + "]";
                lblTamMenor.Content = "[" + this.TamanhoMenorPercurso + "]";

                lbltotal.Content = "Todos os Caminhos Percorridos::";
                TtotalCaminhosSucesso.Content = this.TotalPercorridoSuecsso;

                // validação Ux
                lblValidacao.Visibility = Visibility.Visible;
                lblValidacao.Content = "Não foi possivel encontrar um caminho Válido";

            }





        }


    }
}
