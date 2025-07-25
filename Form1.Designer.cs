namespace Admin_Buisness
{
    partial class Pantalla_Principal
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Pantalla_Principal));
            this.BtnMenu_seleccion = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // BtnMenu_seleccion
            // 
            this.BtnMenu_seleccion.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(56)))), ((int)(((byte)(202)))));
            this.BtnMenu_seleccion.Font = new System.Drawing.Font("MS Reference Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnMenu_seleccion.Location = new System.Drawing.Point(881, 871);
            this.BtnMenu_seleccion.Margin = new System.Windows.Forms.Padding(4);
            this.BtnMenu_seleccion.Name = "BtnMenu_seleccion";
            this.BtnMenu_seleccion.Size = new System.Drawing.Size(280, 128);
            this.BtnMenu_seleccion.TabIndex = 3;
            this.BtnMenu_seleccion.Text = "Acceder";
            this.BtnMenu_seleccion.UseVisualStyleBackColor = false;
            this.BtnMenu_seleccion.Click += new System.EventHandler(this.button1_Click);
            // 
            // Pantalla_Principal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1924, 1055);
            this.Controls.Add(this.BtnMenu_seleccion);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Pantalla_Principal";
            this.Text = "Pnatalla principal";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Pantalla_Principal_Load);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button BtnMenu_seleccion;
    }
}

