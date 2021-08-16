using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace Animacao_3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static Storyboard StoryB = new Storyboard();
        static int estagio = 0;
        static int tentativa = 0;

        public MainWindow()
        {
            InitializeComponent();
            code.Content = code.Content + Criptografia.Encrypted;
        }
        
        private void estagios_ligar(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if ( estagio == 0 )
            {
                StoryB = (Storyboard)TryFindResource("Estagio_I");
                StoryB.Begin();
                estagio = 1;

                // aperte os sintos
                StoryB = (Storyboard)TryFindResource("AperteoCinto");
                StoryB.RepeatBehavior = RepeatBehavior.Forever;
                StoryB.Begin();

                StoryB = (Storyboard)TryFindResource("ligar");
                StoryB.Begin();

                // libera para digitar a senha
                pass.IsEnabled = true;
                pass.Focus();
            }
            else if (estagio == 1)
            {
                // partida do carro
                StoryB = (Storyboard)TryFindResource("Estagio_II");
                StoryB.Begin();
                estagio = 2;

                Partida();
            } else
            {
                // desligar tudo
                Desligar();
                StoryB = (Storyboard)TryFindResource("AperteoCinto");
                StoryB.Stop();
                StoryB = (Storyboard)TryFindResource("ligar");
                StoryB.Stop();
                // desabilita para digitar a senha
                pass.IsEnabled = false;
            }
        }

        private void bntLigar(object sender, MouseButtonEventArgs e)
        { // metodo para ligar ou desligar o carro sem desligar o painel e desativar tudo
            if (estagio == 0) {
                estagios_ligar(null, null);
            } else if(estagio == 2)
            {
                estagio = 1;
                MsgCarOn(false, false);

                StoryB = (Storyboard)TryFindResource("Estagio_I");
                StoryB.Begin();
                StoryB = (Storyboard)TryFindResource("Desacelerar");
                StoryB.Begin();

                // libera para digitar a senha
                pass.IsEnabled = true;
                pass.Focus();
            } else if (estagio == 1)
            {
                estagios_ligar(null, null);
            }
        }

        private void bntDesligar(object sender, MouseButtonEventArgs e)
        { // metodo para desligar o carro independente do estagio da chave que estiver
            if (tentativa >= 3)
            {
                tentativa = 0;
            }
            estagio = 2;
            estagios_ligar(null, null);
            
        }

        private void check(object sender, KeyEventArgs e)
        {   // evento ao clicar em enter faz a verificação e da partida
            if( e.Key == Key.Enter)
            {
                Partida();
                estagios_ligar(null, null);
            }
        }

        private void Partida()
        {
            // da partida no carro caso a senha esteje correta
            if (Criptografia.DesCrypeted(pass.Text))
            {
                StoryB = (Storyboard)TryFindResource("partida");
                StoryB.Begin();

                MsgCarOn(true, true);

                pass.IsEnabled = false;
            } else
            {
                MsgCarOn(true, false);

                StoryB = (Storyboard)TryFindResource("Estagio_I");
                StoryB.Begin();
                estagio = 1;
            }
        }

        private void Desligar()
        {
            // desliga o carro 
            // esconde a mensagem de que o carro está ligado
            // para a aceleração
            MsgCarOn(false, false);

            StoryB = (Storyboard)TryFindResource("DesligarMotor");
            StoryB.Begin();

            StoryB = (Storyboard)TryFindResource("Estagio_0");
            StoryB.Begin();
            estagio = 0;

            pass.Text = "";
        }

        private void MsgCarOn(bool on, bool senha)
        {
            // mostra a mensagem se o carro estiver ligado ou desligado.
            if (on)
            {
                if (senha)
                {
                    StoryB = (Storyboard)TryFindResource("Ligado");
                    ligado.Content = "A senha está correta e o carro foi ligado!";
                    tentativa = 0;
                } else
                {
                    StoryB = (Storyboard)TryFindResource("Erro");
                    ligado.Content = "Senha Incorreta!";
                    tentativa++;
                }

                if (tentativa >= 3)
                {
                    StoryB = (Storyboard)TryFindResource("Alarme");
                    ligado.Content = "O alarme foi ligado!";
                    pass.IsEnabled = false;
                }

                ligado.Visibility = Visibility;

                StoryB.RepeatBehavior = RepeatBehavior.Forever;
                StoryB.Begin();
            } else
            {
                StoryB.Stop();
                ligado.Visibility = Visibility.Hidden;
            }
        }
        

        private void OpemAbout(object sender, RoutedEventArgs e)
        {   // Abre a janela sobre o programa
            About sobre = new About();
            sobre.Show();
        }

        private void AbrirAjuda(object sender, RoutedEventArgs e)
        {   // abre a janela ajuda
            Ajuda ajuda = new Ajuda();
            ajuda.Show();
        }
    }
}
