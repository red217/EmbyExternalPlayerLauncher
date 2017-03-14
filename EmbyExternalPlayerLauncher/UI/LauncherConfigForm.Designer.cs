namespace EmbyExternalPlayerLauncher.UI
{
    partial class LauncherConfigForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LauncherConfigForm));
            this.openFileDialogMpcHc = new System.Windows.Forms.OpenFileDialog();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxPath = new System.Windows.Forms.TextBox();
            this.buttonBrowse = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxArgs = new System.Windows.Forms.TextBox();
            this.buttonDefaultArgs = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxPort = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxTimeout = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxUser = new System.Windows.Forms.TextBox();
            this.textBoxPassword = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textBoxAddress = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // openFileDialogMpcHc
            // 
            this.openFileDialogMpcHc.FileName = "mpc-hc.exe";
            this.openFileDialogMpcHc.Filter = "Executables|*.exe|All files|*.*";
            this.openFileDialogMpcHc.Title = "Select the MPC-HC executable";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(186, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Path to MPC-HC executable:";
            // 
            // textBoxPath
            // 
            this.textBoxPath.Location = new System.Drawing.Point(16, 42);
            this.textBoxPath.Name = "textBoxPath";
            this.textBoxPath.Size = new System.Drawing.Size(415, 22);
            this.textBoxPath.TabIndex = 1;
            // 
            // buttonBrowse
            // 
            this.buttonBrowse.Location = new System.Drawing.Point(327, 6);
            this.buttonBrowse.Name = "buttonBrowse";
            this.buttonBrowse.Size = new System.Drawing.Size(104, 30);
            this.buttonBrowse.TabIndex = 2;
            this.buttonBrowse.Text = "Browse";
            this.buttonBrowse.UseVisualStyleBackColor = true;
            this.buttonBrowse.Click += new System.EventHandler(this.buttonBrowse_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 77);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(227, 17);
            this.label2.TabIndex = 3;
            this.label2.Text = "MPC-HC command line arguments:";
            // 
            // textBoxArgs
            // 
            this.textBoxArgs.Location = new System.Drawing.Point(16, 106);
            this.textBoxArgs.Name = "textBoxArgs";
            this.textBoxArgs.Size = new System.Drawing.Size(415, 22);
            this.textBoxArgs.TabIndex = 4;
            this.textBoxArgs.Text = "/play /fullscreen /start {s} /close";
            // 
            // buttonDefaultArgs
            // 
            this.buttonDefaultArgs.Location = new System.Drawing.Point(327, 70);
            this.buttonDefaultArgs.Name = "buttonDefaultArgs";
            this.buttonDefaultArgs.Size = new System.Drawing.Size(104, 30);
            this.buttonDefaultArgs.TabIndex = 5;
            this.buttonDefaultArgs.Text = "Default";
            this.buttonDefaultArgs.UseVisualStyleBackColor = true;
            this.buttonDefaultArgs.Click += new System.EventHandler(this.buttonDefaultArgs_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 138);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(167, 17);
            this.label3.TabIndex = 6;
            this.label3.Text = "MPC-HC web server port:";
            // 
            // textBoxPort
            // 
            this.textBoxPort.Location = new System.Drawing.Point(327, 135);
            this.textBoxPort.Name = "textBoxPort";
            this.textBoxPort.Size = new System.Drawing.Size(104, 22);
            this.textBoxPort.TabIndex = 7;
            this.textBoxPort.Text = "13579";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 166);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(196, 17);
            this.label4.TabIndex = 8;
            this.label4.Text = "MPC-HC web request timeout:";
            // 
            // textBoxTimeout
            // 
            this.textBoxTimeout.Location = new System.Drawing.Point(327, 163);
            this.textBoxTimeout.Name = "textBoxTimeout";
            this.textBoxTimeout.Size = new System.Drawing.Size(104, 22);
            this.textBoxTimeout.TabIndex = 9;
            this.textBoxTimeout.Text = "2000";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 194);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(118, 17);
            this.label5.TabIndex = 10;
            this.label5.Text = "Emby user name:";
            // 
            // textBoxUser
            // 
            this.textBoxUser.Location = new System.Drawing.Point(221, 191);
            this.textBoxUser.Name = "textBoxUser";
            this.textBoxUser.Size = new System.Drawing.Size(210, 22);
            this.textBoxUser.TabIndex = 11;
            // 
            // textBoxPassword
            // 
            this.textBoxPassword.Location = new System.Drawing.Point(221, 219);
            this.textBoxPassword.Name = "textBoxPassword";
            this.textBoxPassword.PasswordChar = '*';
            this.textBoxPassword.Size = new System.Drawing.Size(210, 22);
            this.textBoxPassword.TabIndex = 13;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(13, 222);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(111, 17);
            this.label6.TabIndex = 12;
            this.label6.Text = "Emby password:";
            // 
            // textBoxAddress
            // 
            this.textBoxAddress.Location = new System.Drawing.Point(16, 270);
            this.textBoxAddress.Name = "textBoxAddress";
            this.textBoxAddress.Size = new System.Drawing.Size(415, 22);
            this.textBoxAddress.TabIndex = 15;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(13, 250);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(298, 17);
            this.label7.TabIndex = 14;
            this.label7.Text = "Emby address (leave empty to auto-discover):";
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(16, 309);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(200, 30);
            this.buttonOK.TabIndex = 16;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(231, 308);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(200, 30);
            this.buttonCancel.TabIndex = 17;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // LauncherConfigForm
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(443, 356);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.textBoxAddress);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.textBoxPassword);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.textBoxUser);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textBoxTimeout);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBoxPort);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.buttonDefaultArgs);
            this.Controls.Add(this.textBoxArgs);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.buttonBrowse);
            this.Controls.Add(this.textBoxPath);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "LauncherConfigForm";
            this.Text = "Configure E2PL";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialogMpcHc;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxPath;
        private System.Windows.Forms.Button buttonBrowse;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxArgs;
        private System.Windows.Forms.Button buttonDefaultArgs;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxPort;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxTimeout;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxUser;
        private System.Windows.Forms.TextBox textBoxPassword;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBoxAddress;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
    }
}