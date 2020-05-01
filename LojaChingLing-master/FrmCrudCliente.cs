using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace LojaCL
{
    public partial class FrmCrudCliente : Form
    {
        public FrmCrudCliente()
        {
            InitializeComponent();
        }

        public void CarregaDgvCliente()
        {
            String str = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\LojaChingLing-master\\DbLoja.mdf;Integrated Security=True;Connect Timeout=30";
            String query = "select * from cliente";
            SqlConnection con = new SqlConnection(str);
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
            cmd.CommandType = CommandType.Text;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable cliente = new DataTable();
            da.Fill(cliente);
            DgvCliente.DataSource = cliente;
            con.Close();
        }

        private void btnSair_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnCadastro_Click(object sender, EventArgs e)
        {
            try
            {
                String str = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\LojaChingLing-master\\DbLoja.mdf;Integrated Security=True;Connect Timeout=30";
                SqlConnection con = new SqlConnection(str);
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandText = "Inserir";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@cpf", txtCpf.Text);
                cmd.Parameters.AddWithValue("@nome", txtNome.Text);
                cmd.Parameters.AddWithValue("@endereco", txtEndereco.Text);
                cmd.Parameters.AddWithValue("@celular", txtCelular.Text);
                cmd.Parameters.AddWithValue("@email", txtEmail.Text);
                con.Open();
                cmd.ExecuteNonQuery();
                CarregaDgvCliente();
                MessageBox.Show("Registro inserido com sucesso!", "Cadastro", MessageBoxButtons.OK);
                con.Close();
                txtCpf.Text = "";
                txtNome.Text = "";
                txtEndereco.Text = "";
                txtCelular.Text = "";
                txtEmail.Text = "";
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message);
            }
        }

        private void btnPesquisa_Click(object sender, EventArgs e)
        {
            try
            {
                String str = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\LojaChingLing-master\\DbLoja.mdf;Integrated Security=True;Connect Timeout=30";
                SqlConnection con = new SqlConnection(str);
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandText = "Localizar";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@cpf", this.txtCpf.Text);
                con.Open();
                SqlDataReader rd = cmd.ExecuteReader();
                if (rd.Read())
                {
                    txtCpf.Text = rd["cpf"].ToString();
                    txtNome.Text = rd["nome"].ToString();
                    txtEndereco.Text = rd["endereco"].ToString();
                    txtCelular.Text = rd["celular"].ToString();
                    txtEmail.Text = rd["email"].ToString();
                }
                else
                {
                    MessageBox.Show("Nenhum registro encontrado!", "Sem registro!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            finally
            {
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            try
            {
                String str = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\LojaChingLing-master\\DbLoja.mdf;Integrated Security=True;Connect Timeout=30";
                SqlConnection con = new SqlConnection(str);
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandText = "Atualizar";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@cpf", this.txtCpf.Text);
                cmd.Parameters.AddWithValue("@nome", this.txtNome.Text);
                cmd.Parameters.AddWithValue("@endereco", this.txtEndereco.Text);
                cmd.Parameters.AddWithValue("@celular", this.txtCelular.Text);
                cmd.Parameters.AddWithValue("@email", this.txtEmail.Text);
                con.Open();
                cmd.ExecuteNonQuery();
                CarregaDgvCliente();
                MessageBox.Show("Registro atualizado com sucesso!", "Atualizar Registro", MessageBoxButtons.OK, MessageBoxIcon.Information);
                con.Close();
                txtCpf.Text = "";
                txtNome.Text = "";
                txtEndereco.Text = "";
                txtCelular.Text = "";
                txtEmail.Text = "";
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message);
            }

        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            try
            {
                String str = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\LojaChingLing-master\\DbLoja.mdf;Integrated Security=True;Connect Timeout=30";
                SqlConnection con = new SqlConnection(str);
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandText = "Excluir";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@cpf", this.txtCpf.Text);
                con.Open();
                cmd.ExecuteNonQuery();
                CarregaDgvCliente();
                MessageBox.Show("Registro apagado com sucesso!", "Excluir Registro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                con.Close();
                txtCpf.Text = "";
                txtNome.Text = "";
                txtEndereco.Text = "";
                txtCelular.Text = "";
                txtEmail.Text = "";
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message);
            }
        }

        private void FrmCrudCliente_Load(object sender, EventArgs e)
        {
            CarregaDgvCliente();
        }

        private void DgvCliente_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.DgvCliente.Rows[e.RowIndex];
                txtCpf.Text = row.Cells[0].Value.ToString();
                txtNome.Text = row.Cells[1].Value.ToString();
                txtEndereco.Text = row.Cells[2].Value.ToString();
                txtCelular.Text = row.Cells[3].Value.ToString();
                txtEmail.Text = row.Cells[4].Value.ToString();
            }
        }

        private void DgvCliente_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
