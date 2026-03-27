using System;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;

namespace SistemaCadastro
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            // Inicializa o arquivo JSON (usuarios.json)
            Database.Inicializar(); 

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new LoginForm());
        }
    }

    // --- JANELA DE LOGIN ---
    public class LoginForm : Form
    {
        private TextBox txtUsuario, txtSenha;
        private Button btnLogar, btnEsqueci;

        public LoginForm()
        {
            this.Text = "Acesso ao Sistema";
            this.Size = new Size(400, 480);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(45, 45, 48); 
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            Label lblTitulo = new Label() { 
                Text = "LOGIN", Left = 50, Top = 30, Height = 50, Width = 300, 
                Font = new Font("Segoe UI", 24, FontStyle.Bold), 
                ForeColor = Color.Cyan, TextAlign = ContentAlignment.MiddleCenter 
            };

            Label lblUser = new Label() { Text = "Usuário:", Left = 50, Top = 100, ForeColor = Color.White, Font = new Font("Segoe UI", 10) };
            txtUsuario = new TextBox() { Left = 50, Top = 125, Width = 280, Font = new Font("Segoe UI", 12), BackColor = Color.WhiteSmoke };

            Label lblPass = new Label() { Text = "Senha:", Left = 50, Top = 170, ForeColor = Color.White, Font = new Font("Segoe UI", 10) };
            txtSenha = new TextBox() { Left = 50, Top = 195, Width = 280, Font = new Font("Segoe UI", 12), PasswordChar = '●', BackColor = Color.WhiteSmoke };

            btnLogar = new Button() { 
                Text = "ENTRAR", Left = 50, Top = 260, Width = 280, Height = 45, 
                BackColor = Color.MediumSpringGreen, FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 12, FontStyle.Bold), Cursor = Cursors.Hand
            };
            btnLogar.Click += BtnLogar_Click;

            btnEsqueci = new Button() { 
                Text = "Esqueci minha senha", Left = 50, Top = 320, Width = 280, Height = 35, 
                BackColor = Color.FromArgb(255, 128, 128), FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold), Cursor = Cursors.Hand
            };
            btnEsqueci.Click += (s, e) => { new RecoveryForm().ShowDialog(); };

            Button btnCadastrar = new Button() { 
                Text = "CADASTRAR NOVO USUÁRIO", Left = 50, Top = 375, Width = 280, Height = 35, 
                BackColor = Color.MediumSlateBlue, FlatStyle = FlatStyle.Flat,
                ForeColor = Color.White, Font = new Font("Segoe UI", 9, FontStyle.Bold), Cursor = Cursors.Hand
            };
            btnCadastrar.Click += (s, e) => { new CadastroForm().ShowDialog(); };

            this.Controls.AddRange(new Control[] { lblTitulo, lblUser, txtUsuario, lblPass, txtSenha, btnLogar, btnEsqueci, btnCadastrar });
        }

        private void BtnLogar_Click(object? sender, EventArgs e)
        {
            // Lógica JSON: Busca na lista carregada
            var usuarios = Database.ObterTodos();
            var userLogado = usuarios.FirstOrDefault(u => u.Login == txtUsuario.Text && u.Senha == txtSenha.Text);

            if (userLogado != null)
            {
                this.Hide();
                new WelcomeForm().Show();
            }
            else 
            { 
                MessageBox.Show("Credenciais Inválidas!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error); 
            }
        }
    }

    // --- JANELA DE RECUPERAÇÃO ---
    public class RecoveryForm : Form
    {
        private TextBox txtValidarUser;
        private Button btnRecuperar;

        public RecoveryForm()
        {
            this.Text = "Recuperar Dados";
            this.Size = new Size(400, 300);
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.FromArgb(30, 30, 30);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;

            Label lblInstrucao = new Label() { 
                Text = "Digite seu login para recuperar os dados:", 
                Left = 40, Top = 40, Width = 320, ForeColor = Color.Yellow, 
                Font = new Font("Segoe UI", 10, FontStyle.Bold) 
            };

            txtValidarUser = new TextBox() { Left = 40, Top = 80, Width = 300, Font = new Font("Segoe UI", 12) };

            btnRecuperar = new Button() { 
                Text = "RECUPERAR DADOS", Left = 40, Top = 140, Width = 300, Height = 50, 
                BackColor = Color.Gold, FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11, FontStyle.Bold), Cursor = Cursors.Hand 
            };
            btnRecuperar.Click += BtnRecuperar_Click;

            this.Controls.AddRange(new Control[] { lblInstrucao, txtValidarUser, btnRecuperar });
        }

        private void BtnRecuperar_Click(object? sender, EventArgs e)
        {
            string loginInformado = txtValidarUser.Text.Trim();

            if (string.IsNullOrWhiteSpace(loginInformado))
            {
                MessageBox.Show("Por favor, digite o login.");
                return;
            }

            // Lógica JSON: Busca o usuário pelo nome
            var usuarios = Database.ObterTodos();
            var userEncontrado = usuarios.FirstOrDefault(u => u.Login.Equals(loginInformado, StringComparison.OrdinalIgnoreCase));

            if (userEncontrado != null)
            {
                MessageBox.Show($"Dados encontrados!\n\nUsuário: {userEncontrado.Login}\nSenha: {userEncontrado.Senha}", 
                                "Recuperação", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            else
            {
                MessageBox.Show("Usuário não encontrado.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }

    // --- JANELA DE SUCESSO ---
    public class WelcomeForm : Form
    {
        public WelcomeForm()
        {
            this.Text = "Dashboard";
            this.Size = new Size(500, 300);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(0, 122, 204); 

            Label lblMsg = new Label() { 
                Text = "Acesso Permitido!\nO sistema está pronto.", 
                Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter, 
                ForeColor = Color.White, Font = new Font("Segoe UI", 18, FontStyle.Italic) 
            };

            this.Controls.Add(lblMsg);
            this.FormClosing += (s, e) => Application.Exit();
        }
    }
}