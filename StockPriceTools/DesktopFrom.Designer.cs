namespace StockPriceTools
{
    partial class DesktopFrom
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
            this.btnValuatie = new System.Windows.Forms.Button();
            this.btnCalcuate = new System.Windows.Forms.Button();
            this.btnInit = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnValuatie
            // 
            this.btnValuatie.Location = new System.Drawing.Point(165, 12);
            this.btnValuatie.Name = "btnValuatie";
            this.btnValuatie.Size = new System.Drawing.Size(154, 150);
            this.btnValuatie.TabIndex = 34;
            this.btnValuatie.Text = "模拟估值";
            this.btnValuatie.UseVisualStyleBackColor = true;
            this.btnValuatie.Click += new System.EventHandler(this.btnValuatie_Click);
            // 
            // btnCalcuate
            // 
            this.btnCalcuate.Location = new System.Drawing.Point(12, 12);
            this.btnCalcuate.Name = "btnCalcuate";
            this.btnCalcuate.Size = new System.Drawing.Size(147, 150);
            this.btnCalcuate.TabIndex = 35;
            this.btnCalcuate.Text = "模拟计算";
            this.btnCalcuate.UseVisualStyleBackColor = true;
            this.btnCalcuate.Click += new System.EventHandler(this.btnCalcuate_Click);
            // 
            // btnInit
            // 
            this.btnInit.Location = new System.Drawing.Point(325, 12);
            this.btnInit.Name = "btnInit";
            this.btnInit.Size = new System.Drawing.Size(154, 150);
            this.btnInit.TabIndex = 36;
            this.btnInit.Text = "初始化";
            this.btnInit.UseVisualStyleBackColor = true;
            this.btnInit.Click += new System.EventHandler(this.btnInit_Click);
            // 
            // DesktopFrom
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(504, 174);
            this.Controls.Add(this.btnInit);
            this.Controls.Add(this.btnCalcuate);
            this.Controls.Add(this.btnValuatie);
            this.Name = "DesktopFrom";
            this.Text = "DesktopFrom";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnValuatie;
        private System.Windows.Forms.Button btnCalcuate;
        private System.Windows.Forms.Button btnInit;
    }
}