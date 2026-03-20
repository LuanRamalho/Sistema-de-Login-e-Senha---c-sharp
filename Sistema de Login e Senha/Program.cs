using System;
using System.Drawing;
using System.Windows.Forms;

namespace SistemaCadastro
{
    static class Program
    {
        // Dados para simulação
        public static string UsuarioCorreto = "admin";
        public static string SenhaCorreta = "12345";

        [STAThread]
        static void Main()
        {
            // ESSA LINHA É OBRIGATÓRIA PARA INICIALIZAR O BANCO DE DADOS:
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
            // Configurações da Janela
            this.Text = "Acesso ao Sistema";
            this.Size = new Size(400, 450);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(45, 45, 48); // Cinza Escuro Moderno
            this.FormBorderStyle = FormBorderStyle.FixedDialog;

            Label lblTitulo = new Label() { 
                Text = "LOGIN", Left = 50, Top = 30, Height = 50, Width = 300, 
                Font = new Font("Segoe UI", 24, FontStyle.Bold), 
                ForeColor = Color.Cyan, TextAlign = ContentAlignment.MiddleCenter 
            };

            // Campo Usuário
            Label lblUser = new Label() { Text = "Usuário:", Left = 50, Top = 100, ForeColor = Color.White, Font = new Font("Segoe UI", 10) };
            txtUsuario = new TextBox() { Left = 50, Top = 125, Width = 280, Font = new Font("Segoe UI", 12), BackColor = Color.WhiteSmoke };

            // Campo Senha
            Label lblPass = new Label() { Text = "Senha:", Left = 50, Top = 170, ForeColor = Color.White, Font = new Font("Segoe UI", 10) };
            txtSenha = new TextBox() { Left = 50, Top = 195, Width = 280, Font = new Font("Segoe UI", 12), PasswordChar = '●', BackColor = Color.WhiteSmoke };

            // Botão Logar
            btnLogar = new Button() { 
                Text = "ENTRAR", Left = 50, Top = 260, Width = 280, Height = 45, 
                BackColor = Color.MediumSpringGreen, FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 12, FontStyle.Bold), Cursor = Cursors.Hand
            };
            btnLogar.Click += BtnLogar_Click;

            // Botão Esqueci Senha
            btnEsqueci = new Button() { 
                Text = "Esqueci minha senha", Left = 50, Top = 320, Width = 280, Height = 35, 
                BackColor = Color.FromArgb(255, 128, 128), FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold), Cursor = Cursors.Hand
            };
            btnEsqueci.Click += (s, e) => { new RecoveryForm().ShowDialog(); };

            Button btnCadastrar = new Button() { 
            Text = "CADASTRAR NOVO USUÁRIO", Left = 50, Top = 370, Width = 280, Height = 30, 
            BackColor = Color.MediumSlateBlue, FlatStyle = FlatStyle.Flat,
            ForeColor = Color.White, Font = new Font("Segoe UI", 8, FontStyle.Bold)

            };
            btnCadastrar.Click += (s, e) => { new CadastroForm().ShowDialog(); };

            this.Controls.Add(btnCadastrar);
            this.Controls.Add(lblTitulo);
            this.Controls.Add(lblUser);
            this.Controls.Add(txtUsuario);
            this.Controls.Add(lblPass);
            this.Controls.Add(txtSenha);
            this.Controls.Add(btnLogar);
            this.Controls.Add(btnEsqueci);
        }

        private void BtnLogar_Click(object sender, EventArgs e)
        {
            using (var conn = Database.GetConnection())
            {
                conn.Open();
                var cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT COUNT(*) FROM Usuarios WHERE Login = $u AND Senha = $s";
                cmd.Parameters.AddWithValue("$u", txtUsuario.Text);
                cmd.Parameters.AddWithValue("$s", txtSenha.Text);

                if (Convert.ToInt32(cmd.ExecuteScalar()) > 0)
                {
                    this.Hide();
                    new WelcomeForm().Show();
                }
                else { MessageBox.Show("Credenciais Inválidas!"); }
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
            this.Text = "Recuperar Senha";
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

            // ADICIONANDO O BOTÃO
            btnRecuperar = new Button() { 
                Text = "RECUPERAR DADOS", Left = 40, Top = 140, Width = 300, Height = 50, 
                BackColor = Color.Gold, FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11, FontStyle.Bold), Cursor = Cursors.Hand 
            };
            btnRecuperar.Click += BtnRecuperar_Click;

            this.Controls.Add(lblInstrucao);
            this.Controls.Add(txtValidarUser);
            this.Controls.Add(btnRecuperar);
        }

        private void BtnRecuperar_Click(object sender, EventArgs e)
        {
            string loginInformado = txtValidarUser.Text;

            if (string.IsNullOrWhiteSpace(loginInformado))
            {
                MessageBox.Show("Por favor, digite o login.");
                return;
            }

            using (var conn = Database.GetConnection())
            {
                conn.Open();
                var cmd = conn.CreateCommand();
                // Busca a senha para o login digitado
                cmd.CommandText = "SELECT Login, Senha FROM Usuarios WHERE Login = $login LIMIT 1";
                cmd.Parameters.AddWithValue("$login", loginInformado);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string loginDb = reader.GetString(0);
                        string senhaDb = reader.GetString(1);

                        MessageBox.Show($"Dados encontrados!\n\nUsuário: {loginDb}\nSenha: {senhaDb}", 
                                        "Recuperação de Acesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Usuário não encontrado no banco de dados.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
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
            this.BackColor = Color.FromArgb(0, 122, 204); // Azul vibrante

            Label lblMsg = new Label() { 
                Text = "Bem-vindo!\nLogin realizado com sucesso.", 
                Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter, 
                ForeColor = Color.White, Font = new Font("Segoe UI", 18, FontStyle.Italic) 
            };

            this.Controls.Add(lblMsg);
            this.FormClosing += (s, e) => Application.Exit(); // Fecha tudo ao sair
        }
    }
}