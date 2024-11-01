using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace SecurityPractice4
{
    public partial class MainForm : Form
    {
        private SymmetricAlgorithm cryptoProvider;

        public MainForm()
        {
            InitializeComponent();
            comboBoxAlgorithm.SelectedIndex = 0; // За замовчуванням DES
        }

        private void buttonEncrypt_Click(object sender, EventArgs e)
        {
            SetCryptoProvider();
            string textToEncrypt = textBoxInput.Text;
            byte[] encryptedData = EncryptData(textToEncrypt);
            textBoxOutput.Text = BitConverter.ToString(encryptedData);
        }

        private void buttonDecrypt_Click(object sender, EventArgs e)
        {
            SetCryptoProvider();
            byte[] dataToDecrypt = ParseHex(textBoxInput.Text);
            string decryptedData = DecryptData(dataToDecrypt);
            textBoxOutput.Text = decryptedData;
        }

        private void SetCryptoProvider()
        {
            switch (comboBoxAlgorithm.SelectedItem.ToString())
            {
                case "DES":
                    cryptoProvider = new DESCryptoServiceProvider();
                    cryptoProvider.Key = Encoding.ASCII.GetBytes("ABCDEFGH");
                    cryptoProvider.IV = Encoding.ASCII.GetBytes("ABCDEFGH");
                    break;
                case "TripleDES":
                    cryptoProvider = new TripleDESCryptoServiceProvider();
                    cryptoProvider.Key = Encoding.ASCII.GetBytes("ABCDEFGHIJKLMNOPQRSTUVWX"); // 24-байтовий ключ
                    cryptoProvider.IV = Encoding.ASCII.GetBytes("12345678");
                    break;
                case "AES":
                    cryptoProvider = new AesCryptoServiceProvider();
                    cryptoProvider.Key = Encoding.ASCII.GetBytes("ABCDEFGHIJKLMNOP");
                    cryptoProvider.IV = Encoding.ASCII.GetBytes("12345678ABCDEFGH");
                    break;
            }
            cryptoProvider.Mode = CipherMode.CBC;
        }

        private byte[] EncryptData(string text)
        {
            byte[] data = Encoding.UTF8.GetBytes(text);
            using (MemoryStream ms = new MemoryStream())
            using (CryptoStream cs = new CryptoStream(ms, cryptoProvider.CreateEncryptor(), CryptoStreamMode.Write))
            {
                cs.Write(data, 0, data.Length);
                cs.Close();
                return ms.ToArray();
            }
        }

        private string DecryptData(byte[] data)
        {
            using (MemoryStream ms = new MemoryStream(data))
            using (CryptoStream cs = new CryptoStream(ms, cryptoProvider.CreateDecryptor(), CryptoStreamMode.Read))
            using (StreamReader reader = new StreamReader(cs))
            {
                return reader.ReadToEnd();
            }
        }

        private byte[] ParseHex(string hex)
        {
            string[] hexValues = hex.Split('-');
            byte[] data = new byte[hexValues.Length];
            for (int i = 0; i < hexValues.Length; i++)
            {
                data[i] = Convert.ToByte(hexValues[i], 16);
            }
            return data;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void MainForm_Load_1(object sender, EventArgs e)
        {

        }
    }
}
