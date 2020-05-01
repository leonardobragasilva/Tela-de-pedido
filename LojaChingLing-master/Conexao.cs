using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace LojaCL
{
    class clConexao
    {
        //Conectar ao SQLServer Express, com a string de conexão
        private static string str = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\LojaChingLing-master\\DbLoja.mdf;Integrated Security=True;Connect Timeout=30";
        //representa a conexão com o banco
        private static SqlConnection con = null;
        //método que obtem a conexão com o banco
        public static SqlConnection obterConexao()
        {
            con = new SqlConnection(str);
            //verificar a conexão
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            try
            {
                con.Open();
            }
            catch (SqlException sqle)
            {
                con = null;
            }
            return con;
        }
        public static void fecharConexao()
        {
        // Se não receber conexão nula, ele fecha a conexão...
        if (con != null)
            {
                con.Close();
            }
        }
    }
}
