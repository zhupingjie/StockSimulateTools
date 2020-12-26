namespace StockSimulateUI.UI
{
    partial class MessageForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MessageForm));
            this.gridMessageList = new System.Windows.Forms.DataGridView();
            this.panel7 = new System.Windows.Forms.Panel();
            this.btnAllHandle = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnHandled = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.gridMessageList)).BeginInit();
            this.panel7.SuspendLayout();
            this.SuspendLayout();
            // 
            // gridMessageList
            // 
            this.gridMessageList.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridMessageList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridMessageList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridMessageList.Location = new System.Drawing.Point(0, 0);
            this.gridMessageList.Name = "gridMessageList";
            this.gridMessageList.ReadOnly = true;
            this.gridMessageList.RowHeadersWidth = 10;
            this.gridMessageList.RowTemplate.Height = 23;
            this.gridMessageList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridMessageList.Size = new System.Drawing.Size(629, 421);
            this.gridMessageList.TabIndex = 50;
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.btnClose);
            this.panel7.Controls.Add(this.btnAllHandle);
            this.panel7.Controls.Add(this.btnDelete);
            this.panel7.Controls.Add(this.btnHandled);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel7.Location = new System.Drawing.Point(0, 421);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(629, 29);
            this.panel7.TabIndex = 49;
            // 
            // btnAllHandle
            // 
            this.btnAllHandle.BackColor = System.Drawing.Color.White;
            this.btnAllHandle.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnAllHandle.Location = new System.Drawing.Point(135, 3);
            this.btnAllHandle.Name = "btnAllHandle";
            this.btnAllHandle.Size = new System.Drawing.Size(70, 23);
            this.btnAllHandle.TabIndex = 52;
            this.btnAllHandle.Text = "全部处理";
            this.btnAllHandle.UseVisualStyleBackColor = false;
            this.btnAllHandle.Click += new System.EventHandler(this.btnAllHandle_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.BackColor = System.Drawing.Color.White;
            this.btnDelete.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnDelete.Location = new System.Drawing.Point(3, 3);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(63, 23);
            this.btnDelete.TabIndex = 51;
            this.btnDelete.Text = "删除";
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnHandled
            // 
            this.btnHandled.BackColor = System.Drawing.Color.White;
            this.btnHandled.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnHandled.Location = new System.Drawing.Point(65, 3);
            this.btnHandled.Name = "btnHandled";
            this.btnHandled.Size = new System.Drawing.Size(70, 23);
            this.btnHandled.TabIndex = 1;
            this.btnHandled.Text = "处理";
            this.btnHandled.UseVisualStyleBackColor = false;
            this.btnHandled.Click += new System.EventHandler(this.btnHandled_Click);
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.White;
            this.btnClose.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnClose.Location = new System.Drawing.Point(204, 3);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(70, 23);
            this.btnClose.TabIndex = 53;
            this.btnClose.Text = "关闭";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // MessageForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(629, 450);
            this.Controls.Add(this.gridMessageList);
            this.Controls.Add(this.panel7);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MessageForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "今日消息";
            this.Load += new System.EventHandler(this.MessageForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gridMessageList)).EndInit();
            this.panel7.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView gridMessageList;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnHandled;
        private System.Windows.Forms.Button btnAllHandle;
        private System.Windows.Forms.Button btnClose;
    }
}