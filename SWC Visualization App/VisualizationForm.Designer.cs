using System;

namespace SWC_Visualization_App
{
    partial class VisualizationForm
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
            this.lblAuthCode = new System.Windows.Forms.Label();
            this.btnGetAuth = new System.Windows.Forms.Button();
            this.btnHelloWorld = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblAuthCode
            // 
            this.lblAuthCode.AutoSize = true;
            this.lblAuthCode.Location = new System.Drawing.Point(12, 9);
            this.lblAuthCode.Name = "lblAuthCode";
            this.lblAuthCode.Size = new System.Drawing.Size(103, 13);
            this.lblAuthCode.TabIndex = 0;
            this.lblAuthCode.Text = "Authentication Code";
            // 
            // btnGetAuth
            // 
            this.btnGetAuth.Location = new System.Drawing.Point(15, 34);
            this.btnGetAuth.Name = "btnGetAuth";
            this.btnGetAuth.Size = new System.Drawing.Size(75, 23);
            this.btnGetAuth.TabIndex = 1;
            this.btnGetAuth.Text = "Get Auth";
            this.btnGetAuth.UseVisualStyleBackColor = true;
            // 
            // btnHelloWorld
            // 
            this.btnHelloWorld.Location = new System.Drawing.Point(15, 64);
            this.btnHelloWorld.Name = "btnHelloWorld";
            this.btnHelloWorld.Size = new System.Drawing.Size(75, 23);
            this.btnHelloWorld.TabIndex = 2;
            this.btnHelloWorld.Text = "Hello World";
            this.btnHelloWorld.UseVisualStyleBackColor = true;
            this.btnHelloWorld.Click += new System.EventHandler(this.Button_HelloWorld_OnButtonClick);
            // 
            // VisualizationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnHelloWorld);
            this.Controls.Add(this.btnGetAuth);
            this.Controls.Add(this.lblAuthCode);
            this.Name = "VisualizationForm";
            this.Text = "Star Wars Visualizer";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblAuthCode;
        private System.Windows.Forms.Button btnGetAuth;
        private System.Windows.Forms.Button btnHelloWorld;
    }
}

