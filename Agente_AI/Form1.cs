namespace Agente_AI
{
    public partial class FrmAgenteIA : Form
    {
        public FrmAgenteIA()
        {
            InitializeComponent();
        }

        private void txtApikey_TextChanged(object sender, EventArgs e)
        {
            txtApikey.PasswordChar = '*';//Lo que hace es para que le clave salga con asterisco 
        }

        private void txtMensaje_TextChanged(object sender, EventArgs e)
        {
            txtMensaje.Multiline = true;//Hace que en el textbox se pueda escribir en varias lineas
        }

        private void btnEnviar_Click(object sender, EventArgs e)
        {
        }

        private void cmbModelo_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}


//REFERENCIAS
/*
  private void btnAceptar_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Soy un hackers");
        }

        private void btnRes_Click(object sender, EventArgs e)
        {
            string val1 = txtVal1.Text;
            string val2 = txtVal2.Text;
            if(rdbSum.Checked == true)
            {
                int oper = int.Parse(val1) + int.Parse(val2);
                string respuesta = $"El resultado de {val1} + {val2} es: " + oper;
                MessageBox.Show(respuesta);
                txtRes.Enabled = true;
                txtRes.Text = respuesta;
            }

            if(rdbRes.Checked == true)
            {
                int oper = int.Parse(val1) - int.Parse(val2);
                string respuesta = $"El resultado de {val1} - {val2} es: " + oper;
                MessageBox.Show(respuesta);
                txtRes.Enabled = true;
                txtRes.Text = respuesta;
            }

            if(rdbMult.Checked == true)
            {
                int oper = int.Parse(val1) * int.Parse(val2);
                string respuesta = $"El resultado de {val1} * {val2} es: " + oper;
                MessageBox.Show(respuesta);
                txtRes.Enabled = true;
                txtRes.Text = respuesta;
            }

        }

        private void txtVal1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(txtVal1.Text.Length > 0)
            {
                txtVal2.Enabled = true;
            }
            else
            {
                txtVal2.Enabled = false;    
            }
        }
 
 */
