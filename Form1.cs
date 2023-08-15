using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CRUDcsv
{
    public partial class Form1 : Form
    {
        public static String CRUDcsv = Directory.GetCurrentDirectory() + "\\CRUDcsv.csv";

        public string BUSCA, ID, NOME, EMAIL, TUDO;
        int LINHA = 0;

        public Random random;

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtId.Text))
                {
                    MessageBox.Show("Forneça o ID");
                }
                else
                {
                    if (LINHA > 0)
                    {
                        bool linhaDeletada = DeletarLinhaCSV(CRUDcsv, LINHA);
                        if (linhaDeletada)
                        {         
                            txtId.Clear();
                            txtNome.Clear();
                            txtEmail.Clear();

                            int numeroAleatorio = random.Next(1, 10001);
                            txtId.Text = numeroAleatorio.ToString();

                            MessageBox.Show("Registro deletado com sucesso !");
                        }
                        else
                        {
                            MessageBox.Show("Não há registro ativo para esse ID");
                        }
                    }
                }
            }
            catch (Exception EX)
            {

                MessageBox.Show("ERRO: " + EX.Message);
            }
            
        }

        private void btnPesquisar_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtId.Text))
                {
                    MessageBox.Show("Forneça o ID");
                }
                else
                {
                    LINHA = BuscarLinhaCSV(CRUDcsv, txtId.Text);

                    BUSCA = Convert.ToString(txtId.Text);

                    string[] lines = File.ReadAllLines(CRUDcsv);

                    using (StreamReader reader = new StreamReader(CRUDcsv))
                    {
                        foreach (string line in lines)
                        {
                            string[] values = line.Split(';');

                            if (values.Contains(BUSCA))
                            {
                                txtId.Text = values[0];
                                txtNome.Text = values[1];
                                txtEmail.Text = values[2];
                                return;
                            }
                        }
                    }

                    MessageBox.Show("Pesquisa não encontrada");
                }
            }
            catch (Exception EX)
            {
                MessageBox.Show("ERRO: " + EX.Message);
            }
        }

        private void btnLancar_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtId.Text))
                {
                    MessageBox.Show("Forneça o ID");
                }
                else
                {
                    if (File.Exists(CRUDcsv))
                    {

                        ID = txtId.Text;
                        NOME = txtNome.Text;
                        EMAIL = txtEmail.Text;

                        TUDO = ID + ";" + NOME + ";" + EMAIL;

                        string[] DadosCompletos = { ID, NOME, EMAIL };
                        using (StreamWriter SW = new StreamWriter(CRUDcsv, true))
                        {
                            SW.WriteLine(TUDO);
                        }

                        txtId.Clear();
                        txtNome.Clear();
                        txtEmail.Clear();

                        int numeroAleatorio = random.Next(1, 10001);
                        txtId.Text = numeroAleatorio.ToString();

                        MessageBox.Show("Entrada efetuada com sucesso !");
                    }
                }
            }
            catch (Exception EX)
            {
                MessageBox.Show("ERRO :" + EX.Message);
            }            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            txtId.Clear();
            txtNome.Clear();
            txtEmail.Clear();

            int numeroAleatorio = random.Next(1, 10001);
            txtId.Text = numeroAleatorio.ToString();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            int numeroAleatorio = random.Next(1, 10001);
            txtId.Text = numeroAleatorio.ToString();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtId.Text))
                {
                    MessageBox.Show("Forneça o ID");
                }
                else
                {
                    LINHA = BuscarLinhaCSV(CRUDcsv, txtId.Text);

                    if (LINHA != -1)
                    {
                        string[] lines = File.ReadAllLines(CRUDcsv);
                        lines[LINHA - 1] = txtId.Text + ";" + txtNome.Text + ";" + txtEmail.Text;

                        File.WriteAllLines(CRUDcsv, lines);

                        MessageBox.Show("Registro editado com sucesso !");

                        txtId.Clear();
                        txtNome.Clear();
                        txtEmail.Clear();

                        int numeroAleatorio = random.Next(1, 10001);
                        txtId.Text = numeroAleatorio.ToString();
                    }
                    else
                    {
                        MessageBox.Show("Registro não encontrado");
                    }
                }
            }
            catch (Exception EX)
            {
                MessageBox.Show("ERRO: " + EX.Message);
            }
        }

        public Form1()
        {
            InitializeComponent();

            random = new Random();
        }

        private void btnSair_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Deseja sair ?", "Confirmação", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private bool DeletarLinhaCSV(string pathArquivoCSV, int numeroLinha)
        {
            string arquivoTemporario = Path.GetTempFileName();

            using (StreamReader reader = new StreamReader(CRUDcsv))
            using (StreamWriter writer = new StreamWriter(arquivoTemporario))
            {
                string linha;
                int linhaAtual = 1;

                while ((linha = reader.ReadLine()) != null)
                {
                    if (linhaAtual != numeroLinha)
                    {
                        writer.WriteLine(linha);
                    }

                    linhaAtual++;
                }
            }

            File.Delete(CRUDcsv);
            File.Move(arquivoTemporario, CRUDcsv);

            return File.Exists(CRUDcsv);
        }

        private int BuscarLinhaCSV(string pathArquivoCSV, string valorBusca)
        {
            using (StreamReader reader = new StreamReader(CRUDcsv))
            {
                string linha;
                int numeroLinha = 0;

                while ((linha = reader.ReadLine()) != null)
                {
                    numeroLinha++;

                    // Verifica se a linha contém o valor de busca
                    if (linha.Contains(valorBusca))
                    {
                        return numeroLinha;
                    }
                }
            }

            return -1; // Valor não encontrado
        }
    }
}
