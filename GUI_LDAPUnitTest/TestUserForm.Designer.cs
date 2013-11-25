namespace GUI_LDAPUnitTest
{
	partial class TestUserForm
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
			this.testUserCNLabel = new System.Windows.Forms.Label();
			this.testUserSNLabel = new System.Windows.Forms.Label();
			this.testUserDNLabel = new System.Windows.Forms.Label();
			this.testUserOtherLabel = new System.Windows.Forms.Label();
			this.button1 = new System.Windows.Forms.Button();
			this.testUserCNTextBox = new System.Windows.Forms.TextBox();
			this.testUserOtherTextBox = new System.Windows.Forms.TextBox();
			this.testUserDNTextBox = new System.Windows.Forms.TextBox();
			this.testUserSNTextBox = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// testUserCNLabel
			// 
			this.testUserCNLabel.AutoSize = true;
			this.testUserCNLabel.Location = new System.Drawing.Point(12, 22);
			this.testUserCNLabel.Name = "testUserCNLabel";
			this.testUserCNLabel.Size = new System.Drawing.Size(103, 13);
			this.testUserCNLabel.TabIndex = 0;
			this.testUserCNLabel.Text = "Insert Test User CN:";
			// 
			// testUserSNLabel
			// 
			this.testUserSNLabel.AutoSize = true;
			this.testUserSNLabel.Location = new System.Drawing.Point(12, 46);
			this.testUserSNLabel.Name = "testUserSNLabel";
			this.testUserSNLabel.Size = new System.Drawing.Size(103, 13);
			this.testUserSNLabel.TabIndex = 1;
			this.testUserSNLabel.Text = "Insert Test User SN:";
			// 
			// testUserDNLabel
			// 
			this.testUserDNLabel.AutoSize = true;
			this.testUserDNLabel.Location = new System.Drawing.Point(12, 71);
			this.testUserDNLabel.Name = "testUserDNLabel";
			this.testUserDNLabel.Size = new System.Drawing.Size(104, 13);
			this.testUserDNLabel.TabIndex = 2;
			this.testUserDNLabel.Text = "Insert Test User DN:";
			// 
			// testUserOtherLabel
			// 
			this.testUserOtherLabel.AutoSize = true;
			this.testUserOtherLabel.Location = new System.Drawing.Point(12, 109);
			this.testUserOtherLabel.Name = "testUserOtherLabel";
			this.testUserOtherLabel.Size = new System.Drawing.Size(115, 13);
			this.testUserOtherLabel.TabIndex = 3;
			this.testUserOtherLabel.Text = "Insert Other Attributes: ";
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(130, 260);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(120, 33);
			this.button1.TabIndex = 4;
			this.button1.Text = "OK";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// testUserCNTextBox
			// 
			this.testUserCNTextBox.Location = new System.Drawing.Point(130, 19);
			this.testUserCNTextBox.Name = "testUserCNTextBox";
			this.testUserCNTextBox.Size = new System.Drawing.Size(224, 20);
			this.testUserCNTextBox.TabIndex = 5;
			// 
			// testUserOtherTextBox
			// 
			this.testUserOtherTextBox.Location = new System.Drawing.Point(16, 135);
			this.testUserOtherTextBox.Multiline = true;
			this.testUserOtherTextBox.Name = "testUserOtherTextBox";
			this.testUserOtherTextBox.Size = new System.Drawing.Size(338, 80);
			this.testUserOtherTextBox.TabIndex = 6;
			// 
			// testUserDNTextBox
			// 
			this.testUserDNTextBox.Location = new System.Drawing.Point(130, 68);
			this.testUserDNTextBox.Name = "testUserDNTextBox";
			this.testUserDNTextBox.Size = new System.Drawing.Size(224, 20);
			this.testUserDNTextBox.TabIndex = 7;
			// 
			// testUserSNTextBox
			// 
			this.testUserSNTextBox.Location = new System.Drawing.Point(130, 43);
			this.testUserSNTextBox.Name = "testUserSNTextBox";
			this.testUserSNTextBox.Size = new System.Drawing.Size(224, 20);
			this.testUserSNTextBox.TabIndex = 8;
			// 
			// testUserForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(380, 305);
			this.Controls.Add(this.testUserSNTextBox);
			this.Controls.Add(this.testUserDNTextBox);
			this.Controls.Add(this.testUserOtherTextBox);
			this.Controls.Add(this.testUserCNTextBox);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.testUserOtherLabel);
			this.Controls.Add(this.testUserDNLabel);
			this.Controls.Add(this.testUserSNLabel);
			this.Controls.Add(this.testUserCNLabel);
			this.Name = "testUserForm";
			this.Text = "Setup Test User";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label testUserCNLabel;
		private System.Windows.Forms.Label testUserSNLabel;
		private System.Windows.Forms.Label testUserDNLabel;
		private System.Windows.Forms.Label testUserOtherLabel;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.TextBox testUserCNTextBox;
		private System.Windows.Forms.TextBox testUserOtherTextBox;
		private System.Windows.Forms.TextBox testUserDNTextBox;
		private System.Windows.Forms.TextBox testUserSNTextBox;
	}
}