using System;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Data.Sqlite;

public class CadastroForm : Form
{
    private TextBox txtNovoUser, txtNovaSenha;

    public CadastroForm()
    {
        this.Text = "Novo Cadastro";
        this.Size = new Size(380, 400);
        this.StartPosition = FormStartPosition.CenterParent;
        this.BackColor = Color.FromArgb(30, 30, 30);
        this.FormBorderStyle = FormBorderStyle.FixedDialog;

        Label lblTitulo = new Label() { 
            Text = "CRIAR CONTA", Top = 20, Height = 50, Width = 380, 
            Font = new Font("Segoe UI", 18, FontStyle.Bold), 
            ForeColor = Color.MediumOrchid, TextAlign = ContentAlignment.MiddleCenter 
        };

        Label lblUser = new Label() { Text = "Novo Usuário:", Left = 40, Top = 80, ForeColor = Color.White };
        txtNovoUser = new TextBox() { Left = 40, Top = 105, Width = 280, Font = new Font("Segoe UI", 12) };

        Label lblPass = new Label() { Text = "Nova Senha:", Left = 40, Top = 160, ForeColor = Color.White };
        txtNovaSenha = new TextBox() { Left = 40, Top = 185, Width = 280, Font = new Font("Segoe UI", 12), PasswordChar = '●' };

        Button btnSalvar = new Button() { 
            Text = "CADASTRAR", Left = 40, Top = 260, Width = 280, Height = 45, 
            BackColor = Color.MediumOrchid, FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI", 12, FontStyle.Bold), Cursor = Cursors.Hand 
        };
        btnSalvar.Click += BtnSalvar_Click;

        this.Controls.AddRange(new Control[] { lblTitulo, lblUser, txtNovoUser, lblPass, txtNovaSenha, btnSalvar });
    }

    private void BtnSalvar_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtNovoUser.Text) || string.IsNullOrWhiteSpace(txtNovaSenha.Text))
        {
            MessageBox.Show("Preencha todos os campos!");
            return;
        }

        try
        {
            using (var conn = Database.GetConnection())
            {
                conn.Open();
                var cmd = conn.CreateCommand();
                cmd.CommandText = "INSERT INTO Usuarios (Login, Senha) VALUES ($login, $senha)";
                cmd.Parameters.AddWithValue("$login", txtNovoUser.Text);
                cmd.Parameters.AddWithValue("$senha", txtNovaSenha.Text);
                cmd.ExecuteNonQuery();
            }
            MessageBox.Show("Usuário cadastrado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }
        catch { MessageBox.Show("Este nome de usuário já existe."); }
    }
}