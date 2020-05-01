﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Xml;
using System.Runtime.Serialization;

namespace LojaCL
{
    public partial class FrmPedido : Form
    {
        SqlConnection con = clConexao.obterConexao();
        public FrmPedido()
        {
            InitializeComponent();
        }

        static int botaoclicado = 0;

        public void CarregacbxCartao()
        {
            String car = "SELECT * FROM cartaovenda";
            SqlCommand cmd = new SqlCommand(car, con);
            clConexao.obterConexao();
            cmd.CommandType = CommandType.Text;
            SqlDataAdapter da = new SqlDataAdapter(car, con);
            DataSet ds = new DataSet();
            da.Fill(ds, "numero");
            cbxCartao.ValueMember = "Id";
            cbxCartao.DisplayMember = "numero";
            cbxCartao.DataSource = ds.Tables["numero"];
            clConexao.fecharConexao();
        }

        public void CarregarcbxProduto()
        {
            String pro = "SELECT Id, nome FROM produto";
            SqlCommand cmd = new SqlCommand(pro, con);
            clConexao.obterConexao();
            cmd.CommandType = CommandType.Text;
            SqlDataAdapter da = new SqlDataAdapter(pro, con);
            DataSet ds = new DataSet();
            da.Fill(ds, "nome");
            cbxProduto.ValueMember = "Id";
            cbxProduto.DisplayMember = "nome";
            cbxProduto.DataSource = ds.Tables["nome"];
            clConexao.fecharConexao();
        }

        private void btnSair_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FrmPedido_Load(object sender, EventArgs e)
        {
            cbxProduto.Enabled = false;
            txtQuantidade.Enabled = false;
            btnNovoItem.Enabled = false;
            btnFinalizar.Enabled = false;
            btnEcluirItem.Enabled = false;
            btnEditarItem.Enabled = false;
            btnAtualizar.Enabled = false;
            CarregacbxCartao();
            cbxCartao.Text = "";
        }

        private void cbxCartao_SelectedIndexChanged(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand("Localizar_CartaoVenda", con);
            cmd.Parameters.AddWithValue("@Id", cbxCartao.SelectedValue);
            cmd.CommandType = CommandType.StoredProcedure;
            clConexao.obterConexao();
            SqlDataReader rd = cmd.ExecuteReader();
            if (rd.Read())
            {
                txtUsuario.Text = rd["usuario"].ToString();
                clConexao.fecharConexao();
                rd.Dispose();
            }
            else
            {
                MessageBox.Show("Nenhum registro encontrado!", "Falha na Pesquisa", MessageBoxButtons.OK, MessageBoxIcon.Information);
                clConexao.fecharConexao();
                rd.Dispose();
            }

        }

        private void btnPedido_Click(object sender, EventArgs e)
        {
            cbxProduto.Enabled = true;
            txtQuantidade.Enabled = true;
            dgvPedido.Enabled = true;
            btnNovoItem.Enabled = true;
            btnFinalizar.Enabled = true;
            btnEcluirItem.Enabled = true;
            btnEditarItem.Enabled = true;
            btnAtualizar.Enabled = false;
            CarregarcbxProduto();
            cbxProduto.Text = "";
            dgvPedido.Columns.Add("ID", "IDProduto");
            dgvPedido.Columns.Add("Usuario", "Usuario");
            dgvPedido.Columns.Add("Produto", "Produto");
            dgvPedido.Columns.Add("Quantidade", "Quantidade");
            dgvPedido.Columns.Add("Valor", "Valor");
            dgvPedido.Columns.Add("Total", "Total");


        }

        private void btnFinalizar_Click(object sender, EventArgs e)
        {
            clConexao.obterConexao();
            SqlCommand cartao = new SqlCommand("AtualizarStatusCartaoVenda", con);
            cartao.CommandType = CommandType.StoredProcedure;
            cartao.Parameters.AddWithValue("@Id", SqlDbType.Int).Value = cbxCartao.SelectedValue;
            foreach (DataGridViewRow dr in dgvPedido.Rows)
            {
                SqlCommand pedidos = new SqlCommand("InserirPedidos", con);
                pedidos.CommandType = CommandType.StoredProcedure;
                pedidos.Parameters.AddWithValue("@id_cartaovenda", SqlDbType.Int).Value = cbxCartao.SelectedValue;
                pedidos.Parameters.AddWithValue("@id_produto", SqlDbType.Int).Value = Convert.ToInt32(dr.Cells[0].Value);
                pedidos.Parameters.AddWithValue("@usuario", SqlDbType.NChar).Value = dr.Cells[1].Value;
                pedidos.Parameters.AddWithValue("@quantidade", SqlDbType.Int).Value = Convert.ToInt32(dr.Cells[3].Value);
                pedidos.Parameters.AddWithValue("@dia_hora", SqlDbType.DateTime).Value = DateTime.Now;
	            pedidos.Parameters.AddWithValue("@valor", SqlDbType.Int).Value = Convert.ToDecimal(dr.Cells[4].Value);
                pedidos.Parameters.AddWithValue("@total", SqlDbType.Int).Value = Convert.ToDecimal(dr.Cells[5].Value);
                clConexao.obterConexao();
                pedidos.ExecuteNonQuery();
                cartao.ExecuteNonQuery();
                clConexao.fecharConexao();
            }
            MessageBox.Show("Pedido realizado com sucesso!", "Pedido", MessageBoxButtons.OK, MessageBoxIcon.Information);
            cbxProduto.Text = "";
            txtQuantidade.Text = "";
            txtValor.Text = "";
            txtValorTotal.Text = "";
            cbxProduto.Enabled = false;
            txtQuantidade.Enabled = false;
            txtValor.Enabled = false;
            btnNovoItem.Enabled = false;
            btnEditarItem.Enabled = false;
            btnEcluirItem.Enabled = false;
            btnFinalizar.Enabled = false;
            //limpar o daraGrid pedido 
            dgvPedido.Rows.Clear();
            dgvPedido.Refresh();
            FrmPrincipal obj = (FrmPrincipal)Application.OpenForms["frmPrincipal"];
            obj.CarregadgvPripedi();

        }

        private void cbxProduto_SelectedIndexChanged(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand("LocalizarProduto", con);
            cmd.Parameters.AddWithValue("@Id", cbxProduto.SelectedValue);
            cmd.CommandType = CommandType.StoredProcedure;
            clConexao.obterConexao();
            SqlDataReader rd = cmd.ExecuteReader();
            if (rd.Read())
            {
                txtValor.Text = rd["valor"].ToString();
                txtId.Text = rd["id"].ToString();
                clConexao.fecharConexao();
                rd.Dispose();
            }
            else 
            {
                MessageBox.Show("Nenhum registro encontrado", "falha na pesquisa", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnNovoItem_Click(object sender, EventArgs e)
        {
            if (botaoclicado == 1)
            {
                SqlCommand pedido2 = new SqlCommand("InserirPedido", con);
                pedido2.CommandType = CommandType.StoredProcedure;
                pedido2.Parameters.AddWithValue("@id_cartaovenda", SqlDbType.Int).Value = cbxCartao.SelectedValue;
                pedido2.Parameters.AddWithValue("@id_produto", SqlDbType.Int).Value = cbxProduto.SelectedValue;
                pedido2.Parameters.AddWithValue("@usuario", SqlDbType.NChar).Value = txtUsuario.Text;
                pedido2.Parameters.AddWithValue("@quantidade", SqlDbType.Int).Value = Convert.ToInt32(txtQuantidade.Text); ;
                pedido2.Parameters.AddWithValue("@dia_hora", SqlDbType.DateTime).Value = DateTime.Now;
                pedido2.Parameters.AddWithValue("@valor", SqlDbType.Int).Value = Convert.ToDecimal(txtValor.Text);
                pedido2.Parameters.AddWithValue("@total", SqlDbType.Int).Value = Convert.ToDecimal(txtQuantidade.Text) * Convert.ToDecimal(txtValor.Text);
                clConexao.obterConexao();
                pedido2.ExecuteNonQuery();
                clConexao.fecharConexao();
                MessageBox.Show("Pedido atualizado", " Atualizado pedido", MessageBoxButtons.OK, MessageBoxIcon.Information);
                botaoclicado = 0;
            }
            else
            {
                DataGridViewRow item = new DataGridViewRow();
                item.CreateCells(dgvPedido);
                item.Cells[0].Value = txtId.Text;
                item.Cells[1].Value = txtUsuario.Text;
                item.Cells[2].Value = cbxProduto.Text;
                item.Cells[3].Value = txtQuantidade.Text;
                item.Cells[4].Value = txtValor.Text;
                item.Cells[5].Value = Convert.ToDecimal(txtValor.Text)*Convert.ToDecimal(txtValor.Text);
                dgvPedido.Rows.Add(item);
                cbxProduto.Text = "";
                txtQuantidade.Text = "";
                txtValor.Text = "";
                txtQuantidade.Text = "";
                decimal soma = 0;
                foreach (DataGridViewRow dr in dgvPedido.Rows)
                    soma += Convert.ToDecimal(dr.Cells[5].Value);
                txtValorTotal.Text = Convert.ToString(soma);

            }
        }

        private void dgvPedido_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow row = this.dgvPedido.Rows[e.RowIndex];
            cbxProduto.Text = row.Cells[2].Value.ToString();
            txtQuantidade.Text = row.Cells[3].Value.ToString();
            txtValor.Text = row.Cells[4].Value.ToString();
            int linha = dgvPedido.CurrentRow.Index;

        }

        private void btnEditarItem_Click(object sender, EventArgs e)
        {
            int linha = dgvPedido.CurrentRow.Index;
            dgvPedido.Rows[linha].Cells[0].Value = txtId.Text;
            dgvPedido.Rows[linha].Cells[2].Value = cbxProduto.Text;
            dgvPedido.Rows[linha].Cells[3].Value = txtQuantidade.Text;
            dgvPedido.Rows[linha].Cells[4].Value = txtValor.Text;
            dgvPedido.Rows[linha].Cells[5].Value = Convert.ToDecimal(txtValor.Text) * Convert.ToDecimal(txtQuantidade.Text);
            decimal soma = 0;
            foreach (DataGridViewRow dr in dgvPedido.Rows)
                soma += Convert.ToDecimal(dr.Cells[5].Value);
            txtValorTotal.Text = Convert.ToString(soma);
            cbxProduto.Text = "";
            txtQuantidade.Text = "";
            txtValor.Text = "";
            txtQuantidade.Text = "";


        }

        private void btnEcluirItem_Click(object sender, EventArgs e)
        {
            int linha = dgvPedido.CurrentRow.Index;
            string querry = "DELETE FROM pedido WHERE (id_cartaovenda = @id_cartaovenda AND id_produto = @id_produto)";
            SqlCommand cmd = new SqlCommand(querry, con);
            DataGridViewRow row = dgvPedido.Rows[linha];
            cmd.Parameters.AddWithValue("@id_cartaovenda", cbxCartao.SelectedValue);
            cmd.Parameters.AddWithValue("@id_produto", row.Cells[0].Value);
            clConexao.obterConexao();
            cmd.ExecuteNonQuery();
            clConexao.fecharConexao();
            dgvPedido.Rows.RemoveAt(linha);
            dgvPedido.Refresh();
            decimal soma = 0;
            foreach (DataGridViewRow dr in dgvPedido.Rows)
                soma += Convert.ToDecimal(dr.Cells[5].Value);
            txtValorTotal.Text = Convert.ToString(soma);
            cbxProduto.Text = "";
            txtQuantidade.Text = "";
            txtValor.Text = "";
            txtQuantidade.Text = "";
        }

        private void btnLocalizar_Click(object sender, EventArgs e)
        {
            botaoclicado = 1;
            cbxProduto.Enabled = true;
            txtQuantidade.Enabled = true;
            btnNovoItem.Enabled = true;
            btnEcluirItem.Enabled = true;
            btnEditarItem.Enabled = true;
            btnFinalizar.Enabled = false;
            btnAtualizar.Enabled = true;
            CarregarcbxProduto();
            try
            {
                SqlConnection con = clConexao.obterConexao();
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandText = "LocalizarPedidoGrid";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id_cartaovenda", cbxCartao.SelectedValue);
                clConexao.obterConexao();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    dgvPedido.ReadOnly = true;
                    dgvPedido.DataSource = ds.Tables[0];
                    clConexao.fecharConexao();
                }
                else
                {
                    MessageBox.Show("nenhunm registro encontrado", "falha", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    clConexao.fecharConexao();
                }
                decimal soma = 0;
                foreach (DataGridViewRow dr in dgvPedido.Rows)
                {
                    soma += Convert.ToDecimal(dr.Cells[5].Value);
                    txtValorTotal.Text = Convert.ToString(soma);
                }
            }
            catch (Exception erro)
            {
                MessageBox.Show("Erro:" + erro);
            }
        }

        private void btnAtualizar_Click(object sender, EventArgs e)
        {
            clConexao.obterConexao();
            foreach (DataGridViewRow dr in dgvPedido.Rows)
            {
                SqlCommand pedidos = new SqlCommand("AtualizarPedidos", con);
                pedidos.CommandType = CommandType.StoredProcedure;
                pedidos.Parameters.AddWithValue("@id_cartaovenda", SqlDbType.Int).Value = cbxCartao.SelectedValue;
                pedidos.Parameters.AddWithValue("@id_produto", SqlDbType.Int).Value = Convert.ToInt32(dr.Cells[0].Value);
                pedidos.Parameters.AddWithValue("@usuario", SqlDbType.NChar).Value = dr.Cells[1].Value;
                pedidos.Parameters.AddWithValue("@quantidade", SqlDbType.Int).Value = Convert.ToInt32(dr.Cells[3].Value);
                pedidos.Parameters.AddWithValue("@dia_hora", SqlDbType.DateTime).Value = DateTime.Now;
                pedidos.Parameters.AddWithValue("@valor", SqlDbType.Decimal).Value = Convert.ToDecimal(dr.Cells[4].Value);
                pedidos.Parameters.AddWithValue("@total", SqlDbType.Decimal).Value = Convert.ToDecimal(dr.Cells[5].Value);
                clConexao.obterConexao();
                pedidos.ExecuteNonQuery();
                clConexao.fecharConexao();

            }
            MessageBox.Show("Pedido atualizado com sucesso", "Atualizar", MessageBoxButtons.OK, MessageBoxIcon.Information);
            cbxProduto.Text = "";
            txtQuantidade.Text = "";
            txtValor.Text = "";
            txtValorTotal.Text = "";
            cbxProduto.Enabled = false;
            txtQuantidade.Enabled = false;
            txtValor.Enabled = false;
            txtValorTotal.Enabled = false;
            btnNovoItem.Enabled = false;
            btnEcluirItem.Enabled = false;
            btnEditarItem.Enabled = false;
            btnFinalizar.Enabled = false;
            dgvPedido.Enabled = false;
            dgvPedido.DataSource = null;
            FrmPrincipal obj = (FrmPrincipal)Application.OpenForms["FrmPrincipal"];
            obj.CarregadgvPripedi();

        }
    }
}
