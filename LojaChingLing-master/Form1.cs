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
    public partial class FrmCartao : Form
    {
        public FrmCartao()
        {
            InitializeComponent();
        }
        public void CarregadgvCartao()
        {
            String str = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\LojaChingLing-master\\DbLoja.mdf;Integrated Security=True;Connect Timeout=30";
            String query = "select * from cartaovenda";
            SqlConnection con = new SqlConnection(str);
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
            cmd.CommandType = CommandType.Text;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable cliente = new DataTable();
            da.Fill(cliente);
            dgvCartao.DataSource = cliente;
            con.Close();
        }

        private void btnCadastrar_Click(object sender, EventArgs e)
        {
            try
            {
                String str = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\LojaChingLing-master\\DbLoja.mdf;Integrated Security=True;Connect Timeout=30";
                SqlConnection con = new SqlConnection(str);
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandText = "Inserir_CartaoVenda";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@numero", txtNumero.Text);
                cmd.Parameters.AddWithValue("@usuario", cbxUsuario.Text);
                con.Open();
                cmd.ExecuteNonQuery();
                CarregadgvCartao();
                FrmPrincipal obj = (FrmPrincipal)Application.OpenForms["FrmPrincipal"];
                obj.CarregadgvPripedi();
                MessageBox.Show("Registro inserido com sucesso!", "Cadastro", MessageBoxButtons.OK);
                con.Close();
                txtNumero.Text = "";
                cbxUsuario.Text = "";
                
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message);
            }
        }

        private void FrmCartao_Load(object sender, EventArgs e)
        {
            if (cbxUsuario.DisplayMember == "")
            {
                cbxUsuario.Enabled = true;
     
            }
            CarregacbxUsuario();
            CarregadgvCartao();
        }

        private void dgvCartao_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void btnSair_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPesquisar_Click(object sender, EventArgs e)
        {
            try
            {
                String str = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\LojaChingLing-master\\DbLoja.mdf;Integrated Security=True;Connect Timeout=30";
                SqlConnection con = new SqlConnection(str);
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandText = "Localizar_CartaoVenda";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", this.txtId.Text);
                con.Open();
                SqlDataReader rd = cmd.ExecuteReader();
                if (rd.Read())
                    {
                    txtId.Text = rd["id"].ToString();
                    txtNumero.Text = rd["numero"].ToString();
                    cbxUsuario.Text = rd["usuario"].ToString();
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
                cmd.CommandText = "Atualizar_CartaoVenda";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("id", this.txtId.Text);
                cmd.Parameters.AddWithValue("@numero", this.txtNumero.Text);
                cmd.Parameters.AddWithValue("@usuario", this.cbxUsuario.Text);
                
                con.Open();
                cmd.ExecuteNonQuery();
                CarregadgvCartao();
                FrmPrincipal obj = (FrmPrincipal)Application.OpenForms["FrmPrincipal"];
                obj.CarregadgvPripedi();
                MessageBox.Show("Registro atualizado com sucesso!", "Atualizar Registro", MessageBoxButtons.OK, MessageBoxIcon.Information);
                con.Close();
                txtId.Text = "";
                txtNumero.Text = "";
                cbxUsuario.Text = "";
                
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
                cmd.CommandText = "Excluir_CartaoVenda";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", this.txtId.Text);
                con.Open();
                cmd.ExecuteNonQuery();
                CarregadgvCartao();
                FrmPrincipal obj = (FrmPrincipal)Application.OpenForms["FrmPrincipal"];
                obj.CarregadgvPripedi();
                MessageBox.Show("Registro apagado com sucesso!", "Excluir Registro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                con.Close();
                txtId.Text = "";
                txtNumero.Text = "";
                cbxUsuario.Text = "";
                
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message);
            }
        }
        SqlConnection con = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\LojaChingLing-master\\DbLoja.mdf;Integrated Security=True;Connect Timeout=30");
        
        public void CarregacbxUsuario()
        {
            string pro = "select id, nome from usuario";
            SqlCommand cmd = new SqlCommand(pro, con);
            con.Open();
            cmd.CommandType = CommandType.Text;
            SqlDataAdapter da = new SqlDataAdapter(pro, con);
            DataSet ds = new DataSet();
            da.Fill(ds, "nome");
            cbxUsuario.ValueMember = "id";
            cbxUsuario.DisplayMember = "nome";
            cbxUsuario.DataSource = ds.Tables["nome"];
            con.Close();
        }

        private void dgvCartao_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dgvCartao.Rows[e.RowIndex];
                txtId.Text = row.Cells[0].Value.ToString();
                txtNumero.Text = row.Cells[1].Value.ToString();
                cbxUsuario.Text = row.Cells[2].Value.ToString();
                
            }
        }

        private void cbxUsuario_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        
    }
}
