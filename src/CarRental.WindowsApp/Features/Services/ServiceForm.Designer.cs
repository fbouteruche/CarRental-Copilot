﻿namespace CarRental.WindowsApp.Features.Services
{
    partial class ServiceForm
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
            this.labelNome = new System.Windows.Forms.Label();
            this.labelValor = new System.Windows.Forms.Label();
            this.txtNome = new System.Windows.Forms.TextBox();
            this.txtValor = new System.Windows.Forms.TextBox();
            this.labelCalculoFixo = new System.Windows.Forms.Label();
            this.labelCalculoDiario = new System.Windows.Forms.Label();
            this.rdbTaxaFixa = new System.Windows.Forms.RadioButton();
            this.rdbCalcDiaria = new System.Windows.Forms.RadioButton();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.btnConfirma = new System.Windows.Forms.Button();
            this.txtId = new System.Windows.Forms.TextBox();
            this.labelId = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblTipoCalculo = new System.Windows.Forms.Label();
            this.lblCadastroServico = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelNome
            // 
            this.labelNome.AutoSize = true;
            this.labelNome.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.labelNome.ForeColor = System.Drawing.Color.Black;
            this.labelNome.Location = new System.Drawing.Point(30, 48);
            this.labelNome.Name = "labelNome";
            this.labelNome.Size = new System.Drawing.Size(35, 13);
            this.labelNome.TabIndex = 0;
            this.labelNome.Text = "Name";
            // 
            // labelValor
            // 
            this.labelValor.AutoSize = true;
            this.labelValor.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.labelValor.ForeColor = System.Drawing.Color.Black;
            this.labelValor.Location = new System.Drawing.Point(34, 74);
            this.labelValor.Name = "labelValor";
            this.labelValor.Size = new System.Drawing.Size(34, 13);
            this.labelValor.TabIndex = 1;
            this.labelValor.Text = "Value";
            // 
            // txtNome
            // 
            this.txtNome.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.txtNome.Location = new System.Drawing.Point(71, 45);
            this.txtNome.MaxLength = 200;
            this.txtNome.Name = "txtNome";
            this.txtNome.Size = new System.Drawing.Size(116, 20);
            this.txtNome.TabIndex = 2;
            // 
            // txtValor
            // 
            this.txtValor.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.txtValor.Location = new System.Drawing.Point(71, 71);
            this.txtValor.MaxLength = 200;
            this.txtValor.Name = "txtValor";
            this.txtValor.Size = new System.Drawing.Size(116, 20);
            this.txtValor.TabIndex = 3;
            this.txtValor.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtValor_KeyPress);
            // 
            // labelCalculoFixo
            // 
            this.labelCalculoFixo.AutoSize = true;
            this.labelCalculoFixo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.labelCalculoFixo.ForeColor = System.Drawing.Color.Black;
            this.labelCalculoFixo.Location = new System.Drawing.Point(91, 145);
            this.labelCalculoFixo.Name = "labelCalculoFixo";
            this.labelCalculoFixo.Size = new System.Drawing.Size(61, 13);
            this.labelCalculoFixo.TabIndex = 5;
            this.labelCalculoFixo.Text = "Fixed Rate";
            // 
            // labelCalculoDiario
            // 
            this.labelCalculoDiario.AutoSize = true;
            this.labelCalculoDiario.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.labelCalculoDiario.ForeColor = System.Drawing.Color.Black;
            this.labelCalculoDiario.Location = new System.Drawing.Point(91, 126);
            this.labelCalculoDiario.Name = "labelCalculoDiario";
            this.labelCalculoDiario.Size = new System.Drawing.Size(66, 13);
            this.labelCalculoDiario.TabIndex = 4;
            this.labelCalculoDiario.Text = "Daily Rate";
            // 
            // rdbTaxaFixa
            // 
            this.rdbTaxaFixa.AutoSize = true;
            this.rdbTaxaFixa.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.rdbTaxaFixa.Location = new System.Drawing.Point(71, 145);
            this.rdbTaxaFixa.Name = "rdbTaxaFixa";
            this.rdbTaxaFixa.Size = new System.Drawing.Size(14, 13);
            this.rdbTaxaFixa.TabIndex = 4;
            this.rdbTaxaFixa.TabStop = true;
            this.rdbTaxaFixa.UseVisualStyleBackColor = true;
            // 
            // rdbCalcDiaria
            // 
            this.rdbCalcDiaria.AutoSize = true;
            this.rdbCalcDiaria.Checked = true;
            this.rdbCalcDiaria.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.rdbCalcDiaria.Location = new System.Drawing.Point(71, 126);
            this.rdbCalcDiaria.Name = "rdbCalcDiaria";
            this.rdbCalcDiaria.Size = new System.Drawing.Size(14, 13);
            this.rdbCalcDiaria.TabIndex = 5;
            this.rdbCalcDiaria.TabStop = true;
            this.rdbCalcDiaria.UseVisualStyleBackColor = true;
            // 
            // btnCancelar
            // 
            this.btnCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancelar.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.btnCancelar.Location = new System.Drawing.Point(152, 225);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(75, 23);
            this.btnCancelar.TabIndex = 7;
            this.btnCancelar.Text = "Cancel";
            this.btnCancelar.UseVisualStyleBackColor = true;
            // 
            // btnConfirma
            // 
            this.btnConfirma.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnConfirma.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.btnConfirma.Location = new System.Drawing.Point(71, 225);
            this.btnConfirma.Name = "btnConfirma";
            this.btnConfirma.Size = new System.Drawing.Size(75, 23);
            this.btnConfirma.TabIndex = 6;
            this.btnConfirma.Text = "Confirm";
            this.btnConfirma.UseVisualStyleBackColor = true;
            this.btnConfirma.Click += new System.EventHandler(this.btnConfirma_Click);
            // 
            // txtId
            // 
            this.txtId.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.txtId.Enabled = false;
            this.txtId.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.txtId.Location = new System.Drawing.Point(71, 19);
            this.txtId.MaxLength = 200;
            this.txtId.Name = "txtId";
            this.txtId.Size = new System.Drawing.Size(116, 20);
            this.txtId.TabIndex = 1;
            this.txtId.Text = "0";
            // 
            // labelId
            // 
            this.labelId.AutoSize = true;
            this.labelId.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.labelId.ForeColor = System.Drawing.Color.Black;
            this.labelId.Location = new System.Drawing.Point(49, 22);
            this.labelId.Name = "labelId";
            this.labelId.Size = new System.Drawing.Size(16, 13);
            this.labelId.TabIndex = 7;
            this.labelId.Text = "Id";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblTipoCalculo);
            this.groupBox1.Controls.Add(this.labelId);
            this.groupBox1.Controls.Add(this.txtId);
            this.groupBox1.Controls.Add(this.rdbTaxaFixa);
            this.groupBox1.Controls.Add(this.labelCalculoFixo);
            this.groupBox1.Controls.Add(this.rdbCalcDiaria);
            this.groupBox1.Controls.Add(this.txtNome);
            this.groupBox1.Controls.Add(this.labelCalculoDiario);
            this.groupBox1.Controls.Add(this.txtValor);
            this.groupBox1.Controls.Add(this.labelNome);
            this.groupBox1.Controls.Add(this.labelValor);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.groupBox1.Location = new System.Drawing.Point(12, 46);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(217, 173);
            this.groupBox1.TabIndex = 52;
            this.groupBox1.TabStop = false;
            // 
            // lblTipoCalculo
            // 
            this.lblTipoCalculo.AutoSize = true;
            this.lblTipoCalculo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTipoCalculo.ForeColor = System.Drawing.Color.Black;
            this.lblTipoCalculo.Location = new System.Drawing.Point(18, 107);
            this.lblTipoCalculo.Name = "lblTipoCalculo";
            this.lblTipoCalculo.Size = new System.Drawing.Size(92, 13);
            this.lblTipoCalculo.TabIndex = 53;
            this.lblTipoCalculo.Text = "Rate Type:";
            // 
            // lblCadastroServico
            // 
            this.lblCadastroServico.AutoSize = true;
            this.lblCadastroServico.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCadastroServico.ForeColor = System.Drawing.Color.Black;
            this.lblCadastroServico.Location = new System.Drawing.Point(30, 19);
            this.lblCadastroServico.Name = "lblCadastroServico";
            this.lblCadastroServico.Size = new System.Drawing.Size(158, 20);
            this.lblCadastroServico.TabIndex = 53;
            this.lblCadastroServico.Text = "Service Registration";
            // 
            // TelaServicoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(239, 261);
            this.Controls.Add(this.lblCadastroServico);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.btnConfirma);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TelaServicoForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Car Rental";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ServiceForm_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelNome;
        private System.Windows.Forms.Label labelValor;
        private System.Windows.Forms.TextBox txtNome;
        private System.Windows.Forms.TextBox txtValor;
        private System.Windows.Forms.Label labelCalculoFixo;
        private System.Windows.Forms.Label labelCalculoDiario;
        private System.Windows.Forms.RadioButton rdbTaxaFixa;
        private System.Windows.Forms.RadioButton rdbCalcDiaria;
        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.Button btnConfirma;
        private System.Windows.Forms.TextBox txtId;
        private System.Windows.Forms.Label labelId;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblTipoCalculo;
        private System.Windows.Forms.Label lblCadastroServico;
    }
}