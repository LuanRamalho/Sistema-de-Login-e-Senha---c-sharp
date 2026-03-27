using System;
using System.Drawing;
using System.Windows.Forms;

public class CadastroForm : Form
{
    private TextBox txtNovoUser, txtNovaSenha;

    public CadastroForm()
    {
        // Configurações de Janela
        this.Text = "Novo Cadastro";
        this.Size = new Size(380, 420);
        this.StartPosition = FormStartPosition.CenterParent;
        this.BackColor = Color.FromArgb(30, 30, 30);
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;

        // Título Estilizado
        Label lblTitulo = new Label() { 
            Text = "NOVA CONTA", 
            Top = 25, 
            Height = 45, 
            Width = 380, 
            Font = new Font("Segoe UI", 20, FontStyle.Bold), 
            ForeColor = Color.MediumOrchid, 
            TextAlign = ContentAlignment.MiddleCenter 
        };

        // Labels e Campos
        Label lblUser = new Label() { Text = "Usuário:", Left = 45, Top = 90, ForeColor = Color.White, Font = new Font("Segoe UI", 10) };
        txtNovoUser = new TextBox() { 
            Left = 45, Top = 115, Width = 280, 
            Font = new Font("Segoe UI", 12), 
            BorderStyle = BorderStyle.FixedSingle 
        };

        Label lblPass = new Label() { Text = "Senha:", Left = 45, Top = 175, ForeColor = Color.White, Font = new Font("Segoe UI", 10) };
        txtNovaSenha = new TextBox() { 
            Left = 45, Top = 200, Width = 280, 
            Font = new Font("Segoe UI", 12), 
            PasswordChar = '●',
            BorderStyle = BorderStyle.FixedSingle 
        };

        // Botão Cadastrar
        Button btnSalvar = new Button() { 
            Text = "CADASTRAR AGORA", 
            Left = 45, Top = 280, Width = 280, Height = 50, 
            BackColor = Color.MediumOrchid, 
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI", 11, FontStyle.Bold), 
            Cursor = Cursors.Hand 
        };
        btnSalvar.FlatAppearance.BorderSize = 0;
        btnSalvar.Click += BtnSalvar_Click;

        this.Controls.AddRange(new Control[] { lblTitulo, lblUser, txtNovoUser, lblPass, txtNovaSenha, btnSalvar });
    }

    private void BtnSalvar_Click(object sender, EventArgs e)
    {
        string user = txtNovoUser.Text.Trim();
        string pass = txtNovaSenha.Text.Trim();

        if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(pass))
        {
            MessageBox.Show("Por favor, preencha todos os campos.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        // Tenta inserir no arquivo JSON via classe Database
        bool sucesso = Database.InserirUsuario(user, pass);

        if (sucesso)
        {
            MessageBox.Show("Usuário registrado com sucesso no JSON!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }
        else
        {
            MessageBox.Show("Erro: Este usuário já está cadastrado.", "Erro de Duplicata", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}