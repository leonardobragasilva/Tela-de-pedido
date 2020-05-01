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
    public partial class FmrCrudLogin : Form
    {
        public FmrCrudLogin()
        {
            InitializeComponent();
        }

        private void btnSair_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void btnLogar_Click(object sender, EventArgs e)
        {
            {
                String str = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\User\\Desktop\\LojaChingLing-master\\DbLoja.mdf;Integrated Security=True;Connect Timeout=30";
                string usu = "select login,senha from usuario where login=@login and senha=@senha";
                SqlConnection con = new SqlConnection(str);
                SqlCommand cmd = new SqlCommand(usu, con);
                cmd.Parameters.AddWithValue("@login", SqlDbType.NChar).Value = txtLogin.Text.Trim();
                cmd.Parameters.AddWithValue("@senha", SqlDbType.NChar).Value = txtSenha.Text.Trim();
                con.Open();
                cmd.CommandType = CommandType.Text;
                SqlDataReader usuario = cmd.ExecuteReader();
                if (usuario.HasRows)
                {
                    this.Hide();
                    FrmPrincipal pri = new FrmPrincipal();
                    pri.Show();
                    con.Close();
                }
                else
                {
                    MessageBox.Show("Login ou senha incorretos! Tente novamente!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtLogin.Text = "";
                    txtSenha.Text = "";
                    con.Close();
                }

            }
        }
    }
}

