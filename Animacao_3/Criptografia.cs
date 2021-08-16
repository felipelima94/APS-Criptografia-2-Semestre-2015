using System;
using System.Text;
using System.Security.Cryptography;

namespace Animacao_3
{
    class Criptografia
    {
        /*
         * Variaveis privadas para serem utilizada para gerar a chave de criptografia
         */
        private static DateTime Data = DateTime.Now; // chama a class DateTime (Data Atual)
        private static string day = twoNum(Data.Day); // pega o dia
        private static string month = twoNum(Data.Month); // pega o mes
        private static string year = Convert.ToString(Data.Year); // pega o ano

        private static string secretKey = day + month + year + "60868"; // cria a chave da criptografia

        private static string twoNum(int num)
        {
            //metodo para verificar se são dois numeros
            string newNum = Convert.ToString(num);
            if (num <= 9)
            {
                newNum = "0" + num;
            }
            return newNum;
        }
        
        private static string Encrypt(string toEncrypt)
        {
            /*
             * Metodo para criptografar as informação
             * informação sendo a senha (Data atual + ultimos digitos do RA)
            */
            byte[] keyArray;
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);

            string key = "60868"; // chave para criptografar a informação
            
            //metodo de criptografia em 
            // ele criptografa a chave que será usado na criptografia
            MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
            keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
            // Sempre libera recursos e libera dados do serviço da criptografia fornecido.

            hashmd5.Clear();

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            // define uma chave(key) secreta para o TripleDES
            tdes.Key = keyArray; // define a hash, a chave em hexadecimal que será usada para a criptografia
            
            // nos escolhemes o EBC(Eletronic code book)
            tdes.Mode = CipherMode.ECB; // modo operação simetrica que será usada para a criptografia

            // modo de preenchimento(se qualquer byte extra precisa ser adicionado)
            tdes.Padding = PaddingMode.PKCS7;

            /* -------------------------------------------------------------------------------- */

            // cria a criptografia baseada na TDES Triple Data Encryption Standard (Padrão de criptografia de dados triplo);
            ICryptoTransform cTransform = tdes.CreateEncryptor();

            // trsnaforma as regioes especifiadas em bytes para array e guarda na variavel "resultArray"
            byte[] resultArray =
                // transforma a informação em bytes
                cTransform.TransformFinalBlock(
                    toEncryptArray, // informação a ser criptografada
                    0, // inicio da contagem de bytes
                    toEncryptArray.Length // limite da contagem
                );

            // Liberar recursos mantidos por tripleDES criptografador
            tdes.Clear();

            //return the encrypted data into unreadable string format
            // retorna o informação em formato string que não pode ser lido
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        private static string Decrypt(string cipherString)
        {
            /*
             * Metodo para descriptografar
             */
            byte[] keyArray;
            byte[] toEncryptArray = Convert.FromBase64String(cipherString);

            string key = "60868"; // chave para criptografar a informação
            
            //metodo de criptografia em 
            // ele criptografa a chave que será usado na criptografia
            MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
            keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
            // Sempre libera recursos e libera dados do serviço da criptografia fornecido.

            hashmd5.Clear();
            
            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            // define uma chave(key) secreta para o TripleDES
            tdes.Key = keyArray; // define a hash, a chave em hexadecimal que será usada para a criptografia
            
            // nos escolhemes o EBC(Eletronic code book)
            tdes.Mode = CipherMode.ECB; // modo operação simetrica que será usada para a criptografia

            // modo de preenchimento(se qualquer byte extra precisa ser adicionado)
            tdes.Padding = PaddingMode.PKCS7;

            /* -------------------------------------------------------------------------------- */

            // cria a criptografia baseada na TDES Triple Data Encryption Standard (Padrão de criptografia de dados triplo);
            ICryptoTransform cTransform = tdes.CreateDecryptor();

            // trsnaforma as regioes especifiadas em bytes para array e guarda na variavel "resultArray"
            byte[] resultArray =
                // transforma a informação em bytes
                cTransform.TransformFinalBlock(
                    toEncryptArray, // informação a ser criptografada
                    0, // inicio da contagem de bytes
                    toEncryptArray.Length // limite da contagem
                );

            // Liberar recursos mantidos por tripleDES criptografador
            tdes.Clear();

            //return the Clear decrypted TEXT
            return UTF8Encoding.UTF8.GetString(resultArray);
        }


        // variaveis publicas para serem usadas para comparação
        public static string Encrypted = Encrypt(secretKey);
        // metodo para verificação da key digitada
        public static bool DesCrypeted (string senhaInfo)
        {
            if (senhaInfo == Decrypt(Encrypted))
            {
                return true;
            } else
            {
                return false;
            }
        }
    }
}
