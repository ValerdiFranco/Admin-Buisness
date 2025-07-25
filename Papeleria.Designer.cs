namespace Admin_Buisness
{
    partial class Papeleria
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.BtnVolver_Seleccion_Pp = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // BtnVolver_Seleccion_Pp
            // 
            this.BtnVolver_Seleccion_Pp.Location = new System.Drawing.Point(187, 245);
            this.BtnVolver_Seleccion_Pp.Name = "BtnVolver_Seleccion_Pp";
            this.BtnVolver_Seleccion_Pp.Size = new System.Drawing.Size(75, 23);
            this.BtnVolver_Seleccion_Pp.TabIndex = 0;
            this.BtnVolver_Seleccion_Pp.Text = "Volver";
            this.BtnVolver_Seleccion_Pp.UseVisualStyleBackColor = true;
            this.BtnVolver_Seleccion_Pp.Click += new System.EventHandler(this.BtnVolver_Seleccion_Pp_Click);
            // 
            // Papeleria
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(716, 415);
            this.Controls.Add(this.BtnVolver_Seleccion_Pp);
            this.Name = "Papeleria";
            this.Text = "Papeleria";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button BtnVolver_Seleccion_Pp;
    }
}