namespace Agente_AI
{
    partial class FrmAgenteIA
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            pnlConfiguración = new Panel();
            btnConectar = new Button();
            txtApikey = new TextBox();
            lblApiKey = new Label();
            cmbModelo = new ComboBox();
            lblModelo = new Label();
            lblConfiguracion = new Label();
            panel2 = new Panel();
            pnlChat = new Panel();
            btnEnviar = new Button();
            txtMensaje = new TextBox();
            rtbChat = new RichTextBox();
            lblChat = new Label();
            pnlConfiguración.SuspendLayout();
            panel2.SuspendLayout();
            pnlChat.SuspendLayout();
            SuspendLayout();
            // 
            // pnlConfiguración
            // 
            pnlConfiguración.BackColor = SystemColors.ControlLightLight;
            pnlConfiguración.BorderStyle = BorderStyle.Fixed3D;
            pnlConfiguración.Controls.Add(btnConectar);
            pnlConfiguración.Controls.Add(txtApikey);
            pnlConfiguración.Controls.Add(lblApiKey);
            pnlConfiguración.Controls.Add(cmbModelo);
            pnlConfiguración.Controls.Add(lblModelo);
            pnlConfiguración.Controls.Add(lblConfiguracion);
            pnlConfiguración.Dock = DockStyle.Left;
            pnlConfiguración.Location = new Point(0, 0);
            pnlConfiguración.Margin = new Padding(2);
            pnlConfiguración.Name = "pnlConfiguración";
            pnlConfiguración.Size = new Size(325, 774);
            pnlConfiguración.TabIndex = 0;
            // 
            // btnConectar
            // 
            btnConectar.Location = new Point(18, 299);
            btnConectar.Margin = new Padding(2);
            btnConectar.Name = "btnConectar";
            btnConectar.Size = new Size(288, 40);
            btnConectar.TabIndex = 0;
            btnConectar.Text = "Conectar";
            btnConectar.UseVisualStyleBackColor = true;
            // 
            // txtApikey
            // 
            txtApikey.Location = new Point(18, 218);
            txtApikey.Margin = new Padding(2);
            txtApikey.Name = "txtApikey";
            txtApikey.Size = new Size(289, 27);
            txtApikey.TabIndex = 0;
            txtApikey.TextChanged += txtApikey_TextChanged;
            // 
            // lblApiKey
            // 
            lblApiKey.AutoSize = true;
            lblApiKey.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblApiKey.Location = new Point(18, 194);
            lblApiKey.Margin = new Padding(2, 0, 2, 0);
            lblApiKey.Name = "lblApiKey";
            lblApiKey.Size = new Size(97, 28);
            lblApiKey.TabIndex = 0;
            lblApiKey.Text = "Clave API";
            // 
            // cmbModelo
            // 
            cmbModelo.FormattingEnabled = true;
            cmbModelo.Items.AddRange(new object[] { "Gemini 2.5 Flash", "Chat GPT" });
            cmbModelo.Location = new Point(18, 122);
            cmbModelo.Margin = new Padding(2);
            cmbModelo.Name = "cmbModelo";
            cmbModelo.Size = new Size(289, 28);
            cmbModelo.TabIndex = 0;
            cmbModelo.Text = "Gemini 2.5 Flash";
            cmbModelo.SelectedIndexChanged += cmbModelo_SelectedIndexChanged;
            // 
            // lblModelo
            // 
            lblModelo.AutoSize = true;
            lblModelo.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblModelo.Location = new Point(18, 98);
            lblModelo.Margin = new Padding(2, 0, 2, 0);
            lblModelo.Name = "lblModelo";
            lblModelo.Size = new Size(136, 28);
            lblModelo.TabIndex = 0;
            lblModelo.Text = "Modelo de AI";
            // 
            // lblConfiguracion
            // 
            lblConfiguracion.AutoSize = true;
            lblConfiguracion.Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblConfiguracion.Location = new Point(22, 24);
            lblConfiguracion.Margin = new Padding(2, 0, 2, 0);
            lblConfiguracion.Name = "lblConfiguracion";
            lblConfiguracion.Size = new Size(190, 37);
            lblConfiguracion.TabIndex = 0;
            lblConfiguracion.Text = "Configuración";
            // 
            // panel2
            // 
            panel2.BackColor = SystemColors.ControlLight;
            panel2.Controls.Add(pnlChat);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(325, 0);
            panel2.Margin = new Padding(2);
            panel2.Name = "panel2";
            panel2.Size = new Size(886, 774);
            panel2.TabIndex = 1;
            // 
            // pnlChat
            // 
            pnlChat.BackColor = SystemColors.ButtonHighlight;
            pnlChat.Controls.Add(btnEnviar);
            pnlChat.Controls.Add(txtMensaje);
            pnlChat.Controls.Add(rtbChat);
            pnlChat.Controls.Add(lblChat);
            pnlChat.Dock = DockStyle.Fill;
            pnlChat.Location = new Point(0, 0);
            pnlChat.Margin = new Padding(2);
            pnlChat.Name = "pnlChat";
            pnlChat.Size = new Size(886, 774);
            pnlChat.TabIndex = 0;
            // 
            // btnEnviar
            // 
            btnEnviar.Location = new Point(712, 688);
            btnEnviar.Margin = new Padding(2);
            btnEnviar.Name = "btnEnviar";
            btnEnviar.Size = new Size(142, 44);
            btnEnviar.TabIndex = 3;
            btnEnviar.Text = "Enviar";
            btnEnviar.UseVisualStyleBackColor = true;
            btnEnviar.Click += btnEnviar_Click;
            // 
            // txtMensaje
            // 
            txtMensaje.Location = new Point(16, 688);
            txtMensaje.Margin = new Padding(2);
            txtMensaje.Multiline = true;
            txtMensaje.Name = "txtMensaje";
            txtMensaje.Size = new Size(681, 45);
            txtMensaje.TabIndex = 2;
            txtMensaje.Text = "Escribr un Mensaje....";
            txtMensaje.TextChanged += txtMensaje_TextChanged;
            // 
            // rtbChat
            // 
            rtbChat.Location = new Point(16, 64);
            rtbChat.Margin = new Padding(2);
            rtbChat.Name = "rtbChat";
            rtbChat.ReadOnly = true;
            rtbChat.Size = new Size(849, 609);
            rtbChat.TabIndex = 1;
            rtbChat.Text = "";
            // 
            // lblChat
            // 
            lblChat.AutoSize = true;
            lblChat.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblChat.Location = new Point(24, 24);
            lblChat.Margin = new Padding(2, 0, 2, 0);
            lblChat.Name = "lblChat";
            lblChat.Size = new Size(83, 41);
            lblChat.TabIndex = 0;
            lblChat.Text = "Chat";
            // 
            // FrmAgenteIA
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1211, 774);
            Controls.Add(panel2);
            Controls.Add(pnlConfiguración);
            Margin = new Padding(2);
            MinimumSize = new Size(964, 569);
            Name = "FrmAgenteIA";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "AGENTE DE AI";
            pnlConfiguración.ResumeLayout(false);
            pnlConfiguración.PerformLayout();
            panel2.ResumeLayout(false);
            pnlChat.ResumeLayout(false);
            pnlChat.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlConfiguración;
        private Panel panel2;
        private Label lblConfiguracion;
        private Label lblModelo;
        private ComboBox cmbModelo;
        private Label lblApiKey;
        private TextBox txtApikey;
        private Button btnConectar;
        private Panel pnlChat;
        private RichTextBox rtbChat;
        private Label lblChat;
        private TextBox txtMensaje;
        private Button btnEnviar;
    }
}
